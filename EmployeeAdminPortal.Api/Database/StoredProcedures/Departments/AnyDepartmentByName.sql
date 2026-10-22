CREATE PROCEDURE AnyDepartmentByName
    @DepartmentName NVARCHAR(100)
AS
BEGIN
    SELECT COUNT(*)
    FROM Departments
    WHERE DepartmentName = @DepartmentName;
END