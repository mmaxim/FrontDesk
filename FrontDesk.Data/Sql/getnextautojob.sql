IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetNextAutoJob' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetNextAutoJob
GO

CREATE procedure ipbased.fd_GetNextAutoJob
(
	@TesterIP NVARCHAR(50),
	@TesterDesc NVARCHAR(50)
)
AS

	DECLARE @jid INT, @eid INT, @sid INT

	-- lock the database before doing this
	BEGIN TRAN GetNext

	-- find the latest job (TODO: use priorities)
	SELECT 
		TOP 1 
		@jid = j.jobID,
		@eid = j.evalID,
		@sid = j.subID
	FROM
		AutoJobTests j
	WHERE
		j.status= 0
	ORDER BY
		j.jobID

	-- set the job in progress
	UPDATE AutoJobTests 
	SET 
		status = 1, testerIP = @TesterIP, testerDesc = @TesterDesc
	WHERE jobID = @jid AND evalID=@eid AND subID=@sid

	-- return data for the job
	SELECT 
		j.*, u.username AS jobcreator, aj.name AS jobname,
		ea.zoneID, ea.zoneMod, e.points, 
		ea.tool, ea.toolArguments, ea.isbuild,
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
	WHERE
		j.jobID = @jid AND j.evalID=@eid AND j.subID=@sid

	COMMIT TRAN GetNext

GO