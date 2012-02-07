IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_UpdateAutoEval' AND type = 'P')
   DROP PROCEDURE ipbased.fd_UpdateAutoEval
GO

CREATE procedure ipbased.fd_UpdateAutoEval
(
	@EvalID INT,
	@ZoneID INT,
	@Tool NVARCHAR(50),
	@ToolArguments NVARCHAR(100),
	@ToolVersion NVARCHAR(50),
	@ToolVersioning INT,
	@TimeLimit INT,
	@RunOnSubmit BIT,
	@Competitive BIT,
	@IsBuild BIT
)
AS
	-- start the new transaction
	BEGIN TRAN UpdAuto

	-- update parent evaluation record
	EXEC ipbased.fd_UpdateEval 
		@EvalID=@EvalID, @TimeLimit=@TimeLimit,
		@RunOnSubmit=@RunOnSubmit, @Competetive=@Competitive

	-- update auto evaluation
	UPDATE 
		EvaluationAuto
	SET
		zoneID=@ZoneID,
		tool=@Tool,
		toolArguments=@ToolArguments,
		zoneMod=getdate(),
		isbuild=@IsBuild,
		toolVersion=@ToolVersion,
		toolVersioning=@ToolVersioning
	WHERE
		evalID=@EvalID

	-- commit the changes
	COMMIT TRAN UpdAuto	
GO