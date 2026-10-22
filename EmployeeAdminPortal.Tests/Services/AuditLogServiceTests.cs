using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using EmployeeAdminPortal.Api.Services;
using Moq;

namespace EmployeeAdminPortal.Tests.Services;

public class AuditLogServiceTests
{
    private readonly Mock<IAuditLogRepository> auditLogRepositoryMock;
    private readonly AuditLogService auditLogService;

    public AuditLogServiceTests()
    {
        auditLogRepositoryMock = new Mock<IAuditLogRepository>();

        auditLogService = new AuditLogService(auditLogRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAuditLogsAsync_ReturnsAuditLogs()
    {
        var auditLogs = new List<AuditLog>
        {
            new AuditLog
            {
                Id = 1,
                EmployeeId = 1,
                UserName = "admin@test.com",
                Action = "Create",
                EntityName = "Employee",
                Details = "Created employee"
            }
        };

        auditLogRepositoryMock.Setup(x => x.GetAuditLogsAsync()).ReturnsAsync(auditLogs);

        var result = await auditLogService.GetAuditLogsAsync();

        Assert.Single(result);
        Assert.Equal("Create", result[0].Action);
    }

    [Fact]
    public async Task GetAuditLogsAsync_ReturnsEmptyList_WhenNoLogsExist()
    {
        auditLogRepositoryMock.Setup(x => x.GetAuditLogsAsync()).ReturnsAsync(new List<AuditLog>());

        var result = await auditLogService.GetAuditLogsAsync();

        Assert.Empty(result);
    }
}