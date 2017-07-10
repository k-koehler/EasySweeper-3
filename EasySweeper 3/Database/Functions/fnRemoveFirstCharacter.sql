ALTER FUNCTION [dbo].[fnRemoveFirstCharacter]
(
	@Str nvarchar(max)
)
RETURNS nvarchar(max)
AS
BEGIN
RETURN	RIGHT(@str, LEN(@Str)-1)
END
