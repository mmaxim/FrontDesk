IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_UpdateRubricEntry' AND type = 'P')
   DROP PROCEDURE ipbased.fd_UpdateRubricEntry
GO

CREATE procedure ipbased.fd_UpdateRubricEntry
(
	@RubID INT,
	@Points FLOAT,
	@Name NVARCHAR(50),
	@Description TEXT,
	@AllowNeg BIT
)
AS

	DECLARE @subID INT, @asstID INT
	DECLARE @oldPoints FLOAT

	-- get asst id
	SELECT @asstID = asstID, @oldPoints = points 
		FROM RubricForest WHERE ID = @RubID

	--update local entry
	UPDATE 
		RubricForest
	SET
		shortdesc = @Name,
		longdesc = @Description,
		points = @Points,
		allowneg=@AllowNeg
	WHERE
		ID = @RubID

	--destroy rubric sub points
	IF (@oldPoints <> @Points)
		DELETE FROM RubricPoints WHERE rubID IN
			(SELECT ID FROM RubricForest WHERE asstID=@asstID)
GO	