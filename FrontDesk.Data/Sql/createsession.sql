IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateSession' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateSession
GO

CREATE PROCEDURE ipbased.fd_CreateSession
(
	@Guid UNIQUEIDENTIFIER,
	@Username NVARCHAR(50),
	@Address NVARCHAR(50)
)
AS
	DECLARE @pid INT

	-- get pid
	SELECT @pid = principalID FROM Users WHERE username=@Username	

	INSERT INTO Sessions 
		(guid, principalID, address, creation)
	VALUES
		(@Guid, @pid, @Address, getdate())

GO
