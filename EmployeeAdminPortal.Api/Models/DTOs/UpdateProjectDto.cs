namespace EmployeeAdminPortal.Api.Models.DTOs;
using System.ComponentModel.DataAnnotations;
public class UpdateProjectDto
{
    [Required]
    public string ProjectName { get; set; } = string.Empty;
}