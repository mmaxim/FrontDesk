IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetAsstGroups' AND type = 'P')
   DROP PROCEDURE fd_GetAsstGroups
GO

CREATE procedure ipbased.fd_GetAsstGroups
(
	@AsstID int
)
AS
	-- do the joins to the necessary necessary information
	SELECT 
		g.principalID, g.groupName, g.creation, g.asstID,
		u.userName AS creator
	FROM 
		Groups g, 
		Users u
	WHERE
		u.principalID = g.creatorID AND
		g.asstID = @AsstID
	ORDER BY 
		g.groupName ASC

GO