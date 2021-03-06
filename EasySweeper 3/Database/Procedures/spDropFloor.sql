CREATE PROCEDURE [dbo].[spDropFloor]	@FloorID int
AS
SET NOCOUNT, XACT_ABORT ON
BEGIN TRY
	BEGIN TRAN
		DELETE	dbo.PlayerFloor
		WHERE	FloorID = @FloorID

		DELETE	dbo.Floor
		WHERE	ID = @FloorID
	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	EXEC spRaiseError
	RETURN 9999
END CATCH
GO
