CREATE OR ALTER FUNCTION [dbo].[tfnFloorPlayerCount]
(
	@FloorID int
)
RETURNS TABLE
AS
RETURN
SELECT	COUNT(*) Players
FROM	dbo.PlayerFloor FP
WHERE	FP.FloorID = @FloorID