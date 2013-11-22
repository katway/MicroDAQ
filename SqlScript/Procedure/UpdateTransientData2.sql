
ALTER  PROCEDURE [dbo].[UpdateTransientData2]
(@MeterID int ,
@MeterType int ,
@MeterState int,
@Quality int,
@Value1 float ,
@Value2 float,
@value3 float,
@last FLOAT OUTPUT,
@ResultState INT OUTPUT,
@RestultMessage VARCHAR(60) OUTPUT)
AS
--更新所有仪表的即时数据
BEGIN

		IF (EXISTS(SELECT  id  FROM meters_value WHERE ID = @MeterID ))
			BEGIN
				SET @last = (SELECT [value1] FROM meters_value WHERE id = @MeterID);
					UPDATE meters_value 
								SET type = @MeterType,
										state = @MeterState,
										value1 = @Value1,
										value2= @Value2,
										value3= @value3,
                    quality=@Quality,
										[time]= GETDATE()
								WHERE id=@MeterID
			END
		--否则插入此仪表的数据
		ELSE
			BEGIN
					--INSERT INTO meters_value (id,time,value1,value2,value3,type,state) VALUES( SELECT @MeterID,GETDATE(),@Value1,@Value2,@value3,@MeterType as type ,@MeterState as state );
				INSERT INTO meters_value (id,time,value1,value2,value3,type,state,quality)  SELECT @MeterID,GETDATE(),@Value1,@Value2,@value3,@MeterType as type ,@MeterState as state,@Quality ;
			END

		declare @uuid VARCHAR(100);
		DECLARE @uidM3 as varchar(50);
		DECLARE @yellowMin FLOAT;
		DECLARE @yellowMax FLOAT;
		DECLARE @RedMin Float;
		DECLARE @RedMax FLOAT;
		DECLARE @Rate INT;
		IF (EXISTS(SELECT  id  FROM meter_uuid WHERE ID = @MeterID) AND (@MeterState=1 OR @MeterState = 8 OR @MeterState = 16))
         BEGIN
             --查询上下限设置
			EXEC GetParameter @MeterID,@uuid OUTPUT,@yellowMin OUTPUT,@yellowMax OUTPUT,@RedMin OUTPUT,@RedMax OUTPUT,@Rate OUTPUT;
            
             --写入报警纪录,更新报警灯状态
            EXEC UpdateAlarm @MeterID,@MeterType,@Value1,@uuid,@yellowMin,@yellowMax,@RedMin,@RedMax,@Rate;
         END
END






