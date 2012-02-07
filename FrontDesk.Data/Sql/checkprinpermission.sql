IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CheckPrinPermission' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CheckPrinPermission
GO

CREATE procedure ipbased.fd_CheckPrinPermission
(
	@PrincipalID INT,
	@CourseID INT,
	@Perm NVARCHAR(50),
	@EntityType NVARCHAR(50),
	@EntityID INT
)
AS
	DECLARE @typeID INT, @objID INT, @permID INT

	--get typeid
	SELECT @typeID = ID FROM PermissionObjectTypes WHERE type=@EntityType
	--get objid
	SELECT @objID = ID FROM PermissionObjects 
		WHERE entityID = @EntityID AND typeID = @typeID
	--get permid
	SELECT @permID = ID FROM Permissions
		WHERE name=@Perm AND typeID = @typeID

	--check for perm
	SELECT Count(*) FROM PermissionGrants WHERE
		principalID = @PrincipalID AND
		objID = @objID AND
		permID = @permID
GO
	