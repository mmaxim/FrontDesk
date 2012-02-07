IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteFile' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeleteFile
GO

CREATE PROCEDURE ipbased.fd_DeleteFile (
	@FileID INT,
	@PrincipalID INT
) 
AS

	DECLARE @path NVARCHAR(1500), @name NVARCHAR(100)
	DECLARE @type INT, @flock INT, @culprit INT

	IF (@type = 1)
	BEGIN
		-- deleting a single file
		EXEC ipbased.fd_ObtainLock @FileID = @FileID, 
		           @PrincipalID = @PrincipalID,
			   @LockID = @flock OUTPUT
		IF (@flock < 0)
			SELECT -1 AS condition, @FileID AS culprit
		ELSE
		BEGIN
			DELETE FROM Files WHERE ID = @FileID
			EXEC ipbased.fd_ReleaseLock @LockID = @flock
			SELECT 0 AS condition, 0 AS culprit
		END
	END
	ELSE
	BEGIN
		--deleting a directory
		EXEC ipbased.fd_ObtainDirLock @FileID = @FileID, 
		      @PrincipalID = @PrincipalID,
		      @LockID = @flock OUTPUT,
		      @Culprit = @culprit OUTPUT

		IF (@flock < 0)
			SELECT -1 as condition, @culprit AS culprit
		ELSE
		BEGIN
			DECLARE @delpath NVARCHAR(1500)
			SELECT @path = filePath, @name = fileName, @type = fileType FROM Files
				WHERE ID = @FileID
			SET @delpath = @path + '\' + @name
			DELETE FROM Files WHERE filePath = @delpath OR filePath LIKE @delpath + '\%' OR
				ID = @FileID
			EXEC ipbased.fd_ReleaseLock @LockID = @flock
			SELECT 0 AS condition, 0 as culprit
		END
	END

GO