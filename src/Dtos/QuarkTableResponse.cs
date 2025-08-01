using System.Text.Json.Serialization;

namespace Soenneker.Quark.Table.Dtos;

/// <summary>
/// Represents a response for QuarkTable server-side processing
/// </summary>
public sealed class QuarkTableResponse
{
    /// <summary>
    /// Gets or sets the draw counter that QuarkTable is expecting back from the server
    /// </summary>
    [JsonPropertyName("draw")]
    public int Draw { get; set; }

    /// <summary>
    /// Gets or sets the total number of records before filtering
    /// </summary>
    [JsonPropertyName("totalRecords")]
    public int TotalRecords { get; set; }

    /// <summary>
    /// Gets or sets the total number of records after filtering
    /// </summary>
    [JsonPropertyName("totalFilteredRecords")]
    public int TotalFilteredRecords { get; set; }

    /// <summary>
    /// Gets or sets the data to be displayed in the table
    /// </summary>
    [JsonPropertyName("data")]
    public object? Data { get; set; }

    /// <summary>
    /// Gets or sets an optional error message to be displayed by QuarkTable
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    /// <summary>
    /// If applicable, a storage continuation token that the client must send back
    /// on the next request. Typically <c>null</c> when the current page is the last page. Optional.
    /// </summary>
    [JsonPropertyName("continuationToken")]
    public string? ContinuationToken { get; set; }

    /// <summary>
    /// Creates a success response for QuarkTable server-side processing
    /// </summary>
    /// <param name="draw">The draw counter from the request</param>
    /// <param name="totalRecords">Total number of records before filtering</param>
    /// <param name="totalFilteredRecords">Total number of records after filtering</param>
    /// <param name="data">The data to be displayed</param>
    /// <param name="continuationToken">Optional continuation token for pagination</param>
    /// <returns>A configured QuarkTableResponse</returns>
    public static QuarkTableResponse Success(int draw, int totalRecords, int totalFilteredRecords, object data, string? continuationToken = null)
    {
        return new QuarkTableResponse
        {
            Draw = draw,
            TotalRecords = totalRecords,
            TotalFilteredRecords = totalFilteredRecords,
            Data = data,
            ContinuationToken = continuationToken
        };
    }

    /// <summary>
    /// Creates an error response for QuarkTable server-side processing
    /// </summary>
    /// <param name="draw">The draw counter from the request</param>
    /// <param name="errorMessage">The error message to display</param>
    /// <returns>A configured QuarkTableResponse with the error message</returns>
    public static QuarkTableResponse Fail(int draw, string errorMessage)
    {
        return new QuarkTableResponse
        {
            Draw = draw,
            Error = errorMessage
        };
    }
} 