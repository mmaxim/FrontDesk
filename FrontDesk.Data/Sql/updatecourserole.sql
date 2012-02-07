IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_UpdateCourseRole' AND type = 'P')
   DROP PROCEDURE ipbased.fd_UpdateCourseRole
GO

CREATE procedure ipbased.fd_UpdateCourseRole
(
	@CourseID INT,
	@Username NVARCHAR(50),
	@Role NVARCHAR(50)
)
AS

	DECLARE @roleid int

	-- get role id
	SELECT @roleid = principalID FROM CourseRoles WHERE name = @Role AND courseID = @CourseID

	-- change the role for the specified user
	UPDATE 
		CourseMembers 
	SET 
		courseRoleID = @roleid 
	FROM 
		CourseMembers cm
	INNER JOIN 
		Users u ON cm.memberPrincipalID = u.principalID AND u.username=@Username
	WHERE
		cm.courseID = @CourseID

GO