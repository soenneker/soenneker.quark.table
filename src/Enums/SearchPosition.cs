using Intellenum;

namespace Soenneker.Quark.Table.Enums;

/// <summary>
/// Defines the possible positions for the search box
/// </summary>
[Intellenum<string>]
public partial class SearchPosition
{
    /// <summary>
    /// Position the search box at the start (left side)
    /// </summary>
    public static readonly SearchPosition Start = new(nameof(Start));

    /// <summary>
    /// Position the search box in the center
    /// </summary>
    public static readonly SearchPosition Center = new(nameof(Center));

    /// <summary>
    /// Position the search box at the end (right side)
    /// </summary>
    public static readonly SearchPosition End = new(nameof(End));
} 