using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Retrieve all tasks owned by the authenticated user.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        int userId = GetCurrentUserId();
        var tasks = await _taskService.GetTasksForUserAsync(userId);
        return Ok(tasks);
    }

    /// <summary>
    /// Retrieve a specific task by ID owned by the authenticated user.
    /// </summary>
    /// <param name="id">Task ID</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(int id)
    {
        int userId = GetCurrentUserId();
        var task = await _taskService.GetTaskByIdForUserAsync(id, userId);
        if (task == null)
        {
            return NotFound(new { message = $"Task with ID {id} was not found." });
        }

        return Ok(task);
    }

    /// <summary>
    /// Create a new task item for the authenticated user.
    /// </summary>
    /// <param name="dto">Create task payload</param>
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int userId = GetCurrentUserId();
        var createdTask = await _taskService.CreateTaskForUserAsync(dto, userId);

        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
    }

    /// <summary>
    /// Update an existing task item owned by the authenticated user.
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <param name="dto">Update task payload</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int userId = GetCurrentUserId();
        var updatedTask = await _taskService.UpdateTaskForUserAsync(id, dto, userId);
        if (updatedTask == null)
        {
            return NotFound(new { message = $"Task with ID {id} was not found or you do not have permission to modify it." });
        }

        return Ok(updatedTask);
    }

    /// <summary>
    /// Delete a task item owned by the authenticated user.
    /// </summary>
    /// <param name="id">Task ID</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = GetCurrentUserId();
        bool success = await _taskService.DeleteTaskForUserAsync(id, userId);
        if (!success)
        {
            return NotFound(new { message = $"Task with ID {id} was not found or you do not have permission to delete it." });
        }

        return NoContent();
    }

    /// <summary>
    /// Extracts the User ID integer from JWT claim.
    /// </summary>
    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("Invalid user identity claim in token.");
        }

        return userId;
    }
}
