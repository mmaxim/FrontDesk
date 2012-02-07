IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetEvalInfo' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetEvalInfo
GO

CREATE procedure ipbased.fd_GetEvalInfo
(
	@EvalID INT
)
AS
	DECLARE @etype NVARCHAR(50)

	-- get type
	SELECT 
		@etype = et.name
	FROM
		Evaluations e
	INNER JOIN
		EvaluationTypes et ON et.ID = e.typeID
	WHERE
		e.ID = @EvalID 

	-- get top level eval record
	IF (@etype = 'auto')
	BEGIN
		SELECT 
			e.*, ea.*
		FROM
			EvaluationsView e
		INNER JOIN
			EvaluationAuto ea ON ea.evalID = @EvalID
		WHERE
			e.ID = @EvalID
		ORDER BY
			e.name DESC
	END	

GO