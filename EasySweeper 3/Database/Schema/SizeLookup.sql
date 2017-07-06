IF NOT EXISTS
(
	SELECT	*
	FROM	sys.tables
	WHERE	Name = 'SizeLookup'
)
CREATE TABLE dbo.SizeLookup
(
	ID int PRIMARY KEY,
	Name nvarchar(20)
)
GO
DELETE dbo.SizeLookup
GO
INSERT dbo.SizeLookup
(
	ID,
	Name
)
VALUES
(
	1,
	N'Small'
),
(
	2,
	N'Medium'
),
(
	3,
	N'Large'
)
GO