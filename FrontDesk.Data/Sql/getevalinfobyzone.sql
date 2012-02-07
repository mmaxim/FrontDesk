IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetEvalInfoByZone' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetEvalInfoByZone
GO

CREATE procedure ipbased.fd_GetEvalInfoByZone
(
	@ZoneID INT
)
AS
	SELECT 
		e.*, ea.*
	FROM
		EvaluationAuto ea
	INNER JOIN
		EvaluationsView e ON ea.evalID = e.ID
	WHERE
		ea.zoneID=@ZoneID
GO