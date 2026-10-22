CREATE PROCEDURE DeleteEmployee
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Employees
    WHERE Id = @Id;
END