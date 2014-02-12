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

CREATE TRIGGER [dbo].[insert_alertRecord_after_meters-value_alert_change]
ON [dbo].[meters_value]
AFTER UPDATE
AS
BEGIN
  declare @oldAlert int ,@value float,@newAlert INT,@ID INT;

             select @oldAlert =alert from deleted;
	SELECT @newAlert =alert from inserted;
	SELECT @ID = id  from inserted;
	SELECT @value = value1 from inserted;
                     IF(@oldAlert!= @newAlert)
                            BEGIN
								IF( @newAlert >= 2)
									BEGIN
                                                DECLARE @alertid VARCHAR(32);
                                                DECLARE @uuid VARCHAR(32);
                                                SET @uuid = (SELECT id FROM processitem WHERE slave =@ID);
                                                SET @alertid = REPLACE(NEWID(), '-', '');
                                                 
                                                INSERT INTO ProcessItemAlertRecord (id,processItemId,[timestamp],[value],[confirm])
                                                                     VALUES(@alertid, @uuid,GETDATE(),@value,'未确认');  
                                                UPDATE ProcessItem SET alertRecordId = @alertid WHERE slave = @ID;
                                                UPDATE meters_value SET alertRecord =  @alertid WHERE id= @ID;
                                           END
                                       ELSE
                                       		BEGIN
                                       			SET @alertid = (SELECT TOP 1 alertRecordId FROM ProcessItem WHERE slave = @ID);
                                       			UPDATE ProcessItemAlertRecord SET resettime =GETDATE()  WHERE Id = @alertid;
                                       		END
                                END

END

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