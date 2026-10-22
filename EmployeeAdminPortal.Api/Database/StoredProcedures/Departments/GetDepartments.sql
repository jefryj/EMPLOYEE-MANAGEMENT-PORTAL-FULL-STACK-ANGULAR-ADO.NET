ALTER PROCEDURE GetDepartments
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        DepartmentName
    FROM Departments
    ORDER BY Id;
END
GO