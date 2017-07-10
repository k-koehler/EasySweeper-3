USE [master]
GO
DROP LOGIN EasySweeper
GO
CREATE LOGIN [EasySweeper] WITH PASSWORD=N'fkfut', DEFAULT_DATABASE=[EasySweeper], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

IF NOT EXISTS
(
	SELECT	*
	FROM	sys.databases
	WHERE	Name = 'EasySweeper'
)
CREATE DATABASE EasySweeper
GO
USE EasySweeper
GO
CREATE USER [EasySweeper] FOR LOGIN [EasySweeper] WITH DEFAULT_SCHEMA=[EasySweeper]
GO
IF NOT EXISTS
(
	SELECT	*
	FROM	sys.tables
	WHERE	name = 'Player'
)
CREATE TABLE dbo.Player
(
	ID int PRIMARY KEY IDENTITY(1,1),
	Name nvarchar(20) NOT NULL
)
GO
IF NOT EXISTS
(
	SELECT	*
	FROM	sys.tables
	WHERE	name = 'Floor'
)
CREATE TABLE dbo.Floor
(
	ID int PRIMARY KEY IDENTITY(1,1),
	Floor int NOT NULL,
	Duration bigint NOT NULL,
	Bonus int NULL,
	Mod int NULL,
	Size int NOT NULL,
	Complexity int NULL,
	Image nvarchar(100) NULL,
	Date datetime2(0) NOT NULL
)
GO
IF NOT EXISTS
(
	SELECT	*
	FROM	sys.tables
	WHERE	Name = 'PlayerFloor'
)
BEGIN
	CREATE TABLE dbo.PlayerFloor
	(
		PlayerID int NOT NULL,
		FloorID int NOT NULL,
		Position int NOT NULL
	)
	ALTER TABLE dbo.PlayerFloor
	ADD CONSTRAINT PK_PlayerFloor
	PRIMARY KEY (PlayerID, FloorID)
END
GO
IF NOT EXISTS
(
	SELECT	*
	FROM	sys.tables
	WHERE	Name = 'SizeLookup'
)
CREATE TABLE dbo.SizeLookup
(
	ID int PRIMARY KEY,
	Name nvarchar(20)
)
GO
DELETE dbo.SizeLookup
GO
INSERT dbo.SizeLookup
(
	ID,
	Name
)
VALUES
(
	1,
	N'Small'
),
(
	2,
	N'Medium'
),
(
	3,
	N'Large'
)
GO
DROP TYPE FloorParticipants
GO
CREATE TYPE FloorParticipants AS TABLE
(
	Name nvarchar(20) NOT NULL,
	Position int NOT NULL,
	UnknownName bit NOT NULL
)
GO
GRANT EXECUTE ON TYPE :: dbo.FloorParticipants TO EasySweeper
GO
GO
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
GO
CREATE OR ALTER PROCEDURE [dbo].[spNewFloor]	@Floor int,
					@Duration bigint,
					@Size int,
					@Mod int = NULL,
					@Bonus int = NULL,
					@Complexity int = NULL,
					@Image nvarchar(100) = NULL,
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

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	EXEC spRaiseError
	RETURN 9999
END CATCH		
GO
CREATE OR ALTER PROCEDURE [dbo].[spFloorAdd]	@Floor int,
					@Duration bigint,
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
	BEGIN TRAN
		DECLARE	@SizeID int,
			@MatchingIDs nvarchar(100)

		-- Convert from our size, to Size Lookup ID
		SELECT	@SizeID = ID
		FROM	SizeLookup
		WHERE	LOWER(Name) = LOWER(@Size)

		-- We need to create a floor, before regestering our participants
		-- As we need the ID of the floor (passed out via @FloorID)
		EXEC spNewFloor	@Floor = @Floor,
				@Duration = @Duration,
				@Size = @SizeID,
				@Mod = @Mod,
				@Bonus = @Bonus,
				@Complexity = @Complexity,
				@Image = @Image,
				@FloorID = @FloorID OUTPUT

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

		SELECT @MatchingIDs = 
		(	
			SELECT	CONVERT(nvarchar(100), ID) + ' '
			FROM	dbo.tfnFloorDuplicates(@FloorID)
			FOR	XML PATH('')
		)
		-- Deal with potential duplicate floors here...
		-- This relies on the transaction being rolled back in the catch block, which is rather lazy
		IF ISNULL(@MatchingIDs, '') <> ''
		BEGIN
			SET @MatchingIDs = 'Duplicate Floor Detected! Floor IDs: ' + @MatchingIDs
			RAISERROR (@MatchingIDs, 16, 1)
		END

		SELECT @MatchingIDs
	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN
	EXEC spRaiseError
	RETURN 9999
END CATCH		

GO
DROP USER EasySweeper
GO
CREATE USER EasySweeper
GO
GRANT EXECUTE ON spFloorAdd TO EasySweeper
GO
CREATE OR ALTER PROCEDURE spTestConnection
AS
SELECT 1
GO
GRANT EXECUTE ON spTestConnection TO EasySweeper
GO
CREATE OR ALTER PROCEDURE [dbo].[Clean]
AS
TRUNCATE TABLE Player
TRUNCATE TABLE FLoor
TRUNCATE TABLE PlayerFLoor 
GO
ALTER FUNCTION [dbo].[tfnFloorDuplicates]
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
ALTER FUNCTION [dbo].[tfnFloorWithPlayers]
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
