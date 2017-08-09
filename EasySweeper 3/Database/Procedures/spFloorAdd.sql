CREATE OR ALTER PROCEDURE [dbo].[spFloorAdd]	@Floor int,
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

DECLARE @S nvarchar(max) =
(
SELECT	*
FROM	@FloorParticipants
FOR XML RAW
)
INSERT SEARCH 
VALUES (@S + @Size + CAST(@Floor AS nvarchar(3)) + N' ' + CAST(@Duration AS nvarchar(10)))

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
	-- If we're given a null name or an empty string as a name
	-- AND the caller thinks that this name is correct something has probably gone wrong
	-- we'll just ignore this player cause fuck knows what has happened...
	WHERE	ISNULL(Name, '') <> ''
	AND	UnknownName < 1
	
	-- Set any names to a name the database will recognise, in the case of an unknown name
	-- There will be some names in the Player table that are (Unknown 1), (Unknown 2) etc
	-- So we want to match these
	UPDATE	#FloorParticipants			
	SET	Name = N'(Unknown ' + CAST(Position AS nvarchar) + ')'
	WHERE	UnknownName = 1

	BEGIN TRAN
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


