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
	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN
	EXEC spRaiseError
	RETURN 9999
END CATCH