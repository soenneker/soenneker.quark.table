using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Soenneker.DataTables.Dtos.ServerSideRequest;
using Soenneker.Quark.Table.Options;

namespace Soenneker.Quark.Table.Abstract;

/// <summary>
/// Interface for the QuarkTable component
/// </summary>
public interface IQuarkTable : IAsyncDisposable
{
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
    /// Gets the total number of records
    /// </summary>
    int TotalRecordsCount { get; }

    /// <summary>
    /// Gets the table options
    /// </summary>
    QuarkTableOptions Options { get; }

    /// <summary>
    /// Handles column sorting for the component-driven approach
    /// </summary>
    /// <param name="columnIndex">The column index to sort</param>
    /// <param name="columnName">The column name</param>
    Task HandleColumnSort(int columnIndex, string columnName);

    /// <summary>
    /// Handles search from child components
    /// </summary>
    /// <param name="searchTerm">The search term</param>
    Task HandleSearch(string searchTerm);

    /// <summary>
    /// Handles navigation to a specific page
    /// </summary>
    /// <param name="page">The page number to navigate to</param>
    Task HandleGoToPage(int page);

    /// <summary>
    /// Clears all current sorting and resets to first page
    /// </summary>
    Task ClearSorting();

    /// <summary>
    /// Resets the table to its initial state (clears sorting and goes to first page)
    /// </summary>
    Task Reset();

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