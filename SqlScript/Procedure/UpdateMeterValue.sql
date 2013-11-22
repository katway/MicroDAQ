



ALTER     PROCEDURE [dbo].[UpdateMeterValue]
(@MeterID int ,
@MeterType int ,
@MeterState int,
@Value1 float ,
@Value2 float,
@value3 float,
@Quality int,
@ResultState INT OUTPUT,
@RestultMessage VARCHAR(60) OUTPUT)
AS
BEGIN
		declare @uuid VARCHAR(100);
		DECLARE @uidM3 as varchar(50);
		DECLARE @yellowMin FLOAT;
		DECLARE @yellowMax FLOAT;
		DECLARE @RedMin Float;
		DECLARE @RedMax FLOAT;
		DECLARE @Rate INT;

--3.0版本将5.0粒子类型改为35，需要改回32
IF (@MeterType=35)
	BEGIN
		SET @MeterType=32;
	END

--3.1版本增加数据折算系统
	DECLARE @valueRate FLOAT;
IF ( EXISTS(SELECT  slave  FROM ProcessItem WHERE slave = @MeterID ) )
BEGIN
   SET @valueRate = (SELECT valuerate FROM ProcessItem WHERE slave = @MeterID);
   IF (@MeterType=32)
            BEGIN
      SET @Value1 = FLOOR(@Value1 * @valueRate);
      SET @Value2 = FLOOR(@Value2 * @valueRate);
      SET @Value3 = FLOOR(@Value3 * @valueRate);
       END
   ELSE
       BEGIN
                    SET @Value1 = @Value1 * @valueRate;
      SET @Value2 = @Value2 * @valueRate;
      SET @Value3 = @Value3 * @valueRate;
       END
END

--更新所有仪表的即时数据
		DECLARE @last FLOAT;
		IF (@MeterID < 11000)
			BEGIN
				EXEC UpdateTransientData2 @MeterID,@MeterType,@MeterState,@Quality,@Value1,@Value2,@value3,@last output,@ResultState OUTPUT ,@RestultMessage;
			END

		IF ( @Quality=192)
			BEGIN
				--更新受控设备的状态和数据
				EXEC UpdateRemoteControl @MeterID,@MeterType,@MeterState,@Value1,@Value2,@value3;
					
				IF (EXISTS(SELECT  id  FROM meters WHERE ID = @MeterID)  AND (@MeterState=1 OR @MeterState=8  OR @MeterState=16))
					BEGIN
											--查询上下限设置
						EXEC GetParameter @MeterID,@uuid OUTPUT,@yellowMin OUTPUT,@yellowMax OUTPUT,@RedMin OUTPUT,@RedMax OUTPUT,@Rate OUTPUT;
											--更新仪表数据
						DECLARE @toWrite as int;
						SET @toWrite =1;						
						SET @toWrite = (SELECT writeflag FROM ProcessItem WHERE slave = @MeterID);
						IF (@toWrite =0)
							BEGIN
								EXEC	UpdateHistoryData2 @MeterID,@MeterType,@MeterState,@Value1,@Value2,@value3,@last,@uuid ,@uidM3,@yellowMin,@yellowMax,@RedMin,@RedMax ,@rate ,@ResultState OUTPUT ,@RestultMessage;
							END
					END
			END
END
		
        


	

























