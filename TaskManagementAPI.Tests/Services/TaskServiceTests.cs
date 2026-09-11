using Moq;
using Xunit;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories;
using TaskManagementAPI.Services;
using TaskStatus = TaskManagementAPI.Models.TaskStatus;

namespace TaskManagementAPI.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _taskService = new TaskService(_taskRepositoryMock.Object);
    }

    [Fact]
    public async Task GetTasksForUserAsync_ShouldReturnOnlyUserTasks()
    {
        // Arrange
        int userId = 10;
        var sampleTasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Task 1", Status = TaskStatus.Pending, Priority = TaskPriority.High, UserId = userId },
            new TaskItem { Id = 2, Title = "Task 2", Status = TaskStatus.InProgress, Priority = TaskPriority.Medium, UserId = userId }
        };

        _taskRepositoryMock
            .Setup(r => r.GetAllByUserIdAsync(userId))
            .ReturnsAsync(sampleTasks);

        // Act
        var result = (await _taskService.GetTasksForUserAsync(userId)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Task 1", result[0].Title);
        Assert.Equal("Pending", result[0].Status);
        Assert.Equal("High", result[0].Priority);
        Assert.Equal("InProgress", result[1].Status);
    }

    [Fact]
    public async Task GetTaskByIdForUserAsync_ShouldReturnTask_WhenTaskBelongsToUser()
    {
        // Arrange
        int taskId = 1;
        int userId = 10;
        var sampleTask = new TaskItem
        {
            Id = taskId,
            Title = "Single Task",
            Description = "Details",
            Status = TaskStatus.Completed,
            Priority = TaskPriority.Low,
            UserId = userId
        };

        _taskRepositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(taskId, userId))
            .ReturnsAsync(sampleTask);

        // Act
        var result = await _taskService.GetTaskByIdForUserAsync(taskId, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(taskId, result.Id);
        Assert.Equal("Completed", result.Status);
    }

    [Fact]
    public async Task GetTaskByIdForUserAsync_ShouldReturnNull_WhenTaskDoesNotExistOrBelongsToAnotherUser()
    {
        // Arrange
        int taskId = 99;
        int userId = 10;

        _taskRepositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(taskId, userId))
            .ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _taskService.GetTaskByIdForUserAsync(taskId, userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateTaskForUserAsync_ShouldCreateAndReturnTask()
    {
        // Arrange
        int userId = 10;
        var dto = new CreateTaskDto
        {
            Title = "New Task",
            Description = "New Description",
            Status = TaskStatus.Pending,
            Priority = TaskPriority.High
        };

        var createdTask = new TaskItem
        {
            Id = 101,
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _taskRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync(createdTask);

        // Act
        var result = await _taskService.CreateTaskForUserAsync(dto, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(101, result.Id);
        Assert.Equal("New Task", result.Title);
        Assert.Equal("High", result.Priority);
    }

    [Fact]
    public async Task UpdateTaskForUserAsync_ShouldUpdate_WhenTaskBelongsToUser()
    {
        // Arrange
        int taskId = 5;
        int userId = 10;
        var existingTask = new TaskItem
        {
            Id = taskId,
            Title = "Old Title",
            Description = "Old Desc",
            Status = TaskStatus.Pending,
            Priority = TaskPriority.Low,
            UserId = userId
        };

        var dto = new UpdateTaskDto
        {
            Title = "Updated Title",
            Description = "Updated Desc",
            Status = TaskStatus.Completed,
            Priority = TaskPriority.High
        };

        _taskRepositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(taskId, userId))
            .ReturnsAsync(existingTask);

        _taskRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem t) => t);

        // Act
        var result = await _taskService.UpdateTaskForUserAsync(taskId, dto, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("Completed", result.Status);
        Assert.Equal("High", result.Priority);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task DeleteTaskForUserAsync_ShouldReturnTrue_WhenTaskDeleted()
    {
        // Arrange
        int taskId = 5;
        int userId = 10;

        _taskRepositoryMock
            .Setup(r => r.DeleteAsync(taskId, userId))
            .ReturnsAsync(true);

        // Act
        var result = await _taskService.DeleteTaskForUserAsync(taskId, userId);

        // Assert
        Assert.True(result);
    }
}
