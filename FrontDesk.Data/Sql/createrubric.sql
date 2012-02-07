IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_CreateRubric' AND type = 'P')
   DROP PROCEDURE ipbased.fd_CreateRubric
GO

CREATE procedure ipbased.fd_CreateRubric
(
	@AsstID INT
)
AS

	BEGIN TRAN CreRub

	--create root rubric entry
	INSERT INTO RubricForest
		(parentID, points, shortdesc, longdesc, asstID, evalID, allowneg)
	VALUES
		(-1, 0, 'Rubric', 'Define the evaluations for this assignment', @AsstID, NULL, 0)

	COMMIT TRAN CreRub

	SELECT @@Identity
GO