CREATE OR ALTER PROCEDURE [dbo].[spSuspiciousFloorRequestCheck]	@SuspiciousMessage nvarchar(max) OUTPUT,
							@SecondsSinceLastFloor int = NULL
AS
/*

Procedure to check whether or not a new floor request is suspicious or not.
The result is set in the @Suspicious output parameter, if set to NULL then floor deemed not suspicious.
The @Suspicious parameter will contain messages delimited by a newline if there are more than 1 thing wrong with the request!

*/


SET NOCOUNT, XACT_ABORT ON

BEGIN TRY
	-- Bail out straight away if we're doing Dev
	IF SYSTEM_USER = 'tresamigos'
	BEGIN
	
		SET @SuspiciousMessage = NULL
		RETURN 0
	END
	
	DECLARE @LastDate datetime2(0) =
	(
		SELECT	LastFloorCreationDate
		FROM	dbo.RequestHistory
		WHERE	NetAddress = CONNECTIONPROPERTY('client_net_address')
	)
	
	EXEC spRequestLog
	
	DECLARE @Messages TABLE
	(
		Message nvarchar(500)
	)
	
	DECLARE @NewLine char(2) = CHAR(13) + CHAR(10)
	
	
	SET @SuspiciousMessage = ''
	SET @SecondsSinceLastFloor = ISNULL(@SecondsSinceLastFloor, 240)
	
	-- If the user is sending floor requests too fast, we're not happy!!
	IF DATEADD(SECOND, @SecondsSinceLastFloor, @LastDate) < GETDATE()
		INSERT @Messages
		VALUES
		(
			'Too soon since last floor create request - try again in ' + CAST(@SecondsSinceLastFloor AS nvarchar(10)) + ' seconds'
		)
	
	
	SELECT	@SuspiciousMessage = Message + @NewLine
	FROM	@Messages
	
	-- Get rid of the last pesky newline
	SET @SuspiciousMessage = LEFT(@SuspiciousMessage, LEN(@SuspiciousMessage) -2)

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN
	EXEC spRaiseError
	RETURN 9999
END CATCH
GO
