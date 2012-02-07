IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_InviteUser' AND type = 'P')
   DROP PROCEDURE fd_InviteUser
GO

CREATE PROCEDURE ipbased.fd_InviteUser
(
	@Invitee nvarchar(50),
	@Invitor nvarchar(50),
	@GroupID int
)
AS

DECLARE @teepid int, @torpid int

-- get pids
SELECT @teepid = principalID FROM Users WHERE userName = @Invitee
SELECT @torpid = principalID FROM Users WHERE userName = @Invitor

INSERT INTO GroupInvitations
		(groupID, invitorID, inviteeID)
	values
		(@GroupID, @torpid, @teepid)

GO