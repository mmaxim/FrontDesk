IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_UpdateFilePermission' AND type = 'P')
   DROP PROCEDURE ipbased.fd_UpdateFilePermission
GO

CREATE procedure ipbased.fd_UpdateFilePermission
(
	@PrincipalID INT,
	@FileID INT,
	@FileAction INT,
	@Grant BIT
)
AS
	DECLARE @count INT

	-- start tran
	BEGIN TRAN UpdFilePerm

	--insert of update existing record
	SELECT @count=Count(*) FROM FilePermissions WHERE principalID=@PrincipalID AND
		fileID=@FileID AND fileaction=@FileAction
	IF (@count=0)
		INSERT INTO FilePermissions 
			(principalID, fileID, fileaction, filegrant)
		VALUES
			(@PrincipalID, @FileID, @FileAction, @Grant)
	ELSE
		UPDATE FilePermissions SET filegrant=@Grant WHERE principalID=@PrincipalID AND
			fileID=@FileID AND fileaction=@FileAction
	
	COMMIT TRAN UpdFilePerm

GO