IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteGroupMember' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeleteGroupMember
GO

CREATE PROCEDURE ipbased.fd_DeleteGroupMember
(
	@UserName nvarchar(50),
	@GroupID int
)
AS

DECLARE @pid int, @mems int

-- get pid
SELECT @pid = principalID FROM Users WHERE userName = @UserName

-- delete the membership row
DELETE FROM GroupMembers WHERE userID = @pid AND groupID = @GroupID

GO