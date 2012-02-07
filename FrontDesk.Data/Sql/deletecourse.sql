IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteCourse' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeleteCourse
GO

CREATE procedure ipbased.fd_DeleteCourse
(
	@CourseID INT
)
AS
	DECLARE @pid INT, @groupid INT

	--take user out of all course sections (TSQL)
	DELETE SectionMembers 
	FROM SectionMembers INNER JOIN
		Sections s ON s.ID = sectionID AND s.CourseID = @CourseID
	WHERE
		userID = @pid

	--take perm obj
	EXEC ipbased.fd_DeletePermObject @EntityID=@CourseID, @EntityType='course'
	
	--delete
	DELETE FROM CourseMembers WHERE courseID=@CourseID
	DELETE FROM CourseRoles WHERE courseID=@CourseID
	DELETE FROM Courses WHERE ID=@CourseID
	
GO