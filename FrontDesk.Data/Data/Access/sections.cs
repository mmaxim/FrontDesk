using System;

using FrontDesk.Components;
using FrontDesk.Data.Provider;
using FrontDesk.Common;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Section data access component
	/// </summary>
	public class Sections : DataAccessComponent {
		
		public Sections(AuthorizedIdent ident) : base(ident) { }

		protected void Log(string msg, int courseID) {
			LogActivity(msg, Activity.COURSE, courseID);
		}	

		/// <summary>
		/// List the members of a section
		/// Direct Provider layer call
		/// </summary>
		public User.UserList GetMembers(int sectionID) {
			return m_dp.GetSectionMembers(sectionID);
		}

		/// <summary>
		/// Get information about a section from the ID
		/// Direct Provider level call
		/// </summary>
		public Section GetInfo(int sectionID) {
			return m_dp.GetSectionInfo(sectionID);
		}

		/// <summary>
		/// Create a new section
		/// </summary>
		public bool Create(string name, string owner, int courseID) {
			
			string actor = Identity.Name;
			
			//Check Permission
			Authorize(courseID, Permission.COURSE, "createsect", courseID, null);

			//Send email to the owner if the owner did not make the change
			if (actor != owner) 
				new EmailWizard(m_ident).SendByUsername(owner, "FrontDesk Section Notification",
					String.Format(Globals.GetMessage("SectionCreate"), actor, name, System.DateTime.Now));

			Section sec = new Section();
			sec.CourseID = courseID;
			sec.Name = name;
			sec.Owner = owner;

			//Make sure the section does not exist
			CheckSectionExists(sec);

			//Create section
			m_dp.CreateSection(sec);

			//Create default permissions
			CreatePermissions(sec.ID, courseID, Permission.SECTION);

			//Log
			Log("User created section: " + name, courseID);

			return true;
		}

		protected void CheckSectionExists(Section sec) {	
			//Check to see if a section with identical attributes exists
			Section.SectionList secs = (new Courses(Globals.CurrentIdentity)).GetSections(sec.CourseID);
			if (secs.Contains(sec))
				throw new DataAccessException("Identical section already exists");
		}

		/// <summary>
		/// Update section information
		/// </summary>
		public bool Update(Section sec) {
			
			//Check permission
			Authorize(sec.CourseID, Permission.SECTION, "update", sec.ID, null);

			//Check for the section already existing
			CheckSectionExists(sec);

			return m_dp.UpdateSection(sec);
		}

		/// <summary>
		/// Remove the section
		/// </summary>
		public bool Delete(int sectionID) {

			//Check permission
			Section sect = GetInfo(sectionID);
			Authorize(sect.CourseID, Permission.SECTION, "delete", sect.ID, null);

			return m_dp.DeleteSection(sectionID);
		}

		/// <summary>
		/// Add a user to a section
		/// switchuser indicates that the user should be dropped 
		/// from all other sections
		/// Email owner of the group if actor != username
		/// </summary>
		public bool AddUser(int sectionID, string username, bool switchuser) {

			Section section = GetInfo(sectionID);
			//Check permission
			Authorize(section.CourseID, Permission.SECTION, "update", sectionID, null);		

			//Check to see if the user is in this group already
			string actor = Identity.Name;
			User.UserList users = GetMembers(sectionID);
			User user = new User(); user.UserName = username;
			if (users.Contains(user))
				throw new DataAccessException("User already exists in the section");

			//Send email to the owner if the owner did not make the change
			
			if (actor != section.Owner) 
				new EmailWizard(m_ident).SendByUsername(section.Owner, "FrontDesk Section Notification",
					String.Format(Globals.GetMessage("SectionAdd"), actor, username, System.DateTime.Now));					

			//TODO: Enforce policy of section membership

			//Create the membership
			m_dp.CreateSectionMember(sectionID, username, switchuser);

			return true;
		}

		/// <summary>
		/// Drop a user from a group
		/// Email owner if actor != the owner
		/// </summary>
		public bool DropUser(int sectionID, string username) {

			string actor = Identity.Name;
			Section section = GetInfo(sectionID);

			//Check permission
			Authorize(section.CourseID, Permission.SECTION, "update", sectionID, null);

			if (actor != section.Owner)
				new EmailWizard(m_ident).SendByUsername(section.Owner, "FrontDesk Section Notification",
					String.Format(Globals.GetMessage("SectionDrop"), actor, username, System.DateTime.Now));

			m_dp.DeleteSectionMember(username, sectionID);

			return true;
		}

		/// <summary>
		/// Gets the grading and submission progress of all users in this section
		/// Direct Provider layer call
		/// </summary>
		public Section.SectionProgress GetGradingProgress(int sectionID, int asstID) {
			return m_dp.GetSectionProgress(sectionID, asstID);
		}

		/// <summary>
		/// Returns the users in this section who are already graded
		/// </summary>
		public User.UserList GetStudentsBySubStatus(int sectionID, int asstID, int status){
			
			Principals prinda = new Principals(Globals.CurrentIdentity);
			User.UserList users = m_dp.GetSectionMembers(sectionID);
			User.UserList retList=new User.UserList();
			
			foreach (User user in users) {
				Components.Submission sub = prinda.GetLatestSubmission(user.PrincipalID, asstID);
				if (sub != null) {
					if (sub.Status == status) {
						retList.Add(user);
					}
				}
			}
			return retList;
		}
	}
}
