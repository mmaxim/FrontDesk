IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetAutoJobs' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetAutoJobs
GO

CREATE procedure ipbased.fd_GetAutoJobs
AS
	SELECT 
		j.ID, j.name, j.creation, 
		u.username
	FROM
		AutoJobs j
	INNER JOIN
		Users u ON u.principalID = j.creatorID
GO