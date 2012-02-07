IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateAssignment' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateAssignment
GO

CREATE PROCEDURE ipbased.fd_CreateAssignment
(
	@CourseID INT,
	@Description NVARCHAR(200),
	@DueDate DATETIME,
	@Creator NVARCHAR(50),
	@AsstID INT OUTPUT,
	@Format NTEXT
)
AS

	DECLARE @pid INT, @rasstID INT
	DECLARE @typeID INT

	BEGIN TRAN CreAsst

	SELECT @pid = principalID FROM Users WHERE userName = @Creator

	-- create the new row
	INSERT INTO Assignments (courseID, description, dueDate, creatorPrincipalID, sturelease, resrelease, format)
		VALUES (@CourseID, @Description, @DueDate, @pid, 0, 0, @Format)
	SET @rasstID = @@Identity

	--create rubric
	EXEC ipbased.fd_CreateRubric @AsstID=@rasstID

	--create permission object
	EXEC ipbased.fd_CreatePermObj @Type='asst', @EntityID=@rasstID

	SET @AsstID = @rasstID

	COMMIT TRAN CreAsst

GO