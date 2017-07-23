CREATE   FUNCTION [dbo].[tfnFloorDuplicatesList]
(
	@FloorID int
)
RETURNS TABLE
AS
RETURN
SELECT
(
	SELECT	CONVERT(nvarchar(100), ID) + ' '
	FROM	dbo.tfnFloorDuplicates(@FloorID)
	FOR	XML PATH('')
) AS MatchingIDs
GO
