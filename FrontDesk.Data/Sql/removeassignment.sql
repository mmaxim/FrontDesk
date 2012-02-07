IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_RemoveAssignment' AND type = 'P')
   DROP PROCEDURE fd_RemoveAssignment
GO

CREATE procedure ipbased.fd_RemoveAssignment
(
	@UserName nvarchar(50)
)
AS