IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateAutoEval' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateAutoEval
GO

CREATE procedure ipbased.fd_CreateAutoEval
(
	@Creator NVARCHAR(50),
	@Tool NVARCHAR(50),
	@ToolArguments NVARCHAR(100),
	@ToolVersion NVARCHAR(50),
	@ToolVersioning INT,
	@TimeLimit INT,
	@AsstID INT,
	@RunOnSubmit BIT,
	@Competitive BIT,
	@IsBuild BIT,
	@Manager INT,
	@ResType NVARCHAR(50)
)
AS
	DECLARE @eid INT, @tid INT
	DECLARE @rbuildid INT

	-- start the new transaction
	BEGIN TRAN CreAuto

	-- get the type id
	SELECT @tid = ID FROM EvaluationTypes WHERE name='auto' 

	-- do the main evaluation insertion
	EXEC ipbased.fd_CreateEval 
		@Creator=@Creator, @TimeLimit=@TimeLimit,
		@AsstID=@AsstID,
		@RunOnSubmit=@RunOnSubmit, @Competitive=@Competitive,
		@TypeID=@tid, @Manager=@Manager, @ResType=@ResType, @RetID=@eid OUTPUT

	-- do the rest of the insertion
	INSERT INTO EvaluationAuto 
		(evalID, tool, toolArguments, zoneMod, isbuild, toolVersion, toolVersioning)
	VALUES
		(@eid, @Tool, @ToolArguments, getdate(), @IsBuild, @ToolVersion, @ToolVersioning)

	-- commit the changes
	COMMIT TRAN CreAuto

	-- return the ID
	SELECT @eid	
GO