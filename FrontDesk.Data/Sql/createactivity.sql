IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateActivity' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateActivity
GO

CREATE procedure ipbased.fd_CreateActivity
(
	@Username NVARCHAR(50),
	@Type INT,
	@ObjID INT,
	@Desc NTEXT
)
AS

	DECLARE @pid INT

	BEGIN TRAN CreAct

	--get pid
	SELECT @pid = principalID FROM Users WHERE username=@Username

	--insert entry	
	INSERT INTO ActivityLog
		(principalID, type, description, acttime, objID)
	VALUES
		(@pid, @Type, @Desc, getdate(), @ObjID)

	COMMIT TRAN CreAct
GO