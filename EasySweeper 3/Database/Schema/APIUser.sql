CREATE TABLE [dbo].[APIUser]
(
	[APIKey] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT [PK__APIUser__BB6E626C46BC6C15] PRIMARY KEY CLUSTERED
	(
		[APIKey] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

