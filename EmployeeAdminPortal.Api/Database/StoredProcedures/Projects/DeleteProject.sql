CREATE PROCEDURE DeleteProject
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Projects
    WHERE Id = @Id;
END
