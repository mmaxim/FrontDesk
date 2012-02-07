IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetCourseMembersLikeLastName' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetCourseMembersLikeLastName
GO

CREATE procedure ipbased.fd_GetCourseMembersLikeLastName
(
	@CourseID INT,
	@LastName nvarchar(50)

)
AS
	SELECT 
		u.*
	FROM 
		CourseMembers cm
	INNER JOIN
		Users u ON cm.memberPrincipalID = u.principalID
	WHERE  
		cm.courseID = @CourseID AND u.lastName LIKE  '%' + @LastName + '%'

	ORDER BY
		u.lastName ASC
	
GO