using System.Text.Json.Serialization;

namespace Soenneker.Quark.Table.Dtos;

/// <summary>
/// Represents search parameters for QuarkTable
/// </summary>
public sealed class QuarkTableSearch
{
    /// <summary>
    /// Gets or sets the search value
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets whether the search is regex-based
    /// </summary>
    [JsonPropertyName("regex")]
    public bool Regex { get; set; }

    /// <summary>
    /// Gets or sets whether the search is case-sensitive
    /// </summary>
    [JsonPropertyName("caseInsensitive")]
    public bool CaseInsensitive { get; set; } = true;
} 