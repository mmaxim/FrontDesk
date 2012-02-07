IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetCourseSettingByName' AND type = 'P')
   DROP PROCEDURE fd_GetCourseSettingByName
GO

CREATE procedure ipbased.fd_GetCourseSettingByName
(
	@CourseID int,
	@SettingName nvarchar(50)
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
		settings.name = @SettingName
GO