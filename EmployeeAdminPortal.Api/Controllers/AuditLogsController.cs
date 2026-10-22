using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAdminPortal.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "1")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService auditLogService;

    public AuditLogsController(IAuditLogService auditLogService)
    {
        this.auditLogService = auditLogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAuditLogs()
    {
        var auditLogs = await auditLogService.GetAuditLogsAsync();

        return Ok(auditLogs);
    }
}