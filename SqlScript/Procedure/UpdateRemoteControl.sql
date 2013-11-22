
ALTER procedure [dbo].[UpdateRemoteControl](
@MeterID int ,
@MeterType int ,
@MeterState int,
@Value1 float ,
@Value2 float,
@value3 float)
as
begin
--更新受控设备的状态和数据
 DECLARE @slave AS VARCHAR(10);
 SET @slave =  CAST(@MeterId AS VARCHAR(10));
 IF (EXISTS(SELECT  slave FROM RemoteControl WHERE slave = @MeterID ))
			BEGIN
				UPDATE RemoteControl SET remainSecond = cast(FLOOR(@Value1) as int ) & 65535  WHERE slave =@slave;
				UPDATE RemoteControl SET state = @MeterState WHERE slave =@MeterID;
			END
End
