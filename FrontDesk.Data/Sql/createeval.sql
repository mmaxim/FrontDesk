IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateEval' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateEval
GO

CREATE PROCEDURE ipbased.fd_CreateEval
(
	@Creator NVARCHAR(50),
	@TimeLimit INT,
	@AsstID INT,
	@RunOnSubmit BIT,
	@Competitive BIT,
	@TypeID INT,
	@Manager INT,
	@ResType NVARCHAR(50),
	@RetID INT OUTPUT
)
AS
	DECLARE @pid INT, @resTypeID INT

	-- do the insertion in a transaction
	BEGIN TRAN CreEval
	SELECT @resTypeID = ID FROM ResultTypes WHERE name=@ResType
	SELECT @pid = principalID FROM Users WHERE username=@Creator
	INSERT INTO Evaluations
		(typeID, timeLimit, asstID, runonsubmit, competitive, creatorID, manager, resTypeID)
	VALUES
		(@TypeID, @TimeLimit, @AsstID, @RunOnSubmit, @Competitive, @pid, @Manager, @resTypeID)
	COMMIT TRAN CreEval

	--give back the ID
	SET @RetID = @@Identity
GO