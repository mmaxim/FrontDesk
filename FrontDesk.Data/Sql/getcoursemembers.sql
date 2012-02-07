IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetCourseMembers' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetCourseMembers
GO

CREATE procedure ipbased.fd_GetCourseMembers
(
	@CourseID INT
)
AS
	SELECT 
		u.*
	FROM 
		CourseMembers cm
	INNER JOIN
		Users u ON cm.memberPrincipalID = u.principalID
	WHERE  
		cm.courseID = @CourseID
	ORDER BY
		u.lastName ASC
	
GO