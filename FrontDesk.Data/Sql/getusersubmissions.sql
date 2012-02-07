IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetUserSubmissions' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetUserSubmissions
GO

CREATE procedure ipbased.fd_GetUserSubmissions
(
	@UserName nvarchar(50),
	@DescID int,
	@Course bit
)
 AS
	DECLARE @pid int

	-- get pid
	SELECT @pid = principalID FROM Users WHERE userName = @UserName

	IF (@Course = 1)
	BEGIN
		-- get all submissions from user and user groups
			SELECT s.*, g.groupName AS submitter FROM
				Submissions s,
				GroupMembers gm,
				Assignments a,
				Groups g
			WHERE
				gm.userID = @pid AND 
				gm.groupID = s.principalID AND
				s.asstID = a.ID AND a.courseID = @DescID AND
				g.principalID = gm.groupID
		UNION
			SELECT s.*, u.username AS submitter FROM 
				Submissions s, Users u, Assignments a 
			WHERE 
				s.principalID = @pid AND 
				u.principalID = @pid AND
				s.asstID = a.ID AND a.courseID = @DescID	
		ORDER BY subTime DESC
	END
	ElSE
	BEGIN
		-- get all submissions from user and user groups
			SELECT s.*, g.groupName AS submitter FROM
				Submissions s,
				GroupMembers gm,
				Groups g
			WHERE
				gm.userID = @pid AND 
				gm.groupID = s.principalID AND
				s.asstID = @DescID AND
				g.principalID = gm.groupID
		UNION
			SELECT s.*, u.username AS submitter FROM Submissions s, Users u WHERE 
				s.principalID = @pid AND 
				s.asstID = @DescID AND
				u.principalID = @pid
		ORDER BY subTime DESC
	END

GO
