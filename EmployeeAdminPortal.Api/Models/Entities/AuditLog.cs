namespace EmployeeAdminPortal.Api.Models.Entities;
public class AuditLog
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public string? Details { get; set; }

    public DateTime CreatedAt { get; set; }
}