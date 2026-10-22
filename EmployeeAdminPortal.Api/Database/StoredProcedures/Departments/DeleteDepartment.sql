CREATE PROCEDURE DeleteDepartment
    @Id INT
AS
BEGIN
    DELETE FROM Departments
    WHERE Id = @Id;
END