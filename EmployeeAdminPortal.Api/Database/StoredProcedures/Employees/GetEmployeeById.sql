ALTER PROCEDURE GetEmployeeById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        e.Id,
        e.Name,
        e.Email,
        e.Phone,
        e.Salary,
        e.PasswordHash,

        e.DepartmentId,
        d.DepartmentName,

        e.ProjectId,
        p.ProjectName,

        e.RoleId,
        r.RoleName

    FROM Employees e
    LEFT JOIN Departments d
        ON e.DepartmentId = d.Id
    LEFT JOIN Projects p
        ON e.ProjectId = p.Id
    LEFT JOIN Roles r
        ON e.RoleId = r.Id
    WHERE e.Id = @Id;
END