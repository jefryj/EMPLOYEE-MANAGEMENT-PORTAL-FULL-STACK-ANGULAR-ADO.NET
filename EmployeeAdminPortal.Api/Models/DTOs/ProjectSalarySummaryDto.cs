namespace EmployeeAdminPortal.Api.Models.DTOs;

public class ProjectSalarySummaryDto
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public decimal AverageSalary { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
}