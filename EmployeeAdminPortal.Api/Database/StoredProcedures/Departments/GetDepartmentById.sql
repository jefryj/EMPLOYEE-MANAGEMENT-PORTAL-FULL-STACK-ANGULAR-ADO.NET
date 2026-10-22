CREATE PROCEDURE GetDepartmentById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        DepartmentName
    FROM Departments
    WHERE Id = @Id;
END