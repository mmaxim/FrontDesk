IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteAutoJob' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeleteAutoJob
GO

CREATE procedure ipbased.fd_DeleteAutoJob
(
	@JobID INT
)
AS
	BEGIN TRAN DelAutoJob

	--take all tests first
	DELETE FROM AutoJobTests WHERE jobID = @JobID

	--take main job
	DELETE FROM AutoJobs WHERE ID = @JobID

	COMMIT TRAN DelAutoJob

GO