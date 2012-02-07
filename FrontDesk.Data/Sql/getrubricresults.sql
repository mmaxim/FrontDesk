IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetRubricResults' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetRubricResults
GO

CREATE procedure ipbased.fd_GetRubricResults
(
	@RubricID INT,
	@SubID INT,
	@Type NVARCHAR(50)
)
AS
	IF (@Type = 'auto')
	BEGIN
		IF (@SubID < 0)
			SELECT * FROM AutoResultsView WHERE rubricID=@RubricID
		ELSE
			SELECT * FROM AutoResultsView WHERE rubricID=@RubricID AND subID=@SubID
	END
	IF (@Type = 'subj')
	BEGIN
		IF (@SubID < 0)
			SELECT * FROM SubjResultsView WHERE rubricID=@RubricID
		ELSE
			SELECT * FROM SubjResultsView WHERE rubricID=@RubricID AND subID=@SubID
	END
GO