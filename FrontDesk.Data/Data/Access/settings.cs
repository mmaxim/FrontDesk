using System;

using FrontDesk.Components;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Summary description for Settings.
	/// </summary>
	public class Settings : DataAccessComponent {
		
		public Settings(AuthorizedIdent ident) : base(ident) { }

		/// <summary>
		/// Gets all the settings for a class within a particular category
		/// Direct Provider layer call
		/// </summary>
		public Setting.SettingList GetCategorySettings(int courseID, int categoryID) {
			return m_dp.GetCategoricalCourseSettings(courseID, categoryID);
		}

		/// <summary>
		/// Gets all the settings for a class within a particular category
		/// Direct Provider layer call
		/// </summary>
		public Setting.SettingList GetAssignmentCategorySettings(int categoryID, int asstID) {
			return m_dp.GetAssignmentCategoricalSettings(categoryID, asstID);
		}


		/// <summary>
		/// Get a setting based on its working name
		/// Direct Provider layer call
		/// </summary>
		public Setting GetSetting(int courseID, string settingName) {
			return m_dp.GetCourseSetting(courseID, settingName);
		}
		
		/// <summary>
		/// Update the course setting value
		/// Direct Provider layer call
		/// </summary>
		public bool UpdateCourseSetting(Setting mySetting){
			return m_dp.UpdateCourseSetting(mySetting);
		}

		/// <summary>
		/// Update the assignment setting value
		/// Direct Provider layer call
		/// </summary>
		public bool UpdateAssignmentSetting(Setting mySetting){
			return m_dp.UpdateAssignmentSetting(mySetting);
		}


		/// <summary>
		/// Get a list of all category settings
		/// Direct Provider layer call
		/// </summary>
		public Setting.Category.CategoryList GetCategories() {
			return m_dp.GetSettingCategories();
		}

	}
}
