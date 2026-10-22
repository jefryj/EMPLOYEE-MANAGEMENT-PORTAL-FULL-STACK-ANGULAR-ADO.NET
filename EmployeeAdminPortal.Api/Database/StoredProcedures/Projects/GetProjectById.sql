CREATE PROCEDURE GetProjectById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        ProjectName
    FROM Projects
    WHERE Id = @Id;
END