CREATE PROCEDURE ChangePassword
(
    @EmployeeId INT,
    @PasswordHash NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Employees
    SET PasswordHash = @PasswordHash
    WHERE Id = @EmployeeId;
END