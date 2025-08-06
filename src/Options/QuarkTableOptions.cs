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
    /// Gets or sets the default page size
    /// </summary>
    [JsonPropertyName("defaultPageSize")]
    public int DefaultPageSize { get; set; } = 10;



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
    /// Gets or sets whether to enable debug logging
    /// </summary>
    [JsonPropertyName("debug")]
    public bool Debug { get; set; } = false;

    /// <summary>
    /// Creates a clone of the current options
    /// </summary>
    /// <returns>A new instance with the same values</returns>
    public QuarkTableOptions Clone()
    {
        return new QuarkTableOptions
        {
            DefaultPageSize = this.DefaultPageSize,
            SearchDebounceMs = this.SearchDebounceMs,
            SearchPosition = this.SearchPosition,
            Debug = this.Debug
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

        return DefaultPageSize == other.DefaultPageSize &&
               SearchDebounceMs == other.SearchDebounceMs &&
               SearchPosition == other.SearchPosition &&
               Debug == other.Debug;
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
            hash = hash * 23 + DefaultPageSize.GetHashCode();
            hash = hash * 23 + SearchDebounceMs.GetHashCode();
            hash = hash * 23 + SearchPosition.GetHashCode();
            hash = hash * 23 + Debug.GetHashCode();
            return hash;
        }
    }
} 