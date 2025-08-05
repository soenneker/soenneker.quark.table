namespace Soenneker.Quark.Table.Enums;

/// <summary>
/// Defines the layout options for QuarkTableControls component
/// </summary>
public enum QuarkTableControlsLayout
{
    /// <summary>
    /// Info on the left, pagination on the right (default)
    /// </summary>
    InfoLeftPaginationRight,
    
    /// <summary>
    /// Info on the right, pagination on the left
    /// </summary>
    InfoRightPaginationLeft,
    
    /// <summary>
    /// Both info and pagination centered
    /// </summary>
    Centered,
    
    /// <summary>
    /// Info and pagination stacked vertically
    /// </summary>
    Stacked
} 