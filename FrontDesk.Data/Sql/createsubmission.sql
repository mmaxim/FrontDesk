IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateSubmission' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateSubmission
GO

CREATE PROCEDURE ipbased.fd_CreateSubmission
(
	@LocationID INT,
	@AsstID INT,
	@PrincipalID INT,
	@Status INT
)
AS
	DECLARE @locid INT

	-- check for null
	IF (@LocationID >= 0)
		SET @locid = @LocationID

	-- create the new row
	INSERT INTO Submissions (directoryID, asstID, principalID, subTime, status)
		 values (@locid, @AsstID, @PrincipalID, getdate(), @Status)

	SELECT @@Identity

GO
