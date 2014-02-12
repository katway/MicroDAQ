IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[meters_value]') AND type in (N'U'))
DROP TABLE [dbo].[meters_value]
GO

CREATE TABLE [dbo].[meters_value] (
[id] int NOT NULL ,
[time] datetime NULL ,
[value1] float(53) NULL ,
[value2] float(53) NULL ,
[value3] float(53) NULL ,
[type] int NULL ,
[state] int NULL ,
[zzTime] datetime NULL ,
[alertTime] datetime NULL ,
[alert] int NULL ,
[quality] int NULL ,
[alertRecord] varchar(32) COLLATE Chinese_PRC_CI_AS NULL 
)
ON [PRIMARY]
GO


CREATE INDEX [_WA_Sys_id_6F7F8B4B] ON [dbo].[meters_value]
([id] ASC) 
ON [PRIMARY]
GO


CREATE TRIGGER [dbo].[insert_alertRecord_after_meters-value_State_change]
ON [dbo].[meters_value]
AFTER UPDATE
AS
BEGIN
  declare @oldState int ,@value float,@newState INT,@ID INT;

             select @oldState =state from deleted;
	SELECT @newState =state   from inserted;
	SELECT @ID = id  from inserted;
	SELECT @value = value1 from inserted;
                     IF(@oldState!= @newState)
                            BEGIN
								IF( @newState >= 2)
									BEGIN
                                                DECLARE @alertid VARCHAR(32);
                                                DECLARE @uuid VARCHAR(32);
                                                SET @uuid = (SELECT id FROM processitem WHERE slave =@ID);
                                                SET @alertid = REPLACE(NEWID(), '-', '');
                                                 
                                                INSERT INTO ProcessItemAlertRecord (id,processItemId,[timestamp],[value],[confirm])
                                                                     VALUES(@alertid, @uuid,GETDATE(),@value,'未确认');  
                                                UPDATE ProcessItem SET alertRecordId = @alertid WHERE slave = @ID;              
                                           END
                                       ELSE
                                       		BEGIN
                                       			SET @alertid = (SELECT TOP 1 alertRecordId FROM ProcessItem WHERE slave = @ID);
                                       			UPDATE ProcessItemAlertRecord SET resettime =GETDATE()  WHERE Id = @alertid;
                                       		END
                                END
END
GO

IF ((SELECT COUNT(*) from ::fn_listextendedproperty('MS_Description', 
'USER', N'dbo', 
'TABLE', N'meters_value', 
'TRIGGER', N'insert_alertRecord_after_meters-value_State_change')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'该过程用于向alertRecord表写入仪表掉线后要存储的历史数据'
, @level0type = 'USER', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'meters_value'
, @level2type = 'TRIGGER', @level2name = N'insert_alertRecord_after_meters-value_State_change'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'该过程用于向alertRecord表写入仪表掉线后要存储的历史数据'
, @level0type = 'USER', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'meters_value'
, @level2type = 'TRIGGER', @level2name = N'insert_alertRecord_after_meters-value_State_change'
GO

CREATE TRIGGER [dbo].[insert_sys-log_after_meters-value_change]
ON [dbo].[meters_value]
AFTER UPDATE
AS

BEGIN
	declare @oldState INT,@newState INT,@ID INT,@oldQuality INT,@newQuality INT; 
	--更新前的数据  
	select @ID = id from meters_value;	
	select @oldState =state from deleted;--从deleted表中取删除的老名字  
	select @newState =state from inserted;
		IF(@oldState!= @newState)
			BEGIN			
				INSERT into sys_log (LogDate,LogLevel,LogLogger,LogMessage)   VALUES(getdate(),'state','microdaq','仪表'+convert(varchar(50),@ID)+'状态从'+ convert(varchar(50),@oldState)+'变为'+convert(varchar(50),@newState));
			END
	select @oldQuality =Quality from deleted;--从deleted表中取删除的老名字  
	select @newQuality =Quality from inserted;
		IF(@oldQuality!= @newQuality)
			BEGIN			
				INSERT into sys_log (LogDate,LogLevel,LogLogger,LogMessage)   VALUES(getdate(),'quality','microdaq','仪表'+convert(varchar(50),@ID)+'数据可靠度从'+ convert(varchar(50),@oldQuality)+'变为'+convert(varchar(50),@newQuality));
			END

END

GO

IF ((SELECT COUNT(*) from ::fn_listextendedproperty('MS_Description', 
'USER', N'dbo', 
'TABLE', N'meters_value', 
'TRIGGER', N'insert_sys-log_after_meters-value_change')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'应该存储过程用于向sys_log表写入各items的state、quality字段的变化纪录'
, @level0type = 'USER', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'meters_value'
, @level2type = 'TRIGGER', @level2name = N'insert_sys-log_after_meters-value_change'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'应该存储过程用于向sys_log表写入各items的state、quality字段的变化纪录'
, @level0type = 'USER', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'meters_value'
, @level2type = 'TRIGGER', @level2name = N'insert_sys-log_after_meters-value_change'
GO

GO