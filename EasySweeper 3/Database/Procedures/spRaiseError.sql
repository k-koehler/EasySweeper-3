CREATE OR ALTER PROCEDURE [dbo].[spRaiseError] 
AS
SET NOCOUNT, XACT_ABORT ON

DECLARE	@ErrorMessage nvarchar(2048),
	@Severity tinyint,
	@State tinyint,
	@ErrorNumber int,
	@Procedure sysname,
	@LineNumber int

SELECT	@ErrorMessage = ERROR_MESSAGE(),
	@Severity = ERROR_SEVERITY(),
	@State = ERROR_STATE(),
	@ErrorNumber = ERROR_NUMBER(),
	@Procedure = ERROR_PROCEDURE(),
	@LineNumber = ERROR_LINE()

IF @ErrorMessage NOT LIKE '***%'
BEGIN
	SELECT	@ErrorMessage =	'*** ' +
				COALESCE(QUOTENAME(@Procedure), 'Dynamic SQL') +
				', Line ' + LTRIM(STR(@LineNumber)) +
				'. Errno ' + LTRIM(STR(@ErrorNumber)) +
				':  ' + @ErrorMessage					
END
RAISERROR(@ErrorMessage, @Severity, @State, @ErrorMessage)