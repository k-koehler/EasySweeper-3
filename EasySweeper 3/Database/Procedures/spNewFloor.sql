CREATE OR ALTER PROCEDURE spNewFloor	@Floor int,
				@Duration bigint,
				@Size int,
				@Mod int = NULL,
				@Bonus int = NULL,
				@Complexity int = NULL,
				@FloorID int = NULL OUTPUT
AS
SET NOCOUNT, XACT_ABORT ON

BEGIN TRY
	BEGIN TRAN
		
		INSERT dbo.Floor
		(
			Floor,
			Duration,
			Size,
			Mod,
			Bonus,
			Complexity
		)
		VALUES
		(
			@Floor,
			@Duration,
			@Size,
			@Mod,
			@Bonus,
			@Complexity
		)

		SET @FloorID = SCOPE_IDENTITY()

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	EXEC spRaiseError
	RETURN 9999
END CATCH		
