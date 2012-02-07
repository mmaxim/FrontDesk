IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetUserGroups' AND type = 'P')
   DROP PROCEDURE fd_GetUserGroups
GO

CREATE PROCEDURE ipbased.fd_GetUserGroups
(
	@UserName nvarchar(50),
	@AsstID int
)
AS

DECLARE @pid int, @gid int

-- get pid
SELECT @pid = principalID FROM Users WHERE userName = @UserName

-- do the joins to the necessary necessary information
SELECT 
	g.principalID, g.groupName, g.creation, g.asstID,
	u.userName AS creator
FROM 
	Groups g, 
	GroupMembers gm, 
	Users u
WHERE
	g.principalID = gm.groupID AND
	gm.userID = @pid AND
	u.principalID = g.creatorID AND
	g.asstID = @AsstID
ORDER BY 
	g.groupName ASC

GO