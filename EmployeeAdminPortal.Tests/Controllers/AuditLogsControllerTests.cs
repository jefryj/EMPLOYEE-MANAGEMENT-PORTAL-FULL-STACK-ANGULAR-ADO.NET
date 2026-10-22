using EmployeeAdminPortal.Api.Controllers;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeAdminPortal.Tests.Controllers;

public class AuditLogsControllerTests
{
    private readonly Mock<IAuditLogService> auditLogServiceMock;
    private readonly AuditLogsController controller;

    public AuditLogsControllerTests()
    {
        auditLogServiceMock = new Mock<IAuditLogService>();

        controller = new AuditLogsController(auditLogServiceMock.Object);
    }

    [Fact]
    public async Task GetAuditLogs_ReturnsAuditLogs()
    {
        var auditLogs = new List<AuditLog>
        {
            new AuditLog
            {
                Id = 1,
                EmployeeId = 1,
                UserName = "admin@test.com",
                Action = "Create",
                EntityName = "Employee"
            }
        };

        auditLogServiceMock.Setup(x => x.GetAuditLogsAsync()).ReturnsAsync(auditLogs);

        var result = await controller.GetAuditLogs();

        var okResult = Assert.IsType<OkObjectResult>(result);

        var returnedLogs =Assert.IsAssignableFrom<List<AuditLog>>(okResult.Value);

        Assert.Single(returnedLogs);
    }

    [Fact]
    public async Task GetAuditLogs_ReturnsOkObjectResult()
    {
        auditLogServiceMock.Setup(x => x.GetAuditLogsAsync()).ReturnsAsync(new List<AuditLog>());

        var result = await controller.GetAuditLogs();

        Assert.IsType<OkObjectResult>(result);
    }
}