IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_ObtainDirLock' AND type = 'P')
   DROP PROCEDURE ipbased.fd_ObtainDirLock
GO

CREATE PROCEDURE ipbased.fd_ObtainDirLock (
	@FileID INT,
	@PrincipalID INT,
	@LockID INT OUTPUT,
	@Culprit INT OUTPUT
) AS

	DECLARE @path NVARCHAR(1500), @name NVARCHAR(100)
	DECLARE @ffiles INT, @lockval INT, @toplock INT, @culpritid INT
	DECLARE @fail BIT
	
	BEGIN TRAN ObDirLock
	
	--Lock top level and store lock id
	SELECT @path = filePath, @name = fileName FROM Files WHERE ID = @FileID
	EXEC ipbased.fd_ObtainLock @FileID=@FileID, @PrincipalID=@PrincipalID, @LockID=@toplock OUTPUT
	
	IF (@toplock < 0)
	BEGIN
		-- Failed on top level
		ROLLBACK TRAN ObDirLock
		SELECT @LockID = -1, @Culprit = @FileID
	END
	ELSE
	BEGIN
		-- Obtain cursor for all files under top level
		DECLARE test_cursor CURSOR FOR
		SELECT ID FROM Files WHERE filePath LIKE @path + '\' + @name + '%'
		OPEN test_cursor
	
		-- execute all lock requirements in a transaction
		SET @fail = 0
		FETCH NEXT FROM test_cursor INTO @ffiles
		WHILE @@FETCH_STATUS = 0
		BEGIN
			--obtain lock on file
			EXEC ipbased.fd_ObtainLock 
				@FileID=@ffiles, @PrincipalID=@PrincipalID, @LockID = @lockval OUTPUT
			IF (@lockval < 0)
			BEGIN
				SET @fail = 1;
				SET @culpritid = @ffiles;
				BREAK;
			END	
	
			--set parent id for lock release
			UPDATE FileLocks SET parentID = @toplock WHERE fileID = @ffiles
			FETCH NEXT FROM test_cursor INTO @ffiles
		END
		CLOSE test_cursor
		DEALLOCATE test_cursor
	
		--if we failed, return the culprit, otherwise give toplevel lock
		IF (@fail = 1)
		BEGIN
			-- rollback the attempt to lock all files
			ROLLBACK TRAN ObDirLock
			--EXEC fd_ReleaseLock @LockID = @toplock
			SELECT @LockID = -1, @Culprit = @culpritid;
		END
		ELSE
		BEGIN
			-- commit all the locks
			COMMIT TRAN ObDirLock
			SELECT @LockID = @toplock, @Culprit = 0;
		END
		
	END

GO