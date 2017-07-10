ALTER FUNCTION [dbo].[tfnFloorDuplicates]
(
	@FloorID int
)
RETURNS TABLE
AS
RETURN
SELECT	F2.ID
FROM	dbo.Floor F1
	INNER JOIN dbo.Floor F2 ON F1.ID <> F2.ID
		AND F1.Floor = F2.Floor
		AND F1.Duration = F2.Duration
		AND F1.Bonus = F2.Bonus
		AND F1.Mod = F2.Mod
		AND F1.Size = F2.Size
		AND F1.Complexity = F2.Complexity
	CROSS APPLY dbo.tfnFloorWithPlayers(F1.ID) PF1
	CROSS APPLY dbo.tfnFloorWithPlayers(F2.ID) PF2
WHERE	F1.ID = @FloorID
AND	F2.ID <> @FloorID
AND	PF1.PlayerIDs = PF2.PlayerIDs