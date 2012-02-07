IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetCourseSettings' AND type = 'P')
   DROP PROCEDURE fd_GetCourseSettings
GO

CREATE procedure ipbased.fd_GetCourseSettings
(
	@CourseID int,
	@CategoryID int
)
AS
	SELECT 
		courseset.ID, courseset.courseID, 
		settings.name AS Setting, categories.name as Category, 
		types.settingInputType as SettingType,
		courseset.settingValue as SettingValue,
		settings.activator
	FROM 
		CourseSettings courseset,
		SettingInputTypes types,
		SettingCategories categories,
		Settings settings
	WHERE 
		courseset.courseID = @CourseID AND
		courseset.settingID = settings.ID AND
		settings.settingCategoryID = categories.ID AND
		settings.settingInputTypeID = types.ID AND
		categories.ID = @CategoryID

	ORDER BY settings.activator DESC
		
GO