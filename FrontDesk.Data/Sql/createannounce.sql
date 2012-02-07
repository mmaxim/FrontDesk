IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateAnnounce' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateAnnounce
GO

CREATE procedure ipbased.fd_CreateAnnounce
(
	@Poster NVARCHAR(50),
	@Desc NTEXT,
	@CourseID INT,
	@Title NVARCHAR(200)
)
AS
	DECLARE @pid INT, @now DATETIME, @annouID INT

	BEGIN TRAN CreAnnou

	-- get poster pid
	SELECT @pid = principalID FROM Users WHERE userName = @Poster
	
	-- insert the announcement
	SELECT @now = getdate()
	INSERT INTO Announcements
		(posterID, content, creation, modified, courseID, title)
	VALUES
		(@pid, @Desc, @now, @now, @CourseID, @Title)
	SELECT @annouID = @@Identity

	COMMIT TRAN CreAnnou

	-- return the ID value of the new annou
	SELECT @annouID
GO