IF NOT EXISTS
(
	SELECT	*
	FROM	sys.tables
	WHERE	name = 'Floor'
)
CREATE TABLE dbo.Floor
(
	ID int PRIMARY KEY IDENTITY(1,1),
	Floor int NOT NULL,
	Duration bigint NOT NULL,
	Bonus int NULL,
	Mod int NULL,
	Size int NOT NULL,
	Complexity int NULL,
	Image nvarchar(100) NULL
)
GO