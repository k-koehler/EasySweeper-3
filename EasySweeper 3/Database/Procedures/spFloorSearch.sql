CREATE PROCEDURE [dbo].[spFloorSearch]		@FloorIDs dbo.IntSet READONLY,
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
