namespace TaskManagementAPI.Models;

/// <summary>
/// Represents the status of a task item.
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// Task has been created and is pending action.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Task is currently being worked on.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Task has been completed.
    /// </summary>
    Completed = 2
}
