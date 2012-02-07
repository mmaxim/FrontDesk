IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_UpdateSection' AND type = 'P')
   DROP PROCEDURE fd_UpdateSection
GO

CREATE procedure ipbased.fd_UpdateSection
(
	@SectionID int,
	@Name nvarchar(50),
	@Owner nvarchar(50)
)
AS
	DECLARE @pid int

	-- get owner pid
	SELECT @pid = principalID FROM Users WHERE userName = @Owner

	-- update table
	UPDATE Sections 
	SET
		name = @Name, ownerID = @pid 
	WHERE
		ID = @SectionID 
GO