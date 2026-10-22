using EmployeeAdminPortal.Api.Models.Entities;

namespace EmployeeAdminPortal.Api.Services.Interfaces;

public interface IAuditLogService
{
    Task<List<AuditLog>> GetAuditLogsAsync();
}
