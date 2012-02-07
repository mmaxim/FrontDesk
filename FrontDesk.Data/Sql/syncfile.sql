IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_SyncFile' AND type = 'P')
   DROP PROCEDURE ipbased.fd_SyncFile
GO

CREATE procedure ipbased.fd_SyncFile
(
	@FileID INT,
	@Name NVARCHAR(1500),
	@Path NVARCHAR(100),
	@Type INT,
	@Modified DATETIME,
	@SpecialType INT,
	@Alias NVARCHAR(100),
	@Readonly BIT,
	@Size INT,
	@Description TEXT
)
 AS

	
	-- update file table
	UPDATE Files SET fileName = @Name, filePath = @Path, 
		fileType = @Type, fileModified = @Modified, 
		fileSpecialType = @SpecialType, 
		fileAlias = @Alias, readonly = @Readonly, fileSize = @Size,
		description=@Description
	WHERE ID = @FileID

GO