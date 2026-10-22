CREATE PROCEDURE AddEmployee
(
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Salary DECIMAL(18,2),
    @PasswordHash NVARCHAR(MAX),
    @DepartmentId INT,
    @ProjectId INT = NULL,
    @RoleId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Employees
    (
        Name,
        Email,
        Phone,
        Salary,
        PasswordHash,
        DepartmentId,
        ProjectId,
        RoleId
    )
    VALUES
    (
        @Name,
        @Email,
        @Phone,
        @Salary,
        @PasswordHash,
        @DepartmentId,
        @ProjectId,
        @RoleId
    );
END