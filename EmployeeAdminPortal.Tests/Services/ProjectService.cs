using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using EmployeeAdminPortal.Api.Services;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Microsoft.Extensions.Logging;

namespace EmployeeAdminPortal.Tests.Services;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> projectRepositoryMock;
    private readonly Mock<IAuditLogRepository> auditLogRepositoryMock;
    private readonly Mock<ICurrentUserService> currentUserServiceMock;
    private readonly IMemoryCache memoryCache;

    private readonly Mock<ILogger<ProjectService>> loggerMock;

    private readonly ProjectService projectService;

    public ProjectServiceTests()
    {
        projectRepositoryMock = new Mock<IProjectRepository>();
        auditLogRepositoryMock = new Mock<IAuditLogRepository>();
        currentUserServiceMock = new Mock<ICurrentUserService>();

        memoryCache = new MemoryCache(new MemoryCacheOptions());
        loggerMock = new Mock<ILogger<ProjectService>>();

        currentUserServiceMock.Setup(x => x.UserId).Returns(1);

        currentUserServiceMock.Setup(x => x.UserName).Returns("admin@test.com");

        projectService = new ProjectService(projectRepositoryMock.Object,auditLogRepositoryMock.Object,
        currentUserServiceMock.Object,memoryCache,loggerMock.Object);
    }
    [Fact]
    public async Task GetByIdAsync_ReturnsProject_WhenProjectExists()
    {
        var project = new Project
        {
            Id = 1,
            ProjectName = "Portal"
        };

        projectRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(project);

        var result = await projectService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Portal", result.ProjectName);
    }
    [Fact]
    public async Task GetProjectsAsync_ReturnsProjects()
    {
        var projects = new List<Project>
        {
            new()
            {
                Id = 1,
                ProjectName = "Portal"
            }
        };

        projectRepositoryMock.Setup(x => x.GetProjectsAsync()).ReturnsAsync(projects);

        var result = await projectService.GetProjectsAsync();

        Assert.Single(result);
    }
    [Fact]
    public async Task AddAsync_ThrowsInvalidOperationException_WhenProjectAlreadyExists()
    {
        var dto = new AddProjectDto
        {
            ProjectName = "Portal"
        };

        projectRepositoryMock.Setup(x => x.ExistsByNameAsync("Portal")).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => projectService.AddAsync(dto));
    }
    [Fact]
    public async Task AddAsync_AddsProjectSuccessfully()
    {
        var dto = new AddProjectDto
        {
            ProjectName = "Portal"
        };

        projectRepositoryMock.Setup(x => x.ExistsByNameAsync("Portal")).ReturnsAsync(false);

        await projectService.AddAsync(dto);

        projectRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Project>()),Times.Once);

        auditLogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<AuditLog>()),Times.Once);
    }
    [Fact]
    public async Task UpdateAsync_UpdatesProjectSuccessfully()
    {
        var project = new Project
        {
            Id = 1,
            ProjectName = "Old Project"
        };

        projectRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(project);

        var dto = new UpdateProjectDto
        {
            ProjectName = "New Project"
        };

        await projectService.UpdateAsync(1, dto);

        projectRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Project>()),Times.Once);

        auditLogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<AuditLog>()),Times.Once);
    }
    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenProjectNotFound()
    {
        projectRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Project?)null);

        var dto = new UpdateProjectDto
        {
            ProjectName = "New Project"
        };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => projectService.UpdateAsync(1, dto));
    }
    [Fact]
    public async Task DeleteAsync_DeletesProjectSuccessfully()
    {
        var project = new Project
        {
            Id = 1,
            ProjectName = "Portal"
        };

        projectRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(project);

        await projectService.DeleteAsync(1);

        projectRepositoryMock.Verify(x => x.DeleteAsync(1),Times.Once);

        auditLogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<AuditLog>()),Times.Once);
    }
    [Fact]
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenProjectNotFound()
    {
        projectRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Project?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => projectService.DeleteAsync(1));
    }
}