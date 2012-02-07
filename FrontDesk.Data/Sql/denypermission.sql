IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DenyPermission' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DenyPermission
GO

CREATE procedure ipbased.fd_DenyPermission
(
	@PrincipalID INT,
	@Perm NVARCHAR(50),
	@EntityType NVARCHAR(50),
	@EntityID INT
)
AS
	DECLARE @prmid INT, @typeid INT, @objid INT

	BEGIN TRAN DenTran

	-- get type id
	SELECT @typeid = ID FROM PermissionObjectTypes WHERE type=@EntityType
	-- get perm id
	SELECT @prmid = ID FROM Permissions 
		WHERE name=@Perm AND typeID = @typeid
	-- get obj id
	SELECT @objid = ID FROM PermissionObjects
		WHERE typeID=@typeid AND entityID=@EntityID

	--deny permission
	DELETE FROM PermissionGrants WHERE
		principalID=@PrincipalID AND objID = @objid AND permID=@prmid

	COMMIT TRAN DenTran

GO