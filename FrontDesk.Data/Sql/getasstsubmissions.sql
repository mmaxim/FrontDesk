IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetAsstSubmissions' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetAsstSubmissions
GO

-- *** referenced by deletrubricentry.sql
CREATE procedure ipbased.fd_GetAsstSubmissions
(
	@AsstID INT
)
AS
		SELECT s.*, g.groupName AS submitter, f.filealias AS subname FROM
			Submissions s,
			Groups g,
			Files f
		WHERE
			s.asstID = @AsstID AND
			g.principalID = s.principalID AND
			f.ID = s.directoryID
	UNION
		SELECT s.*, u.username AS submitter, f.filealias AS subname FROM 
			Submissions s, 
			Users u,
			Files f
		WHERE 
			s.principalID = u.principalID AND 
			s.asstID = @AsstID AND
			f.ID = s.directoryID
	ORDER BY subTime DESC

GO