using System.ComponentModel.DataAnnotations;

public class ChangePasswordDto
{   
    public int EmployeeId { get; set; }

    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
}