IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_ObtainLock' AND type = 'P')
   DROP PROCEDURE ipbased.fd_ObtainLock
GO

CREATE procedure ipbased.fd_ObtainLock
(
	@FileID INT,
	@PrincipalID INT,
	@LockID INT OUTPUT
)
 AS
	DECLARE @lockCount INT

	-- start the transaction to lock the table
	BEGIN TRAN ObLock

	-- make sure the file is not already locked
	IF (EXISTS(SELECT * FROM FileLocks WHERE fileID = @FileID))
		SELECT @LockID = -1
	ELSE
	BEGIN
		--lock the file by inserting a record into the FileLocks table
		INSERT INTO FileLocks 
			(fileID, principalID, creation, parentID) 
		VALUES
			(@FileID, @PrincipalID, getdate(), -1)
		
		-- return the lockid
		SELECT @LockID = @@Identity
	END

	COMMIT TRAN ObLock
GO