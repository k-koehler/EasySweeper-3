USE master

IF NOT EXISTS
(
	SELECT	*
	FROM	sys.databases
	WHERE	Name = 'EasySweeper'
)
CREATE DATABASE EasySweeper
GO