IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteEval' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeleteEval
GO

CREATE procedure ipbased.fd_DeleteEval
(
	@EvalID INT
)
AS
	DECLARE @etype NVARCHAR(50)
	DECLARE @resID INT

	BEGIN TRAN DelAuto
	
	-- get type
	SELECT 
		@etype = et.name
	FROM
		Evaluations e
	INNER JOIN
		EvaluationTypes et ON et.ID = e.typeID
	WHERE
		e.ID = @EvalID 

	-- delete main record
	IF (@etype = 'auto')
	BEGIN
		--delete all results
		DECLARE res_cursor CURSOR FOR
		SELECT resID FROM ResultsAuto WHERE evalID=@EvalID
		OPEN res_cursor
		FETCH NEXT FROM res_cursor INTO @resID
		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC ipbased.fd_DeleteResult @ResID=@resID
			FETCH NEXT FROM res_cursor INTO @resID
		END
		CLOSE res_cursor
		DEALLOCATE res_cursor

		DELETE FROM EvaluationAuto WHERE evalID = @EvalID
	END

	--update rubric
	UPDATE RubricForest SET evalID=NULL WHERE evalID=@EvalID

	-- delete dependencies
	DELETE FROM EvaluationDeps WHERE evalID = @EvalID OR depID = @EvalID

	-- delete parent evalation record
	DELETE FROM Evaluations WHERE ID = @EvalID
	
	COMMIT TRAN DelAuto

GO