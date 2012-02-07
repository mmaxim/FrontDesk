IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CheckFilePermission' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CheckFilePermission
GO

CREATE procedure ipbased.fd_CheckFilePermission
(
	@PrincipalID INT,
	@FileID INT,
	@FileAction INT
)
AS
	DECLARE @suc INT, @grant BIT
	DECLARE @cpid INT, @lFileID INT
	DECLARE @rfpath NVARCHAR(1500), @name NVARCHAR(100), @fpath NVARCHAR(1500), @path NVARCHAR(1500)

	--load vars
	SET @suc=0
	SET @lFileID=@FileID
	
	-- load up the principalID cursor
	DECLARE pid_cursor CURSOR FOR
		SELECT @PrincipalID UNION
		SELECT groupid FROM GroupMembers WHERE userID=@PrincipalID UNION
		SELECT courseRoleID FROM CourseMembers WHERE memberPrincipalID=@PrincipalID

	WHILE (@lFileID >= 0) 
	BEGIN
		-- open the pid cursor		
		OPEN pid_cursor
		
		--check for perm from pids
		FETCH NEXT FROM pid_cursor INTO @cpid
		WHILE (@@FETCH_STATUS = 0)
		BEGIN
			-- check for a grant record
			SELECT @grant=filegrant FROM FilePermissions WHERE
				fileID=@lFileID AND principalID=@cpid AND fileaction=@FileAction
			IF (@grant IS NOT NULL)
			BEGIN
				IF (@grant = 0)
				BEGIN
					SET @suc=-1
					BREAK
				END
				ELSE
				BEGIN
					SET @suc=1
					BREAK
				END
			END	
			FETCH NEXT FROM pid_cursor INTO @cpid		
		END
		
		CLOSE pid_cursor
	
		-- stop on success
		IF (@suc = 1 OR @suc = -1)
			BREAK
		ELSE
		BEGIN
			--move back to parent and try again
			SELECT @fpath = filePath FROM Files WHERE ID=@lFileID
			IF (LEN(@fpath) <= 3)
				SET @lFileID=-1
			ELSE
			BEGIN
				-- calculate the parent ID
				SET @rfpath = REVERSE(@fpath)
				SET @name = REVERSE(SUBSTRING(@rfpath, 0, CHARINDEX('\', @rfpath)))
				SET @path = SUBSTRING(@fpath, 0, LEN(@fpath)-LEN(@name))
				IF (CHARINDEX('\', @path) = 0)
					SET @path = 'c:\'
				SELECT @lFileID = ID FROM Files WHERE fileName=@name AND filePath=@path
			END
		END
	END
	
	-- kill pid cursor and return result
	DEALLOCATE pid_cursor
	IF (@suc = -1)
		SET @suc=0
	SELECT @suc
GO