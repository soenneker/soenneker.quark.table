using System.Collections.Generic;
using Soenneker.Quark.Table.Dtos;

namespace Soenneker.Quark.Table.Options;

/// <summary>
/// Event arguments for QuarkTable order events
/// </summary>
public sealed class QuarkTableOrderEventArgs
{
    /// <summary>
    /// Gets or sets the column name that was ordered
    /// </summary>
    public string? Column { get; set; }

    /// <summary>
    /// Gets or sets the direction of ordering (asc, desc, none)
    /// </summary>
    public string? Direction { get; set; }

    /// <summary>
    /// Gets or sets the current list of all orders
    /// </summary>
    public List<QuarkTableOrder>? Orders { get; set; }
} 