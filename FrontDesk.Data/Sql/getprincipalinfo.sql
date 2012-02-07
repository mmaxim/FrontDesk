IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetPrincipalInfo' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetPrincipalInfo
GO

CREATE PROCEDURE ipbased.fd_GetPrincipalInfo
(
	@Pid INT
)
AS
	DECLARE @type INT

	-- get the type
	SELECT @type = type FROM Principals WHERE ID = @Pid

	-- if the principal is a user
	IF (@type = 1)
		SELECT 
			@type AS type, u.userName AS name, @Pid AS principalID
		FROM 
			Users u
		WHERE 
			u.principalID = @Pid
	-- if the principal is a group
	ELSE IF (@type = 2)	
		SELECT 
			@type AS type, g.groupName AS name, @Pid AS principalID
		FROM 
			Groups g
		WHERE 
			g.principalID = @Pid
	-- if the principal is a role
	ELSE IF (@type = 3)
		SELECT
			@type AS type, cr.name AS name, @Pid AS principalID
		FROM
			CourseRoles cr
		WHERE
			cr.principalID = @Pid	
GO