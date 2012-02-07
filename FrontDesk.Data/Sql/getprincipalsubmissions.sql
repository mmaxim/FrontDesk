IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetPrincipalSubmissions' AND type = 'P')
   DROP PROCEDURE fd_GetPrincipalSubmissions
GO

CREATE procedure ipbased.fd_GetPrincipalSubmissions
(
	@Pid int,
	@AsstID int
)
 AS
	DECLARE @ptype int

	-- get the type of the principal
	SELECT @ptype = type FROM Principals WHERE ID = @Pid
	
	-- user
	IF (@ptype = 1)
	BEGIN
		-- get all submissions from user and user groups
			SELECT s.*, g.groupName AS submitter FROM
				Submissions s,
				GroupMembers gm,
				Groups g
			WHERE
				gm.userID = @pid AND 
				gm.groupID = s.principalID AND
				s.asstID = @AsstID AND
				g.principalID = gm.groupID
		UNION
			SELECT s.*, u.username AS submitter FROM Submissions s, Users u WHERE 
				s.principalID = @pid AND 
				s.asstID = @AsstID AND
				u.principalID = @pid
			
		ORDER BY subTime DESC
	END
	ELSE
	BEGIN
		--group
		SELECT 
			s.*, g.groupName AS submitter  
		FROM 
			Submissions s
		INNER JOIN 
			Groups g ON g.principalID = @Pid
		WHERE 
			s.principalID = @pid AND s.asstID = @AsstID
		ORDER BY 
			s.subTime DESC
	END

GO