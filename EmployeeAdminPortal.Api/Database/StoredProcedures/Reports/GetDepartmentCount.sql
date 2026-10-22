CREATE PROCEDURE GetDepartmentCount
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.Id AS DepartmentId,
        d.DepartmentName,
        COUNT(e.Id) AS EmployeeCount
    FROM Departments d
    LEFT JOIN Employees e
        ON d.Id = e.DepartmentId
    GROUP BY
        d.Id,
        d.DepartmentName
    ORDER BY
        d.DepartmentName;
END