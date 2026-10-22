CREATE PROCEDURE GetEmployeeSummary
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        COUNT(*) AS TotalEmployees,
        ISNULL(AVG(Salary), 0) AS AverageSalary,
        ISNULL(MIN(Salary), 0) AS MinSalary,
        ISNULL(MAX(Salary), 0) AS MaxSalary
    FROM Employees;
END