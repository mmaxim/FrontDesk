IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetSectionMembers' AND type = 'P')
   DROP PROCEDURE fd_GetSectionMembers
GO

CREATE procedure ipbased.fd_GetSectionMembers
(
	@SectionID int
)
AS
	SELECT 
		u.principalID, u.firstName, 
	       	u.lastName, u.username, 
	      	u.email, u.admin, u.lastLogin
	FROM
		Users u,
		SectionMembers s
	WHERE
		u.principalID = s.userID AND
		s.sectionID = @SectionID
	ORDER BY 
		u.lastName
	
GO