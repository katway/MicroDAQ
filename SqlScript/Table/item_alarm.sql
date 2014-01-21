IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[item_alarm]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [dbo].[item_alarm]

GO
/****** Object:  Table [dbo].[item_alarm]    Script Date: 11/22/2013 10:45:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[item_alarm](
	[ID] [char](32) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[itemSlave] [int] NULL,
	[alarmSlave] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
