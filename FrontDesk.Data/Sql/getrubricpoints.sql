IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetRubricPoints' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetRubricPoints
GO

CREATE procedure ipbased.fd_GetRubricPoints
(
	@RubricID INT,
	@SubID INT
)
AS
	DECLARE @count INT
	DECLARE @points FLOAT

	-- create the entry if it doesn't exist
	SELECT @count = Count(*) FROM RubricPoints WHERE 
		rubID=@RubricID AND subID=@SubID
	IF (@count = 0)
		SELECT 0.0 AS points, 0 AS status
	ELSE
		-- get the points
		SELECT points AS points, 1 AS status FROM RubricPoints 
			WHERE rubID=@RubricID AND subID=@SubID
	
GO