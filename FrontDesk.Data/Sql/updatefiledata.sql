IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_UpdateFileData' AND type = 'P')
   DROP PROCEDURE fd_UpdateFileData
GO

CREATE procedure ipbased.fd_UpdateFileData
(
	@FileID int,
	@FileData image,
	@Size int
)
AS

	BEGIN TRAN UpdFilDat

	--update size
	UPDATE Files SET fileSize = @Size WHERE ID = @FileID

	--update data
	UPDATE FileData SET data = @FileData WHERE fileID = @FileID
			
	COMMIT TRAN UpdFilDat
GO