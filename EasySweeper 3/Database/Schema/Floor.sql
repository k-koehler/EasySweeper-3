IF NOT EXISTS
(
	SELECT	*
	FROM	sys.tables
	WHERE	name = 'Floor'
)
CREATE TABLE [dbo].[Floor]
(
	[ID] [int] IDENTITY (1,1) NOT NULL,
	[Floor] [int] NOT NULL,
	[Bonus] [int] NULL,
	[Mod] [int] NULL,
	[Size] [int] NOT NULL,
	[Complexity] [int] NULL,
	[Image] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Date] [datetime2] NOT NULL,
	[Duration] [int] NOT NULL,
	CONSTRAINT [PK_Floor_ID] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

