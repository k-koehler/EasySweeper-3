IF NOT EXISTS
(
	SELECT	*
	FROM	sys.tables
	WHERE	name = 'Player'
)
CREATE TABLE dbo.Player
(
	ID int PRIMARY KEY IDENTITY(1,1),
	Name nvarchar(20)
)