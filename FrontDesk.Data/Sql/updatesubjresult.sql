IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_UpdateSubjResult' AND type = 'P')
   DROP PROCEDURE ipbased.fd_UpdateSubjResult
GO

CREATE procedure ipbased.fd_UpdateSubjResult
(
	@ResID INT,
	@Points FLOAT,
	@Comment TEXT,
	@SubjType INT
)
AS
	DECLARE @rubID INT, @subID INT

	-- start the new transaction
	BEGIN TRAN UpdSubjRes

	SELECT @rubID=rubricID, @subID=subID FROM Results
		WHERE ID=@ResID

	--update points
	UPDATE Results SET points=@Points WHERE ID = @ResID

	--update remainder of data
	UPDATE ResultsSubj SET subjType=@SubjType, comment=@Comment
		WHERE resID = @ResID

	-- update rubric sub points
	EXEC ipbased.fd_UpdateRubricSubPoints @RubricID=@rubID, @SubID=@subID

	-- commit the changes
	COMMIT TRAN UpdSubjRes	
GO