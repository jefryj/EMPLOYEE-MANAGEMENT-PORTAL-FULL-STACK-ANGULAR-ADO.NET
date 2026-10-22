CREATE PROCEDURE GetEmployeeByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Name,
        Email,
        PasswordHash,
        RoleId
    FROM Employees
    WHERE Email = @Email;
END