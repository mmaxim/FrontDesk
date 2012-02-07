IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteSection' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeleteSection
GO

CREATE procedure ipbased.fd_DeleteSection
(
	@SectionID INT
)
AS
	-- start the delete transaction
	BEGIN TRAN DelSect

	-- delete all members
	DELETE FROM SectionMembers WHERE sectionID = @SectionID
	-- delete the section
	DELETE FROM Sections WHERE ID = @SectionID

	--delete permission object
	EXEC ipbased.fd_DeletePermObject @EntityID=@SectionID, @EntityType='section'

	-- commit the transaction
	COMMIT TRAN DelSect
GO