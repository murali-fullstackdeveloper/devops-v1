using Moq;
using Xunit;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Helpers;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        _authService = new AuthService(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _jwtTokenGeneratorMock.Object
        );
    }

    [Fact]
    public async Task RegisterAsync_ShouldCreateUserAndReturnToken_WhenValidDtoProvided()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Name = "John Doe",
            Email = "john@example.com",
            Password = "Password123!"
        };

        _userRepositoryMock
            .Setup(r => r.ExistsByEmailAsync(dto.Email))
            .ReturnsAsync(false);

        _passwordHasherMock
            .Setup(h => h.HashPassword(dto.Password))
            .Returns("hashed_password_123");

        var createdUser = new User
        {
            Id = 1,
            Name = dto.Name,
            Email = dto.Email.ToLower(),
            PasswordHash = "hashed_password_123",
            CreatedAt = DateTime.UtcNow
        };

        _userRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(createdUser);

        _jwtTokenGeneratorMock
            .Setup(g => g.GenerateToken(createdUser))
            .Returns(("jwt_token_sample", DateTime.UtcNow.AddMinutes(60)));

        // Act
        var result = await _authService.RegisterAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("jwt_token_sample", result.Token);
        Assert.Equal(1, result.User.Id);
        Assert.Equal("john@example.com", result.User.Email);
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowInvalidOperationException_WhenEmailAlreadyExists()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Name = "John Doe",
            Email = "existing@example.com",
            Password = "Password123!"
        };

        _userRepositoryMock
            .Setup(r => r.ExistsByEmailAsync(dto.Email))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(dto));
        Assert.Contains("already exists", exception.Message);
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "user@example.com",
            Password = "Password123!"
        };

        var existingUser = new User
        {
            Id = 5,
            Name = "Valid User",
            Email = "user@example.com",
            PasswordHash = "correct_hashed_pass"
        };

        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(existingUser);

        _passwordHasherMock
            .Setup(h => h.VerifyPassword(dto.Password, existingUser.PasswordHash))
            .Returns(true);

        _jwtTokenGeneratorMock
            .Setup(g => g.GenerateToken(existingUser))
            .Returns(("valid_jwt_token", DateTime.UtcNow.AddMinutes(60)));

        // Act
        var result = await _authService.LoginAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("valid_jwt_token", result.Token);
        Assert.Equal(5, result.User.Id);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowUnauthorizedAccessException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "user@example.com",
            Password = "WrongPassword"
        };

        var existingUser = new User
        {
            Id = 5,
            Name = "Valid User",
            Email = "user@example.com",
            PasswordHash = "correct_hashed_pass"
        };

        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(existingUser);

        _passwordHasherMock
            .Setup(h => h.VerifyPassword(dto.Password, existingUser.PasswordHash))
            .Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(dto));
        Assert.Equal("Invalid email or password.", exception.Message);
    }
}
