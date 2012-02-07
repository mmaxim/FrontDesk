IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetBackups' AND type = 'P')
   DROP PROCEDURE fd_GetBackups
GO

CREATE procedure ipbased.fd_GetBackups
(
	@CourseID int
)
AS
	IF (@CourseID < 0)
	BEGIN
		SELECT 
			b.ID, b.creation, b.backedup, b.fileLocation, b.courseID,
			u.username AS creator	
		FROM 
			Backups b
		INNER JOIN
			Users u ON u.principalID = b.creator
		ORDER BY
			b.creation DESC
	END
	ELSE
	BEGIN
		SELECT 
			b.ID, b.creation, b.backedup, b.fileLocation, b.courseID,
			u.username AS creator	
		FROM 
			Backups b
		INNER JOIN
			Users u ON u.principalID = b.creator
		WHERE 
			b.courseID = @CourseID
		ORDER BY
			b.creation DESC
	END

GO