IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteSectionMember' AND type = 'P')
   DROP PROCEDURE fd_DeleteSectionMember
GO

CREATE procedure ipbased.fd_DeleteSectionMember
(
	@UserName nvarchar(50),
	@SectionID int
)
AS
	DECLARE @pid int

	-- get user pid
	SELECT @pid = principalID FROM Users WHERE userName = @UserName

	-- delete the membership
	DELETE FROM SectionMembers WHERE sectionID = @SectionID AND userID = @pid
GO