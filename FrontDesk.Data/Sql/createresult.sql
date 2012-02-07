IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateResult' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateResult
GO

CREATE PROCEDURE ipbased.fd_CreateResult
(
	@RubricID INT,
	@SubID INT,
	@Points FLOAT,
	@Type NVARCHAR(50),
	@Grader NVARCHAR(50),
	@ResID INT OUTPUT
)
AS

	DECLARE @pid INT, @tid INT

	-- get prin id
	SELECT @pid = principalID FROM Users WHERE username = @Grader

	-- get type id
	SELECT @tid = ID FROM ResultTypes WHERE name = @Type

	-- into record into results table	
	INSERT INTO Results
		(rubricID, subID, resType, graderID, points)
	VALUES
		(@RubricID, @SubID, @tid, @pid, @Points)

	-- return identity
	SET @ResID = @@Identity

	-- update rubric points
	EXEC ipbased.fd_UpdateRubricSubPoints @RubricID=@RubricID, @SubID=@SubID

GO