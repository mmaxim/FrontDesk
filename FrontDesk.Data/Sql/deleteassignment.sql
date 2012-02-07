IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteAssignment' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeleteAssignment
GO

CREATE PROCEDURE ipbased.fd_DeleteAssignment
(
	@AsstID INT
)
AS
	BEGIN TRAN DelAsst

	--take perm obj
	EXEC ipbased.fd_DeletePermObject @EntityID=@AsstID, @EntityType='asst'

	--take assignment
	DELETE FROM Assignments WHERE ID = @AsstID	

	COMMIT TRAN DelAsst

GO