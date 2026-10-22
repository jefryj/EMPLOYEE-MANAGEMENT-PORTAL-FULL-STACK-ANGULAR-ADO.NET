public class EmployeeSearchDto
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public int? DepartmentId { get; set; }

    public int? ProjectId { get; set; }

    public string? SortBy { get; set; }

    public bool IsDescending { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}