IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateRole' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateRole
GO

CREATE procedure ipbased.fd_CreateRole
(
	@Name NVARCHAR(50),
	@CourseID INT,
	@Staff BIT
)
AS
	
	BEGIN TRAN CreRole

	DECLARE @pid INT

	--create principal
	INSERT INTO Principals (type) VALUES (3)
	SELECT @pid = @@Identity	

	--create role
	INSERT INTO CourseRoles
		(principalID, name, courseID, isstaff)
	VALUES
		(@pid, @Name, @CourseID, @Staff)

	COMMIT TRAN CreRole

	-- return pid
	SELECT @pid
GO