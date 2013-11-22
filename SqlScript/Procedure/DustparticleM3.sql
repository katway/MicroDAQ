CREATE PROCEDURE [dbo].[DustparticleM3]
  @MeterID AS int,
	@MeterType as int,
	@MeterState as int,
	@value1 as FLOAT,
	@value2 as FLOAT,
	@value3 as FLOAT
AS
BEGIN
	DECLARE @uidM3 as VARCHAR(32);	
	DECLARE @uuid as VARCHAR(32);	
	DECLARE @warning as int ;
	DECLARE @last as DATE;
	SET @uidM3=null;
							IF (@meterType=32)
								BEGIN
									IF (@MeterID<10000)
										BEGIN
											DECLARE @idM3 int;
											SET @idM3 = @MeterID+30000;
											SET @uidM3 = (SELECT TOP 1 uuid FROM meter_uuid WHERE id =@idM3);
									
										END
									
									IF (@uidM3 is not null)
										BEGIN
											DECLARE @sql3 as nvarchar(500);
											DECLARE @now as datetime;
											SET @now = GETDATE();


											DECLARE @GetM3SQL as nvarchar(500);
											DECLARE @V1 FloAT;
											DECLARE @V2 FLOAT;
											--SET @GetM3SQL ='SELECT @V1 = ISNULL(SUM(value),0), @V2 = ISNULL(SUM(value2),0),GETDATE() FROM ZZ' + @uuid + ' WHERE timestamp > DATEADD(second,-2070,GETDATE()) AND timestamp < GETDATE()';
											SET @GetM3SQL ='SELECT @V1 = SUM(value), @V2 = SUM(value2) FROM ZZ' + @uuid + ' WHERE timestamp > DATEADD(second,-2070,GETDATE()) AND timestamp < GETDATE()';
											EXEC sp_executesql @GetM3SQL,N'@V1 FLOAT OUTPUT,@V2 FLOAT OUTPUT',@V1 OUTPUT,@V2 OUTPUT;
											
											EXEC UpdateTransientData2 @idM3,@MeterType,@MeterState,192, @V1,@V2,0,@last OUTPUT,@ResultState OUTPUT,@RestultMessage OUTPUT;
											--SET @sql3 = 'INSERT INTO ZZ'+ @uidM3 + ' (value,value2,timestamp)  SELECT ISNULL(SUM(value),0), ISNULL(SUM(value2),0),GETDATE() FROM ZZ' + @uuid + ' WHERE timestamp > DATEADD(second,-2070,GETDATE()) AND timestamp < GETDATE()';
											--SET @sql3 = 'INSERT INTO ZZ'+ @uidM3 + ' (value,value2,timestamp)  SELECT @V1,@V2,GETDATE()'
											SET @sql3 = 'INSERT INTO ZZ'+ @uidM3 + '  (value,value2,timestamp,productionState,flag) '
																									+' SELECT @V1 AS value,@V2 AS value2,GETDATE() AS timestamp,productionState  AS productionState,@warning AS flag FROM processItem '
																									+' WHERE ID = '''+ @uuid +'''';
											EXEC sp_executesql @sql3,N'@V1 FLOAT,@V2 FLOAT,@Warning int',@V1,@V2,@Warning;
											UPDATE meters_value SET zztime = GETDATE() WHERE ID = @idM3;
										END
								END

END