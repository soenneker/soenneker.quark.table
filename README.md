[![](https://img.shields.io/nuget/v/soenneker.quark.table.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.quark.table/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.quark.table/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.quark.table/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.quark.table.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.quark.table/)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.quark.table/)

# Soenneker.Quark.Table

A powerful, native Blazor table component with search, pagination, sorting, and server-side processing.

## Features

- 🔍 **Search**: Real-time search with debouncing
- 📄 **Pagination**: First, previous, next, last, and direct page navigation
- 🔄 **Sorting**: Multi-column sorting with visual indicators
- ⚡ **Server-Side Processing**: Efficient data loading with continuation tokens
- 🎨 **Customizable**: Extensive styling and configuration options
- 📱 **Responsive**: Mobile-friendly design
- ♿ **Accessible**: Keyboard navigation and ARIA support
- 🔧 **Multiple Modes**: Server-side, manual, and direct data modes

## Installation

```bash
dotnet add package Soenneker.Quark.Table
```

Register services in your `Program.cs`:

```csharp
using Soenneker.Quark.Table.Registrars;

builder.Services.AddQuarkTable();
```

## Quick Start

### Server-Side Mode

```razor
@using Soenneker.Quark.Table
@using Soenneker.Quark.Table.Dtos

<QuarkTable OnServerSideRequest="HandleServerSideRequest">
</QuarkTable>

@code {
    private async Task<QuarkTableResponse> HandleServerSideRequest(QuarkTableRequest request)
    {
        // Your server-side processing logic here
        var data = await GetDataAsync(request.Start, request.Length, request.Search?.Value);
        
        return QuarkTableResponse.Success(
            request.Draw, 
            totalRecords, 
            filteredRecords, 
            data
        );
    }
}
```

### Manual Mode

```razor
<QuarkTable ManualMode="true"
            OnManualRequest="HandleManualRequest"
            TotalRecords="1000"
            ShowSearch="true"
            ShowPagination="true">
    <ManualHeader>
            <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Email</th>
            </tr>
    </ManualHeader>
    <ManualBody>
            @foreach (var item in items)
            {
                <tr>
                    <td>@item.Id</td>
                    <td>@item.Name</td>
                    <td>@item.Email</td>
                </tr>
            }
    </ManualBody>
</QuarkTable>

@code {
    private async Task HandleManualRequest(QuarkTableRequest request)
    {
        // Handle the request manually
        // Update your data source based on request parameters
    }
}
```

### Direct Data Mode

```razor
<QuarkTable DirectData="@tableData"
            ShowSearch="true"
            ShowPagination="true">
</QuarkTable>

@code {
    private List<List<string>> tableData = new()
    {
        new() { "ID", "Name", "Email" },
        new() { "1", "John Doe", "john@example.com" },
        new() { "2", "Jane Smith", "jane@example.com" }
    };
}
```

## Key Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `OnServerSideRequest` | `Func<QuarkTableRequest, Task<QuarkTableResponse>>` | Server-side data processing callback |
| `OnManualRequest` | `EventCallback<QuarkTableRequest>` | Manual mode request callback |
| `ManualMode` | `bool` | Enable manual mode (default: false) |
| `DirectData` | `object` | Direct data for client-side processing |
| `ManualHeader` | `RenderFragment` | Custom header content for manual mode |
| `ManualBody` | `RenderFragment` | Custom body content for manual mode |
| `ShowSearch` | `bool` | Show search input (default: true) |
| `ShowPagination` | `bool` | Show pagination controls (default: true) |
| `ShowPageSizeSelector` | `bool` | Show page size selector (default: true) |
| `Options` | `QuarkTableOptions` | Table configuration options |

## Configuration Options

```csharp
var options = new QuarkTableOptions
{
    Sortable = true,
    DefaultPageSize = 10,
    PageSizeOptions = [10, 25, 50, 100],
    ShowPageSizeSelector = true,
    ShowSearch = true,
    SearchPlaceholder = "Search...",
    SearchDebounceMs = 300,
    SearchPosition = SearchPosition.End,
    ShowPagination = true,
    MaxPageButtons = 5,
    ServerSide = true,
    ShowInfo = true
};
```

## Data Transfer Objects

### QuarkTableRequest
```csharp
public class QuarkTableRequest
{
    public int Draw { get; set; }
    public int Start { get; set; }
    public int Length { get; set; }
    public QuarkTableSearch? Search { get; set; }
    public List<QuarkTableOrder>? Order { get; set; }
    public string? ContinuationToken { get; set; }
}
```

### QuarkTableResponse
```csharp
public class QuarkTableResponse
{
    public int Draw { get; set; }
    public int TotalRecords { get; set; }
    public int TotalFilteredRecords { get; set; }
    public object? Data { get; set; }
    public string? Error { get; set; }
    public string? ContinuationToken { get; set; }
    
    public static QuarkTableResponse Success(int draw, int totalRecords, int filteredRecords, object data, string? continuationToken = null);
    public static QuarkTableResponse Fail(int draw, string errorMessage);
}
```

## Event Callbacks

```csharp
<QuarkTable OnServerSideRequest="HandleServerSideRequest"
            OnSearch="HandleSearch"
            OnPageSizeChanged="HandlePageSizeChanged"
            OnGoToPage="HandleGoToPage"
            OnOrder="HandleOrder"
            OnInitialize="HandleInitialize">
    <!-- Table content -->
</QuarkTable>

@code {
    private async Task HandleSearch(string searchTerm)
    {
        // Handle search term changes
    }

    private async Task HandlePageSizeChanged(int pageSize)
    {
        // Handle page size changes
    }

    private async Task HandleGoToPage(int page)
    {
        // Handle page navigation
    }

    private async Task HandleOrder(QuarkTableOrderEventArgs args)
    {
        // Handle column sorting
    }

    private async Task HandleInitialize()
    {
        // Handle component initialization
    }
}
```

## Demo

Check out the [live demo](https://soenneker.github.io/soenneker.quark.table/) to see the component in action.

## License

This project is licensed under the MIT License.