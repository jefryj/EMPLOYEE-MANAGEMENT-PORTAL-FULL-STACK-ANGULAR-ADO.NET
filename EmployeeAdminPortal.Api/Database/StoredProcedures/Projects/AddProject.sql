CREATE PROCEDURE AddProject
(
    @ProjectName NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Projects
    (
        ProjectName
    )
    VALUES
    (
        @ProjectName
    );
END