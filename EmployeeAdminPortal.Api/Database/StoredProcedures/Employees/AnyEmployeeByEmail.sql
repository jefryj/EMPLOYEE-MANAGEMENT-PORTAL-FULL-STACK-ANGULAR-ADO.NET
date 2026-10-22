CREATE PROCEDURE AnyEmployeeByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*)
    FROM Employees
    WHERE Email = @Email;
END