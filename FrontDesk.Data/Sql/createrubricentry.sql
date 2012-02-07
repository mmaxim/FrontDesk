IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateRubricEntry' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateRubricEntry
GO

CREATE procedure ipbased.fd_CreateRubricEntry
(
	@ParentID INT,
	@Name NVARCHAR(50),
	@Description TEXT,
	@Points FLOAT,
	@EvalID INT
)
AS
	DECLARE @asstID INT
	
	BEGIN TRAN CreRubEnt

	--get asst id
	SELECT @asstID = asstID FROM RubricForest WHERE ID = @ParentID

	IF (@EvalID < 0)
		SET @EvalID = NULL

	--insert new entry	
	INSERT INTO RubricForest 
		(parentID, points, shortdesc, longdesc, asstID, evalID, allowneg)
	VALUES
		(@ParentID, @Points, @Name, @Description, @asstID, @EvalID, 0)	

	COMMIT TRAN CreRubEnt

	SELECT @@Identity
GO