using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using EmployeeAdminPortal.Api.Services;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Microsoft.Extensions.Logging;
namespace EmployeeAdminPortal.Tests.Services;

public class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> departmentRepositoryMock;
    private readonly Mock<IAuditLogRepository> auditLogRepositoryMock;
    private readonly Mock<ICurrentUserService> currentUserServiceMock;
    private readonly IMemoryCache memoryCache;

    private readonly Mock<ILogger<DepartmentService>> loggerMock;

    private readonly DepartmentService departmentService;

    public DepartmentServiceTests()
    {
        departmentRepositoryMock = new Mock<IDepartmentRepository>();
        auditLogRepositoryMock = new Mock<IAuditLogRepository>();
        currentUserServiceMock = new Mock<ICurrentUserService>();

        memoryCache = new MemoryCache( new MemoryCacheOptions());
        loggerMock = new Mock<ILogger<DepartmentService>>();

        currentUserServiceMock.Setup(x => x.UserId).Returns(1);

        currentUserServiceMock.Setup(x => x.UserName).Returns("admin@test.com");

        departmentService = new DepartmentService(departmentRepositoryMock.Object,auditLogRepositoryMock.Object,
            currentUserServiceMock.Object,memoryCache,loggerMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsDepartment_WhenDepartmentExists()
    {
        var department = new Department
        {
            Id = 1,
            DepartmentName = "IT"
        };

        departmentRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(department);

        var result =await departmentService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("IT", result.DepartmentName);
    }
    [Fact]
    public async Task GetDepartmentsAsync_ReturnsDepartments()
    {
        var departments = new List<Department>
        {
            new()
            {
                Id = 1,
                DepartmentName = "IT"
            }
        };

        departmentRepositoryMock.Setup(x => x.GetDepartmentsAsync()).ReturnsAsync(departments);

        var result =await departmentService.GetDepartmentsAsync();

        Assert.Single(result);
    }
    [Fact]
    public async Task AddAsync_ThrowsInvalidOperationException_WhenDepartmentAlreadyExists()
    {
        var dto = new AddDepartmentDto
        {
            DepartmentName = "IT"
        };

        departmentRepositoryMock.Setup(x => x.ExistsByNameAsync("IT")).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => departmentService.AddAsync(dto));
    }
    [Fact]
    public async Task AddAsync_AddsDepartmentSuccessfully()
    {
        var dto = new AddDepartmentDto
        {
            DepartmentName = "IT"
        };

        departmentRepositoryMock.Setup(x => x.ExistsByNameAsync("IT")).ReturnsAsync(false);

        await departmentService.AddAsync(dto);

        departmentRepositoryMock.Verify( x => x.AddAsync(It.IsAny<Department>()), Times.Once);

        auditLogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<AuditLog>()), Times.Once);
    }
    [Fact]
    public async Task UpdateAsync_UpdatesDepartmentSuccessfully()
    {
        var department = new Department
        {
            Id = 1,
            DepartmentName = "IT"
        };

        departmentRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(department);

        var dto = new UpdateDepartmentDto
        {
            DepartmentName = "Development"
        };

        await departmentService.UpdateAsync(1, dto);

        departmentRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Department>()),Times.Once);

        auditLogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<AuditLog>()),Times.Once);
    }
    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenDepartmentNotFound()
    {
        departmentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Department?)null);

        var dto = new UpdateDepartmentDto
        {
            DepartmentName = "Development"
        };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => departmentService.UpdateAsync(1, dto));
    }
    [Fact]
    public async Task DeleteAsync_DeletesDepartmentSuccessfully()
    {
        var department = new Department
        {
            Id = 1,
            DepartmentName = "IT"
        };

        departmentRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(department);

        await departmentService.DeleteAsync(1);

        departmentRepositoryMock.Verify( x => x.DeleteAsync(1), Times.Once);

        auditLogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<AuditLog>()),Times.Once);
    }
    [Fact]
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenDepartmentNotFound()
    {
        departmentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Department?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => departmentService.DeleteAsync(1));
    }
}