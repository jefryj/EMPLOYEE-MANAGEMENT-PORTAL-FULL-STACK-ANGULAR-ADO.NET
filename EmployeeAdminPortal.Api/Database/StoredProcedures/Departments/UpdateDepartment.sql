CREATE PROCEDURE UpdateDepartment
(
    @Id INT,
    @DepartmentName NVARCHAR(100)
)
AS
BEGIN
    UPDATE Departments
    SET DepartmentName = @DepartmentName
    WHERE Id = @Id;
END