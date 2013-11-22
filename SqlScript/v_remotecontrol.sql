GO
/****** Object:  View [dbo].[v_remoteControl]    Script Date: 11/22/2013 10:49:16 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[v_remoteControl]') AND OBJECTPROPERTY(id, N'IsView') = 1)
DROP VIEW [dbo].[v_remoteControl]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_remoteControl] AS 
SELECT     CAST(slave AS int) AS id, ISNULL(cycle, 0) AS cycle, ISNULL(command, 0) AS command, ISNULL(cmdstate, 0) AS cmdstate, 'CTR' as type
FROM         dbo.RemoteControl
WHERE     (slave IS NOT NULL)
UNION

SELECT
c.id, (cycle & pr.alertBuzzer) as cycle,command,cmdState,'WLT' AS type
FROM
(
SELECT
id as id,MAX(cycle) AS cycle,MIN(command) AS command,MIN(cmdstate) AS cmdState
FROM
(
SELECT     ia.alarmSlave AS id,  m.alert AS cycle, 1 AS command, 1 AS cmdState
FROM         dbo.meters_value m INNER JOIN
                      item_alarm ia ON m.id = ia.itemSlave
--SELECT     p.alarm AS id,  m.alert AS cycle, 1 AS command, 1 AS cmdState
--FROM         dbo.meters_value m INNER JOIN
--                      dbo.ProcessItem p ON m.id = p.slave
) b
GROUP BY B.id
HAVING B.ID IS NOT NULL
) c LEFT JOIN  dbo.ProcessItem pr ON c.id = pr.slave
GO
EXEC dbo.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'USER',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_remoteControl'