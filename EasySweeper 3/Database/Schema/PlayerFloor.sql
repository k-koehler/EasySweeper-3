IF NOT EXISTS
(
	SELECT	*
	FROM	sys.tables
	WHERE	Name = 'PlayerFloor'
)
CREATE TABLE dbo.PlayerFloor
(
	PlayerID int NOT NULL,
	FloorID int NOT NULL
)
GO
ALTER TABLE dbo.PlayerFloor
ADD CONSTRAINT PK_PlayerFloor
PRIMARY KEY (PlayerID, FloorID)
GO

