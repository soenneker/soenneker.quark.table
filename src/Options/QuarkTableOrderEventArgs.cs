using Soenneker.DataTables.Dtos.ServerSideRequest;
using System.Collections.Generic;

namespace Soenneker.Quark;

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
    public List<DataTableOrderRequest>? Orders { get; set; }
} 
