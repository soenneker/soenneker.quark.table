using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Soenneker.DataTables.Dtos.ServerSideRequest;

namespace Soenneker.Quark.Table.Abstract;

/// <summary>
/// Interface for the QuarkTable component
/// </summary>
public interface IQuarkTable : IAsyncDisposable
{
    /// <summary>
    /// Gets the current search term
    /// </summary>
    string SearchTerm { get; }

    /// <summary>
    /// Gets the current page number
    /// </summary>
    int CurrentPage { get; }

    /// <summary>
    /// Gets the current page size
    /// </summary>
    int PageSize { get; }

    /// <summary>
    /// Gets the total number of pages
    /// </summary>
    int TotalPages { get; }

    /// <summary>
    /// Clears all current sorting and resets to first page
    /// </summary>
    Task ClearSorting();

    /// <summary>
    /// Clears the search term and resets to first page
    /// </summary>
    Task ClearSearch();

    /// <summary>
    /// Resets the table to its initial state (clears search, sorting, and goes to first page)
    /// </summary>
    Task Reset();

    /// <summary>
    /// Sets the search term programmatically and triggers a search
    /// </summary>
    /// <param name="searchTerm">The search term to set</param>
    Task SetSearchTerm(string searchTerm);

    /// <summary>
    /// Gets the current list of orders
    /// </summary>
    /// <returns>A copy of the current orders</returns>
    List<DataTableOrderRequest> GetCurrentOrders();

    /// <summary>
    /// Sets the orders programmatically and triggers a reload
    /// </summary>
    /// <param name="orders">The orders to set</param>
    Task SetOrders(List<DataTableOrderRequest> orders);

    /// <summary>
    /// Sorts a column by index (for use in manual mode)
    /// </summary>
    /// <param name="columnIndex">The column index to sort</param>
    Task SortColumnByIndex(int columnIndex);

    /// <summary>
    /// Gets the current sort direction for a column index
    /// </summary>
    /// <param name="columnIndex">The column index</param>
    /// <returns>The sort direction ("asc", "desc", or null if not sorted)</returns>
    string? GetSortDirection(int columnIndex);

    /// <summary>
    /// Gets the CSS class for a column index based on its sort state
    /// </summary>
    /// <param name="columnIndex">The column index</param>
    /// <returns>The CSS class for the sort state</returns>
    string GetSortClassByIndex(int columnIndex);

    /// <summary>
    /// Gets the sort indicator for a column index
    /// </summary>
    /// <param name="columnIndex">The column index</param>
    /// <returns>The sort indicator (↑, ↓, or ↕)</returns>
    string GetSortIndicatorByIndex(int columnIndex);
} 