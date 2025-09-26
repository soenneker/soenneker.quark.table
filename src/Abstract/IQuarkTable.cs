using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Soenneker.DataTables.Dtos.ServerSideRequest;

namespace Soenneker.Quark;

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
    /// Gets whether the table has loaded data at least once
    /// </summary>
    bool HasLoadedOnce { get; }

    /// <summary>
    /// Gets the current search term
    /// </summary>
    string? SearchTerm { get; }

    /// <summary>
    /// Gets the current sort by field
    /// </summary>
    string? SortBy { get; }

    /// <summary>
    /// Gets the current sort direction
    /// </summary>
    string? SortDirection { get; }

    /// <summary>
    /// Gets the table options
    /// </summary>
    QuarkTableOptions Options { get; }

    /// <summary>
    /// Handles column sorting for the component-driven approach
    /// </summary>
    /// <param name="columnIndex">The column index to sort</param>
    ValueTask HandleColumnSort(int columnIndex);

    /// <summary>
    /// Registers a column header component and returns its index
    /// </summary>
    /// <param name="columnHeader">The column header component to register</param>
    /// <returns>The column index</returns>
    int RegisterColumn(QuarkTh columnHeader);

    /// <summary>
    /// Handles search from child components
    /// </summary>
    /// <param name="searchTerm">The search term</param>
    ValueTask HandleSearch(string searchTerm);

    /// <summary>
    /// Handles navigation to a specific page
    /// </summary>
    /// <param name="page">The page number to navigate to</param>
    ValueTask HandleGoToPage(int page);

    /// <summary>
    /// Navigates to a specific page
    /// </summary>
    /// <param name="page">The page number to navigate to</param>
    ValueTask GoToPage(int page);

    /// <summary>
    /// Clears all current sorting and resets to first page
    /// </summary>
    ValueTask ClearSorting();

    /// <summary>
    /// Resets the table to its initial state (clears sorting and goes to first page)
    /// </summary>
    ValueTask Reset();

    /// <summary>
    /// Gets the current list of orders
    /// </summary>
    /// <returns>A copy of the current orders</returns>
    List<DataTableOrderRequest> GetCurrentOrders();

    /// <summary>
    /// Sets the orders programmatically and triggers a reload
    /// </summary>
    /// <param name="orders">The orders to set</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    ValueTask SetOrders(List<DataTableOrderRequest> orders);

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

    /// <summary>
    /// Cancels any ongoing operations and resets the loading state
    /// </summary>
    ValueTask CancelOperations();

    /// <summary>
    /// Updates the continuation token paging with response data
    /// </summary>
    /// <param name="recordCount">The number of records in the current response</param>
    /// <param name="continuationToken">The continuation token from the response</param>
    /// <param name="tokenUsedForCurrentPage">The continuation token that was used to reach the current page</param>
    void UpdateContinuationTokenPaging(int recordCount, string? continuationToken, string? tokenUsedForCurrentPage = null);
} 
