ALTER PROCEDURE [dbo].[GetParameter]
(@MeterID int,
@uuid VARCHAR(100) OUTPUT,
@yellowMin FLOAT OUTPUT,
@yellowMax FLOAT OUTPUT,
@RedMin Float OUTPUT,
@RedMax FLOAT OUTPUT,
@Rate int OUTPUT)
AS
BEGIN
IF EXISTS(SELECT  id  FROM meters WHERE ID = @MeterID )
		BEGIN	
				SET @RedMin = (SELECT ISNULL(minimum, 0) FROM ProcessItem WHERE slave =@MeterID);
				SET @RedMax = (SELECT ISNULL(maximum, 0) FROM ProcessItem WHERE slave =@MeterID);
				SET @yellowMin =(SELECT ISNULL(yellowMin, 0) FROM ProcessItem WHERE slave =@MeterID);
				SET @yellowMax =(SELECT ISNULL(yellowMax, 0) FROM ProcessItem WHERE slave =@MeterID);

				SET @uuid = (SELECT uuid FROM meter_uuid WHERE id =@MeterID);

				SET @Rate = (SELECT ISNULL(updaterate,60) FROM ProcessItem WHERE id =@uuid);
		END
END

