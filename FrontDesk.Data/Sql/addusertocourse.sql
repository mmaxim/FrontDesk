IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_AddUserToCourse' AND type = 'P')
   DROP PROCEDURE ipbased.fd_AddUserToCourse
GO

CREATE PROC ipbased.fd_AddUserToCourse
(
	@CourseID INT,
	@UserName NVARCHAR(50),
	@Role NVARCHAR(50)
) AS

	DECLARE @pid INT, @roleid INT

	-- get pid
	SELECT @pid = principalID FROM Users WHERE userName = @UserName

	-- get role id
	SELECT @roleid = principalID FROM CourseRoles WHERE name=@Role AND courseID = @CourseID

	-- create membership row
	INSERT INTO CourseMembers
		(courseID, memberPrincipalID, courseRoleID)
	VALUES
		(@CourseID, @pid, @roleid)

GO