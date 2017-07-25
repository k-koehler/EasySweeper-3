CREATE TYPE [dbo].[StringSet] AS TABLE
(
	[Value] [nvarchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
CREATE TYPE [dbo].[IntSet] AS TABLE
(
	[Value] [int] NULL
)
GO
CREATE TYPE [dbo].[FloorParticipants] AS TABLE
(
	[Name] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Position] [int] NOT NULL,
	[UnknownName] [bit] NOT NULL
)
GO
CREATE TABLE [dbo].[PlayerFloor]
(
	[PlayerID] [int] NOT NULL,
	[FloorID] [int] NOT NULL,
	[Position] [int] NOT NULL,
	CONSTRAINT [PK_PlayerFloor] PRIMARY KEY CLUSTERED
	(
		[PlayerID] ASC,
		[FloorID] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

CREATE TABLE [dbo].[Player]
(
	[ID] [int] IDENTITY (1,1) NOT NULL,
	[Name] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT [PK__Player__3214EC279BD924E3] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

CREATE TABLE [dbo].[RequestHistory]
(
	[NetAddress] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LastFloorCreationDate] [datetime2] NULL,
	[Login] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT [PK__FloorCre__F538721E7C35DAB7] PRIMARY KEY CLUSTERED
	(
		[NetAddress] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

CREATE TABLE [dbo].[SizeLookup]
(
	[ID] [int] NOT NULL,
	[Name] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT [PK__SizeLook__3214EC278D7DE51E] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

CREATE TABLE [dbo].[APIUser]
(
	[APIKey] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT [PK__APIUser__BB6E626C46BC6C15] PRIMARY KEY CLUSTERED
	(
		[APIKey] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

CREATE TABLE [dbo].[Floor]
(
	[ID] [int] IDENTITY (1,1) NOT NULL,
	[Floor] [int] NOT NULL,
	[Bonus] [int] NULL,
	[Mod] [int] NULL,
	[Size] [int] NOT NULL,
	[Complexity] [int] NULL,
	[Image] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Date] [datetime2] NOT NULL,
	[Duration] [int] NOT NULL,
	CONSTRAINT [PK__Floor__3214EC27C3BD0DB1] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO
CREATE OR ALTER VIEW [dbo].[vwFloor]
AS
SELECT	F.ID,
	F.Duration,
	P.P1,
	P.P2,
	P.P3,
	P.P4,
	P.P5,
	F.Floor,
	F.Bonus,
	F.Mod,
	SL.ID [SizeID],
	SL.Name [Size],
	F.Complexity,
	F.Image,
	F.Date
FROM	Floor F
	INNER JOIN dbo.SizeLookup SL ON F.Size = SL.ID
	CROSS APPLY dbo.tfnFloorWithPlayersTable(F.ID) P
GO
CREATE OR ALTER FUNCTION [dbo].[tfnFloorWithPlayersTable]
(
	@FloorID int
)
RETURNS TABLE
AS
RETURN
SELECT	
(
	SELECT	P.Name
	FROM	dbo.PlayerFloor PF
		INNER JOIN dbo.Player P ON PF.PlayerID = P.ID
	WHERE	PF.FloorID = @FloorID
	AND	PF.Position = 0
) AS [P1],
(
	SELECT	P.Name
	FROM	dbo.PlayerFloor PF
		INNER JOIN dbo.Player P ON PF.PlayerID = P.ID
	WHERE	PF.FloorID = @FloorID
	AND	PF.Position = 1
	
) AS [P2],
(
	SELECT	P.Name
	FROM	dbo.PlayerFloor PF
		INNER JOIN dbo.Player P ON PF.PlayerID = P.ID
	WHERE	PF.FloorID = @FloorID
	AND	PF.Position = 2
) AS [P3],
(
	SELECT	P.Name
	FROM	dbo.PlayerFloor PF
		INNER JOIN dbo.Player P ON PF.PlayerID = P.ID
	WHERE	PF.FloorID = @FloorID
	AND	PF.Position = 3
) AS [P4],
(
	SELECT	P.Name
	FROM	dbo.PlayerFloor PF
		INNER JOIN dbo.Player P ON PF.PlayerID = P.ID
	WHERE	PF.FloorID = @FloorID
	AND	PF.Position = 4
) AS P5
GO
CREATE OR ALTER   FUNCTION [dbo].[tfnFloorWithPlayers]
(
	@FloorID int
)
RETURNS TABLE
AS
RETURN
SELECT	@FloorID [FloorID],
	dbo.fnRemoveFirstCharacter
	((
		SELECT	',' + CONVERT(nvarchar(10), PF.PlayerID)
		FROM	dbo.PlayerFloor PF
		WHERE	PF.FloorID = F.ID
		ORDER BY	PF.Position
		FOR	XML PATH ('')
	))
	AS PlayerIDs,
	dbo.fnRemoveFirstCharacter
	((	
		SELECT	',' + CONVERT(nvarchar(200), P.Name)
		FROM	dbo.PlayerFloor PF
			INNER JOIN dbo.Player P ON PF.PlayerID = P.Id
		WHERE	PF.FloorID = F.ID
		ORDER BY	PF.Position
		FOR	XML PATH ('')		
	))AS PlayerNames
FROM	Floor F
WHERE	F.ID = @FloorID
GO
CREATE OR ALTER   FUNCTION [dbo].[fnRemoveFirstCharacter]
(
	@Str nvarchar(max)
)
RETURNS nvarchar(max)
AS
BEGIN
RETURN	RIGHT(@str, LEN(@Str)-1)
END
GO
CREATE OR ALTER   FUNCTION [dbo].[tfnFloorDuplicates]
(
	@FloorID int
)
RETURNS TABLE
AS
RETURN
SELECT	F2.ID
FROM	dbo.Floor F1
	INNER JOIN dbo.Floor F2 ON F1.ID <> F2.ID
		AND F1.Floor = F2.Floor
		AND F1.Duration = F2.Duration
		AND F1.Bonus = F2.Bonus
		AND F1.Mod = F2.Mod
		AND F1.Size = F2.Size
		AND F1.Complexity = F2.Complexity
	CROSS APPLY dbo.tfnFloorWithPlayers(F1.ID) PF1
	CROSS APPLY dbo.tfnFloorWithPlayers(F2.ID) PF2
WHERE	F1.ID = @FloorID
AND	F2.ID <> @FloorID
AND	PF1.PlayerIDs = PF2.PlayerIDs
GO
CREATE OR ALTER   FUNCTION [dbo].[tfnFloorDuplicatesList]
(
	@FloorID int
)
RETURNS TABLE
AS
RETURN
SELECT
(
	SELECT	CONVERT(nvarchar(100), ID) + ' '
	FROM	dbo.tfnFloorDuplicates(@FloorID)
	FOR	XML PATH('')
) AS MatchingIDs
GO
CREATE OR ALTER   PROCEDURE [dbo].[spRaiseError] 
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
GO
CREATE OR ALTER PROCEDURE [dbo].[spFloorSearch]		@FloorIDs dbo.IntSet READONLY,
					@FloorParticipants dbo.FloorParticipants READONLY,
					@DurationFrom int = NULL,
					@DurationTo int = NULL,
					@Bonuses dbo.IntSet READONLY,
					@Mods dbo.IntSet READONLY,
					@Sizes dbo.StringSet READONLY,
					@Complexities dbo.IntSet READONLY,
					@Image nvarchar(100) = NULL,
					@DateFrom datetime2(0) = NULL,
					@DateTo datetime2(0) = NULL
AS

SET NOCOUNT, XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

BEGIN TRY
	DECLARE	@Select nvarchar(max),
		@From nvarchar(max),
		@Where nvarchar(max),
		@Params nvarchar(max) = '',
		@Sql nvarchar(max),
		@SizeLookupID int
SET @Select = N' 
SELECT	F.ID,
	F.P1,
	F.P2,
	F.P3,
	F.P4,
	F.P5,
	F.Duration,
	F.Floor,
	F.Bonus,
	F.Mod,
	F.Size,
	F.Complexity,
	F.Image,
	F.Date	
'

SET @From = N'
FROM	dbo.vwFloor F
'

SET @Where =' 
WHERE	1=1
'

IF EXISTS (SELECT * FROM @FloorIDs)
	SET @Where = @Where + N' AND F.ID IN (SELECT Value FROM @FloorIDs)'


IF @DurationFrom IS NOT NULL
	SET @Where = @Where + N' AND F.Duration >= @DurationFrom '

IF @DurationTo IS NOT NULL
	SET @Where = @Where + N' AND F.Duration <= @DurationTo '

IF EXISTS (SELECT * FROM @Bonuses)
	SET @Where = @Where + N' AND F.ID IN (SELECT Value FROM @Bonus)'

IF EXISTS (SELECT * FROM @Mods)
	SET @Where = @Where + N' AND F.ID IN (SELECT Value FROM @Mod)'

IF EXISTS (SELECT * FROM @Sizes)
BEGIN
	SELECT	DISTINCT SL.ID
	INTO	#SizeIDs
	FROM	@Sizes S
		INNER JOIN SizeLookup SL ON S.Value LIKE SL.Name

	SET @Where = @Where + N' AND F.SizeID IN (SELECT ID FROM #SizeIDs)'
END
	
IF EXISTS (SELECT * FROM @Complexities)
	SET @Where = @Where + N' AND F.ID IN (SELECT Value FROM @Complexities)'

IF @Image IS NOT NULL
	SET @Where = @Where + N' AND F.Image LIKE ''%'' + @Image + ''%'' '

IF @DateFrom IS NOT NULL
	SET @Where = @Where + N' AND F.Date >= @DateFrom '

IF @DateTo IS NOT NULL
	SET @Where = @Where + N' AND F.Date <= @DateTo '

SET @Sql = @Select + @From + @Where

SET @Params = '@FloorIDs dbo.IntSet READONLY,
	     @DurationFrom int,
	     @DurationTo int,
	     @Bonuses dbo.IntSet READONLY,
	     @Mods dbo.IntSet READONLY,	     
	     @Complexities dbo.IntSet READONLY,
	     @Image nvarchar(100),
	     @DateFrom datetime2(0),
	     @DateTo datetime2(0)'

EXEC sys.sp_executesql @SQL, @Params, 
	@FloorIDs = @FloorIDs,
	@DurationFrom = @DurationFrom,
	@DurationTo = @DurationTo,
	@Bonuses = @Bonuses,
	@Mods = @Mods,
	@Complexities = @Complexities,
	@Image = @Image,
	@DateFrom = @DateFrom,
	@DateTo = @DateTo 

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	EXEC spRaiseError
	RETURN 9999
END CATCH
GO
CREATE OR ALTER   PROCEDURE [dbo].[spNewFloor]	@Floor int,
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
CREATE OR ALTER   PROCEDURE spTestConnection
AS
SELECT 1
GO
CREATE OR ALTER   PROCEDURE [dbo].[spFloorAdd]	@Floor int,
				@Duration int,
				@Size nvarchar(20),
				@FloorParticipants dbo.FloorParticipants READONLY,
				@Mod int = NULL,
				@Bonus int = NULL,
				@Complexity int = NULL,
				@Image nvarchar(100) = NULL,
				@FloorID int = NULL OUTPUT
AS
SET NOCOUNT, XACT_ABORT ON

BEGIN TRY

	DECLARE	@SizeID int,
		@MatchingIDs nvarchar(100)

	-- Convert from our size, to Size Lookup ID
	SELECT	@SizeID = ID
	FROM	SizeLookup
	WHERE	LOWER(Name) = LOWER(@Size)

	-- We need to sanitise the input table, so have to pull out everything into a temp table
	SELECT	*
	INTO	#FloorParticipants
	FROM	@FloorParticipants
	
	-- Set any names to a name the database will recognise, in the case of an unknown name
	-- There will be some names in the Player table that are (Unknown 1), (Unknown 2) etc
	-- So we want to match these
	UPDATE	#FloorParticipants			
	SET	Name = N'(Unknown ' + CAST(Position AS nvarchar) + ')'
	WHERE	UnknownName = 1

	BEGIN TRAN
		-- We need to CREATE OR ALTER a floor, before regestering our participants
		-- As we need the ID of the floor (passed out via @FloorID)
		EXEC spNewFloor	@Floor = @Floor,
				@Duration = @Duration,
				@Size = @SizeID,
				@Mod = @Mod,
				@Bonus = @Bonus,
				@Complexity = @Complexity,
				@Image = @Image,
				@FloorID = @FloorID OUTPUT	

		-- Update the player table with only new names
		MERGE	dbo.Player AS P --Target
		USING	#FloorParticipants AS FP --Source
		ON	P.Name = FP.Name
		WHEN NOT MATCHED BY TARGET THEN
			INSERT
			(
				Name
			)
			VALUES
			(
				FP.Name
			)
		;

		INSERT dbo.PlayerFloor
		(
			FloorID,
			PlayerID,
			Position
		)
		SELECT	@FloorID,
			P.ID,
			FP.Position
		FROM	#FloorParticipants FP
			INNER JOIN dbo.Player P ON FP.Name = P.Name

		SELECT	@MatchingIDs = MatchingIDs
		FROM	tfnFloorDuplicatesList(@FloorID)

		-- Deal with potential duplicate floors here...
		-- This relies on the transaction being rolled back in the catch block, which is rather lazy
		IF ISNULL(@MatchingIDs, '') <> ''
		BEGIN
			SET @MatchingIDs = 'Duplicate Floor Detected! Floor IDs: ' + @MatchingIDs
			RAISERROR (@MatchingIDs, 16, 1)
		END
	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN
	EXEC spRaiseError
	RETURN 9999
END CATCH
GO
CREATE OR ALTER   PROCEDURE spAPIKeyCheck	@Key uniqueidentifier,
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
CREATE OR ALTER   PROCEDURE [dbo].[Clean]
AS
TRUNCATE TABLE Player
TRUNCATE TABLE FLoor
TRUNCATE TABLE PlayerFLoor
GO
CREATE OR ALTER PROCEDURE	 [dbo].[spRequestLog]
AS
BEGIN TRY
	IF EXISTS
	(
		SELECT	*
		FROM	dbo.RequestHistory
		WHERE	NetAddress = CONNECTIONPROPERTY('client_net_address')
	)
	BEGIN
		UPDATE	dbo.RequestHistory
		SET	LastFloorCreationDate = GETDATE(),
			Login = SYSTEM_USER
	END
	ELSE
	BEGIN
		INSERT	dbo.RequestHistory 
		(
			NetAddress,
			LastFloorCreationDate,
			Login
		)
		VALUES 
		(
			CONVERT( nvarchar(48), CONNECTIONPROPERTY('client_net_address')),
			GETDATE(),
			SYSTEM_USER
		)
	END
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN
	EXEC spRaiseError
	RETURN 9999
END CATCH
GO
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
			'Too soon since last floor CREATE OR ALTER request - try again in ' + CAST(@SecondsSinceLastFloor AS nvarchar(10)) + ' seconds'
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
CREATE USER [EasyWinterface] FOR LOGIN [EasyWinterface] WITH DEFAULT_SCHEMA=[EasyWinterface]
GO
GRANT EXECUTE ON TYPE :: dbo.FloorParticipants TO EasyWinterface
GO
GRANT EXECUTE ON spTestConnection TO EasyWinterface
GO
GRANT EXECUTE ON spAPIKeyCheck TO EasyWinterface
GO
GRANT EXECUTE ON TYPE :: dbo.IntSet TO EasyWinterface
GO
GRANT EXECUTE ON TYPE :: dbo.StringSet TO EasyWinterface