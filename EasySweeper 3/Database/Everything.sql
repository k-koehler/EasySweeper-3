USE master
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
Use EasySweeper
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
	Image nvarchar(100) NULL
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
		FloorID int NOT NULL
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
GO
CREATE OR ALTER PROCEDURE spFloorAdd	@Floor int,
				@Duration bigint,
				@Size nvarchar(20),
				@FloorParticipants dbo.FloorParticipants READONLY,
				@Mod int = NULL,
				@Bonus int = NULL,
				@Complexity int = NULL
AS
SET NOCOUNT, XACT_ABORT ON

BEGIN TRY
	BEGIN TRAN
		DECLARE	@FloorID int,
			@SizeID int

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
		

		-- Update the player table with any new names
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
			PlayerID
		)
		SELECT	@FloorID,
			P.ID
		FROM	#FloorParticipants FP
			INNER JOIN dbo.Player P ON FP.Name = P.Name
	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	EXEC spRaiseError
	RETURN 9999
END CATCH		
GO
CREATE OR ALTER PROCEDURE spTestConnection
AS
SELECT 1