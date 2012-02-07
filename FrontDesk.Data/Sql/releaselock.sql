IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_ReleaseLock' AND type = 'P')
   DROP PROCEDURE fd_ReleaseLock
GO

CREATE procedure ipbased.fd_ReleaseLock
(
	@LockID int
)
 AS
	DELETE FROM FileLocks WHERE ID = @LockID OR parentID = @LockID

GO