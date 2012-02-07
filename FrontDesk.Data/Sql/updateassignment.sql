IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_UpdateAssignment' AND type = 'P')
   DROP PROCEDURE ipbased.fd_UpdateAssignment
GO

CREATE procedure ipbased.fd_UpdateAssignment
(
	@AsstID INT,
	@Description NVARCHAR(200),
	@DueDate DATETIME,
	@ContentID INT,
	@StuRelease BIT,
	@ResRelease BIT,
	@Format NTEXT
)
AS
	-- update assignment record
	UPDATE 
		Assignments 
	SET 
		description = @Description,
		contentID = @ContentID,
		dueDate = @DueDate, 
		sturelease = @StuRelease,
		resrelease = @ResRelease,
		format = @Format
	WHERE ID = @AsstID

GO