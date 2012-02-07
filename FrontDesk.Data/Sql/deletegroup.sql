IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteGroup' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeleteGroup
GO

CREATE PROCEDURE ipbased.fd_DeleteGroup
(
	@GroupID int
)
AS

	BEGIN TRAN RemoveGM

	--take everything
	DELETE FROM GroupInvitations WHERE groupID = @GroupID
	DELETE FROM GroupMembers WHERE groupID=@GroupID
	DELETE FROM Groups WHERE principalID = @GroupID
	DELETE FROM Principals WHERE ID = @GroupID

	-- commit the transaction
	IF (@@Error = 0)
		COMMIT TRAN RemoveGM
	ELSE
		ROLLBACK TRAN RemoveGM

GO