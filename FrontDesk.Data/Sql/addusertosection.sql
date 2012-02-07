IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_AddUserToSection' AND type = 'P')
   DROP PROCEDURE fd_AddUserToSection
GO

CREATE procedure ipbased.fd_AddUserToSection
(
	@SectionID int,
	@UserName nvarchar(50),
	@Switch bit
)
AS
	DECLARE @pid int

	-- get user pid
	SELECT @pid = principalID FROM Users WHERE userName = @UserName

	-- start the transaction
	BEGIN TRAN SectionAdd	

	-- drop user from all other sections
	IF (@Switch = 1)
		DELETE FROM SectionMembers WHERE userID = @pid

	-- create the new section membership
	INSERT INTO SectionMembers
		(sectionID, userID)
	values
		(@SectionID, @pid)

	-- commit the transaction
	COMMIT TRAN SectionAdd
GO