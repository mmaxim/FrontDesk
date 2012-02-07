IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_UpdateEval' AND type = 'P')
   DROP PROCEDURE fd_UpdateEval
GO

CREATE procedure ipbased.fd_UpdateEval
(
	@EvalID int,
	@TimeLimit int,
	@RunOnSubmit bit,
	@Competetive bit
)
AS
	DECLARE @pid int

	-- update the eval attribs
	UPDATE 
		Evaluations
	SET
		timeLimit=@TimeLimit,
		runonsubmit=@RunOnSubmit,
		competitive=@Competetive
	WHERE
		ID = @EvalID

GO