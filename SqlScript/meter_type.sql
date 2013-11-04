IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[meter_type]') AND type in (N'U'))
DROP TABLE [dbo].[meter_type]
GO
CREATE TABLE [dbo].[meter_type](
	[type] [int] NOT NULL,
	[name] [varchar](64) NULL,
	[protocol] [varchar](3) NULL
) ON [PRIMARY]

GO