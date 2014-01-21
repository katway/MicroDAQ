

CREATE TRIGGER [dbo].[metersuuid_after_on_processitem]
ON [dbo].[ProcessItem]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN

--TRUNCATE TABLE meter_uuid
--INSERT INTO meter_uuid (uuid,id,alarm) SELECT id as uuid ,cast (slave as int )as id , alarm from ProcessItem;

truncate TABLE meters;
INSERT INTO meters SELECT slave as id , t.type FROM ProcessItem  p join meter_type t  on p.protocolType  = t.protocol;

END

GO