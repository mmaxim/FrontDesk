IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateSubjResult' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateSubjResult
GO

CREATE procedure ipbased.fd_CreateSubjResult
(
	@FileID INT,
	@Line INT,
	@Type INT,
	@Comment TEXT,
	@Grader NVARCHAR(50),
	@Points FLOAT,
	@SubID INT,
	@RubricID INT
)
AS

	DECLARE @resid INT
	DECLARE @fid INT, @lid INT

	BEGIN TRAN CreSubjRes

	-- insert into parent
	EXEC ipbased.fd_CreateResult @RubricID=@RubricID, @SubID=@SubID, 
		@Points=@Points, @Type='subj', @Grader=@Grader, @ResID=@resid OUTPUT

	-- set up NULLs
	IF (@FileID >= 0)
	BEGIN
		SET @fid = @FileID
		SET @lid = @Line
	END

	-- into record into results table	
	INSERT INTO ResultsSubj
		(fileID, line, subjType, comment, resID)
	VALUES
		(@fid, @lid, @Type, @Comment, @resid)

	-- commit tran
	COMMIT TRAN CreSubjRes

	-- return high level ID
	SELECT @resid
GO