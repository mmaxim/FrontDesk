IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeletePermObject' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeletePermObject
GO

CREATE PROCEDURE ipbased.fd_DeletePermObject
(
	@EntityID INT,
	@EntityType NVARCHAR(50)
)
AS

	DECLARE @oid INT, @tid INT

	BEGIN TRAN DelPermObj

	--get type
	SELECT @tid = ID FROM PermissionObjectTypes WHERE type=@EntityType

	--get objid
	SELECT @oid = ID FROM PermissionObjects 
		WHERE typeID=@tid AND entityID=@EntityID

	--take grants
	DELETE FROM PermissionGrants WHERE objID=@oid

	--take object
	DELETE FROM PermissionObjects WHERE ID=@oid

	COMMIT TRAN DelPermObj

GO