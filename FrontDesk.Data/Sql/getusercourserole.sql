IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetUserCourseRole' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetUserCourseRole
GO

CREATE procedure ipbased.fd_GetUserCourseRole
(
	@Username NVARCHAR(50),
	@CourseID INT
)
AS

	DECLARE @pid INT

	--get pid
	SELECT @pid = principalID FROM Users WHERE username=@Username

	--get role
	SELECT
		cr.*
	FROM
		CourseRoles cr
	INNER JOIN
		CourseMembers cm ON 
			cm.memberPrincipalID = @pid AND cm.courseRoleID = cr.principalID
	WHERE
		cr.courseID = @CourseID		
	
GO