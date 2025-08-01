using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Quark.Table.Dtos;

/// <summary>
/// Represents a request for QuarkTable server-side processing
/// </summary>
public sealed class QuarkTableRequest
{
    /// <summary>
    /// Gets or sets the draw counter for tracking requests
    /// </summary>
    [JsonPropertyName("draw")]
    public int Draw { get; set; }

    /// <summary>
    /// Gets or sets the starting record number
    /// </summary>
    [JsonPropertyName("start")]
    public int Start { get; set; }

    /// <summary>
    /// Gets or sets the number of records to retrieve
    /// </summary>
    [JsonPropertyName("length")]
    public int Length { get; set; }

    /// <summary>
    /// Gets or sets the search parameters
    /// </summary>
    [JsonPropertyName("search")]
    public QuarkTableSearch? Search { get; set; }

    /// <summary>
    /// Gets or sets the ordering parameters
    /// </summary>
    [JsonPropertyName("order")]
    public List<QuarkTableOrder>? Order { get; set; }

    /// <summary>
    /// Gets or sets the continuation token for pagination
    /// </summary>
    [JsonPropertyName("continuationToken")]
    public string? ContinuationToken { get; set; }
} 