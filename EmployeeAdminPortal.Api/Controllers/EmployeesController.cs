using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EmployeeAdminPortal.Api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeAdminPortal.Api.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService employeeService;

    public EmployeesController( IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees([FromQuery] EmployeeSearchDto searchDto)
    {
        var employees =  await employeeService.GetEmployeesAsync(searchDto);

        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEmployeeById( int id)
    {
        var employee =  await employeeService.GetByIdAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [Authorize(Roles = "1")]
    [HttpPost]
    public async Task<IActionResult> AddEmployee(AddEmployeeDto dto)
    {
        await employeeService.AddAsync(dto);
        return Ok();
    }

    [Authorize(Roles = "1")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEmployee(int id,UpdateEmployeeDto dto)
    {
        await employeeService.UpdateAsync(id, dto);

        return Ok(new
{
    Message = "Employee updated successfully."
});
    }

    [Authorize(Roles = "1")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        await employeeService.DeleteAsync(id);

        return Ok(new
        {
            Message = "Employee deleted successfully."
        });
    }
}