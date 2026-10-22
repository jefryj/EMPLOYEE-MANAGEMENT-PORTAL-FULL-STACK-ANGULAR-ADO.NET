using EmployeeAdminPortal.Api.Controllers;
using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeAdminPortal.Tests.Controllers;

public class ProjectsControllerTests
{
    private readonly Mock<IProjectService> projectServiceMock;
    private readonly ProjectsController controller;

    public ProjectsControllerTests()
    {
        projectServiceMock = new Mock<IProjectService>();

        controller = new ProjectsController(projectServiceMock.Object);
    }
    [Fact]
    public async Task GetProjects_ReturnsProjects()
    {
        var projects = new List<Project>
        {
            new()
            {
                Id = 1,
                ProjectName = "Portal"
            }
        };

        projectServiceMock.Setup(x => x.GetProjectsAsync()).ReturnsAsync(projects);

        var result = await controller.GetProjects();

        var okResult = Assert.IsType<OkObjectResult>(result);

        var returnedProjects = Assert.IsAssignableFrom<List<Project>>(okResult.Value);

        Assert.Single(returnedProjects);
    }
    [Fact]
    public async Task GetProjectById_ReturnsProject_WhenProjectExists()
    {
        var project = new Project
        {
            Id = 1,
            ProjectName = "Portal"
        };

        projectServiceMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(project);

        var result = await controller.GetProjectById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);

        var returnedProject = Assert.IsType<Project>(okResult.Value);

        Assert.Equal(1, returnedProject.Id);
    }
    [Fact]
    public async Task UpdateProject_ReturnsOkObjectResult()
    {
        var dto = new UpdateProjectDto
        {
            ProjectName = "Updated Project"
        };

        var result = await controller.UpdateProject(1, dto);

        Assert.IsType<OkObjectResult>(result);
    }
    [Fact]
    public async Task DeleteProject_ReturnsOkObjectResult()
    {
        var result = await controller.DeleteProject(1);

        Assert.IsType<OkObjectResult>(result);
    }
    [Fact]
    public async Task UpdateProject_CallsService()
    {
        var dto = new UpdateProjectDto
        {
            ProjectName = "Updated Project"
        };

        await controller.UpdateProject(1, dto);

        projectServiceMock.Verify(x => x.UpdateAsync(1, dto),Times.Once);
    }
    [Fact]
    public async Task DeleteProject_CallsService()
    {
        await controller.DeleteProject(1);

        projectServiceMock.Verify(x => x.DeleteAsync(1),Times.Once);
    }
}