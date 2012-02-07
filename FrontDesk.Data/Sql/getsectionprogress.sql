IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetSectionProgress' AND type = 'P')
   DROP PROCEDURE ipbased.fd_GetSectionProgress
GO

CREATE procedure ipbased.fd_GetSectionProgress
(
	@SectionID INT,
	@AsstID INT
)
AS

	DECLARE @pid INT
	DECLARE @totalStu INT, @totalSub INT, @totalGraded INT
	DECLARE @gradedSubsU INT, @gradedSubsG INT
	DECLARE @SubsU INT, @SubsG INT

	-- get totalStu
	SELECT @totalStu=Count(*) FROM SectionMembers WHERE sectionID=@SectionID

	DECLARE sect_cursor CURSOR FOR
	SELECT userID FROM SectionMembers WHERE sectionID=@SectionID
	OPEN sect_cursor	
	
	SET @totalSub = 0
	SET @totalGraded = 0
	-- run through prins for stats
	FETCH NEXT FROM sect_cursor INTO @pid
	WHILE @@FETCH_STATUS = 0
	BEGIN

		-- check student graded
		SELECT @gradedSubsU=Count(*) FROM
			Submissions s,
			GroupMembers gm,
			Groups g
		WHERE
			gm.userID = @pid AND 
			gm.groupID = s.principalID AND
			s.asstID = @AsstID AND
			s.status = 2 AND
			g.principalID = gm.groupID
		
		SELECT @gradedSubsG=Count(*) FROM 
			Submissions s, Users u 
		WHERE 
			s.principalID = @pid AND 
			s.asstID = @AsstID AND
			s.status = 2 AND
			u.principalID = @pid	

		IF (@gradedSubsU > 0 OR @gradedSubsG > 0)
			SET @totalGraded = @totalGraded + 1	

		-- check for any submissions
		SELECT @SubsU=Count(*) FROM
			Submissions s,
			GroupMembers gm,
			Groups g
		WHERE
			gm.userID = @pid AND 
			gm.groupID = s.principalID AND
			s.asstID = @AsstID AND
			g.principalID = gm.groupID
		
		SELECT @SubsG=Count(*) FROM 
			Submissions s, Users u 
		WHERE 
			s.principalID = @pid AND 
			s.asstID = @AsstID AND
			u.principalID = @pid	

		IF (@SubsU > 0 OR @SubsG > 0)
			SET @totalSub = @totalSub + 1

		FETCH NEXT FROM sect_cursor INTO @pid
	END

	CLOSE sect_cursor
	DEALLOCATE sect_cursor

	SELECT @totalStu AS totalStudents, @totalGraded AS totalGraded, @totalSub AS totalSubmit

GO