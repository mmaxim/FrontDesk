IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateBackup' AND type = 'P')
   DROP PROCEDURE fd_CreateBackup
GO

CREATE procedure ipbased.fd_CreateBackup
(
	@Creator nvarchar(50),
	@BackUp nvarchar(100),
	@DataFile nvarchar(100),
	@CourseID int
)
AS
	DECLARE @pid int

	-- get pid and insert
	SELECT @pid = principalID FROM Users WHERE username=@Creator
	INSERT INTO
		Backups (creator, fileLocation, creation, backedup, courseID)
	VALUES
		(@pid, @DataFile, getdate(), @BackUp, @CourseID)

GO