using System.ComponentModel.DataAnnotations;
public class UpdateEmployeeDto
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
    public int DepartmentId { get; set; }

    public int? ProjectId { get; set; }
}