namespace EmployeeAdminPortal.Api.Models.Entities;
public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public decimal Salary { get; set; }

    public string PasswordHash { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public int? ProjectId { get; set; }

    public string? ProjectName { get; set; }

    public int RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;
}