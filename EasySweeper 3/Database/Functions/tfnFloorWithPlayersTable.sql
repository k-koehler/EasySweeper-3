CREATE FUNCTION [dbo].[tfnFloorWithPlayersTable]
(
	@FloorID int
)
RETURNS TABLE
AS
RETURN
SELECT	
(
	SELECT	P.Name
	FROM	dbo.PlayerFloor PF
		INNER JOIN dbo.Player P ON PF.PlayerID = P.ID
	WHERE	PF.FloorID = @FloorID
	AND	PF.Position = 1
) AS [P1],
(
	SELECT	P.Name
	FROM	dbo.PlayerFloor PF
		INNER JOIN dbo.Player P ON PF.PlayerID = P.ID
	WHERE	PF.FloorID = @FloorID
	AND	PF.Position = 2
	
) AS [P2],
(
	SELECT	P.Name
	FROM	dbo.PlayerFloor PF
		INNER JOIN dbo.Player P ON PF.PlayerID = P.ID
	WHERE	PF.FloorID = @FloorID
	AND	PF.Position = 3
) AS [P3],
(
	SELECT	P.Name
	FROM	dbo.PlayerFloor PF
		INNER JOIN dbo.Player P ON PF.PlayerID = P.ID
	WHERE	PF.FloorID = @FloorID
	AND	PF.Position = 4
) AS [P4],
(
	SELECT	P.Name
	FROM	dbo.PlayerFloor PF
		INNER JOIN dbo.Player P ON PF.PlayerID = P.ID
	WHERE	PF.FloorID = @FloorID
	AND	PF.Position = 5
) AS P5
GO
