//Mike Maxim
//FrontDesk Data provider interface

using System;
using System.Web;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Configuration;
using System.Reflection;
using System.Data;
using System.Collections;

using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Components.Filesys;

namespace FrontDesk.Data.Provider {
	

	internal class DataProviderFactory {

		protected static IDataProvider m_instance;
		/// <summary>
		/// Provide an instance of the configured provider
		/// </summary>
		public static void GetInstance(IProvidee provi) {

			String assemblyPath = ConfigurationSettings.AppSettings["DataProviderAssemblyPath"];
			String className = ConfigurationSettings.AppSettings["DataProviderClassName"];

			//use the cache because the reflection used later is expensive
			if (Globals.Context == null) {
				if (m_instance == null)
					provi.Acquire((m_instance = (IDataProvider) Assembly.LoadFrom(
						assemblyPath).GetType(className).GetConstructor(new Type[0]).Invoke(null)));
				else
					provi.Acquire(m_instance);
			}
			else {
				Cache cache = Globals.Context.Cache;
				if (null == cache["IDataProvider"]) {

					// assemblyPath presented in virtual form, must convert to physical path
					assemblyPath = Globals.Context.Server.MapPath(Globals.Context.Request.ApplicationPath + "/bin/" + assemblyPath);					

					// Uuse reflection to store the constructor of the class that implements IWebForumDataProvider
					try {
						IDataProvider dp = (IDataProvider) Assembly.LoadFrom(assemblyPath).GetType(
							className).GetConstructor(new Type[0]).Invoke(null);
						cache.Insert("IDataProvider", dp, new CacheDependency(assemblyPath));
					}
					catch (Exception) {
						// could not locate DLL file
						HttpContext.Current.Response.Write("<b>ERROR:</b> Could not locate file: <code>" + assemblyPath + "</code> or could not locate class <code>" + className + "</code> in file.");
						HttpContext.Current.Response.End();
					}
				}
				provi.Acquire((IDataProvider)cache["IDataProvider"]);
			}
		}
	}

	/// <summary>
	/// Marker interface for Provider transactions
	/// </summary>
	public interface IProviderTransaction { }

	/// <summary>
	/// Provides interface functionality for accessing FrontDesk data storage
	/// </summary>
	public interface IDataProvider {

		/// <summary>
		/// Being a database transaction
		/// </summary>
		IProviderTransaction BeginTransaction();

		/// <summary>
		/// Commit a running transaction
		/// </summary>
		bool CommitTransaction(IProviderTransaction tran);

		/// <summary>
		/// Rollback a running transaction
		/// </summary>
		bool RollbackTransaction(IProviderTransaction tran);

		/// <summary>
		/// Validate a user login attempt and return a User object
		/// Will also register the last login time in the Data source
		/// </summary>
		bool ValidateUser(string username, string password);

		/// <summary>
		/// Check a permission for a user and course
		/// </summary>
		bool CheckPermission(string username, int courseID, string perm, int entid, string enttype,
			IProviderTransaction tran);

		/// <summary>
		/// Check a permission from a principal ID
		/// </summary>
		bool CheckPermission(int principalID, int courseID, string perm, int entid, string enttype,
			IProviderTransaction tran);
		
		/// <summary>
		/// Creates a user doing the appropriate error checking
		/// </summary>
		bool CreateUser(User user, string password, IProviderTransaction tran);

		/// <summary>
		/// Change a user's password
		/// </summary>
		bool ChangePassword(string username, string password);

		/// <summary>
		/// Create a new group
		/// </summary>
		bool CreateGroup(Group group);

		/// <summary>
		/// Create a new section
		/// </summary>
		bool CreateSection(Section section);

		/// <summary>
		/// Create auto evaluation
		/// </summary>
		bool CreateAutoEvaluation(AutoEvaluation eval);

		/// <summary>
		/// Create a rubric for an assignment
		/// </summary>
		bool CreateRubric(int asstID);

		/// <summary>
		/// Cretae an entry in the rubric
		/// </summary>
		bool CreateRubricEntry(string name, string desc, int parentID, double points, int evalID);

		/// <summary>
		/// Create a new canned response
		/// </summary>
		bool CreateCannedResponse(int rubricID, string comment, double points, int type);

		/// <summary>
		/// Update a rubric entry
		/// </summary>
		bool UpdateRubricEntry(Rubric rub);

		/// <summary>
		/// Update a canned response
		/// </summary>
		bool UpdateCannedResponse(CannedResponse can);

		/// <summary>
		/// Delete a rubric entry
		/// </summary>
		bool DeleteRubricEntry(int rubID);

		/// <summary>
		/// Delete a canned response
		/// </summary>
		bool DeleteCannedResponse(int canID);

		/// <summary>
		/// Create a new auto job in the queue
		/// </summary>
		bool CreateAutoJob(AutoJob job);

		/// <summary>
		/// Create an auto job test
		/// </summary>
		bool CreateAutoJobTest(int jobID, int evalID, int subID, bool onsubmit);

		/// <summary>
		/// Delete an auto job
		/// </summary>
		bool DeleteAutoJob(int jobID, IProviderTransaction tran);

		/// <summary>
		/// Delete an auto job test
		/// </summary>
		bool DeleteAutoJobTest(int jobID, int evalID, int subID);

		/// <summary>
		/// Delete a submission
		/// </summary>
		bool DeleteSubmission(int subID);

		/// <summary>
		/// Create a new entry in the activity log
		/// </summary>
		bool CreateActivity(Activity act);

		/// <summary>
		/// Invite a user into the group
		/// </summary>
		bool InviteUser(string invitee, string invitor, int groupID);

		/// <summary>
		/// Delete an invitation record
		/// </summary>
		bool DeleteInvitation(Invitation invite);

		/// <summary>
		/// Remove a user from a group
		/// </summary>
		bool DeleteGroupMember(string username, Group group);

		/// <summary>
		/// Delete a user from the course. Takes groups and section memberships
		/// </summary>
		bool DeleteCourseMember(string username, int courseID);

		/// <summary>
		/// Remove a user from a group
		/// </summary>
		bool DeleteSectionMember(string username, int sectionID);

		/// <summary>
		/// Delete a section
		/// </summary>
		bool DeleteSection(int sectionID);

		/// <summary>
		/// Update the rubric point total for a submission
		/// </summary>
		bool UpdateRubricSubPoints(int rubID, int subID);

		/// <summary>
		/// Put a user into a course
		/// </summary>
		bool CreateCourseMember(string username, int courseID, string role, IProviderTransaction tran);

		/// <summary>
		/// Put a user in a group
		/// </summary>
		bool CreateGroupMember(Invitation invite);

		/// <summary>
		/// Create a session entry
		/// </summary>
		bool CreateSession(Guid guid, string username, string address);

		/// <summary>
		/// Get info about a session from GUID
		/// </summary>
		Session GetSessionInfo(Guid guid);

		/// <summary>
		/// Put a user in a section
		/// </summary>
		bool CreateSectionMember(int sectionID, string username, bool switchu);

		/// <summary>
		/// Get the groups the user is a member of
		/// </summary>
		Group.GroupList GetUserGroups(string username, int asstID);

		/// <summary>
		/// Get a list of groups the user is invited to
		/// </summary>
		Invitation.InvitationList GetUserInvites(string username, int asstID);

		/// <summary>
		/// Return a list of all the sections a user is in
		/// </summary>
		Section.SectionList GetUserSections(int courseID, string username);

		/// <summary>
		/// Get all the users in the group
		/// </summary>
		User.UserList GetGroupMembers(int groupID);

		/// <summary>
		/// List the members of a section
		/// </summary>
		User.UserList GetSectionMembers(int sectionID);

		/// <summary>
		/// Get the grading progress of a section
		/// </summary>
		Section.SectionProgress GetSectionProgress(int sectionID, int asstID) ;

		/// <summary>
		/// Get all the members of a course
		/// </summary>
		User.UserList GetCourseMembers(int courseID, IProviderTransaction tran);

		/// <summary>
		/// Get all the roles for the course
		/// </summary>
		CourseRole.CourseRoleList GetCourseRoles(int courseID, IProviderTransaction tran);

		/// <summary>
		/// Get info about a course role
		/// </summary>
		CourseRole GetCourseRoleInfo(string role, int courseID);

		/// <summary>
		/// Get info about a course role from principal ID
		/// </summary>
		CourseRole GetCourseRoleInfo(int principalID);

		/// <summary>
		/// Get types for a permission
		/// </summary>
		Permission.PermissionList GetTypePermissions(string etype);

		/// <summary>
		/// Get a user's role
		/// </summary>
		CourseRole GetUserCourseRole(string username, int courseID, IProviderTransaction tran);

		/// <summary>
		/// List courses the user is enrolled in
		/// </summary>
		Course.CourseList GetUserCourses(string username);

		/// <summary>
		/// Get a course object
		/// </summary>
		bool GetCourseInfo(int courseid, Course course);

		/// <summary>
		/// Get a user object
		/// </summary>
		bool GetUserInfo(string username, User user, IProviderTransaction tran);

		/// <summary>
		/// Get a user's password
		/// </summary>
		string GetUserPassword(string username);

		/// <summary>
		/// Get information about an evaluation
		/// </summary>
		Evaluation GetEvalInfo(int evalID);

		/// <summary>
		/// Get information about an evaluation from zone ID
		/// </summary>
		AutoEvaluation GetAutoEvalInfoByZone(int zoneID);

		/// <summary>
		/// Get information about a rubric entry
		/// </summary>
		Rubric GetRubricInfo(int rubID);

		/// <summary>
		/// Get rubric info from an evaluation ID
		/// </summary>
		Rubric GetRubricInfoFromEval(int evalID);

		/// <summary>
		/// Get the points for a rubric entry
		/// </summary>
		bool GetRubricPoints(int rubID, int subID, out double points);

		/// <summary>
		/// Get results for this rubric
		/// </summary>
		Result.ResultList GetRubricResults(int rubID, int subID, string type);

		/// <summary>
		/// Get a list of rubric child entries
		/// </summary>
		Rubric.RubricList GetRubricChildren(int rubID);

		/// <summary>
		/// Get a list of canned responses for a rubric entry
		/// </summary>
		CannedResponse.CannedResponseList GetRubricCannedResponses(int rubID);

		/// <summary>
		/// Get result information
		/// </summary>
		Result GetResultInfo(int resID);

		/// <summary>
		/// Get info about a canned response
		/// </summary>
		CannedResponse GetCannedInfo(int canID);

		/// <summary>
		/// Get information about principal
		/// </summary>
		bool GetPrincipalInfo(int pid, Principal principal);

		/// <summary>
		/// Get courses in DataSet form
		/// </summary>
		Course.CourseList GetCourses();

		/// <summary>
		/// Get all instructors
		/// </summary>
		User.UserList GetAllUsers();

		/// <summary>
		/// Get all active jobs known
		/// </summary>
		AutoJob.AutoJobList GetAllJobs();

		/// <summary>
		/// Get all auto job tests
		/// </summary>
		AutoJobTest.AutoJobTestList GetAutoJobTests(int jobID);

		/// <summary>
		/// Get all aut job tests
		/// </summary>
		AutoJobTest.AutoJobTestList GetAllAutoJobTests();

		/// <summary>
		/// Get all auto tests for a submission
		/// </summary>
		AutoJobTest.AutoJobTestList GetSubAutoJobTests(int subID);

		/// <summary>
		/// Get all user asst jobs
		/// </summary>
		AutoJob.AutoJobList GetUserAsstJobs(string username, int asstID);

		/// <summary>
		/// Get all asst auto jobs
		/// </summary>
		AutoJob.AutoJobList GetAllAsstJobs(int asstID, IProviderTransaction tran);

		/// <summary>
		/// Claim a job (set job in progress)
		/// </summary>
		AutoJobTest ClaimJob(string ipaddress, string desc);

		/// <summary>
		/// Get all backups created for system
		/// </summary>
		Backup.BackupList GetBackups(int courseID);

		/// <summary>
		/// Get group information
		/// </summary>
		Group GetGroupInfo(int groupid);

		/// <summary>
		/// Create a new course
		/// </summary>
		bool CreateCourse(Course course);

		/// <summary>
		/// Create a new course role
		/// </summary>
		bool CreateCourseRole(CourseRole role);

		/// <summary>
		/// Assign a role a permission
		/// </summary>
		bool AssignPermission(int principalID, string perm, string etype, int entityID);

		/// <summary>
		/// Deny a permission
		/// </summary>
		bool DenyPermission(int principalID, string perm, string etype, int entityID);

		/// <summary>
		/// Create a auto result
		/// </summary>
		bool CreateAutoResult(AutoResult result);

		/// <summary>
		/// Create a subj result
		/// </summary>
		bool CreateSubjResult(SubjResult result);

		/// <summary>
		/// Update a subjective result
		/// </summary>
		bool UpdateSubjResult(SubjResult res);

		/// <summary>
		/// Create a line affected reference
		/// </summary>
		bool CreateSubjLineAffect(int resID, int line);

		/// <summary>
		/// Synchronize the data with the course
		/// </summary>
		bool UpdateCourse(Course course);

		/// <summary>
		/// Update a course members role in the course
		/// </summary>
		bool UpdateCourseRole(string username, int courseID, string role, IProviderTransaction tran);

		/// <summary>
		/// Update section
		/// </summary>
		bool UpdateSection(Section sec);

		/// <summary>
		/// Update features ofd submission
		/// </summary>
		bool UpdateSubmission(Submission sub);

		/// <summary>
		/// Update user information
		/// </summary>
		bool UpdateUser(User user, IProviderTransaction tran);

		/// <summary>
		/// Synchronize the data with the course
		/// </summary>
		bool UpdateAssignment(Assignment asst);

		/// <summary>
		/// Update a course setting
		/// </summary>
		bool UpdateCourseSetting(Setting mySetting);

		/// <summary>
		/// Update an assignment setting
		/// </summary>
		bool UpdateAssignmentSetting(Setting mySetting);

		/// <summary>
		/// Create a new assignment as the creator
		/// </summary>
		bool CreateAssignment(Assignment asst);

		/// <summary>
		/// Create a backup
		/// </summary>
		bool CreateBackup(Backup back);

		/// <summary>
		/// Delete the assignement
		/// </summary>
		bool DeleteAssignment(int asstID);

		/// <summary>
		/// Delet an auto evaluation
		/// </summary>
		bool DeleteEval(int evalID);

		/// <summary>
		/// Delete a result
		/// </summary>
		bool DeleteResult(int resID);

		/// <summary>
		/// Update an announcement
		/// </summary>
		bool UpdateAnnouncement(Announcement announce);

		/// <summary>
		/// Update a auto evaluation
		/// </summary>
		bool UpdateAutoEvaluation(AutoEvaluation eval);

		/// <summary>
		/// Create a new announcement
		/// </summary>
		bool CreateAnnouncement(Announcement annou);

		/// <summary>
		/// Delete the announcement
		/// </summary>
		bool DeleteAnnouncement(int announcementID);

		/// <summary>
		/// Delete the course
		/// </summary>
		bool DeleteCourse(int courseID);

		/// <summary>
		/// Delete a session
		/// </summary>
		bool DeleteSession(Guid guid);

		/// <summary>
		/// Take a group
		/// </summary>
		bool DeleteGroup(int groupID);

		/// <summary>
		/// Gets all the assignments for a given course in admin mode
		/// </summary>
		Assignment.AssignmentList GetCourseAssignments(int courseID);

		/// <summary>
		/// Get all the announcements for a course
		/// </summary>
		Announcement.AnnouncementList GetCourseAnnouncements(int courseID); 

		/// <summary>
		/// Get announcement info
		/// </summary>
		Announcement GetAnnouncementInfo(int annID);
		
		/// <summary>
		/// List all the sections in a course
		/// </summary>
		Section.SectionList GetCourseSections(int courseID);

		/// <summary>
		/// Get section info
		/// </summary>
		Section GetSectionInfo(int sectionID);

		/// <summary>
		/// Gets all the settings of a specified category for this course
		/// </summary>
		Setting.SettingList GetCategoricalCourseSettings(int courseID, int categoryID);
		
		/// <summary>
		/// Gets all the settings of a specified type for a given assignment
		/// </summary>
		Setting.SettingList GetAssignmentCategoricalSettings( int categoryID, int asstID);


		/// <summary>
		/// Get assignment information
		/// </summary>
		bool GetAssignmentInfo(int asstID, Assignment asst);

		/// <summary>
		/// Get all auto evaluations from assignments
		/// </summary>
		Evaluation.EvaluationList GetAssignmentEvals(int asstID, string type);

		/// <summary>
		/// Get the rubric for the given assignment
		/// </summary>
		Rubric GetAssignmentRubric(int asstID);

		/// <summary>
		/// Get all submissions for an assignment
		/// </summary>
		Components.Submission.SubmissionList GetAssignmentSubmissions(int asstID);

		/// <summary>
		/// Return a list of evaluations the given eval depends on
		/// </summary>
		Evaluation.EvaluationList GetEvalDependencies(int evalID);

		/// <summary>
		/// Create evaluation dependency
		/// </summary>
		bool CreateEvalDependency(int evalID, int depID);

		/// <summary>
		/// Delete evaluation dependency
		/// </summary>
		bool DeleteEvalDependency(int evalID, int depID);

		/// <summary>
		/// Get all the submission groups for speced asst
		/// </summary>
		Group.GroupList GetAssignmentGroups(int asstID);

		/// <summary>
		/// Get user submissions
		/// </summary>
		Components.Submission.SubmissionList GetUserSubmissions(string username, int asstid, bool course);

		/// <summary>
		/// Get submission info
		/// </summary>
		Components.Submission GetSubmissionInfo(int subID);

		/// <summary>
		/// Return all submissions in all courses
		/// </summary>
		Components.Submission.SubmissionList GetAllSubmissions();

		/// <summary>
		/// Grab a submission from its direcftoryID
		/// </summary>
		Components.Submission GetSubmissionInfoFromDirID(int dirID);

		/// <summary>
		/// Get activity for a specific object and type
		/// </summary>
		Activity.ActivityList GetObjectActivity(int objID, int type);

		/// <summary>
		/// Get all results for a submission
		/// </summary>
		Result.ResultList GetSubmissionResults(int subID, string type);

		/// <summary>
		/// Get lines affected by subjective result
		/// </summary>
		bool GetSubjResultLines(SubjResult res);

		/// <summary>
		/// Get a listing of subjective results on a file
		/// </summary>
		Result.ResultList GetFileSubjResults(int fileID);

		/// <summary>
		/// Get principal submissions
		/// </summary>
		Components.Submission.SubmissionList GetPrincipalSubmissions(int pid, int asstid);

		/// <summary>
		/// Create a new submission entry
		/// </summary>
		bool CreateSubmission(Components.Submission sub); 

		/// Create a file at specified path
		/// </summary>
		void CreateFile(CFile file);

		/// <summary>
		/// Get FS root
		/// </summary>
		string GetRootPath();

		/// <summary>
		/// Get directory contents
		/// </summary>
		CFile.FileList ListDirectory(CFile directory);

		/// <summary>
		/// Bring file object data and data store up to date
		/// </summary>
		void SyncFile(CFile file);

		/// <summary>
		/// Delete a file
		/// </summary>
		void DeleteFile(CFile file, int principalID);

		/// <summary>
		/// Copy a file
		/// </summary>
		void CopyFile(CFile dest, CFile src, int principalID);

		/// <summary>
		/// Move a file
		/// </summary>
		void MoveFile(CFile dest, CFile src, int principalID);

		/// <summary>
		/// Lock a file
		/// </summary>
		CFileLock ObtainLock(CFile file, int principalID);

		/// <summary>
		/// Release a lock
		/// </summary>
		void ReleaseLock(CFileLock filelock);

		/// <summary>
		/// Get lock information
		/// </summary>
		CFileLock GetLockByLockID(int lockid);

		/// <summary>
		/// Get lock information
		/// </summary>
		CFileLock GetLockByFileID(int fileid);

		/// <summary>
		/// Get file object corresponding to absolute path
		/// </summary>
		CFile GetFile(string path);

		/// <summary>
		/// Get file object corresponding to ID
		/// </summary>
		CFile GetFile(int id);

		/// <summary>
		/// Get all files
		/// </summary>
		CFile.FileList GetAllFiles();

		/// <summary>
		/// Get the parent of a file
		/// </summary>
		CFile GetFileParent(CFile file);

		/// <summary>
		/// Authorize an action on a file
		/// </summary>
		bool AuthorizeFile(int principalID, CFile file, FileAction action);

		/// <summary>
		/// Set file permissions for the speced file
		/// </summary>
		void UpdateFilePermission(CFile file, CFilePermission perm);

		/// <summary>
		/// Searches for course members using wildcard search
		/// </summary>
		User.UserList GetCourseMembersByUsername(string username, int courseID);
		User.UserList GetCourseMembersByLastname(string lastname, int courseID);

		
		/// <summary>
		/// Get a setting from an ID
		/// </summary>
		Setting GetCourseSetting(int courseID, string settingName);
		Setting.Category.CategoryList GetSettingCategories();


	}
}
