IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateGroup' AND type = 'P')
   DROP PROCEDURE fd_CreateGroup
GO

CREATE PROCEDURE ipbased.fd_CreateGroup
(
	@GroupName nvarchar(150),
	@Creator nvarchar(50),
	@AsstID int
)
AS

DECLARE @pid int, @gpid int

SELECT @pid = principalID FROM Users WHERE userName = @Creator

-- do this as a transaction
BEGIN TRAN GroupInsert

-- create group principal
INSERT INTO Principals (type) values (2)
SELECT @gpid = @@Identity

-- create the group
INSERT INTO Groups (principalID, groupName, creation, creatorID, asstID)
	values (@gpid, @GroupName, getdate(), @pid, @AsstID)

-- create the group membership
INSERT INTO GroupMembers (groupID, userID) values (@gpid, @pid)

COMMIT TRAN GroupInsert

SELECT @gpid

GO