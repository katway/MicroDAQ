IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[meters]') AND type in (N'U'))
DROP TABLE [dbo].[meters]
GO

CREATE TABLE [dbo].[meters](
	[id] [int] NOT NULL,
	[type] [int] NULL
) ON [PRIMARY]

GO