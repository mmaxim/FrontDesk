IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_AcceptInvite' AND type = 'P')
   DROP PROCEDURE fd_AcceptInvite
GO

CREATE PROCEDURE ipbased.fd_AcceptInvite
(
	@InviteID int
)
AS

DECLARE @pid int, @groupID int

-- get pid
SELECT @pid = inviteeID, @groupID = groupID FROM GroupInvitations WHERE ID = @InviteID

BEGIN TRAN Invite

-- create membership row
INSERT INTO GroupMembers
	(groupID, userID)
values
	(@groupID, @pid)

-- delete invite row
DELETE FROM GroupInvitations WHERE ID = @InviteID

-- do it
COMMIT TRAN Invite

GO