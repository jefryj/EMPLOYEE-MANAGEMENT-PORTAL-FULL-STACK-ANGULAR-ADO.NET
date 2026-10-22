namespace EmployeeAdminPortal.Api.Models.DTOs;

public class DepartmentSalarySummaryDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public decimal AverageSalary { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
}