IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetUserInvites' AND type = 'P')
   DROP PROCEDURE fd_GetUserInvites
GO

CREATE PROCEDURE ipbased.fd_GetUserInvites
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
	u.userName AS creator,
	ui.userName AS invitor,
	gm.ID,
	@UserName AS invitee
FROM 
	Groups AS g, 
	GroupInvitations AS gm, 
	Users AS u,
	Users AS ui
WHERE
	g.principalID = gm.groupID AND
	gm.inviteeID = @pid AND
	u.principalID = g.creatorID AND
	ui.principalID = gm.invitorID AND
	g.asstID = @AsstID
ORDER BY
	g.groupName ASC

GO