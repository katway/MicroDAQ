


ALTER    PROCEDURE [dbo].[UpdateHistoryData3]
(@MeterID int ,
@MeterType int ,
@MeterState int,
@Value1 float ,
@Value2 float,
@value3 float,
@last FLOAT,
@uuid VARCHAR(100) ,
@uidM3 varchar(50),
@yellowMin FLOAT ,
@yellowMax FLOAT ,
@RedMin Float ,
@RedMax FLOAT,
@Rate int ,
@ResultState INT OUTPUT,
@RestultMessage VARCHAR(60) OUTPUT)
AS 
BEGIN
	IF EXISTS(SELECT  id  FROM meters WHERE ID = @MeterID )
		BEGIN	
			--向每个设备的数据表中插入数据			
			IF(@uuid is not null )
			BEGIN
			if exists (select name from sysobjects where [name] = 'ZZ'+@uuid and xtype='U')
				BEGIN
					DECLARE @zzTime DateTime ;
					SET @zzTime  = (SELECT TOP 1 [zztime] FROM meters_value WHERE id =@MeterID);	
					--DECLARE @last DateTime ;
					--SET @last = (SELECT [time] FROM tmp_meters_lastupdate);					
					--DECLARE @rate int ;
					DECLARE @Warning int;
					DECLARE @filter FLOAT;
					--SET @rate = (SELECT updaterate FROM ProcessItem WHERE id = @uuid);
					SET @filter = (SELECT ISNULL([filter], 0.0) FROM ProcessItem WHERE id = @uuid);
					declare @sql2 NVARCHAR(500);
					IF (@zzTime is null OR ABS(DATEDIFF(ss,@zztime, GETDATE())) >= @Rate)
					--IF ((@last is null OR ABS(@value1 - @last) > @filter)OR ABS(DATEDIFF(ss,@last, GETDATE())) >= @rate)
						BEGIN		
							IF (@value1<@yellowmin or @value1>@yellowmax)	
								BEGIN
									IF (@meterType=32) 
										BEGIN 
											set @yellowMin = 0 ;
										END
									SET @value2 =  ABS(@yellowMin) + FLOOR(RAND() * (@yellowMax-@yellowMin));	
									SET @Warning =1;
								END
							ELSE
								BEGIN
									SET @value2=@value1;
									SET @Warning =0;
								END
							IF (@meterID<11000)
								BEGIN
									--将生成的Value2写回到即时数据表
									UPDATE meters_value SET value2= @Value2	WHERE id=@MeterID;
									
									--将数据写入ZZ表
									--SET	@sql2 = 'INSERT INTO ZZ'+ @uuid + ' (value,value2,timestamp) VALUES('
										--+ CAST(@Value1 AS VARCHAR(10)) + ',' + CAST(@Value2 AS VARCHAR(10))  + ', GETDATE())';
									SET	@sql2 = 'INSERT INTO ZZ'+ @uuid + ' (value,value2,timestamp,productionState,flag) '
											+' SELECT @Value1 AS value,@Value2 AS value2,GETDATE() AS timestamp,productionState  AS productionState,@warning AS flag FROM processItem '
											+' WHERE ID = '''+ @uuid +'''';
									--set  @sql2 =  'WHERE processItemID = '+ @uuid ;
		
									--SET	@sql2 = 'INSERT INTO ZZ'+ @uuid + ' (value,timestamp) VALUES(' + CAST(@Value1 AS VARCHAR(30)) + ',' + 'GETDATE())';
									EXEC sp_executesql @sql2,N'@Value1 FLOAT,@Value2 FLOAT,@Warning int',@Value1,@Value2,@Warning;

								END
							
							UPDATE meters_value SET zzTime = GETDATE() WHERE id =@MeterID;

						END	
				END
			END
		END
END








