IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateAutoJob' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateAutoJob
GO

CREATE procedure ipbased.fd_CreateAutoJob
(
	@Name NVARCHAR(50),
	@Creator NVARCHAR(50),
	@AsstID INT
)
AS

	DECLARE @pid INT

	-- get the pid
	SELECT @pid = principalID FROM Users WHERE username=@Creator

	-- insert the record
	INSERT INTO AutoJobs 
		(name, creatorID, creation, asstID)
	VALUES
		(@Name, @pid, getdate(), @AsstID)

	-- return the ID of the autojob
	SELECT @@Identity
GO