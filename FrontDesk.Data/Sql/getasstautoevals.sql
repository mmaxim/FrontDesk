IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetAsstEvals' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetAsstEvals
GO

CREATE procedure ipbased.fd_GetAsstEvals
(
	@AsstID INT,
	@Type NVARCHAR(50)
)
AS
	
	IF (@Type = 'auto')
	BEGIN
		SELECT
			ea.*,
			e.*
		FROM
			EvaluationsView e
		INNER JOIN
			EvaluationAuto ea ON ea.evalID = e.ID
		WHERE
			e.asstID = @AsstID
		ORDER BY
			e.name DESC
	END
GO
