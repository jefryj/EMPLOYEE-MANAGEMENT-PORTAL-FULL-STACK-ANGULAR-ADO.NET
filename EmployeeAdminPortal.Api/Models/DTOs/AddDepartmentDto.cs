namespace EmployeeAdminPortal.Api.Models.DTOs;
using System.ComponentModel.DataAnnotations;
public class AddDepartmentDto
{
    [Required]
    public string DepartmentName { get; set; } = string.Empty;
}