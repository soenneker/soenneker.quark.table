using System;
using System.Linq;
using System.Text.Json.Serialization;
using Soenneker.Quark.Table.Enums;

namespace Soenneker.Quark.Table.Options;

/// <summary>
/// Configuration options for QuarkTable
/// </summary>
public sealed class QuarkTableOptions
{
    /// <summary>
    /// Gets or sets whether to enable sorting
    /// </summary>
    [JsonPropertyName("sortable")]
    public bool Sortable { get; set; } = true;

    /// <summary>
    /// Gets or sets the default page size
    /// </summary>
    [JsonPropertyName("defaultPageSize")]
    public int DefaultPageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the available page size options
    /// </summary>
    [JsonPropertyName("pageSizeOptions")]
    public int[] PageSizeOptions { get; set; } = [10, 25, 50, 100];

    /// <summary>
    /// Gets or sets whether to show the page size selector
    /// </summary>
    [JsonPropertyName("showPageSizeSelector")]
    public bool ShowPageSizeSelector { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to show the search box
    /// </summary>
    [JsonPropertyName("showSearch")]
    public bool ShowSearch { get; set; } = true;

    /// <summary>
    /// Gets or sets the search placeholder text
    /// </summary>
    [JsonPropertyName("searchPlaceholder")]
    public string SearchPlaceholder { get; set; } = "Search...";

    /// <summary>
    /// Gets or sets the search debounce delay in milliseconds
    /// </summary>
    [JsonPropertyName("searchDebounceMs")]
    public int SearchDebounceMs { get; set; } = 300;

    /// <summary>
    /// Gets or sets the position of the search box
    /// </summary>
    [JsonPropertyName("searchPosition")]
    public SearchPosition SearchPosition { get; set; } = SearchPosition.End;

    /// <summary>
    /// Gets or sets whether to show pagination controls
    /// </summary>
    [JsonPropertyName("showPagination")]
    public bool ShowPagination { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum number of page buttons to show
    /// </summary>
    [JsonPropertyName("maxPageButtons")]
    public int MaxPageButtons { get; set; } = 5;

    /// <summary>
    /// Gets or sets whether to enable server-side processing
    /// </summary>
    [JsonPropertyName("serverSide")]
    public bool ServerSide { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to show the info text (e.g., "Showing X to Y of Z entries")
    /// </summary>
    [JsonPropertyName("showInfo")]
    public bool ShowInfo { get; set; } = true;

    /// <summary>
    /// Creates a deep copy of the current options
    /// </summary>
    /// <returns>A new QuarkTableOptions instance with the same values</returns>
    public QuarkTableOptions Clone()
    {
        return new QuarkTableOptions
        {
            Sortable = this.Sortable,
            DefaultPageSize = this.DefaultPageSize,
            PageSizeOptions = this.PageSizeOptions.Clone() as int[] ?? this.PageSizeOptions,
            ShowPageSizeSelector = this.ShowPageSizeSelector,
            ShowSearch = this.ShowSearch,
            SearchPlaceholder = this.SearchPlaceholder,
            SearchDebounceMs = this.SearchDebounceMs,
            SearchPosition = this.SearchPosition,
            ShowPagination = this.ShowPagination,
            MaxPageButtons = this.MaxPageButtons,
            ServerSide = this.ServerSide,
            ShowInfo = this.ShowInfo
        };
    }

    /// <summary>
    /// Determines if this options instance equals another
    /// </summary>
    /// <param name="obj">The options to compare with</param>
    /// <returns>True if the options are equal, false otherwise</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not QuarkTableOptions other)
            return false;

        return Sortable == other.Sortable &&
               DefaultPageSize == other.DefaultPageSize &&
               ShowPageSizeSelector == other.ShowPageSizeSelector &&
               ShowSearch == other.ShowSearch &&
               SearchPlaceholder == other.SearchPlaceholder &&
               SearchDebounceMs == other.SearchDebounceMs &&
               SearchPosition == other.SearchPosition &&
               ShowPagination == other.ShowPagination &&
               MaxPageButtons == other.MaxPageButtons &&
               ServerSide == other.ServerSide &&
               ShowInfo == other.ShowInfo &&
               PageSizeOptions.SequenceEqual(other.PageSizeOptions);
    }

    /// <summary>
    /// Gets the hash code for this options instance
    /// </summary>
    /// <returns>The hash code</returns>
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Sortable);
        hash.Add(DefaultPageSize);
        hash.Add(ShowPageSizeSelector);
        hash.Add(ShowSearch);
        hash.Add(SearchPlaceholder);
        hash.Add(SearchDebounceMs);
        hash.Add(SearchPosition);
        hash.Add(ShowPagination);
        hash.Add(MaxPageButtons);
        hash.Add(ServerSide);
        hash.Add(ShowInfo);
        hash.Add(PageSizeOptions);
        return hash.ToHashCode();
    }
} 