IF EXISTS(SELECT name FROM sysobjects
      WHERE name = 'fd_GetAssignmentSettings' AND type = 'P')
   DROP PROCEDURE fd_GetAssignmentSettings
GO

CREATE procedure ipbased.fd_GetAssignmentSettings
(
	@AsstID int,
	@CategoryID int
)
AS
	SELECT 
		asstset.ID, asstset.asstID, 
		settings.name AS Setting, categories.name as Category, 
		types.settingInputType as SettingType,
		asstset.settingValue as SettingValue,
		settings.activator
	FROM 
		AssignmentSettings asstset,
		SettingInputTypes types,
		SettingCategories categories,
		Settings settings
	WHERE 
		asstset.asstID = @AsstID AND
		asstset.settingID = settings.ID AND
		settings.settingCategoryID = categories.ID AND
		settings.settingInputTypeID = types.ID AND
		categories.ID = @CategoryID

	ORDER BY settings.activator DESC
		
GO