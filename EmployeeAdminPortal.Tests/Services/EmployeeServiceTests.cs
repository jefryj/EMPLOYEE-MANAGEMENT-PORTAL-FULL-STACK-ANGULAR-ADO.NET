using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using EmployeeAdminPortal.Api.Services;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Microsoft.Extensions.Logging;

namespace EmployeeAdminPortal.Tests.Services;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> employeeRepositoryMock;
    private readonly Mock<IAuditLogRepository> auditLogRepositoryMock;
    private readonly Mock<ICurrentUserService> currentUserServiceMock;
    private readonly IMemoryCache memoryCache;
    private readonly Mock<ILogger<EmployeeService>> loggerMock;

    private readonly EmployeeService employeeService;
    

    public EmployeeServiceTests()
    {
        employeeRepositoryMock = new Mock<IEmployeeRepository>();
        auditLogRepositoryMock = new Mock<IAuditLogRepository>();
        currentUserServiceMock = new Mock<ICurrentUserService>();
        memoryCache = new MemoryCache(new MemoryCacheOptions());
        loggerMock = new Mock<ILogger<EmployeeService>>();

        currentUserServiceMock.Setup(x => x.UserId).Returns(1);

        currentUserServiceMock.Setup(x => x.UserName).Returns("admin@test.com");

        employeeService = new EmployeeService(employeeRepositoryMock.Object,auditLogRepositoryMock.Object,
        currentUserServiceMock.Object,memoryCache,loggerMock.Object);

    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEmployee_WhenEmployeeExists()
    {
            
            var employee = new Employee
            {
                Id = 1,
                Name = "Jefry",
                Email = "jefry@test.com"
            };

            employeeRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(employee);

            
            var result = await employeeService.GetByIdAsync(1);

            
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Jefry", result.Name);
    }
    [Fact]
    public async Task GetEmployeesAsync_ReturnsEmployees()
    {

        var employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                Name = "Jefry",
                Email = "jefry@test.com"
            },
            new Employee
            {
                Id = 2,
                Name = "John",
                Email = "john@test.com"
            }
        };

        employeeRepositoryMock.Setup(x => x.GetEmployeesAsync(It.IsAny<EmployeeSearchDto>())).ReturnsAsync(employees);

        var result = await employeeService.GetEmployeesAsync(new EmployeeSearchDto());

        Assert.Equal(2, result.Count);
    }
    [Fact]
    public async Task AddAsync_ThrowsInvalidOperationException_WhenEmailAlreadyExists()
    {
        
        var dto = new AddEmployeeDto
        {
            Name = "Jefry",
            Email = "jefry@test.com",
            Phone = "1234567890",
            Salary = 50000,
            DepartmentId = 1,
            Password = "Password@123",
            RoleId = 1
        };

        employeeRepositoryMock.Setup(x => x.AnyByEmailAsync(dto.Email)).ReturnsAsync(true);

        
        await Assert.ThrowsAsync<InvalidOperationException>(() => employeeService.AddAsync(dto));
    }
    [Fact]
    public async Task AddAsync_AddsEmployeeSuccessfully()
    {
        var dto = new AddEmployeeDto
        {
            Name = "Jefry",
            Email = "jefry@test.com",
            Phone = "1234567890",
            Salary = 50000,
            DepartmentId = 1,
            Password = "Password@123",
            RoleId = 1
        };

        employeeRepositoryMock.Setup(x => x.AnyByEmailAsync(dto.Email)).ReturnsAsync(false);

        await employeeService.AddAsync(dto);

        employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>()),Times.Once);

        auditLogRepositoryMock.Verify( x => x.AddAsync(It.IsAny<AuditLog>()),Times.Once);
    }
    [Fact]
    public async Task UpdateAsync_UpdatesEmployeeSuccessfully()
    {

        var employee = new Employee
        {
            Id = 1,
            Name = "Old Name",
            Email = "old@test.com"
        };

        employeeRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(employee);

        var dto = new UpdateEmployeeDto
        {
            Name = "New Name",
            Email = "new@test.com",
            Phone = "1234567890",
            Salary = 50000,
            DepartmentId = 1
        };

        await employeeService.UpdateAsync(1, dto);

        employeeRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Employee>()),Times.Once);

        auditLogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<AuditLog>()),Times.Once);
    }
    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenEmployeeNotFound()
    {
        employeeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Employee?)null);

        var dto = new UpdateEmployeeDto
        {
            Name = "Test",
            Email = "test@test.com",
            Phone = "1234567890",
            Salary = 50000,
            DepartmentId = 1
        };

        await Assert.ThrowsAsync<KeyNotFoundException>( () => employeeService.UpdateAsync(1, dto));
    }
    [Fact]
    public async Task DeleteAsync_DeletesEmployeeSuccessfully()
    {
        var employee = new Employee
        {
            Id = 1,
            Name = "Jefry",
            Email = "jefry@test.com"
        };

        employeeRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(employee);

        await employeeService.DeleteAsync(1);

        employeeRepositoryMock.Verify( x => x.DeleteAsync(1),Times.Once);

        auditLogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<AuditLog>()),Times.Once);
    }
}
