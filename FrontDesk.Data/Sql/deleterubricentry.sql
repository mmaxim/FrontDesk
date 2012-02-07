IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteRubricEntry' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeleteRubricEntry
GO

CREATE procedure ipbased.fd_DeleteRubricEntry
(
	@RubID INT
)
AS
	DECLARE @resID INT, @parID INT, @asstID INT
	DECLARE @subID INT

	BEGIN TRAN DelRubEnt

	SELECT @parID = parentID, @asstID=asstID FROM RubricForest WHERE ID=@RubID

	--delete all results
	DECLARE res_cursor CURSOR FOR
	SELECT ID FROM Results WHERE rubricID=@RubID
	OPEN res_cursor
	FETCH NEXT FROM res_cursor INTO @resID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC ipbased.fd_DeleteResult @ResID=@resID
		FETCH NEXT FROM res_cursor INTO @resID
	END
	CLOSE res_cursor
	DEALLOCATE res_cursor

	--make the delete possible
	UPDATE RubricForest SET evalID=NULL WHERE ID = @RubID

	--take canned comments
	DELETE FROM CannedComments WHERE rubricID = @RubID

	--take rubric points
	DELETE FROM RubricPoints WHERE rubID = @RubID
	DELETE FROM RubricPoints WHERE rubID IN
		(SELECT ID FROM RubricForest WHERE asstID=@asstID)
	
	--delete
	DELETE FROM RubricForest WHERE ID = @RubID

	COMMIT TRAN DelRubEnt
GO	