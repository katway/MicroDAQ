GO
/****** Object:  Table [dbo].[ValueRate]    Script Date: 11/22/2013 10:46:55 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[ValueRate]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [dbo].[ValueRate]


CREATE TABLE [dbo].[ValueRate] (
[ID] int NOT NULL ,
[ValueRate] float(53) NOT NULL DEFAULT (1.00) 
)
ON [PRIMARY]
GO

CREATE INDEX [_WA_Sys_ID_573DED66] ON [dbo].[ValueRate]
([ID] ASC) 
ON [PRIMARY]
GO

CREATE TRIGGER [dbo].[updateProcessitemValueRate_AfterInsertUpdate_ValueRate]
ON [dbo].[ValueRate]
AFTER INSERT, UPDATE
AS
BEGIN

  DECLARE @id int;
  DECLARE @rate float;
  DECLARE MyCursor CURSOR FOR SELECT id,valuerate FROM valuerate ORDER BY id;

 OPEN MyCursor;
  FETCH NEXT FROM MyCursor INTO @id,@rate;
  

WHILE @@FETCH_STATUS = 0
    
BEGIN
    
        IF (@id =0 ) 
           BEGIN
               update processitem set valuerate = @rate;              
           END
         ELSE
           BEGIN
               update processitem set valuerate = @rate WHERE slave = @id;              
           END

        FETCH NEXT FROM MyCursor    INTO @id   ,@rate;
    
END


  CLOSE MyCursor

  DEALLOCATE MyCursor
END
GO