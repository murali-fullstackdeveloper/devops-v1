namespace TaskManagementAPI.Models;

/// <summary>
/// Represents the urgency/priority level of a task item.
/// </summary>
public enum TaskPriority
{
    /// <summary>
    /// Low priority level.
    /// </summary>
    Low = 0,

    /// <summary>
    /// Medium priority level (Default).
    /// </summary>
    Medium = 1,

    /// <summary>
    /// High priority level.
    /// </summary>
    High = 2
}
