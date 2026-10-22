namespace EmployeeAdminPortal.Api.Models.DTOs;

public class AuditLogDto
{
    public int Id { get; set; }

    public string Action { get; set; } = string.Empty;

    public string PerformedBy { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}