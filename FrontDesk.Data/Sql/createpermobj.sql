IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreatePermObj' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreatePermObj
GO

CREATE procedure ipbased.fd_CreatePermObj
(
	@Type NVARCHAR(50),
	@EntityID INT
)
AS
	DECLARE @typeID INT

	--create permission object
	SELECT @typeID = ID FROM PermissionObjectTypes WHERE type=@Type
	INSERT INTO PermissionObjects (typeID, entityID)
		VALUES (@typeID, @EntityID)	

GO