ALTER FUNCTION [dbo].[tfnPlayerInFloor]
(
	@PlayerName nvarchar(20),
	@FloorID int
)
RETURNS TABLE
AS
RETURN
SELECT	CASE WHEN EXISTS
	(
		SELECT	*
		FROM	dbo.Floor F
			INNER JOIN dbo.PlayerFloor PF ON F.ID = PF.FloorID
		WHERE	PF.PlayerID = 
			(
				SELECT	TOP 1 ID
				FROM	dbo.Player
				WHERE	Name = @PlayerName	
			)
		AND	F.ID = @FloorID
	)
	THEN 1
	ELSE 0 END AS IsInFloor
