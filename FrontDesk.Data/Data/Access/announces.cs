using System;

using FrontDesk.Components;
using FrontDesk.Common;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {
	/// <summary>
	/// Announcements data access component
	/// </summary>
	public class Announcements : DataAccessComponent {
		
		public Announcements(AuthorizedIdent ident) : base(ident) { }

		/// <summary>
		/// Create a new announcement
		/// </summary>
		public bool Create(string poster, string title, string desc, int courseid) {

			//Check permission
			Authorize(courseid, Permission.COURSE, "createannou", courseid, null);

			Announcement annou = new Announcement();
			annou.Description = desc;
			annou.Poster = poster;
			annou.CourseID = courseid;
			annou.Title = title;

			//HTMLize the desc
			annou.Description = HTMLWizard.LineBreakToBR(annou.Description);
			
			return m_dp.CreateAnnouncement(annou);
		}

		/// <summary>
		/// Get info about an announcement
		/// Direct Provider layer call
		/// </summary>
		public Announcement GetInfo(int annID) {
			return m_dp.GetAnnouncementInfo(annID);
		}

		/// <summary>
		/// Update the annnouncement
		/// </summary>
		public bool Update(Announcement announce) {
		
			//Check permission
			Authorize(announce.CourseID, Permission.COURSE, "updateannou", announce.CourseID, null);

			announce.Description = HTMLWizard.LineBreakToBR(announce.Description);
			return m_dp.UpdateAnnouncement(announce);
		}

		/// <summary>
		/// Delete the assignment
		/// </summary>
		public bool Delete(int announcementID) {
			
			//Check permission
			Announcement annou = GetInfo(announcementID);
			Authorize(annou.CourseID, Permission.COURSE, "deleteannou", annou.CourseID, null);

			return m_dp.DeleteAnnouncement(announcementID);
		}

	}
}
