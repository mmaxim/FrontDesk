IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetEvalDependencies' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetEvalDependencies
GO

CREATE procedure ipbased.fd_GetEvalDependencies
(
	@EvalID int
)
AS

	SELECT
		ea.*,
		e.*
	FROM
		EvaluationDeps ed
	INNER JOIN
		EvaluationsView e ON e.ID = ed.depID
	INNER JOIN
		EvaluationAuto ea ON ea.evalID = e.ID
	WHERE
		ed.evalID = @EvalID
	ORDER BY
		e.name DESC

GO