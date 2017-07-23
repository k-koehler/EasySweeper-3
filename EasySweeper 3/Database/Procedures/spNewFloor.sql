CREATE   PROCEDURE [dbo].[spNewFloor]	@Floor int,
				@Duration int,
				@Size int,
				@Mod int = NULL,
				@Bonus int = NULL,
				@Complexity int = NULL,
				@Image nvarchar(100) = NULL,
				@FloorID int = NULL OUTPUT
AS
SET NOCOUNT, XACT_ABORT ON

BEGIN TRY
	INSERT dbo.Floor
	(
		Floor,
		Duration,
		Size,
		Mod,
		Bonus,
		Complexity,
		Date
	)
	VALUES
	(
		@Floor,
		@Duration,
		@Size,
		@Mod,
		@Bonus,
		@Complexity,
		GETDATE()
	)
	
	SET @FloorID = SCOPE_IDENTITY()
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	EXEC spRaiseError
	RETURN 9999
END CATCH
GO
