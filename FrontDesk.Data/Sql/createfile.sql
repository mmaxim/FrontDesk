IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateFile' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateFile
GO

CREATE procedure ipbased.fd_CreateFile
(
	@Path NVARCHAR(1500),
	@Name NVARCHAR(100),
	@Type INT,
	@Readonly BIT
)
 AS
	IF (EXISTS(SELECT ID FROM Files WHERE filePath = @Path AND fileName = @Name))
	BEGIN
		SELECT * FROM Files WHERE filePath = @Path AND fileName = @Name
	END
	ELSE
	BEGIN
		-- create the file
		INSERT INTO Files (fileName, filePath, fileType, fileAlias, fileCreated, fileModified, fileSpecialType, readonly, fileSize) 
				values (@Name, @Path, @Type, @Name, getdate(), getdate(), 1, @Readonly, 0)
		SELECT * FROM Files WHERE ID = @@Identity
	END

GO