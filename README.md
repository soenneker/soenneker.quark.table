[![](https://img.shields.io/nuget/v/soenneker.quark.table.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.quark.table/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.quark.table/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.quark.table/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.quark.table.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.quark.table/)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.quark.table/)

# QuarkTable - Component-Driven Blazor Table Library

A modern, component-driven Blazor table library that forces users to use Razor markup for complete control over table structure and behavior.

## Features

- **Component-Driven**: Every part of the table is a separate Razor component
- **Full Control**: Users must define table structure using Blazor markup
- **Sortable Headers**: Individual column headers can be made sortable
- **Search Integration**: Built-in search functionality with debouncing
- **Pagination**: Server-side pagination with customizable controls
- **Server-Side Processing**: Full support for server-side data processing
- **Customizable**: Each component can be customized independently

## Components

### Core Components

- **QuarkTable**: Main container component that manages state and events
- **QuarkThead**: Table header container
- **QuarkTh**: Table header cell with optional sorting
- **QuarkTr**: Table row component
- **QuarkTd**: Table data cell component

### Feature Components

- **QuarkTableSearch**: Standalone search component
- **QuarkTablePagination**: Pagination controls component
- **QuarkTableInfo**: Information display component

## Basic Usage

```razor
<QuarkTable TotalRecords="100" OnServerSideRequest="HandleServerSideRequest">
    <QuarkThead>
        <QuarkTr>
            <QuarkTh ColumnIndex="0" ColumnName="Name" Sortable="true">Name</QuarkTh>
            <QuarkTh ColumnIndex="1" ColumnName="Email" Sortable="true">Email</QuarkTh>
            <QuarkTh ColumnIndex="2" ColumnName="Age" Sortable="false">Age</QuarkTh>
        </QuarkTr>
    </QuarkThead>
    
    <tbody>
        @foreach (var person in people)
        {
            <QuarkTr Key="@person.Id">
                <QuarkTd>@person.Name</QuarkTd>
                <QuarkTd>@person.Email</QuarkTd>
                <QuarkTd>@person.Age</QuarkTd>
            </QuarkTr>
        }
    </tbody>
</QuarkTable>
```

## Component-Driven Approach

QuarkTable is designed to be 100% component-driven, meaning all table functionality is controlled through Blazor components that communicate with each other. This approach provides complete control over the table structure and behavior.

### How It Works

The component-driven approach uses cascading parameters to allow child components to communicate with the parent QuarkTable:

1. **QuarkTable** provides itself as a cascading value to all child components
2. **Child components** (QuarkTableSearch, QuarkTablePagination, etc.) receive the QuarkTable instance
3. **Events flow** from child components back to the parent QuarkTable
4. **Data flows** from the parent QuarkTable to child components for display

### Complete Component-Driven Example

```razor
<QuarkTable @ref="tableRef" 
            TotalRecords="@totalRecords"
            OnManualRequest="HandleManualRequest">
    
    <!-- Search component that communicates with the table -->
    <div class="quark-table-controls mb-3">
        <QuarkTableSearch Placeholder="Search people..." 
                          OnSearch="OnSearchHandler" />
    </div>
    
    <!-- Manual header using components -->
    <QuarkThead>
        <QuarkTr>
            <QuarkTh ColumnIndex="0" ColumnName="Name" Sortable="true">Name</QuarkTh>
            <QuarkTh ColumnIndex="1" ColumnName="Email" Sortable="true">Email</QuarkTh>
            <QuarkTh ColumnIndex="2" ColumnName="Age" Sortable="true">Age</QuarkTh>
            <QuarkTh ColumnIndex="3" ColumnName="Department" Sortable="false">Department</QuarkTh>
        </QuarkTr>
    </QuarkThead>
    
    <!-- Manual body using components -->
    <tbody>
        @if (currentData != null)
        {
            @foreach (var person in currentData)
            {
                <QuarkTr Key="@person.Id">
                    <QuarkTd>@person.Name</QuarkTd>
                    <QuarkTd>@person.Email</QuarkTd>
                    <QuarkTd>@person.Age</QuarkTd>
                    <QuarkTd>@person.Department</QuarkTd>
                </QuarkTr>
            }
        }
    </tbody>
    
    <!-- Pagination component that communicates with the table -->
    @if (tableRef != null)
    {
        <QuarkTablePagination CurrentPage="@tableRef.CurrentPage"
                              TotalPages="@tableRef.TotalPages"
                              TotalRecords="@tableRef.TotalRecordsCount"
                              PageSize="@tableRef.PageSize" />
    }
</QuarkTable>

@code {
    private QuarkTable? tableRef;
    private List<Person> people = new();
    private List<Person> currentData = new();
    private int totalRecords = 0;

    private async Task HandleManualRequest(DataTableServerSideRequest request)
    {
        var filteredPeople = people.AsQueryable();

        // Apply search
        if (!string.IsNullOrEmpty(request.Search?.Value))
        {
            var searchTerm = request.Search.Value.ToLower();
            filteredPeople = filteredPeople.Where(p => 
                p.Name.ToLower().Contains(searchTerm) ||
                p.Email.ToLower().Contains(searchTerm));
        }

        // Apply sorting
        if (request.Order?.Any() == true)
        {
            foreach (var order in request.Order)
            {
                filteredPeople = order.Column switch
                {
                    0 => order.Dir == "asc" ? filteredPeople.OrderBy(p => p.Name) : filteredPeople.OrderByDescending(p => p.Name),
                    1 => order.Dir == "asc" ? filteredPeople.OrderBy(p => p.Email) : filteredPeople.OrderByDescending(p => p.Email),
                    2 => order.Dir == "asc" ? filteredPeople.OrderBy(p => p.Age) : filteredPeople.OrderByDescending(p => p.Age),
                    _ => filteredPeople
                };
            }
        }

        totalRecords = filteredPeople.Count();
        currentData = filteredPeople.Skip(request.Start).Take(request.Length).ToList();
        StateHasChanged();
    }

    private async Task OnSearchHandler(string searchTerm)
    {
        // The search is handled automatically by the QuarkTable component
        // This method is called for additional logging or side effects
        Console.WriteLine($"Search triggered: {searchTerm}");
        await Task.CompletedTask;
    }
}
```

### Advanced Usage

#### Custom Search Component

```razor
<QuarkTable ShowSearch="false">
    <div class="quark-table-controls">
        <QuarkTableSearch Placeholder="Search employees..." 
                          SearchTerm="@searchTerm" 
                          SearchTermChanged="OnSearchChanged" />
    </div>
    
    <!-- Table content -->
</QuarkTable>
```

#### Custom Pagination

```razor
<QuarkTable ShowPagination="false">
    <!-- Table content -->
    
    <QuarkTablePagination ShowPagination="true"
                          CurrentPage="@(tableRef?.CurrentPage ?? 1)"
                          TotalPages="@(tableRef?.TotalPages ?? 1)"
                          TotalRecords="@(tableRef?.TotalRecords ?? 0)"
                          PageSize="@(tableRef?.PageSize ?? 10)"
                          MaxPageButtons="7" />
</QuarkTable>
```

#### Custom Info Display

```razor
<QuarkTableInfo CurrentPage="@(tableRef?.CurrentPage ?? 1)"
                TotalPages="@(tableRef?.TotalPages ?? 1)"
                TotalRecords="@(tableRef?.TotalRecords ?? 0)"
                PageSize="@(tableRef?.PageSize ?? 10)" />
```

#### Complete Control Panel

```razor
<QuarkTable @ref="tableRef" TotalRecords="@totalRecords" OnManualRequest="HandleRequest">
    <!-- Control components that communicate with the table -->
    <div class="quark-table-controls mb-3">
        <div class="d-flex justify-content-between align-items-center">
            <div class="d-flex align-items-center gap-3">
                <QuarkTableSearch Placeholder="Search employees..." 
                                  OnSearch="OnSearchHandler"
                                  DebounceMs="500" />
                
                @if (tableRef != null)
                {
                    <QuarkTablePageSizeSelector PageSize="@tableRef.PageSize"
                                               PageSizeOptions="[5, 10, 25, 50, 100]" />
                }
            </div>
            
            <div class="d-flex gap-2">
                <button class="btn btn-outline-secondary btn-sm" @onclick="ClearSorting">
                    Clear Sorting
                </button>
                <button class="btn btn-outline-secondary btn-sm" @onclick="ResetTable">
                    Reset Table
                </button>
            </div>
        </div>
    </div>
    
    <!-- Table content -->
</QuarkTable>
```

## QuarkTable Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `TotalRecords` | `int` | - | Total number of records for pagination |
| `OnServerSideRequest` | `Func<DataTableServerSideRequest, Task<DataTableServerResponse>>` | - | Server-side data processing function |
| `ShowSearch` | `bool` | `true` | Show built-in search controls |
| `ShowPagination` | `bool` | `true` | Show built-in pagination controls |
| `ShowPageSizeSelector` | `bool` | `true` | Show page size selector |
| `ShowInfo` | `bool` | `true` | Show information text |
| `SearchPlaceholder` | `string` | `"Search..."` | Search input placeholder |
| `SearchDebounceMs` | `int` | `300` | Search debounce delay in milliseconds |
| `MaxPageButtons` | `int` | `5` | Maximum number of page buttons |
| `DefaultPageSize` | `int` | `10` | Default page size |
| `SearchPosition` | `SearchPosition` | `End` | Position of search controls |

## QuarkTh Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Sortable` | `bool` | `true` | Enable sorting for this column |
| `ColumnIndex` | `int` | - | Column index for sorting |
| `ColumnName` | `string` | - | Column name for display |

## Server-Side Processing

The library supports full server-side processing through the `OnServerSideRequest` parameter:

```csharp
private async Task<DataTableServerResponse> HandleServerSideRequest(DataTableServerSideRequest request)
{
    var query = dbContext.Employees.AsQueryable();

    // Apply search
    if (!string.IsNullOrEmpty(request.Search?.Value))
    {
        var searchTerm = request.Search.Value.ToLower();
        query = query.Where(e => 
            e.Name.ToLower().Contains(searchTerm) ||
            e.Email.ToLower().Contains(searchTerm));
    }

    // Apply sorting
    if (request.Order?.Any() == true)
    {
        foreach (var order in request.Order)
        {
            query = order.Column switch
            {
                0 => order.Dir == "asc" ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name),
                1 => order.Dir == "asc" ? query.OrderBy(e => e.Email) : query.OrderByDescending(e => e.Email),
                _ => query
            };
        }
    }

    var totalRecords = await query.CountAsync();
    var pagedData = await query.Skip(request.Start).Take(request.Length).ToListAsync();

    return new DataTableServerResponse
    {
        Data = pagedData.Select(e => new List<string> { e.Name, e.Email }).ToList(),
        TotalRecords = totalRecords
    };
}
```

## Events

### QuarkTable Events

- `OnInitialize`: Called when the table is initialized
- `OnSearch`: Called when search term changes
- `OnPageSizeChanged`: Called when page size changes
- `OnGoToPage`: Called when navigating to a specific page
- `OnOrder`: Called when column sorting changes

### QuarkTableSearch Events

- `SearchTermChanged`: Called when search term changes

## Styling

The library includes CSS classes for styling:

- `.quark-table-wrapper`: Main table wrapper
- `.quark-table`: Table element
- `.quark-table-sortable`: Sortable header cells
- `.quark-table-sorted-asc`: Ascending sorted column
- `.quark-table-sorted-desc`: Descending sorted column
- `.quark-table-pagination`: Pagination container
- `.quark-table-pagination-btn`: Pagination buttons
- `.quark-table-search`: Search container
- `.quark-table-info`: Information display

## Migration from Previous Version

The new component-driven approach requires manual table structure definition:

**Before (Data-Driven):**
```razor
<QuarkTable Data="@people" Headers="@headers" />
```

**After (Component-Driven):**
```razor
<QuarkTable TotalRecords="@people.Count" OnServerSideRequest="HandleRequest">
    <QuarkThead>
        <QuarkTr>
            <QuarkTh ColumnIndex="0" ColumnName="Name">Name</QuarkTh>
            <QuarkTh ColumnIndex="1" ColumnName="Email">Email</QuarkTh>
        </QuarkTr>
    </QuarkThead>
    <tbody>
        @foreach (var person in people)
        {
            <QuarkTr Key="@person.Id">
                <QuarkTd>@person.Name</QuarkTd>
                <QuarkTd>@person.Email</QuarkTd>
            </QuarkTr>
        }
    </tbody>
</QuarkTable>
```

## Benefits

### Component-Driven Benefits

1. **Full Control**: Users have complete control over table structure and behavior
2. **Type Safety**: Compile-time checking of table structure and component interactions
3. **Flexibility**: Easy to customize individual components without affecting others
4. **Performance**: No dynamic column generation overhead, all components are statically defined
5. **Maintainability**: Clear separation of concerns with each component having a single responsibility
6. **Extensibility**: Easy to add new features to specific components without breaking others
7. **Event-Driven**: All interactions flow through Blazor events, making debugging and testing easier
8. **Reusability**: Components can be reused across different tables with different configurations
9. **Consistency**: All components follow the same pattern for communication and state management
10. **Developer Experience**: IntelliSense support for all component properties and methods

## License

This project is licensed under the MIT License.