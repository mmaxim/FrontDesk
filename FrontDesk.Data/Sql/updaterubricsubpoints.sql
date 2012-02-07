IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_UpdateRubricSubPoints' AND type = 'P')
   DROP PROCEDURE ipbased.fd_UpdateRubricSubPoints
GO

CREATE procedure ipbased.fd_UpdateRubricSubPoints
(
	@RubricID INT,
	@SubID INT
)
AS
	DECLARE @tpoints FLOAT
	DECLARE @rpoints FLOAT	
	DECLARE @parentID INT, @chilID INT
	DECLARE @allowneg BIT

	-- get initial points
	SELECT @parentID=parentID FROM RubricForest WHERE ID=@RubricID

	--make right initial points assignment
	IF (@parentID >= 0)
	BEGIN
		SELECT @tpoints = points, @allowneg=allowneg FROM RubricForest
			WHERE ID = @RubricID
		IF @tpoints < 0
			SET @tpoints = 0
	END
	ELSE
		SET @tpoints = 0

	-- grab all the auto results for the rubric entry and sub
	DECLARE ares_cursor CURSOR FOR
	SELECT points FROM Results WHERE 
		rubricID=@RubricID AND subID=@SubID
	OPEN ares_cursor	
	
	-- sum up auto results
	FETCH NEXT FROM ares_cursor INTO @rpoints
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @tpoints = @tpoints + @rpoints
		FETCH NEXT FROM ares_cursor INTO @rpoints
	END

	CLOSE ares_cursor
	DEALLOCATE ares_cursor

	-- stop neg is disallowed
	if (@allowneg = 0 AND @tpoints < 0)
		SET @tpoints = 0

	-- grab rubric children
	DECLARE chil_cursor CURSOR FOR
	SELECT r.ID 
	FROM 
		RubricForestView r 
	INNER JOIN
		RubricPoints rp ON rp.rubID = r.ID AND rp.subID = @SubID
	WHERE r.parentID = @RubricID
	OPEN chil_cursor

	-- sum up their point total
	FETCH NEXT FROM chil_cursor INTO @chilID
	WHILE @@FETCH_STATUS = 0
	BEGIN	
		SELECT @rpoints = points FROM RubricPoints 
			WHERE rubID=@chilID AND subID=@subID

		SET @tpoints = @tpoints + @rpoints
		FETCH NEXT FROM chil_cursor INTO @chilID
	END

	-- update rubric points entry
	IF ((SELECT Count(*) FROM RubricPoints WHERE rubID=@RubricID AND subID=@SubID) > 0)	
		UPDATE RubricPoints SET points = @tpoints WHERE
			rubID=@RubricID AND subID=@SubID
	ELSE
		INSERT INTO RubricPoints (rubID, subID, points) VALUES
			(@RubricID, @SubID, @tpoints)


	CLOSE chil_cursor
	DEALLOCATE chil_cursor
	
	-- let the parent update
	IF (@parentID >= 0)
		EXEC ipbased.fd_UpdateRubricSubPoints @RubricID=@parentID, @SubID=@SubID
GO