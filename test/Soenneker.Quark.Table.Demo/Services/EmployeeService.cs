using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Logging;
using Soenneker.Quark.Table.Demo.Dtos;
using Soenneker.Quark.Table.Dtos;
using Soenneker.Utils.AutoBogus;

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

    public async Task<QuarkTableResponse> GetEmployees(QuarkTableRequest request)
    {
        _logger.LogDebug("Server-side request: Start={Start}, Length={Length}, Search='{Search}', ContinuationToken='{Token}'", 
            request.Start, request.Length, request.Search?.Value, request.ContinuationToken ?? "null");

        IEnumerable<Employee> filteredData = _employees.AsEnumerable();

        // Apply search
        if (!string.IsNullOrEmpty(request.Search?.Value))
        {
            string searchTerm = request.Search.Value.ToLower();
            filteredData = filteredData.Where(e => 
                e.Name.ToLower().Contains(searchTerm) ||
                e.Department.ToLower().Contains(searchTerm) ||
                e.Email.ToLower().Contains(searchTerm) ||
                e.Status.ToLower().Contains(searchTerm));
        }

        // Apply sorting - using the actual column names from the table data
        if (request.Order?.Any() == true)
        {
            foreach (QuarkTableOrder order in request.Order)
            {
                filteredData = order.Column?.ToLower() switch
                {
                    "name" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.Name) : 
                        filteredData.OrderByDescending(e => e.Name),
                    "department" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.Department) : 
                        filteredData.OrderByDescending(e => e.Department),
                    "email" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.Email) : 
                        filteredData.OrderByDescending(e => e.Email),
                    "salary" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.Salary) : 
                        filteredData.OrderByDescending(e => e.Salary),
                    "hiredate" or "hire date" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.HireDate) : 
                        filteredData.OrderByDescending(e => e.HireDate),
                    "status" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.Status) : 
                        filteredData.OrderByDescending(e => e.Status),
                    "id" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.Id) : 
                        filteredData.OrderByDescending(e => e.Id),
                    _ => filteredData
                };
            }
        }

        int totalRecords = filteredData.Count();
        IEnumerable<Employee> pagedData = filteredData.Skip(request.Start).Take(request.Length);

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

        return QuarkTableResponse.Success(request.Draw, totalRecords, totalRecords, tableData);
    }

    public async Task<List<Employee>> GetFilteredEmployees(QuarkTableRequest request)
    {
        IEnumerable<Employee> filteredData = _employees.AsEnumerable();

        // Apply search
        if (!string.IsNullOrEmpty(request.Search?.Value))
        {
            string searchTerm = request.Search.Value.ToLower();
            filteredData = filteredData.Where(e => 
                e.Name.ToLower().Contains(searchTerm) ||
                e.Department.ToLower().Contains(searchTerm) ||
                e.Email.ToLower().Contains(searchTerm) ||
                e.Status.ToLower().Contains(searchTerm));
        }

        // Apply sorting - using the actual column names from the table data
        if (request.Order?.Any() == true)
        {
            foreach (QuarkTableOrder order in request.Order)
            {
                filteredData = order.Column?.ToLower() switch
                {
                    "name" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.Name) : 
                        filteredData.OrderByDescending(e => e.Name),
                    "department" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.Department) : 
                        filteredData.OrderByDescending(e => e.Department),
                    "email" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.Email) : 
                        filteredData.OrderByDescending(e => e.Email),
                    "salary" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.Salary) : 
                        filteredData.OrderByDescending(e => e.Salary),
                    "hiredate" or "hire date" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.HireDate) : 
                        filteredData.OrderByDescending(e => e.HireDate),
                    "status" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.Status) : 
                        filteredData.OrderByDescending(e => e.Status),
                    "id" => order.Direction == "asc" ? 
                        filteredData.OrderBy(e => e.Id) : 
                        filteredData.OrderByDescending(e => e.Id),
                    _ => filteredData
                };
            }
        }

        return filteredData.Skip(request.Start).Take(request.Length).ToList();
    }

    public int GetTotalCount()
    {
        return _employees.Count;
    }

    public List<Employee> GetAllEmployees()
    {
        return _employees.ToList();
    }
} 