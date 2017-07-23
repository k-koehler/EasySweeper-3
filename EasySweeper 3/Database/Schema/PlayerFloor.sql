IF NOT EXISTS
(
	SELECT	*
	FROM	sys.tables
	WHERE	Name = 'PlayerFloor'
)
CREATE TABLE [dbo].[PlayerFloor]
(
	[PlayerID] [int] NOT NULL,
	[FloorID] [int] NOT NULL,
	[Position] [int] NOT NULL,
	CONSTRAINT [PK_PlayerFloor] PRIMARY KEY CLUSTERED
	(
		[PlayerID] ASC,
		[FloorID] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

