using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using EmployeeAdminPortal.Api.Services.Interfaces;

namespace EmployeeAdminPortal.Api.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository auditLogRepository;

    public AuditLogService(IAuditLogRepository auditLogRepository)
    {
        this.auditLogRepository = auditLogRepository;
    }

    public async Task<List<AuditLog>> GetAuditLogsAsync()
    {   
        
        return await auditLogRepository.GetAuditLogsAsync();
    }
}