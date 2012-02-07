using System;
using System.IO;
using System.Xml;
using System.Data;

using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Provider;
using FrontDesk.Tools;
using FrontDesk.Data.Filesys;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Course data access component
	/// </summary>
	public class Courses : DataAccessComponent {

		public Courses(AuthorizedIdent ident) : base(ident) { }

		/// <summary>
		/// Return all the courses in the course
		/// Results in direct call to Provider layer
		/// </summary>
		public Course.CourseList GetAll() {
			return m_dp.GetCourses();
		}

		/// <summary>
		/// Get all the backups for the course
		/// Direct Provider layer call
		/// </summary>
		public Backup.BackupList GetBackups(int courseID) {
			return m_dp.GetBackups(courseID);
		}

		/// <summary>
		/// Update a course
		/// </summary>
		public bool Update(Course course) {
			//TODO: Verify information from the course	
			return m_dp.UpdateCourse(course);
		}

		/// <summary>
		/// Update the role of a user in the specified course
		/// </summary>
		public bool UpdateRole(string username, int courseID, string role, IProviderTransaction tran) {
			
			//Check permission
			Authorize(courseID, Permission.COURSE, "updateuserrole", courseID, tran);
			
			return m_dp.UpdateCourseRole(username, courseID, role, tran);
		}

		/// <summary>
		/// Delete a course form the database
		/// </summary>
		public bool Delete(int courseID) {
			//TODO: Enforce deletion policy 
			//(whether or not everything related to the course is also deleted)
			return m_dp.DeleteCourse(courseID);
		}

		/// <summary>
		/// Create the course
		/// </summary>
		public bool Create(string name, string number, string instructor) {
			Course course = new Course();

			//TODO: Verify these values
			course.Name = name;
			course.Number = number;

			//Create course
			m_dp.CreateCourse(course);

			//Get all data
			course = GetInfo(course.ID);

			//Define default roles
			CourseRole role = new CourseRole();
			role.CourseID = course.ID;
			role.Name = "Student"; role.Staff = false;
			m_dp.CreateCourseRole(role); 
			role.Name = "TA"; role.Staff = true; 
			m_dp.CreateCourseRole(role);
			role.Name = "Instructor"; role.Staff = true;
			m_dp.CreateCourseRole(role);

			//Assign filesys permissions		
			CourseRole student = GetRoleInfo("Student", course.ID);
			CourseRole ta = GetRoleInfo("TA", course.ID);
			CourseRole ins = GetRoleInfo("Instructor", course.ID);
			CFilePermission.FilePermissionList full = new CFilePermission.FilePermissionList();
			full.AddRange(CFilePermission.CreateFullAccess(ta.PrincipalID));
			full.AddRange(CFilePermission.CreateFullAccess(ins.PrincipalID));
			full.Add(new CFilePermission(student.PrincipalID, FileAction.READ, true));

			//Create content area
			FileSystem fs = new FileSystem(m_ident);
			string cpath = @"c:\ccontent\" + course.ID;
			CFile cdir = fs.CreateDirectory(cpath, false, full);
			
			course.ContentID = cdir.ID;
			Update(course);
			CFile ldir = fs.CreateDirectory(cpath + @"\" + "lnotes", false, null);
			ldir.Alias = "Lecture Notes"; fs.UpdateFileInfo(ldir, false);
		
			//Put operator in course temporarily	
			m_dp.CreateCourseMember(m_ident.Name, course.ID, "Instructor", null);

			//Assign course perms
			CreatePermissions(course.ID, course.ID, Permission.COURSE);

			if (instructor != m_ident.Name) {
				//Add instructor
				AddUser(instructor, "Instructor", course.ID, null);

				//Take operator out
				RemoveUser(m_ident.Name, course.ID);
			}

			return true;
		}

		/// <summary>
		/// Create a course role
		/// </summary>
		public CourseRole CreateRole(int courseID, string name, bool isstaff) {
			
			//Check permission
			Authorize(courseID, Permission.COURSE, "createrole", courseID, null);
			
			CourseRole role = new CourseRole();
			role.CourseID = courseID;
			role.Name = name;
			role.Staff = isstaff;

			//Create role
			m_dp.CreateCourseRole(role);

			return role;
		}

		/// <summary>
		/// Add users to the course from an XML stream
		/// </summary>
		public void BatchAddUsers(Stream xmlStream, int courseID, bool sync) {
			//TODO: Error checking
			User mem = new User();
			Users usersDB = new Users(m_ident);

			DataSet xmldata = new DataSet();
			xmldata.ReadXml(xmlStream);

			DataRowCollection users = xmldata.Tables["User"].Rows;

			IProviderTransaction tran = m_dp.BeginTransaction();
			foreach (DataRow row in users) {
				mem.UserName = (string) row["UserName"];
				string role = (string) row["Role"];

				//Make sure the user is in the system
				if (null == usersDB.GetInfo(mem.UserName, tran)) {
					m_dp.RollbackTransaction(tran);
					throw new DataAccessException("User: " + mem.UserName +
						" is not in the FrontDesk database");
				}
				
				User.UserList cmems = GetMembers(courseID, tran);
				//The user already exists in the course
				if (cmems.Contains(mem)) {
					if (sync) 
						UpdateRole(mem.UserName, courseID, role, tran);
					else {
						m_dp.RollbackTransaction(tran);
						throw new DataAccessException("User: " + mem.UserName + 
							" already exists in the course. " + 
							"Do a merge if you want to change the roles");
					}
				}
				else
					m_dp.CreateCourseMember(mem.UserName, courseID, role, tran);
			}
			m_dp.CommitTransaction(tran);
		}

		public string Backup(int courseID) {

			string zfile, wzfile;
			
			Authorize(courseID, Permission.COURSE, "createback", courseID, null);

			//Create our external sink file
			IExternalSink extsink = 
				ArchiveToolFactory.GetInstance().CreateArchiveTool(".zip") as IExternalSink;
			Course course = GetInfo(courseID);

			zfile = course.Name+DateTime.Now.Hour+DateTime.Now.Minute+DateTime.Now.Second+".zip";
			wzfile = Globals.BackupDirectoryName + "/" + zfile;
			zfile = Path.Combine(Globals.BackupDirectory, zfile);
			extsink.CreateSink(zfile);

			//Backup Info
			//Backup Results

			//Back up submissions
			Users users = new Users(m_ident);
			User.UserList mems = GetMembers(courseID, null);

			foreach (User mem in mems) 
				users.Backup(mem.UserName, courseID, extsink);

			extsink.CloseSink();
			
			//Log in database
			new Backups(m_ident).Create(GetInfo(courseID).Name, wzfile, courseID);

			return zfile;
		}

		/// <summary>
		/// Add a user into a course
		/// </summary>
		public bool AddUser(string username, string role, int courseID, IProviderTransaction tran) {
			
			//Check permission
			if (m_ident.Name != username)
				Authorize(courseID, Permission.COURSE, "adduser", courseID, tran);

			User.UserList cmems = GetMembers(courseID, null);

			//Check an make sure the user isn't already in the course
			foreach (User user in cmems)
				if (user.UserName == username)
					throw new DataAccessException("User already enrolled in course");

			return m_dp.CreateCourseMember(username, courseID, role, tran);
		}

		/// <summary>
		/// Remove a user from a course
		/// </summary>
		public bool RemoveUser(string user, int courseID) {	

			//Kill all submissions
			Submissions subda = new Submissions(m_ident);
			Components.Submission.SubmissionList subs = new Users(m_ident).GetCourseSubmissions(user, courseID);

			foreach (Components.Submission sub in subs)
				subda.Delete(sub.ID);

			return m_dp.DeleteCourseMember(user, courseID);
		}

		/// <summary>
		/// Get information about the course
		/// </summary>
		public Course GetInfo(int courseID) {
			Course course = new Course();

			//TODO: Check error condition
			m_dp.GetCourseInfo(courseID, course);

			return course;
		}

		/// <summary>
		/// Get all members of a course
		/// Direct Provider layer call
		/// </summary>
		public User.UserList GetMembers(int courseID, IProviderTransaction tran) {
			return m_dp.GetCourseMembers(courseID, tran);
		}

		/// <summary>
		/// Get total points available for course
		/// </summary>
		public double GetTotalPoints(int courseID) {
			
			Assignment.AssignmentList assts = GetAssignments(courseID);
			double points=0.0;

			Assignments asstda = new Assignments(m_ident);
			foreach (Assignment asst in assts)
				points += asstda.GetRubric(asst.ID).Points;

			return points;
		}

		/// <summary>
		/// Get all staff from a course
		/// </summary>
		public User.UserList GetStaff(int courseID, IProviderTransaction tran) {
			User.UserList staff = new User.UserList();
			User.UserList all = GetMembers(courseID, tran);

			//Filter out students
			foreach (User cm in all) 
				 if (GetRole(cm.UserName, courseID, tran).Staff)
					 staff.Add(cm);

			return staff;
		}

		/// <summary>
		/// Get the assignments from this course
		/// Direct Provider layer call
		/// </summary>
		public Assignment.AssignmentList GetAssignments(int courseID) {
			return m_dp.GetCourseAssignments(courseID);
		}

		/// <summary>
		/// Get the student assignments from this course
		/// </summary>
		public Assignment.AssignmentList GetStudentAssignments(int courseID) {
			Assignment.AssignmentList assts = m_dp.GetCourseAssignments(courseID);
			Assignment.AssignmentList sassts = new Assignment.AssignmentList();
			foreach (Assignment asst in assts)
				if (asst.StudentRelease)
					sassts.Add(asst);
			return sassts;
		}

		/// <summary>
		/// List the sections in a course
		/// Direct Provider layer call
		/// </summary>
		public Section.SectionList GetSections(int courseID) {
			return m_dp.GetCourseSections(courseID);
		}

		/// <summary>
		/// Get a user's role
		/// </summary>
		public CourseRole GetRole(string username, int courseID, IProviderTransaction tran) {
			return m_dp.GetUserCourseRole(username, courseID, tran);
		}

		/// <summary>
		/// Get info of a role
		/// Direct Provider layer call
		/// </summary>
		public CourseRole GetRoleInfo(string role, int courseID) {
			return m_dp.GetCourseRoleInfo(role, courseID);
		}

		/// <summary>
		/// Get info of a role
		/// Direct Provider layer call
		/// </summary>
		public CourseRole GetRoleInfo(int principalID) {
			return m_dp.GetCourseRoleInfo(principalID);
		}

		/// <summary>
		/// Get all the role in a course
		/// Direct Provider layer call
		/// </summary>
		public CourseRole.CourseRoleList GetRoles(int courseID, IProviderTransaction tran) {
			return m_dp.GetCourseRoles(courseID, tran);
		}

		/// <summary>
		/// Get staff roles from a course
		/// </summary>
		public CourseRole.CourseRoleList GetTypedRoles(int courseID, bool staff, IProviderTransaction tran) {
			CourseRole.CourseRoleList roles = GetRoles(courseID, tran);
			CourseRole.CourseRoleList sroles = new CourseRole.CourseRoleList();
			foreach (CourseRole role in roles)
				if ((staff && role.Staff) || (!staff && !role.Staff))
					sroles.Add(role);
			return sroles;
		}

		/// <summary>
		/// Get all announcements for a course
		/// Direct Provider layer call
		/// </summary>
		public Announcement.AnnouncementList GetAnnouncements(int courseID) {
			return m_dp.GetCourseAnnouncements(courseID);
		}

		/// <summary>
		/// Get all students who match this username 
		/// Direct Provider layer call
		/// </summary>

		public User.UserList GetMembersByUsername(string username, int courseID){
			return m_dp.GetCourseMembersByUsername(username, courseID);
		}

		/// <summary>
		/// Get all students who match this lastname 
		/// Direct Provider layer call
		/// </summary>

		public User.UserList GetMembersByLastname(string lastname, int courseID){
			return m_dp.GetCourseMembersByLastname(lastname, courseID);
		}

	}
}
