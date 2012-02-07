IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CopyDirectory' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CopyDirectory
GO

CREATE PROCEDURE ipbased.fd_CopyDirectory (
	@FileID INT,
	@DestID INT,
	@PrincipalID INT,
	@Move BIT
) AS

	DECLARE @flock INT, @ffile INT
	DECLARE @culprit INT, @ret_stat INT
	DECLARE @path NVARCHAR(1500), @name NVARCHAR(100), @type INT
	DECLARE @newpath NVARCHAR(1500), @fpath NVARCHAR(1500)
	DECLARE @DestPath NVARCHAR(1500), @DestName NVARCHAR(100)
	
	--lock the directory
	EXEC ipbased.fd_ObtainDirLock @FileID = @FileID, 
			      @PrincipalID = @PrincipalID,
			      @LockID = @flock OUTPUT,
			      @Culprit = @culprit OUTPUT
	
	IF (@flock < 0)
		SELECT -1 AS condition, @Culprit AS culprit
	ELSE
	BEGIN
		-- set up the destination variable
		SELECT @DestPath = filePath, @DestName = fileName FROM Files
			WHERE ID = @DestID
		IF (@DestPath <> 'c:\')
			SET @DestPath = @DestPath + '\' + @DestName
		ELSE
			SET @DestPath = @DestPath + @DestName

		-- get the directory
		SELECT @path = filePath, @name = fileName FROM Files
			WHERE ID = @FileID
		-- list the directory
		DECLARE dir_cursor INSENSITIVE CURSOR FOR
		SELECT ID FROM Files WHERE filePath LIKE @path + '\' + @name + '%'
		OPEN dir_cursor
	
		-- create new path
		SET @path = @path + '\' + @name
		-- copy the directory
		EXEC @ret_stat = ipbased.fd_CopyFile @FileID = @FileID, @PrincipalID = @PrincipalID,
			@DestPath = @DestPath, @Move = @Move, @Lock = 0
		SET @newpath = @DestPath + '\' + @name
	
		FETCH NEXT FROM dir_cursor INTO @ffile
		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- copy the file
			SELECT @fpath = filePath FROM Files WHERE ID = @ffile
			SET @fpath = REPLACE(@fpath, @path, @newpath) -- do the copy
			EXEC ipbased.fd_CopyFile @FileID = @ffile, @PrincipalID = @PrincipalID,
				@DestPath = @fpath,
				@Move = @Move, @Lock = 0		
	
			FETCH NEXT FROM dir_cursor INTO @ffile
		END			
		
		CLOSE dir_cursor
		DEALLOCATE dir_cursor
	
		-- release file locks
		EXEC ipbased.fd_ReleaseLock @LockID=@flock
	
		SELECT 0 as condition, 0 as culprit				
	END

GO