IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CheckPermission' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CheckPermission
GO

CREATE procedure ipbased.fd_CheckPermission
(
	@Username NVARCHAR(50),
	@CourseID INT,
	@Perm NVARCHAR(50),
	@EntityType NVARCHAR(50),
	@EntityID INT
)
AS
	DECLARE @typeID INT, @objID INT, @permID INT
	DECLARE @pid INT, @roleid INT

	--get pid
	SELECT @pid = principalID FROM Users WHERE username=@Username
	--get roleid
	SELECT @roleid = courseRoleID FROM CourseMembers 
		WHERE courseID=@CourseID AND memberPrincipalID=@pid 
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
		(principalID = @roleid OR principalID = @pid) AND
		objID = @objID AND
		permID = @permID
GO
	