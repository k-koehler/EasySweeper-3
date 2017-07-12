ALTER PROCEDURE spFloorSearchSingle	@FloorID int = NULL,
				@DurationFrom bigint = NULL,
				@DurationTo bigint = NULL,
				@Bonus int = NULL,
				@Mod int = NULL,
				@Size nvarchar(20) = NULL,
				@Complexity int = NULL,
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
SELECT	* 
'

SET @From = N'
FROM	dbo.Floor F 
'

SET @Where =' 
WHERE	1=1
'
IF @FloorID IS NOT NULL
	SET @Where = @Where + N' AND F.ID = @FloorID '

IF @DurationFrom IS NOT NULL
	SET @Where = @Where + N' AND F.Duration >= @DurationFrom '

IF @DurationTo IS NOT NULL
	SET @Where = @Where + N' AND F.Duration <= @DurationTo '

IF @Bonus IS NOT NULL
	SET @Where = @Where + N' AND F.Bonus = @Bonus '

IF @Mod IS NOT NULL
	SET @Where = @Where + N' AND F.Mod = @Mod '

SELECT	@SizeLookupID = ID
FROM	dbo.SizeLookup
WHERE	Name LIKE @Size

IF @SizeLookupID IS NOT NULL
BEGIN
	SET @From = N' INNER JOIN dbo.SizeLookup S ON S.ID = F.Size '
	SET @Where = N' AND S.ID = @SizeLookupID '
END

IF @Complexity IS NOT NULL
	SET @Where = @Where + N' AND F.Complexity = @Complexity '

IF @Image IS NOT NULL
	SET @Where = @Where + N' AND F.Image LIKE @Image '

IF @DateFrom IS NOT NULL
	SET @Where = @Where + N' AND F.Date >= @DateFrom '

IF @DateTo IS NOT NULL
	SET @Where = @Where + N' AND F.Date <= @DateTo '

SET @Sql = @Select + @From + @Where

SET @Params = '@FloorID int,
	     @DurationFrom bigint,
	     @DurationTo bigint,
	     @Bonus int,
	     @Mod int,
	     @SizeLookupID int,
	     @Complexity int,
	     @Image nvarchar(100),
	     @DateFrom datetime2(0),
	     @DateTo datetime2(0)'

EXEC sys.sp_executesql @SQL, @Params, 
	@FloorID = @FloorID,
	@DurationFrom = @DurationFrom,
	@DurationTo = @DurationTo,
	@Bonus = @Bonus,
	@Mod = @Mod,
	@SizeLookupID = @SizeLookupID,
	@Complexity = @Complexity,
	@Image = @Image,
	@DateFrom = @DateFrom,
	@DateTo = @DateTo

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	EXEC spRaiseError
	RETURN 9999
END CATCH

