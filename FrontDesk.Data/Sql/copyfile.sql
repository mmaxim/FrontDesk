IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CopyFile' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CopyFile
GO

CREATE PROCEDURE ipbased.fd_CopyFile (
	@FileID INT,
	@PrincipalID INT,
	@DestPath NVARCHAR(1500),
	@Move BIT,
	@Lock BIT
) 
AS

	DECLARE @flock INT

	--lock the file
	IF (@Lock = 1)
	BEGIN
		EXEC ipbased.fd_ObtainLock @FileID = @FileID, 
			           @PrincipalID = @PrincipalID,
				   @LockID = @flock OUTPUT
	END	
	ELSE
		SET @flock = 1

	IF (@flock < 0)
		RETURN (-1);
	ELSE
	BEGIN
		BEGIN TRAN CopFile
		IF (@Move = 0)
		BEGIN
			--duplicate file on copy
			INSERT INTO Files (fileName, filePath, fileType, fileCreated, fileModified, fileSpecialType, fileAlias, readonly, fileSize, description)
				SELECT fileName, filePath, fileType, fileCreated, fileModified, fileSpecialType, fileAlias, readonly, fileSize, description
				FROM Files WHERE ID = @FileID
			--duplicate file permissions
			INSERT INTO FilePermissions (fileID, principalID, fileaction, filegrant)
				SELECT * FROM FilePermissions WHERE fileID=@FileID
		END
	
		--move  file
		UPDATE Files SET filePath = @DestPath WHERE ID = @FileID
		COMMIT TRAN CopFile

		--release lock
		EXEC ipbased.fd_ReleaseLock @LockID = @flock

		RETURN (0)
	END

GO