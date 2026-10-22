CREATE PROCEDURE GetProjectCount
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id AS ProjectId,
        p.ProjectName,
        COUNT(e.Id) AS EmployeeCount
    FROM Projects p
    LEFT JOIN Employees e
        ON p.Id = e.ProjectId
    GROUP BY
        p.Id,
        p.ProjectName
    ORDER BY
        p.ProjectName;
END