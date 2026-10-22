CREATE PROCEDURE AnyProjectByName
    @ProjectName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*)
    FROM Projects
    WHERE ProjectName = @ProjectName;
END