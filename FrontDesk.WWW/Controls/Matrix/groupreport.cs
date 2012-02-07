using System;
using System.Web.UI;

using FrontDesk.Pages;
using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Data.Access;

namespace FrontDesk.Controls.Matrix {
	
	/// <summary>
	/// Group report class
	/// </summary>
	public class GroupReport : Pagelet {
		
		public GroupReport() { }

		protected User.UserList GetUserSet(int sectionID, int courseID) {
			if (sectionID < 0) 
				return new Courses(Globals.CurrentIdentity).GetMembers(courseID, null);
			else
				return new Sections(Globals.CurrentIdentity).GetMembers(sectionID);
		}

		protected string GetAsstPoints(User user, int asstID) {
			Components.Submission latsub = 
				new Principals(Globals.CurrentIdentity).GetLatestGradedSubmission(user.PrincipalID, asstID);
			double total = new Assignments(Globals.CurrentIdentity).GetRubric(asstID).Points;

			if (latsub != null) {	
				double userp = new Users(Globals.CurrentIdentity).GetAsstPoints(user.UserName, asstID);
				return String.Format("{0} / {1} ({2}%)", userp, total, Math.Round((userp/total)*100.0, 2));
			} else 
				return String.Format("?? / {0}", total);
		}
	}
}
