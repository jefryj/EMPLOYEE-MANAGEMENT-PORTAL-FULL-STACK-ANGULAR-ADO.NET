using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EmployeeAdminPortal.Api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
namespace EmployeeAdminPortal.Api.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService projectService;

    public ProjectsController(IProjectService projectService)
    {
        this.projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await projectService.GetProjectsAsync();

        return Ok(projects);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        var project = await projectService.GetByIdAsync(id);

        if (project is null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    [Authorize(Roles = "1")]
    [HttpPost]
    public async Task<IActionResult> AddProject([FromBody] AddProjectDto dto)
    {
        await projectService.AddAsync(dto);

        return Ok();
    }

    [Authorize(Roles = "1")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProject(int id, UpdateProjectDto dto)
    {
        await projectService.UpdateAsync(id, dto);

        return Ok(new
        {
            Message = "Project updated successfully."
        });
    }

    [Authorize(Roles = "1")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        await projectService.DeleteAsync(id);

        return Ok(new
        {
            Message = "Project deleted successfully."
        });
    }
}