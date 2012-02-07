IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteResult' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeleteResult
GO

CREATE procedure ipbased.fd_DeleteResult
(
	@ResID INT
)
AS
	DECLARE @rtype NVARCHAR(50)
	DECLARE @rubID INT, @subID INT
	
	BEGIN TRAN DelRes
	-- get type
	SELECT 
		@rtype = rt.name
	FROM
		Results r
	INNER JOIN
		ResultTypes rt ON rt.ID = r.resType
	WHERE
		r.ID = @ResID 

	-- get rubric id and sub id
	SELECT @rubID=rubricID, @subID=subID FROM Results
		WHERE ID=@ResID

	-- delete main record
	IF (@rtype = 'auto')
		DELETE FROM ResultsAuto WHERE resID = @ResID
	ELSE IF (@rtype = 'subj')
	BEGIN
		DELETE FROM ResultsCodeLines WHERE resID = @ResID
		DELETE FROM ResultsSubj WHERE resID = @ResID
	END

	-- delete parent result record
	DELETE FROM Results WHERE ID = @ResID
	
	-- update rubric sub entry
	EXEC ipbased.fd_UpdateRubricSubPoints @RubricID=@rubID, @SubID=@subID

	COMMIT TRAN DelRes

GO