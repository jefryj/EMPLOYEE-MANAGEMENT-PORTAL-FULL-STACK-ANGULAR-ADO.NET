using EmployeeAdminPortal.Api.Models.Entities;

namespace EmployeeAdminPortal.Api.Repositories.Interfaces;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog);

    Task<List<AuditLog>> GetAuditLogsAsync();
}