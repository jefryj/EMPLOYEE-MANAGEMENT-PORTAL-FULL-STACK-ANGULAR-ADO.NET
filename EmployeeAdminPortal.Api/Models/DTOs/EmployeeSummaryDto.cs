namespace EmployeeAdminPortal.Api.Models.DTOs;

public class EmployeeSummaryDto
{
    public int TotalEmployees { get; set; }
    public decimal AverageSalary { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
}