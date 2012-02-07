IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetResultInfo' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetResultInfo
GO

CREATE procedure ipbased.fd_GetResultInfo
(
	@ResID int
)
AS
	DECLARE @rtype nvarchar(50)

	-- get type
	SELECT 
		@rtype = rt.name
	FROM
		Results r
	INNER JOIN
		ResultTypes rt ON rt.ID = r.resType
	WHERE
		r.ID = @ResID 

	-- get top level eval record
	IF (@rtype = 'auto')
		SELECT * FROM AutoResultsView WHERE ID=@ResID
	ELSE IF (@rtype = 'subj')
		SELECT * FROM SubjResultsView WHERE ID=@ResID

GO