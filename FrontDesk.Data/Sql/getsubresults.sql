IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetSubResults' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetSubResults
GO

CREATE procedure ipbased.fd_GetSubResults
(
	@SubID INT,
	@Type NVARCHAR(50)
)
AS
	
	IF (@Type='auto')
		SELECT * FROM AutoResultsView WHERE subID=@SubID
	ELSE IF (@Type='subj')
		SELECT * FROM SubjResultsView WHERE subID=@SubID

GO