CREATE PROCEDURE UpdateProject
(
    @Id INT,
    @ProjectName NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Projects
    SET ProjectName = @ProjectName
    WHERE Id = @Id;
END