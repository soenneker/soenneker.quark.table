using System.Text.Json.Serialization;

namespace Soenneker.Quark.Table.Dtos;

/// <summary>
/// Represents ordering parameters for QuarkTable
/// </summary>
public sealed class QuarkTableOrder
{
    /// <summary>
    /// Gets or sets the column name to order by
    /// </summary>
    [JsonPropertyName("column")]
    public string? Column { get; set; }

    /// <summary>
    /// Gets or sets the direction of ordering (asc, desc)
    /// </summary>
    [JsonPropertyName("direction")]
    public string? Direction { get; set; }
} 