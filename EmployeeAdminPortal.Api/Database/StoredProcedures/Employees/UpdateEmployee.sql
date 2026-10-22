CREATE PROCEDURE UpdateEmployee
(
    @Id INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Salary DECIMAL(18,2),
    @DepartmentId INT,
    @ProjectId INT = NULL,
    @RoleId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Employees
    SET
        Name = @Name,
        Email = @Email,
        Phone = @Phone,
        Salary = @Salary,
        DepartmentId = @DepartmentId,
        ProjectId = @ProjectId,
        RoleId = @RoleId
    WHERE Id = @Id;
END