using EmployeeAdminPortal.Api.Models.Configuration;
using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using EmployeeAdminPortal.Api.Services;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Microsoft.Extensions.Logging;

namespace EmployeeAdminPortal.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> authRepositoryMock;
    private readonly Mock<IAuditLogRepository> auditLogRepositoryMock;
    private readonly Mock<ICurrentUserService> currentUserServiceMock;

    private readonly Mock<IEmployeeRepository> employeeRepositoryMock;

    private readonly Mock<ILogger<AuthService>> loggerMock;
    private readonly AuthService authService;

    public AuthServiceTests()
    {
        authRepositoryMock = new Mock<IAuthRepository>();
        employeeRepositoryMock = new Mock<IEmployeeRepository>();
        auditLogRepositoryMock = new Mock<IAuditLogRepository>();
        currentUserServiceMock = new Mock<ICurrentUserService>();

        currentUserServiceMock.Setup(x => x.UserId).Returns(1);

        currentUserServiceMock.Setup(x => x.UserName).Returns("admin@test.com");

        var jwtSettings = Options.Create(new JwtSettings
        {
            Key = "ThisIsATestKeyWithAtLeastThirtyTwoCharacters",
            Issuer = "TestIssuer",
            Audience = "TestAudience"
        });

        loggerMock = new Mock<ILogger<AuthService>>();
        authService = new AuthService(authRepositoryMock.Object,jwtSettings,auditLogRepositoryMock.Object,currentUserServiceMock.Object, loggerMock.Object, employeeRepositoryMock.Object);
    }
    [Fact]
    public async Task LoginAsync_ReturnsToken_WhenCredentialsAreValid()
    {
        var employee = new Employee
        {
            Id = 1,
            Email = "jefry@test.com",
            RoleId = 1
        };

        var hasher = new PasswordHasher<Employee>();

        employee.PasswordHash = hasher.HashPassword(employee, "Password123");

        authRepositoryMock.Setup(x => x.GetEmployeeByEmailAsync(employee.Email)).ReturnsAsync(employee);

        var result = await authService.LoginAsync(
            new LoginDto
            {
                Email = employee.Email,
                Password = "Password123"
            });

        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }
    [Fact]
    public async Task LoginAsync_ThrowsUnauthorizedAccessException_WhenEmployeeNotFound()
    {
        authRepositoryMock.Setup(x => x.GetEmployeeByEmailAsync(It.IsAny<string>())).ReturnsAsync((Employee?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => authService.LoginAsync(new LoginDto
                {
                    Email = "missing@test.com",
                    Password = "Password123"
                }));
    }
    [Fact]
    public async Task LoginAsync_ThrowsUnauthorizedAccessException_WhenPasswordIsIncorrect()
    {
        var employee = new Employee
        {
            Id = 1,
            Email = "jefry@test.com",
            RoleId = 1
        };

        var hasher = new PasswordHasher<Employee>();

        employee.PasswordHash = hasher.HashPassword(employee, "CorrectPassword");

        authRepositoryMock.Setup(x => x.GetEmployeeByEmailAsync(employee.Email)).ReturnsAsync(employee);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => authService.LoginAsync(new LoginDto
                {
                    Email = employee.Email,
                    Password = "WrongPassword"
                }));
    }
    [Fact]
    public async Task ChangePasswordAsync_ChangesPasswordSuccessfully()
    {
        var employee = new Employee
        {
            Id = 1,
            Email = "jefry@test.com"
        };

        var hasher = new PasswordHasher<Employee>();

        employee.PasswordHash = hasher.HashPassword(employee, "OldPassword");

        employeeRepositoryMock.Setup(x => x.GetByIdAsync(employee.Id)).ReturnsAsync(employee);

        await authService.ChangePasswordAsync(
            new ChangePasswordDto
            {
                EmployeeId = employee.Id,
                CurrentPassword = "OldPassword",
                NewPassword = "NewPassword"
            });

        employeeRepositoryMock.Verify(x => x.GetByIdAsync(employee.Id), Times.Once);

        authRepositoryMock.Verify(x => x.ChangePasswordAsync(employee.Id,It.IsAny<string>()),Times.Once);

        auditLogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<AuditLog>()),Times.Once);
    }
    [Fact]
    public async Task ChangePasswordAsync_ThrowsKeyNotFoundException_WhenEmployeeNotFound()
    {
        employeeRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Employee?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => authService.ChangePasswordAsync(new ChangePasswordDto
        {
            EmployeeId = 1
        }));

        employeeRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task ChangePasswordAsync_ThrowsUnauthorizedAccessException_WhenCurrentPasswordIsWrong()
    {
        var employee = new Employee
        {
            Id = 1,
            Email = "jefry@test.com"
        };

        var hasher = new PasswordHasher<Employee>();

        employee.PasswordHash = hasher.HashPassword(employee, "CorrectPassword");

        employeeRepositoryMock.Setup(x => x.GetByIdAsync(employee.Id)).ReturnsAsync(employee);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => authService.ChangePasswordAsync(
                new ChangePasswordDto
                {
                    EmployeeId = employee.Id,
                    CurrentPassword = "WrongPassword",
                    NewPassword = "NewPassword"
                }));

        employeeRepositoryMock.Verify(x => x.GetByIdAsync(employee.Id), Times.Once);
    }
}