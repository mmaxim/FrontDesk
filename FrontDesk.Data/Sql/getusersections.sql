IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetUserSections' AND type = 'P')
   DROP PROCEDURE fd_GetUserSections
GO

CREATE procedure ipbased.fd_GetUserSections
(
	@Username nvarchar(50),
	@CourseID int
)
AS
	-- get user sections
	SELECT 
		s.ID, s.name, s.courseID, u.userName AS owner
	FROM
		Sections s
	INNER JOIN
		Users u1 ON u1.username = @Username INNER JOIN
		Users u ON u.principalID = s.ownerID INNER JOIN
		SectionMembers sm ON sm.sectionID = s.ID AND sm.userID = u1.principalID
	WHERE
		s.courseID = @CourseID
	ORDER BY
		s.name

GO