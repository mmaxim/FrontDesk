IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetGroupMembers' AND type = 'P')
   DROP PROCEDURE fd_GetGroupMembers
GO

CREATE PROCEDURE ipbased.fd_GetGroupMembers
(
	@GroupID int
)
AS

SELECT
	u.* 
FROM
	Users AS u,
	GroupMembers AS gm
WHERE
	gm.groupID = @GroupID AND
	u.principalID = gm.userID
ORDER BY
	u.userName ASC

GO