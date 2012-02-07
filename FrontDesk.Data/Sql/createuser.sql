IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateNewUser' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateNewUser
GO

CREATE procedure ipbased.fd_CreateNewUser
(
	@UserName nvarchar(50),
	@PassWord nvarchar(50),
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50),
	@Admin bit,
	@VerifyKey NVARCHAR(50)
)
AS
	DECLARE @pid int

	BEGIN TRAN UserCreate
	
	INSERT INTO Principals (type) values (1)
	SELECT @pid = @@Identity
	INSERT INTO Users (principalID, username, password, firstName, lastName, email, admin, lastLogin, verifykey) values
		(@pid, @UserName, @Password, @FirstName, @LastName, @Email, @Admin, getdate(), @VerifyKey)
	
	COMMIT TRAN UserCreate

	SELECT @pid
GO
