ALTER PROCEDURE GetProjects
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        ProjectName
    FROM Projects
    ORDER BY Id;
END
GO