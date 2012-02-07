IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateSection' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateSection
GO

CREATE procedure ipbased.fd_CreateSection
(
	@SectionName NVARCHAR(50),
	@Owner NVARCHAR(50),
	@CourseID INT
)
AS
	DECLARE @pid INT, @typeID INT, @secID INT

	BEGIN TRAN CreSect

	-- get owner pid
	SELECT @pid = principalID FROM Users WHERE userName = @Owner
	
	-- create the section
	INSERT INTO Sections
		(ownerID, name, courseID)
	VALUES
		(@pid, @SectionName, @CourseID)
	SELECT @secID = @@Identity
	
	--create permission object
	EXEC ipbased.fd_CreatePermObj @Type='section', @EntityID=@secID

	-- return the ID
	SELECT @secID

	COMMIT TRAN CreSect
GO