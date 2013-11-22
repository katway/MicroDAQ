ALTER  PROCEDURE [dbo].[UpdateAlarm]
(
   @meterID INT,
   @MeterType INT,
   @value1 FLOAT,
   @uuid VARCHAR(100),
   @yellowMin FLOAT,
   @yellowMax FLOAT,
   @redMin Float,
   @redMax FLOAT,
   @rate INT
)
AS
BEGIN
    DECLARE @logic varchar(10);
	SET @logic = null;	
	DECLARE @lastAlert DateTime;
	SET @lastAlert = (SELECT [alerttime] FROM meters_value WHERE id = @MeterID);
	
	IF (@lastAlert is null OR ABS(DATEDIFF(ss,@lastAlert, GETDATE())) >= @rate)
			BEGIN
					SET @logic = (SELECT TOP 1 logic  FROM ProcessItem WHERE id = @uuid);
							IF (@logic ='between')
									BEGIN
											DECLARE @alertid VARCHAR(32);
				
											IF ( @Value1 < @yellowMin OR  @Value1 >@yellowMax)
													BEGIN
															SET @alertid = REPLACE(NEWID(), '-', '');
															--INSERT INTO ProcessItemAlertRecord (id,processItemId,[timestamp],[value],[confirm])
																					--VALUES(@alertid, @uuid,GETDATE(),@Value1,'未确认');
															--UPDATE ProcessItem SET alertRecordId = @alertid WHERE slave = @MeterID;
															UPDATE meters_value SET alertTime = GETDATE() WHERE id =@MeterID;
													END
											
									END
                   --计算报警灯状态
					IF (1=1 ) 
							BEGIN
									IF  (@logic ='between')
									BEGIN

											IF (@Value1 < @yellowMax AND @Value1 > @yellowMin)
												BEGIN
													--未超标
													UPDATE meters_value SET alert =1 WHERE id = @MeterID;
												END
											ELSE
												BEGIN
													IF (@Value1 < @RedMin OR @Value1 > @RedMax )
														BEGIN
															--超红线
															UPDATE meters_value SET alert = 12 WHERE id = @MeterID;
														END
													ELSE
														BEGIN
															--超黄线
															UPDATE meters_value SET alert = 2 WHERE id = @MeterID;
														END
												END
										END
										ELSE
										BEGIN
												UPDATE meters_value SET alert = 1 WHERE id = @MeterID;
										END
							END

	END
		
END
