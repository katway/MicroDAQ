
ALTER  PROCEDURE [dbo].[UpdateControlState]
(@MeterId int,
@Command int,
@CommandState int,
@CommandDate int
)
AS
BEGIN
	DECLARE @slave AS VARCHAR(10);
	SET @slave =  CAST(@MeterId AS VARCHAR(10));
	IF ( EXISTS( SELECT slave FROM RemoteControl  WHERE slave = @slave ))
		BEGIN TRANSACTION
			UPDATE RemoteControl SET CmdState = @CommandState WHERE slave LIKE @slave;
		COMMIT;

END
