using Bogus;
using Microsoft.Extensions.Logging;
using Soenneker.DataTables.Dtos.ServerResponse;
using Soenneker.DataTables.Dtos.ServerSideRequest;
using Soenneker.Dtos.Results.Paged;
using Soenneker.Quark.Table.Demo.Dtos;
using Soenneker.Utils.AutoBogus;
using Soenneker.Utils.Delay;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Quark.Table.Demo.Services;

public class EmployeeService
{
    private readonly ILogger<EmployeeService> _logger;
    private readonly List<Employee> _employees;
    private readonly Faker<Employee> _faker;

    public EmployeeService(ILogger<EmployeeService> logger)
    {
        _logger = logger;
        
        _faker = new AutoFaker<Employee>()
            .RuleFor(e => e.Id, f => f.Random.Int(1, 1000))
            .RuleFor(e => e.Name, f => f.Person.FullName)
            .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.Name))
            .RuleFor(e => e.Department, f => f.PickRandom("Engineering", "Marketing", "Sales", "HR", "Finance", "Operations"))
            .RuleFor(e => e.Salary, f => f.Random.Decimal(30000, 150000))
            .RuleFor(e => e.HireDate, f => f.Date.Past(5))
            .RuleFor(e => e.Status, f => f.PickRandom("Active", "Inactive", "On Leave"));

        _employees = _faker.Generate(150);
        _logger.LogInformation("EmployeeService initialized with {Count} employee records", _employees.Count);
    }

    public async Task<DataTableServerResponse> GetEmployees(DataTableServerSideRequest serverSideRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Server-side request: Start={Start}, Length={Length}, Search='{Search}', ContinuationToken='{Token}'", 
            serverSideRequest.Start, serverSideRequest.Length, serverSideRequest.Search?.Value, serverSideRequest.ContinuationToken ?? "null");

        IEnumerable<Employee> filteredData = _employees.AsEnumerable();

        // Apply search
        if (!string.IsNullOrEmpty(serverSideRequest.Search?.Value))
        {
            string searchTerm = serverSideRequest.Search.Value.ToLower();
            filteredData = filteredData.Where(e => 
            e.Id.ToString().Contains(searchTerm) ||
                e.Name.ToLower().Contains(searchTerm) ||
                e.Department.ToLower().Contains(searchTerm) ||
                e.Email.ToLower().Contains(searchTerm) ||
                e.Status.ToLower().Contains(searchTerm));
        }

        // Apply sorting
        if (serverSideRequest.Order != null && serverSideRequest.Order.Count > 0)
        {
            _logger.LogDebug("Applying sorting: {OrderCount} orders", serverSideRequest.Order.Count);
            
            IOrderedEnumerable<Employee>? orderedData = null;
            
            foreach (DataTableOrderRequest order in serverSideRequest.Order)
            {
                _logger.LogDebug("Sorting column {Column} in direction {Direction}", order.Column, order.Dir);
                
                if (orderedData == null)
                {
                    // First sort
                    orderedData = order.Dir.ToLower() == "asc" 
                        ? filteredData.OrderBy(e => GetEmployeeProperty(e, order.Column))
                        : filteredData.OrderByDescending(e => GetEmployeeProperty(e, order.Column));
                }
                else
                {
                    // Additional sorts
                    orderedData = order.Dir.ToLower() == "asc" 
                        ? orderedData.ThenBy(e => GetEmployeeProperty(e, order.Column))
                        : orderedData.ThenByDescending(e => GetEmployeeProperty(e, order.Column));
                }
            }
            
            if (orderedData != null)
            {
                filteredData = orderedData;
            }
        }

        int totalRecords = filteredData.Count();
        IEnumerable<Employee> pagedData = filteredData.Skip(serverSideRequest.Start).Take(serverSideRequest.Length);

        var tableData = new List<List<string>>();

        // Add headers as the first row
        tableData.Add([
            "Name",
            "Department", 
            "Email",
            "Salary",
            "Hire Date",
            "Status",
            "Id"
        ]);

        foreach (Employee employee in pagedData)
        {
            tableData.Add([
                employee.Name,
                employee.Department,
                employee.Email,
                employee.Salary.ToString("C"),
                employee.HireDate.ToString("MMM dd, yyyy"),
                employee.Status,
                employee.Id.ToString()
            ]);
        }

        await DelayUtil.Delay(500, _logger, cancellationToken);

        return DataTableServerResponse.Success(serverSideRequest.Draw, totalRecords, totalRecords, tableData);
    }

    public async Task<List<Employee>> GetFilteredEmployees(DataTableServerSideRequest serverSideRequest, CancellationToken cancellationToken = default)
    {
        IEnumerable<Employee> filteredData = _employees.AsEnumerable();

        // Apply search
        if (!string.IsNullOrEmpty(serverSideRequest.Search?.Value))
        {
            string searchTerm = serverSideRequest.Search.Value.ToLower();
            filteredData = filteredData.Where(e => 
                e.Name.ToLower().Contains(searchTerm) ||
                e.Department.ToLower().Contains(searchTerm) ||
                e.Email.ToLower().Contains(searchTerm) ||
                e.Status.ToLower().Contains(searchTerm));
        }

        // Apply sorting
        if (serverSideRequest.Order != null && serverSideRequest.Order.Count > 0)
        {
            _logger.LogDebug("Applying sorting to filtered employees: {OrderCount} orders", serverSideRequest.Order.Count);
            
            IOrderedEnumerable<Employee>? orderedData = null;
            
            foreach (DataTableOrderRequest order in serverSideRequest.Order)
            {
                _logger.LogDebug("Sorting column {Column} in direction {Direction}", order.Column, order.Dir);
                
                if (orderedData == null)
                {
                    // First sort
                    orderedData = order.Dir.ToLower() == "asc" 
                        ? filteredData.OrderBy(e => GetEmployeeProperty(e, order.Column))
                        : filteredData.OrderByDescending(e => GetEmployeeProperty(e, order.Column));
                }
                else
                {
                    // Additional sorts
                    orderedData = order.Dir.ToLower() == "asc" 
                        ? orderedData.ThenBy(e => GetEmployeeProperty(e, order.Column))
                        : orderedData.ThenByDescending(e => GetEmployeeProperty(e, order.Column));
                }
            }
            
            if (orderedData != null)
            {
                filteredData = orderedData;
            }
        }

        return filteredData.Skip(serverSideRequest.Start).Take(serverSideRequest.Length).ToList();
    }

    public int GetTotalCount()
    {
        return _employees.Count;
    }

    public List<Employee> GetAllEmployees()
    {
        return _employees.ToList();
    }

    public async Task<PagedResult<Employee>> GetEmployeesPaged(DataTableServerSideRequest serverSideRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("GetEmployeesPaged: Start={Start}, Length={Length}, Search='{Search}', ContinuationToken='{Token}'", 
            serverSideRequest.Start, serverSideRequest.Length, serverSideRequest.Search?.Value, serverSideRequest.ContinuationToken ?? "null");

        IEnumerable<Employee> filteredData = _employees.AsEnumerable();

        // Apply search
        if (!string.IsNullOrEmpty(serverSideRequest.Search?.Value))
        {
            string searchTerm = serverSideRequest.Search.Value.ToLower();
            filteredData = filteredData.Where(e => 
                e.Name.ToLower().Contains(searchTerm) ||
                e.Department.ToLower().Contains(searchTerm) ||
                e.Email.ToLower().Contains(searchTerm) ||
                e.Status.ToLower().Contains(searchTerm));
        }

        // Apply sorting
        if (serverSideRequest.Order != null && serverSideRequest.Order.Count > 0)
        {
            _logger.LogDebug("Applying sorting: {OrderCount} orders", serverSideRequest.Order.Count);
            
            IOrderedEnumerable<Employee>? orderedData = null;
            
            foreach (DataTableOrderRequest order in serverSideRequest.Order)
            {
                _logger.LogDebug("Sorting column {Column} in direction {Direction}", order.Column, order.Dir);
                
                if (orderedData == null)
                {
                    // First sort
                    orderedData = order.Dir.ToLower() == "asc" 
                        ? filteredData.OrderBy(e => GetEmployeeProperty(e, order.Column))
                        : filteredData.OrderByDescending(e => GetEmployeeProperty(e, order.Column));
                }
                else
                {
                    // Additional sorts
                    orderedData = order.Dir.ToLower() == "asc" 
                        ? orderedData.ThenBy(e => GetEmployeeProperty(e, order.Column))
                        : orderedData.ThenByDescending(e => GetEmployeeProperty(e, order.Column));
                }
            }
            
            if (orderedData != null)
            {
                filteredData = orderedData;
            }
        }

        int totalRecords = filteredData.Count();
        List<Employee> pagedData = filteredData.Skip(serverSideRequest.Start).Take(serverSideRequest.Length).ToList();

        // Simulate continuation token logic
        string? continuationToken = null;
        if (serverSideRequest.Start + serverSideRequest.Length < totalRecords)
        {
            // Generate a simple continuation token for demo purposes
            continuationToken = $"page_{serverSideRequest.Start + serverSideRequest.Length}_{serverSideRequest.Length}";
        }

        await DelayUtil.Delay(500, _logger, cancellationToken);

        return new PagedResult<Employee>
        {
            Items = pagedData,
            TotalCount = totalRecords,
            PageSize = serverSideRequest.Length,
            ContinuationToken = continuationToken
        };
    }

    private static object GetEmployeeProperty(Employee employee, int columnIndex)
    {
        return columnIndex switch
        {
            0 => employee.Name,
            1 => employee.Department,
            2 => employee.Email,
            3 => employee.Salary,
            4 => employee.HireDate,
            5 => employee.Status,
            6 => employee.Id,
            _ => employee.Name // Default to name for unknown columns
        };
    }
} 
