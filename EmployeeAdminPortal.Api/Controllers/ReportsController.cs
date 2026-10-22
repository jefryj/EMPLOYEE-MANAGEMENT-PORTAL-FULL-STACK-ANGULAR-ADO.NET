using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace EmployeeAdminPortal.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService reportService;

    public ReportsController(IReportService reportService)
    {
        this.reportService = reportService;
    }

    [HttpGet("employee-summary")]
    public async Task<IActionResult> GetEmployeeSummary()
    {
        var summary = await reportService.GetEmployeeSummaryAsync();

        return Ok(summary);
    }

    [HttpGet("department-count")]
    public async Task<IActionResult> GetDepartmentCount()
    {
        var departments = await reportService.GetDepartmentCountAsync();

        return Ok(departments);
    }

    [HttpGet("department-salary-summary")]
    public async Task<IActionResult> GetDepartmentSalarySummary()
    {
        var departments = await reportService.GetDepartmentSalarySummaryAsync();

        return Ok(departments);
    }

    [HttpGet("project-count")]
    public async Task<IActionResult> GetProjectCount()
    {
        var projects = await reportService.GetProjectCountAsync();

        return Ok(projects);
    }

    [HttpGet("project-salary-summary")]
    public async Task<IActionResult> GetProjectSalarySummary()
    {
        var projects = await reportService.GetProjectSalarySummaryAsync();

        return Ok(projects);
    }
}