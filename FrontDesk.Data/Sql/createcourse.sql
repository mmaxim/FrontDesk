IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateNewCourse' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateNewCourse
GO

CREATE procedure ipbased.fd_CreateNewCourse
(
	@CourseName NVARCHAR(150),
	@CourseNumber NVARCHAR(50)
)
AS
	DECLARE @typeID INT, @courseID INT

	BEGIN TRAN CreCourse

	-- create the course record
	INSERT INTO Courses (courseName,  courseNumber, available) VALUES
			(@CourseName,  @CourseNumber, 1)
	SELECT @courseID = @@Identity

	--create permission object
	EXEC ipbased.fd_CreatePermObj @Type='course', @EntityID=@courseID

	--return courseID
	SELECT @courseID

	COMMIT TRAN CreCourse
GO