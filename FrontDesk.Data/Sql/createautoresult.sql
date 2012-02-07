IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateAutoResult' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateAutoResult
GO

CREATE procedure ipbased.fd_CreateAutoResult
(
	@EvalID INT,
	@SubID INT,
	@Result TEXT,
	@Points INT,
	@Success INT,
	@Grader NVARCHAR(50),
	@Comp FLOAT
)
AS
	DECLARE @pid INT, @rubid INT
	DECLARE @oid INT, @resid INT

	-- get rub id
	SELECT @rubid = ID FROM RubricForest WHERE evalID = @EvalID

	BEGIN TRAN CreAutoRes

	-- check for identical results
	SELECT 
		@oid = ra.resID 
	FROM 
		ResultsAuto ra
	INNER JOIN
		Results r ON r.subID = @SubID AND r.ID = ra.resID
	WHERE 
		ra.evalID = @EvalID
	DELETE FROM ResultsAuto WHERE resID = @oid
	DELETE FROM Results WHERE ID = @oid

	-- insert into parent
	EXEC ipbased.fd_CreateResult @RubricID = @rubid, @SubID = @SubID, 
		@Points=@Points, @Type='auto', @Grader=@Grader, @ResID=@resid OUTPUT

	-- into record into results table	
	INSERT INTO ResultsAuto
		(evalID, resID, result, success, compete)
	VALUES
		(@EvalID, @resid, @Result, @Success, @Comp)

	-- commit tran
	COMMIT TRAN CreAutoRes

	-- return high level ID
	SELECT @resid
GO