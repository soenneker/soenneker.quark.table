using System.Text.Json.Serialization;
using Soenneker.Quark.Table.Enums;
using System.Linq;

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
    /// Gets or sets the maximum number of page buttons to show
    /// </summary>
    [JsonPropertyName("maxPageButtons")]
    public int MaxPageButtons { get; set; } = 5;

    /// <summary>
    /// Gets or sets whether to show the page size selector
    /// </summary>
    [JsonPropertyName("showPageSizeSelector")]
    public bool ShowPageSizeSelector { get; set; } = true;

    /// <summary>
    /// Creates a clone of the current options
    /// </summary>
    /// <returns>A new instance with the same values</returns>
    public QuarkTableOptions Clone()
    {
        return new QuarkTableOptions
        {
            Sortable = this.Sortable,
            DefaultPageSize = this.DefaultPageSize,
            PageSizeOptions = this.PageSizeOptions,
            SearchDebounceMs = this.SearchDebounceMs,
            SearchPosition = this.SearchPosition,
            MaxPageButtons = this.MaxPageButtons,
            ShowPageSizeSelector = this.ShowPageSizeSelector
        };
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object
    /// </summary>
    /// <param name="obj">The object to compare with the current object</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not QuarkTableOptions other)
            return false;

        return Sortable == other.Sortable &&
               DefaultPageSize == other.DefaultPageSize &&
               SearchDebounceMs == other.SearchDebounceMs &&
               SearchPosition == other.SearchPosition &&
               MaxPageButtons == other.MaxPageButtons &&
               ShowPageSizeSelector == other.ShowPageSizeSelector &&
               (PageSizeOptions == null && other.PageSizeOptions == null ||
                PageSizeOptions != null && other.PageSizeOptions != null &&
                PageSizeOptions.SequenceEqual(other.PageSizeOptions));
    }

    /// <summary>
    /// Serves as the default hash function
    /// </summary>
    /// <returns>A hash code for the current object</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + Sortable.GetHashCode();
            hash = hash * 23 + DefaultPageSize.GetHashCode();
            hash = hash * 23 + SearchDebounceMs.GetHashCode();
            hash = hash * 23 + SearchPosition.GetHashCode();
            hash = hash * 23 + MaxPageButtons.GetHashCode();
            hash = hash * 23 + ShowPageSizeSelector.GetHashCode();
            hash = hash * 23 + (PageSizeOptions != null ? string.Join(",", PageSizeOptions).GetHashCode() : 0);
            return hash;
        }
    }
} 