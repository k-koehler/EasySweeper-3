IF NOT EXISTS
(
	SELECT	*
	FROM	sys.tables
	WHERE	Name = 'SizeLookup'
)
CREATE TABLE [dbo].[SizeLookup]
(
	[ID] [int] NOT NULL,
	[Name] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT [PK_SizeLookup_ID] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
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