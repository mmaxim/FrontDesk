IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteAutoJobTest' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeleteAutoJobTest
GO

CREATE procedure ipbased.fd_DeleteAutoJobTest
(
	@JobID INT,
	@SubID INT,
	@EvalID INT
)
AS
	DECLARE @tcount INT

	BEGIN TRAN DelAutoJobTest

	--set record to done
	UPDATE AutoJobTests SET
		status=2 
	WHERE jobID=@JobID AND subID=@SubID AND evalID=@EvalID

	--take main job record if last test
	SELECT @tcount=Count(*) FROM
		AutoJobTests
	WHERE
		jobID=@JobID AND status < 2
	IF (@tcount = 0)
	BEGIN
		DELETE FROM AutoJobTests WHERE jobID=@JobID
		DELETE FROM AutoJobs WHERE ID=@JobID
	END

	COMMIT TRAN DelAutoJobTest

GO