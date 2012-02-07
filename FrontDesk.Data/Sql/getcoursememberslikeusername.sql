IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetCourseMembersLikeUserName' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetCourseMembersLikeUserName
GO

CREATE procedure ipbased.fd_GetCourseMembersLikeUserName
(
	@CourseID INT,
	@UserName nvarchar(50)

)
AS
	SELECT 
		u.*
	FROM 
		CourseMembers cm
	INNER JOIN
		Users u ON cm.memberPrincipalID = u.principalID
	WHERE  
		cm.courseID = @CourseID AND u.username LIKE  '%' + @UserName + '%'

	ORDER BY
		u.username ASC
	
GO