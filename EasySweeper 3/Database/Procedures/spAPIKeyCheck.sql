CREATE OR ALTER PROCEDURE spAPIKeyCheck	@Key uniqueidentifier,
				@Valid bit = 0 OUTPUT
AS
BEGIN TRY
	BEGIN TRAN
		SELECT	@Valid = 
			CASE WHEN EXISTS
			(
				SELECT	*
				FROM	dbo.APIUser
			)
			THEN 1
			ELSE 0
			END
	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN
	EXEC spRaiseError
	RETURN 9999
END CATCH
GO
