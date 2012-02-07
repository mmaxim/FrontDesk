IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_DeleteCourseMember' AND type = 'P')
   DROP PROCEDURE ipbased.fd_DeleteCourseMember
GO

CREATE procedure ipbased.fd_DeleteCourseMember
(
	@CourseID INT,
	@Username NVARCHAR(50)
)
AS
	DECLARE @pid INT, @groupid INT

	--get user pid
	SELECT @pid = principalID FROM Users WHERE username = @Username

	--start the transaction
	BEGIN TRAN RemCourseMem

	--take user out of course
	DELETE FROM CourseMembers 
	WHERE 
		memberPrincipalID = @pid AND 
		courseID = @CourseID
	
	--take user out of all groups in course
	DECLARE group_cursor CURSOR FOR
		SELECT 
			gm.groupID
		FROM 
			GroupMembers gm
		INNER JOIN
			Groups g ON g.principalID = gm.groupID INNER JOIN
			Assignments a ON a.ID = @CourseID AND g.asstID = a.ID
		WHERE
			gm.userID = @pid		
	OPEN group_cursor
	FETCH NEXT FROM group_cursor INTO @groupid
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		-- punt the user
		EXEC ipbased.fd_DeleteGroupMember @Username = @Username, @GroupID = @groupid
		--rescind all user invitations
		DELETE FROM GroupInvitations WHERE groupID = @groupid

		FETCH NEXT FROM group_cursor INTO @groupid
	END
	CLOSE group_cursor
	DEALLOCATE group_cursor

	--take user out of all course sections (TSQL)
	DELETE SectionMembers 
	FROM SectionMembers INNER JOIN
		Sections s ON s.ID = sectionID AND s.CourseID = @CourseID
	WHERE
		userID = @pid

	--commit the transaction
	COMMIT TRAN RemCourseMem
	
GO