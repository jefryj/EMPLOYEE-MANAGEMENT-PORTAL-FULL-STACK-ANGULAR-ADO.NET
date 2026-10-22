namespace EmployeeAdminPortal.Api.Models.DTOs;

public class DepartmentCountDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
}