namespace EmployeeAdminPortal.Api.Models.DTOs;
using System.ComponentModel.DataAnnotations;
public class UpdateDepartmentDto
{
    [Required]
    public string DepartmentName { get; set; } = string.Empty;
}