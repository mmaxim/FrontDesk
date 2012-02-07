IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_AssignPermission' AND type = 'P')
   DROP PROCEDURE ipbased.fd_AssignPermission
GO

CREATE procedure ipbased.fd_AssignPermission
(
	@PrincipalID INT,
	@Perm NVARCHAR(50),
	@EntityType NVARCHAR(50),
	@EntityID INT
)
AS
	DECLARE @prmid INT, @typeid INT, @objid INT

	BEGIN TRAN AssTran

	-- get type id
	SELECT @typeid = ID FROM PermissionObjectTypes WHERE type=@EntityType
	-- get perm id
	SELECT @prmid = ID FROM Permissions 
		WHERE name=@Perm AND typeID = @typeid
	-- get obj id
	SELECT @objid = ID FROM PermissionObjects
		WHERE typeID=@typeid AND entityID=@EntityID

	-- assign permission
	INSERT INTO PermissionGrants
		 (principalID, objID, permID)
	VALUES 
		(@PrincipalID, @objid, @prmid) 

	COMMIT TRAN AssTran

GO