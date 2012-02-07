IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetAllAutoJobTests' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetAllAutoJobTests
GO

CREATE procedure ipbased.fd_GetAllAutoJobTests
AS
	-- select up all jobs
	SELECT 
		j.*, u.username AS jobcreator, aj.name AS jobname,
		ea.zoneID, ea.zoneMod,
		ea.tool, ea.toolArguments,
		e.*
	FROM
		AutoJobTests j
	INNER JOIN
		EvaluationAuto ea ON ea.evalID = j.evalID
	INNER JOIN
		EvaluationsView e ON e.ID = j.evalID
	INNER JOIN
		AutoJobs aj ON aj.ID = j.jobID
	INNER JOIN 
		Users u ON u.principalID = aj.creatorID
	ORDER BY
		j.jobID
GO