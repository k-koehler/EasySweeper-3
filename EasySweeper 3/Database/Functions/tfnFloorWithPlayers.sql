CREATE OR ALTER FUNCTION [dbo].[tfnFloorWithPlayers]
(
	@FloorID int
)
RETURNS TABLE
AS
RETURN
SELECT	@FloorID [FloorID],
	dbo.fnRemoveFirstCharacter
	((
		SELECT	',' + CONVERT(nvarchar(10), PF.PlayerID)
		FROM	dbo.PlayerFloor PF
		WHERE	PF.FloorID = F.ID
		ORDER BY	PF.Position
		FOR	XML PATH ('')
	))
	AS PlayerIDs,
	dbo.fnRemoveFirstCharacter
	((	
		SELECT	',' + CONVERT(nvarchar(200), P.Name)
		FROM	dbo.PlayerFloor PF
			INNER JOIN dbo.Player P ON PF.PlayerID = P.Id
		WHERE	PF.FloorID = F.ID
		ORDER BY	PF.Position
		FOR	XML PATH ('')		
	))AS PlayerNames
FROM	Floor F
WHERE	F.ID = @FloorID
GO
