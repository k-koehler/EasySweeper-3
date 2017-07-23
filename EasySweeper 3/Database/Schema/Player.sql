IF NOT EXISTS
(
	SELECT	*
	FROM	sys.tables
	WHERE	name = 'Player'
)
CREATE TABLE [dbo].[Player]
(
	[ID] [int] IDENTITY (1,1) NOT NULL,
	[Name] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT [PK__Player_ID] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

