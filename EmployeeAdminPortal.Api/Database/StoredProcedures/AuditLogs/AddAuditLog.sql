CREATE PROCEDURE AddAuditLog
(
    @EmployeeId INT,
    @UserName NVARCHAR(100),
    @Action NVARCHAR(50),
    @EntityName NVARCHAR(50),
    @Details NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO AuditLogs
    (
        EmployeeId,
        UserName,
        Action,
        EntityName,
        Details,
        CreatedAt
    )
    VALUES
    (
        @EmployeeId,
        @UserName,
        @Action,
        @EntityName,
        @Details,
        GETDATE()
    );
END