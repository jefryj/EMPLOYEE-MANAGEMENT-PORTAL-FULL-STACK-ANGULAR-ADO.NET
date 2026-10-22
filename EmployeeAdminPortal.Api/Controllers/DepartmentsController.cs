using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EmployeeAdminPortal.Api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeAdminPortal.Api.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        this.departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments()
    {
        var departments = await departmentService.GetDepartmentsAsync();

        return Ok(departments);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDepartmentById(int id)
    {
        var department = await departmentService.GetByIdAsync(id);

        if (department is null)
        {
            return NotFound();
        }

        return Ok(department);
    }
    [Authorize(Roles = "1")]
    [HttpPost]
    public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentDto dto)
    {
        await departmentService.AddAsync(dto);

        return Ok(new
        {
            Message = "Department added successfully."
        });
    }
    [Authorize(Roles = "1")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDepartment(int id, UpdateDepartmentDto dto)
    {
        await departmentService.UpdateAsync(id, dto);

        return Ok(new
        {
            Message = "Department updated successfully."
        });
    }
    [Authorize(Roles = "1")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        await departmentService.DeleteAsync(id);

        return Ok(new
        {
            Message = "Department deleted successfully."
        });
    }
}