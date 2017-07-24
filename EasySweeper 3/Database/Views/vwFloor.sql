CREATE OR ALTER VIEW [dbo].[vwFloor]
AS
SELECT	F.ID,
	F.Duration,
	P.P1,
	P.P2,
	P.P3,
	P.P4,
	P.P5,
	F.Floor,
	F.Bonus,
	F.Mod,
	SL.ID [SizeID],
	SL.Name [Size],
	F.Complexity,
	F.Image,
	F.Date
FROM	Floor F
	INNER JOIN dbo.SizeLookup SL ON F.Size = SL.ID
	CROSS APPLY dbo.tfnFloorWithPlayersTable(F.ID) P
GO
