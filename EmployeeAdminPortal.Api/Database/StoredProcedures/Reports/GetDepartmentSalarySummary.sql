CREATE PROCEDURE GetDepartmentSalarySummary
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.Id AS DepartmentId,
        d.DepartmentName,
        COUNT(e.Id) AS EmployeeCount,
        ISNULL(AVG(e.Salary), 0) AS AverageSalary,
        ISNULL(MIN(e.Salary), 0) AS MinSalary,
        ISNULL(MAX(e.Salary), 0) AS MaxSalary
    FROM Departments d
    LEFT JOIN Employees e
        ON d.Id = e.DepartmentId
    GROUP BY
        d.Id,
        d.DepartmentName
    ORDER BY
        d.DepartmentName;
END
