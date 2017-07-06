DROP TYPE FloorParticipants
GO
CREATE TYPE FloorParticipants AS TABLE
(
	Name nvarchar(20) NOT NULL,
	Position int NOT NULL,
	UnknownName bit NOT NULL
)
GO