IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetUserCourses' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetUserCourses
GO

CREATE procedure ipbased.fd_GetUserCourses
(
	@UserName NVARCHAR(50)
)
 AS
	DECLARE @pid INT

	--get the pid
	SELECT @pid = principalID FROM Users WHERE userName = @UserName

	SELECT 
		c.*
	FROM 
		CourseMembers cm
	INNER JOIN
		Courses c ON cm.courseID = c.ID
	WHERE
		cm.memberPrincipalID = @pid
	ORDER BY
		c.courseName ASC
GO