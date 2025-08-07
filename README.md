[![](https://img.shields.io/nuget/v/soenneker.quark.table.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.quark.table/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.quark.table/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.quark.table/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.quark.table.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.quark.table/)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.quark.table/)

# Soenneker.Quark.Table

A modern, component-driven Blazor table library that provides complete control over table structure and behavior through Razor components.

## Features

- **Component-Driven**: Every part of the table is a separate Razor component
- **Full Control**: Users define table structure using Blazor markup
- **Sortable Headers**: Individual column headers can be made sortable
- **Search Integration**: Built-in search functionality with debouncing
- **Pagination**: Server-side pagination with customizable controls
- **Server-Side Processing**: Full support for server-side data processing
- **Continuation Token Support**: Advanced pagination with continuation tokens
- **Loading States**: Smooth loading behavior with overlay

## Components

### Core Components
- **QuarkTable**: Main container component that manages state and events
- **QuarkTableElement**: Table wrapper component
- **QuarkThead**: Table header container
- **QuarkTh**: Table header cell with optional sorting
- **QuarkTbody**: Table body container
- **QuarkTr**: Table row component
- **QuarkTd**: Table data cell component

### Feature Components
- **QuarkTableSearch**: Standalone search component
- **QuarkTablePagination**: Pagination controls component
- **QuarkTableInfo**: Information display component (shows "x-y of z" format)
- **QuarkTableBottomBar**: Bottom bar layout wrapper for info and pagination components
- **QuarkTableTopBar**: Top bar layout component for header content
- **QuarkTableLeft**: Left-aligned container component for use in topbar/bottombar
- **QuarkTableRight**: Right-aligned container component for use in topbar/bottombar
- **QuarkTableBarControls**: Container for buttons and custom controls
- **QuarkTableNoData**: No data state component with customizable content
- **QuarkTableLoader**: Loading state component
- **QuarkTablePageSizeSelector**: Page size selector component

### Legacy Components
- **QuarkTableControls**: Legacy name for QuarkTableBottomBar (still supported)

## Basic Usage

```razor
<QuarkTable TotalRecords="100" OnInteraction="OnInteraction">
    <QuarkTableSearch Placeholder="Search employees..." />
    
    <QuarkTableElement>
        <QuarkThead>
            <QuarkTr>
                <QuarkTh Sortable="true" Searchable="true">Name</QuarkTh>
                <QuarkTh Sortable="true" Searchable="true">Email</QuarkTh>
                <QuarkTh Sortable="false" Searchable="true">Age</QuarkTh>
            </QuarkTr>
        </QuarkThead>
        
        <QuarkTbody>
            @foreach (var person in people)
            {
                <QuarkTr Key="@person.Id">
                    <QuarkTd>@person.Name</QuarkTd>
                    <QuarkTd>@person.Email</QuarkTd>
                    <QuarkTd>@person.Age</QuarkTd>
                </QuarkTr>
            }
        </QuarkTbody>
    </QuarkTableElement>
    
    <QuarkTablePagination />
</QuarkTable>
```

## Custom Components

### QuarkTableNoData
Display a custom "no data" state when there are no records to show:

```razor
<QuarkTableNoData>
    <div style="text-align: center; padding: 2rem;">
        <h4>No records found</h4>
        <p>Try adjusting your search criteria.</p>
    </div>
</QuarkTableNoData>
```

### QuarkTableInfo
Display table information independently of pagination:

```razor
<QuarkTableInfo />
<!-- or with custom content -->
<QuarkTableInfo>
    <span>Showing @start-@end of @total records</span>
</QuarkTableInfo>
```

### QuarkTableBottomBar
Layout wrapper that combines info and pagination components with flexible layout options:

```razor
<!-- Default layout: Info left, pagination right -->
<QuarkTableBottomBar>
    <QuarkTableLeft>
        <QuarkTableInfo />
    </QuarkTableLeft>
    <QuarkTableRight>
        <QuarkTablePagination />
    </QuarkTableRight>
</QuarkTableBottomBar>

<!-- Reversed layout: Info right, pagination left -->
<QuarkTableBottomBar ControlsLayout="QuarkTableControlsLayout.InfoRightPaginationLeft">
    <QuarkTableInfo />
    <QuarkTablePagination />
</QuarkTableBottomBar>

<!-- Using Left/Right components for custom layouts -->
<QuarkTableBottomBar>
    <QuarkTableLeft>
        <QuarkTableInfo />
        <QuarkTablePageSizeSelector />
    </QuarkTableLeft>
    <QuarkTableRight>
        <QuarkTablePagination />
    </QuarkTableRight>
</QuarkTableBottomBar>
```

## Server-Side Processing

```csharp
private async Task OnInteraction(DataTableServerSideRequest request)
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

    currentData = pagedData;
    totalRecords = totalRecords;
    StateHasChanged();
}
```

## Continuation Token Support

```csharp
private async Task OnInteraction(DataTableServerSideRequest request)
{
    PagedResult<Employee> pagedResult = await EmployeeService.GetEmployeesPaged(request);
    _currentEmployees = pagedResult.Items;

    if (_quarkTable != null)
    {
        _quarkTable.UpdateContinuationTokenPaging(
            pagedResult.Items.Count,
            pagedResult.ContinuationToken,
            request.ContinuationToken);
    }
}
```

## Key Parameters

### QuarkTable
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `TotalRecords` | `int` | `0` | Total number of records for pagination |
| `Visible` | `bool` | `true` | Whether the table is visible |
| `OnManualRequest` | `EventCallback<DataTableServerSideRequest>` | - | Server-side data processing callback |
| `OnOrder` | `EventCallback<QuarkTableOrderEventArgs>` | - | Called when column sorting changes |
| `Options` | `QuarkTableOptions` | `new()` | Table configuration options |

### QuarkTh
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Sortable` | `bool` | `true` | Enable sorting for this column |
| `Searchable` | `bool` | `true` | Enable search for this column |

### QuarkTableOptions
| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `DefaultPageSize` | `int` | `10` | Default page size |
| `SearchDebounceMs` | `int` | `300` | Search debounce delay in milliseconds |
| `SearchPosition` | `SearchPosition` | `End` | Position of the search box |
| `Debug` | `bool` | `false` | Enable debug logging |

## Events

- `OnInitialize`: Called when the table is initialized
- `OnManualRequest`: Called when data needs to be loaded
- `OnOrder`: Called when column sorting changes
- `OnPageSizeChanged`: Called when page size changes
- `OnGoToPage`: Called when navigating to a specific page