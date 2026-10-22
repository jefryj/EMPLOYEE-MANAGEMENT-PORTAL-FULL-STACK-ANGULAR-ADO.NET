ALTER PROCEDURE GetEmployees
(
    @Name NVARCHAR(100) = NULL,
    @Email NVARCHAR(100) = NULL,
    @DepartmentId INT = NULL,
    @ProjectId INT = NULL,

    @SortBy NVARCHAR(50) = NULL,
    @IsDescending BIT = 0,

    @PageNumber INT = 1,
    @PageSize INT = 10
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        e.Id,
        e.Name,
        e.Email,
        e.Phone,
        e.Salary,
        e.DepartmentId,
        d.DepartmentName,
        e.ProjectId,
        p.ProjectName,
        e.RoleId,
        r.RoleName
    FROM Employees e
    INNER JOIN Departments d
        ON e.DepartmentId = d.Id
    LEFT JOIN Projects p
        ON e.ProjectId = p.Id
    INNER JOIN Roles r
        ON e.RoleId = r.Id
    WHERE
        (@Name IS NULL OR e.Name LIKE '%' + @Name + '%')
        AND (@Email IS NULL OR e.Email LIKE '%' + @Email + '%')
        AND (@DepartmentId IS NULL OR e.DepartmentId = @DepartmentId)
        AND (@ProjectId IS NULL OR e.ProjectId = @ProjectId)

    ORDER BY
        CASE WHEN @SortBy = 'Name' AND @IsDescending = 0
            THEN e.Name END ASC,

        CASE WHEN @SortBy = 'Name' AND @IsDescending = 1
            THEN e.Name END DESC,

        CASE WHEN @SortBy = 'Email' AND @IsDescending = 0
            THEN e.Email END ASC,

        CASE WHEN @SortBy = 'Email' AND @IsDescending = 1
            THEN e.Email END DESC,

        CASE WHEN @SortBy = 'ProjectName' AND @IsDescending = 0
            THEN p.ProjectName END ASC,

        CASE WHEN @SortBy = 'ProjectName' AND @IsDescending = 1
            THEN p.ProjectName END DESC,

        CASE WHEN @SortBy = 'DepartmentName' AND @IsDescending = 0
            THEN d.DepartmentName END ASC,

        CASE WHEN @SortBy = 'DepartmentName' AND @IsDescending = 1
            THEN d.DepartmentName END DESC,

        CASE WHEN @SortBy = 'Salary' AND @IsDescending = 0
            THEN e.Salary END ASC,

        CASE WHEN @SortBy = 'Salary' AND @IsDescending = 1
            THEN e.Salary END DESC,

        e.Id ASC

    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO