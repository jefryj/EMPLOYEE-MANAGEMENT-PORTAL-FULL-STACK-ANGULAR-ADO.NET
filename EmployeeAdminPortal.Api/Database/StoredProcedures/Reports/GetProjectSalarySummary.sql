CREATE PROCEDURE GetProjectSalarySummary
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id AS ProjectId,
        p.ProjectName,
        COUNT(e.Id) AS EmployeeCount,
        ISNULL(AVG(e.Salary), 0) AS AverageSalary,
        ISNULL(MIN(e.Salary), 0) AS MinSalary,
        ISNULL(MAX(e.Salary), 0) AS MaxSalary
    FROM Projects p
    LEFT JOIN Employees e
        ON p.Id = e.ProjectId
    GROUP BY
        p.Id,
        p.ProjectName
    ORDER BY
        p.ProjectName;
END