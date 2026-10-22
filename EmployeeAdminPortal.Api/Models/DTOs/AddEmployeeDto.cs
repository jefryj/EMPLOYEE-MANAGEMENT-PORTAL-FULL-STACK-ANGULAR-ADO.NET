namespace EmployeeAdminPortal.Api.Models.DTOs;
using System.ComponentModel.DataAnnotations;
public class AddEmployeeDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [Range(1, 100000000)]
    public decimal Salary { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
    [Required]
    public int DepartmentId { get; set; }

    public int? ProjectId { get; set; }

    [Required]
    public int RoleId { get; set; }
}