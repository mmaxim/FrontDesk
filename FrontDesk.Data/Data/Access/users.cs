using System;
using System.IO;
using System.Xml;
using System.Data;

using FrontDesk.Components.Evaluation;
using FrontDesk.Components;
using FrontDesk.Common;
using FrontDesk.Data.Provider;
using FrontDesk.Data.Filesys;
using FrontDesk.Tools;
using FrontDesk.Tools.Export;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Users facade
	/// </summary>
	public class Users : DataAccessComponent {

		public Users(AuthorizedIdent ident) : base(ident) { }

		/// <summary>
		/// Returns extended user information
		/// If the user is unknown, error handling takes place
		/// </summary>
		public User GetInfo(string username, IProviderTransaction tran) {
			User user = new User();
			if (!m_dp.GetUserInfo(username, user, tran)) {
				//TODO: Do error handling stuff
				return null;
			}
			return user;
		}

		/// <summary>
		/// Get the courses the user is registered for
		/// Direct Provider call
		/// </summary>
		public Course.CourseList GetCourses(string username) {
			return m_dp.GetUserCourses(username);
		}

		/// <summary>
		/// Return a list of all groups the user is in
		/// Direct Provider level call
		/// </summary>
		public Group.GroupList GetGroups(string username, int asstID) {
			return m_dp.GetUserGroups(username, asstID);
		}

		/// <summary>
		/// Get principal submissions
		/// Direct Provider layer call
		/// </summary>
		public Components.Submission.SubmissionList GetAsstSubmissions(string username, int asstID) {
			return m_dp.GetUserSubmissions(username, asstID, false);
		}

		/// <summary>
		/// Get all submissions for all assignments
		/// Direct Provider layer call
		/// </summary>
		public Components.Submission.SubmissionList GetCourseSubmissions(string username, int courseID) {
			return m_dp.GetUserSubmissions(username, courseID, true);
		}

		/// <summary>
		/// Get all the sections a user in for a given course
		/// </summary>
		public Section.SectionList GetSections(string username, int courseID) {
			return m_dp.GetUserSections(courseID, username);
		}

		/// <summary>
		/// Get a PrincipalList with the user and all their groups
		/// </summary>
		public Principal.PrincipalList GetPrincipals(string username, int asstID) {

			Principal.PrincipalList plist = new Principal.PrincipalList();
			User user = new User();

			//Add the user principal
			m_dp.GetUserInfo(username, user, null);
			plist.Add(user);

			//Add the groups
			Group.GroupList glist = m_dp.GetUserGroups(username, asstID);
			plist.AddRange(glist);

			return plist;
		}

		/// <summary>
		/// Get earned points by a user for an assignment
		/// </summary>
		public double GetAsstPoints(string username, int asstID) {
			Submission sub = new Principals(m_ident).GetLatestGradedSubmission(
				GetInfo(username, null).PrincipalID, asstID);

			if (sub != null) 
				return new Submissions(m_ident).GetPoints(sub.ID);
			else
				return 0;
		}

		/// <summary>
		/// Get total points for a course
		/// </summary>
		public double GetCoursePoints(string username, int courseID) {
			Assignment.AssignmentList assts = 
				new Courses(Globals.CurrentIdentity).GetAssignments(courseID);
			double points=0;

			foreach (Assignment asst in assts) 
				points += GetAsstPoints(username, asst.ID);
			
			return points;
		}

		/// <summary>
		/// Get a user's grades for a course in "export" format
		/// </summary>
		public ExportRow GetCourseExport(string username, int courseID) {

			double totpoints, points;
			ExportRow row = new ExportRow();
			Assignment.AssignmentList assts = new Courses(m_ident).GetAssignments(courseID);

			//User
			row.Fields.Add(username);
			
			totpoints = 0;
			foreach (Assignment asst in assts) {
				row.Fields.AddRange(GetAsstExport(username, asst.ID, out points).Fields);
				totpoints += points;
			}

			//Course Total
			row.Fields.Insert(1, totpoints.ToString());

			return row;
		}

		/// <summary>
		/// Get the export heading for a course export
		/// </summary>
		public ExportRow GetCourseExportHeading(int courseID) {
			
			ExportRow row = new ExportRow();
			Assignment.AssignmentList assts = new Courses(m_ident).GetAssignments(courseID);
			
			//User
			row.Fields.Add("Username");
			//Course Total
			row.Fields.Add("Course Total");
			foreach (Assignment asst in assts)
				row.Fields.AddRange(GetAsstExportHeading(asst.ID).Fields);

			return row;
		}

		public ExportRow GetAsstExport(Components.Submission sub, int asstID, out double totpoints) {
			Rubrics rubda = new Rubrics(m_ident);
			ExportRow row = new ExportRow();

			//Get all rubric entries for the assignment
			Rubric rub = new Assignments(m_ident).GetRubric(asstID);
			Rubric.RubricList rublist = rubda.Flatten(rub);

			//Tally	
			//Cats
			double points=0;
			foreach (Rubric rubent in rublist) {
				if (sub == null)
					row.Fields.Add("0");
				else {
					double catpoints = rubda.GetPoints(rubent.ID, sub.ID);
					points += catpoints;
					row.Fields.Add(catpoints.ToString());
				}
			}

			//Total
			row.Fields.Insert(0, points.ToString());
			totpoints = points;
			
			return row;
		}

		/// <summary>
		/// Get a user's grades for an asst in "export" format
		/// </summary>
		public ExportRow GetAsstExport(string username, int asstID, out double totpoints) {
			return GetAsstExport(
				new Principals(m_ident).GetLatestGradedSubmission(GetInfo(username, null).PrincipalID, asstID),
				asstID, out totpoints);
		}

		/// <summary>
		/// Get the heading row for an export
		/// </summary>
		public ExportRow GetAsstExportHeading(int asstID) {

			Rubrics rubda = new Rubrics(m_ident);
			ExportRow row = new ExportRow();

			//Get all rubric entries for the assignment
			Assignment asst = new Assignments(m_ident).GetInfo(asstID);
			Rubric rub = new Assignments(m_ident).GetRubric(asstID);
			Rubric.RubricList rublist = rubda.Flatten(rub);

			//Total
			row.Fields.Add(asst.Description + " Total");
			foreach (Rubric rubent in rublist)
				row.Fields.Add(rubent.Name);

			return row;
		}

		/// <summary>
		/// Return the list of invitations the user has
		/// Direct Provider layer call
		/// </summary>
		public Invitation.InvitationList GetInvitations(string username, int asstID) {
			return m_dp.GetUserInvites(username, asstID);
		}

		/// <summary>
		/// Return all users in the system
		/// Results in a direct call to the Provider layer
		/// </summary>
		public User.UserList GetAll() {
			return m_dp.GetAllUsers();
		}

		private void SendVerifyEmail(User user) {
			string body = String.Format("To activate your FrontDesk account, please attempt to login and enter this verification key when prompted: {0} ",
				user.VerifyKey);

			new EmailWizard(m_ident).SendByEmail(user.Email, "FrontDesk Account Activation", body);
		}

		/// <summary>
		/// Create a new user
		/// </summary>
		public bool Create(string username, string password, 
						   string first, string last, string email,
						   IProviderTransaction tran) {
			
			if (GetInfo(username, null) != null)
				throw new DataAccessException("User already exists");

			if (password.Length < Globals.MinPasswordLength)
				throw new 
					DataAccessException("Password must be at least " + Globals.MinPasswordLength + 
					" characters long!");

			User user = new User();
			user.UserName = username;
			user.FirstName = first;
			user.LastName = last;
			user.Email = email;
			user.VerifyKey = Globals.GeneratePassword(8);

			bool result = m_dp.CreateUser(user, password, tran);
			if (result) 
				SendVerifyEmail(user);
			return result;
		}

		/// <summary>
		/// Update user information
		/// </summary>
		public bool Update(User user, IProviderTransaction tran) {
			return m_dp.UpdateUser(user, tran);
		}

		/// <summary>
		/// Email forgotten password
		/// </summary>
		public bool ForgotPassword(string username) {

			string password = m_dp.GetUserPassword(username);
			if (password == null)
				throw new DataAccessException("Sorry, no such username registered with the system");

			//Send mail
			new EmailWizard(m_ident).SendByUsername(username, "FrontDesk Notification",
				"Hello, you password is: " + password);
			
			return true;
		}

		/// <summary>
		/// Get a user's password
		/// Direct Provider layer call
		/// </summary>
		public string GetPassword(string username) {
			return m_dp.GetUserPassword(username);
		}

		/// <summary>
		/// Change the user's password
		/// </summary>
		public bool ChangePassword(string username, string password) {

			//Validate the password quality
			if (password.Length < Globals.MinPasswordLength)
				throw new 
					DataAccessException("Password must be at least " + Globals.MinPasswordLength + 
									    " characters long!");

			//Send mail and change the password
			if (username != m_ident.Name) 
				new EmailWizard(m_ident).SendByUsername(username, "FrontDesk Notification",
					String.Format(Globals.GetMessage("ChangePassword"), username, password));

			return m_dp.ChangePassword(username, password);
		}

		/// <summary>
		/// Create a set of users specified by XML data
		/// </summary>
		public void BatchCreate(Stream xmlStream, bool sync) {
			User user = new User();
			Users userDB = new Users(Globals.CurrentIdentity);
			DataSet xmldata = new DataSet();
			xmldata.ReadXml(xmlStream);
			DataRowCollection users = xmldata.Tables["User"].Rows;

			IProviderTransaction tran = m_dp.BeginTransaction();
			foreach (DataRow row in users) {
				
				user.UserName = (string) row["UserName"];
				user.FirstName = (string) row["FirstName"];
				user.LastName = (string) row["LastName"];
				user.Email = (string) row["Email"];

				//If the user is already in the system
				if (null != userDB.GetInfo(user.UserName, tran)) {
					if (sync) 
						m_dp.UpdateUser(user, tran);
					else {
						m_dp.RollbackTransaction(tran);
						throw new DataAccessException("User: " + user.UserName +
							" is already in the FrontDesk database. Use a merge to add additional users.");
					}
				}
				else {
					string passwd = Globals.GeneratePassword(Globals.MinPasswordLength);
					m_dp.CreateUser(user, passwd, tran);
					new EmailWizard(m_ident).SendByUsername(user.UserName, "FrontDesk: User Account Created",
						String.Format(Globals.GetMessage("CreateUser"), user.UserName, passwd,
						user.Email));
				}
			}
			m_dp.CommitTransaction(tran);
		}

		public string Backup(string username, int courseID) {
			return Backup(username, courseID, -1, null);
		}

		public string Backup(string username, int courseID, IExternalSink extsink) {
			return Backup(username, courseID, -1, extsink);
		}

		public string Backup(string username, int courseID, int asstID) {
			return Backup(username, courseID, asstID, null);
		}

		public string Backup(string username, int courseID, int asstID, IExternalSink extsink) {

			string zfile, wzfile, backdesc=username+": ";
			//Create our external sink file
			if (extsink == null) {
				extsink = ArchiveToolFactory.GetInstance().CreateArchiveTool(".zip") as IExternalSink;
				zfile = username+DateTime.Now.Hour+DateTime.Now.Minute+DateTime.Now.Second+".zip";
				wzfile = Globals.BackupDirectoryName + "/" + zfile;
				zfile = Path.Combine(Globals.BackupDirectory, zfile);
				extsink.CreateSink(zfile);
			}
			else {
				wzfile = zfile = ""; // In the middle of a backup
			}
			//Backup Results

			//Back up submissions
			FileSystem fs = new FileSystem(m_ident);
			Components.Submission.SubmissionList subs;
			if (asstID < 0) {
				if (zfile != "") 
					backdesc += (new Courses(m_ident).GetInfo(courseID)).Name;
				subs = GetCourseSubmissions(username, courseID);
			}
			else {
				if (zfile != "") 
					backdesc += new Assignments(m_ident).GetInfo(asstID).Description;
				subs = GetAsstSubmissions(username, asstID);
			}

			foreach (Components.Submission sub in subs) 
				fs.ExportData(username, sub.LocationID, extsink, true);

			//If we are doing this not in a batch
			if (zfile != "") extsink.CloseSink();

			//Log backup in database
			if (zfile != "") new Backups(Globals.CurrentIdentity).Create(
								 backdesc, wzfile, courseID);
			
			return zfile;
		}

		/// <summary>
		/// Determines if username/password combo is legal
		/// </summary>
		public bool IsValid(string username, string password) {
			return m_dp.ValidateUser(username, password);
		}

	}
}
