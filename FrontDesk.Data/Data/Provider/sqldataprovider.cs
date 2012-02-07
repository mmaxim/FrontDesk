//Mike Maxim
//SQL Server data provider

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using FrontDesk.Data.Filesys;
using FrontDesk.Data.Filesys.Provider;
using FrontDesk.Components.Filesys;
using FrontDesk.Components.Evaluation;
using FrontDesk.Components;
using FrontDesk.Common;

namespace FrontDesk.Data.Provider {
	
	/// <summary>
	/// SQL Server implementation of the DataProvider interface
	/// </summary>
	internal class SqlDataProvider : IDataProvider {
		
		public SqlDataProvider() {
			
		}

		public class SqlProviderTransaction : IProviderTransaction {
			
			protected SqlTransaction m_trans;
			protected SqlConnection m_conn;

			public SqlTransaction Transaction {
				get { return m_trans; }
				set { m_trans = value; }
			}

			public SqlConnection Connection {
				get { return m_conn; }
				set { m_conn = value; }
			}
		}

		public IProviderTransaction BeginTransaction() {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);

			myConnection.Open();
			SqlTransaction stran = myConnection.BeginTransaction();

			SqlProviderTransaction tran = new SqlProviderTransaction();
			tran.Transaction = stran;
			tran.Connection = myConnection;
			return tran;
		}

		public bool CommitTransaction(IProviderTransaction tran) {
			SqlProviderTransaction stran = tran as SqlProviderTransaction;
			stran.Transaction.Commit();
			stran.Connection.Close();
			return true;
		}

		public bool RollbackTransaction(IProviderTransaction tran) {
			SqlProviderTransaction stran = tran as SqlProviderTransaction;
			stran.Transaction.Rollback();
			stran.Connection.Close();
			return true;
		}

		/// <summary>
		/// Use SQL Server stored procedure to validate user
		/// Also set the last login
		/// </summary>
		public bool ValidateUser(string username, string password) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_CheckUserCredentials", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterUsername = new SqlParameter("@Username", SqlDbType.NVarChar, 50);
			parameterUsername.Value = username;
			myCommand.Parameters.Add(parameterUsername);

			SqlParameter parameterPassword = new SqlParameter("@Password", SqlDbType.NVarChar, 20);
			parameterPassword.Value = password;
			myCommand.Parameters.Add(parameterPassword);

			// Execute the command
			myConnection.Open();
			bool retVal = Convert.ToBoolean(myCommand.ExecuteScalar());
			myConnection.Close();

			return retVal;
		}

		/// <summary>
		/// Use SQL Server stored proc to list the courses a user is a member in
		/// </summary>
		public Course.CourseList GetUserCourses(string username) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetUserCourses", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			
			Course.CourseList courses = new Course.CourseList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Course course = new Course();
				FormEntityFromReader(reader, course);
				courses.Add(course);
			}
			myConnection.Close();

			return courses;
		}

		public Group.GroupList GetUserGroups(string username, int asstID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetUserGroups", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = asstID;
			myCommand.Parameters.Add(parameter);
			
			Group.GroupList groups = new Group.GroupList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Group group = new Group();
				FormEntityFromReader(reader, group);
				groups.Add(group);
			}
			myConnection.Close();

			return groups;
		}

		public Invitation.InvitationList GetUserInvites(string username, int asstID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetUserInvites", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = asstID;
			myCommand.Parameters.Add(parameter);
			
			Invitation.InvitationList invites = new Invitation.InvitationList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Invitation inv = new Invitation();
				FormEntityFromReader(reader, inv);
				invites.Add(inv);
			}
			myConnection.Close();

			return invites;
		}

		public User.UserList GetGroupMembers(int groupID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetGroupMembers", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@GroupID", SqlDbType.Int, 4);
			parameter.Value = groupID;
			myCommand.Parameters.Add(parameter);
			
			User.UserList users = new User.UserList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				User user = new User();
				FormEntityFromReader(reader, user);
				users.Add(user);
			}
			myConnection.Close();

			return users;
		}

		public Section GetSectionInfo(int sectionID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM SectionsView WHERE ID=@SectionID", myConnection);
			
			SqlParameter parameter = new SqlParameter("@SectionID", SqlDbType.Int, 4);
			parameter.Value = sectionID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			Section section = new Section();
			FormEntityFromReader(reader, section);
			myConnection.Close();

			return section;
		}

		public Section.SectionProgress GetSectionProgress(int sectionID, int asstID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetSectionProgress", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@SectionID", SqlDbType.Int, 4);
			parameter.Value = sectionID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = asstID;
			myCommand.Parameters.Add(parameter);
			
			Section.SectionProgress progress = new Section.SectionProgress();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			progress.TotalGraded = (int) reader["totalGraded"];
			progress.TotalStudents = (int) reader["totalStudents"];
			progress.TotalSubmissions = (int) reader["totalSubmit"];
			myConnection.Close();

			return progress;
		}

		public User.UserList GetSectionMembers(int sectionID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetSectionMembers", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@SectionID", SqlDbType.Int, 4);
			parameter.Value = sectionID;
			myCommand.Parameters.Add(parameter);
			
			User.UserList users = new User.UserList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				User user = new User();
				FormEntityFromReader(reader, user);
				users.Add(user);
			}
			myConnection.Close();

			return users;
		}

		public void ReleaseLock(CFileLock flock) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_ReleaseLock", myConnection);

			
			myCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter parameter = new SqlParameter("@LockID", SqlDbType.Int, 4);
			parameter.Value = flock.ID;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}

		public CFileLock GetLockByLockID(int lockid) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetLockByID", myConnection);

			myConnection.Open();
			myCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter parameter = new SqlParameter("@ID", SqlDbType.Int, 4);
			parameter.Value = lockid;
			myCommand.Parameters.Add(parameter);

			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) throw new FileOperationException("Lock does not exist");
			
			CFileLock flock = FormFileLockFromSqlReader(reader);
			myConnection.Close();

			return flock;
		}

		public CFileLock GetLockByFileID(int fileid) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetLockByFile", myConnection);

			
			myCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = fileid;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			
			CFileLock flock = FormFileLockFromSqlReader(reader);
			
			reader.Close();
			myConnection.Close();

			return flock;
		}

		/// <summary>
		/// Get directory contents
		/// </summary>
		public CFile.FileList ListDirectory(CFile directory) {
			return ListDirFlat(Path.Combine(directory.Path, directory.Name));
		}

		protected CFile.FileList ListDirFlat(string path) {

			CFile.FileList files = new CFile.FileList();
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM Files WHERE filePath=@Path ORDER BY fileType DESC, fileName", 
				myConnection);

			SqlParameter parameter = new SqlParameter("@Path", SqlDbType.NVarChar, CFile.MaxPath);
			parameter.Value = path;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				CFile file = new CFile();
				FormFileFromSqlReader(reader, file);
				if (file.Name != GetRootPath())
					files.Add(file);
			}
			reader.Close();
			myConnection.Close();

			return files;
		}

		public string GetRootPath() {
			return @"c:\";
		}

		private void FormFileFromSqlReader(SqlDataReader reader, CFile file) {
			file.Name = (string) reader["fileName"];
			file.Path = (string) reader["filePath"];
			file.Type = (CFile.FileType) reader["fileType"];
			file.ID = (int) reader["ID"];
			file.Alias = (string) reader["fileAlias"];
			file.FileCreated = (DateTime) reader["fileCreated"];
			file.FileModified = (DateTime) reader["fileModified"];
			file.SpecType = (CFile.SpecialType) reader["fileSpecialType"];
			file.ReadOnly = (bool) reader["readonly"];
			file.Size = (int) reader["fileSize"];
			if (reader["description"] is DBNull)
				file.Description = "";
			else
				file.Description = (string) reader["description"];
		}

		private CFileLock FormFileLockFromSqlReader(SqlDataReader reader) {
			CFileLock flock = new CFileLock();

			flock.Creation = (DateTime) reader["creation"];
			flock.FileID = (int) reader["fileID"];
			flock.ID = (int) reader["ID"];
			flock.UserName = (string) reader["userName"];
			flock.LockParent = (int) reader["parentID"];

			return flock;
		}

		public void SyncFile(CFile file) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_SyncFile", myConnection);

			myConnection.Open();
			myCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = file.ID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Name", SqlDbType.NVarChar, CFile.MaxName);
			parameter.Value = file.Name;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Path", SqlDbType.NVarChar, CFile.MaxPath);
			parameter.Value = file.Path;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Type", SqlDbType.Int, 4);
			parameter.Value = file.Type;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Modified", SqlDbType.DateTime, 8);
			parameter.Value = file.FileModified;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@SpecialType", SqlDbType.Int, 4);
			parameter.Value = file.SpecType;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Alias", SqlDbType.NVarChar, CFile.MaxName);
			parameter.Value = file.Alias;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Readonly", SqlDbType.Bit, 1);
			parameter.Value = file.ReadOnly;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Size", SqlDbType.Int, 4);
			parameter.Value = file.Size;
			myCommand.Parameters.Add(parameter);
			if (file.Description.Length > 0) {
				parameter = new SqlParameter("@Description", SqlDbType.Text, file.Description.Length);
				parameter.Value = file.Description;
				myCommand.Parameters.Add(parameter);
			} else {
				parameter = new SqlParameter("@Description", SqlDbType.Text, 16);
				parameter.Value = DBNull.Value;
				myCommand.Parameters.Add(parameter);
			}

			myCommand.ExecuteNonQuery();

			myConnection.Close();
		}

		public CFile GetFileParent(CFile file) {
			return GetFile(file.Path);
		}

		public CFileLock ObtainLock(CFile file, int principalID) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand; 
			
			if (file.IsDirectory())
				throw new FileOperationException("Users not allowed to lock directories");
			else
				myCommand = new SqlCommand("ipbased.fd_ObtainLock", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = file.ID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@PrincipalID", SqlDbType.Int, 4);
			parameter.Value = principalID;
			myCommand.Parameters.Add(parameter);
			SqlParameter flock = new SqlParameter("@LockID", SqlDbType.Int, 4);
			flock.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(flock);

			myConnection.Open();
			myCommand.ExecuteNonQuery();		
			myConnection.Close();

			if ((int)flock.Value >= 0) 
				return GetLockByLockID((int)flock.Value);
			else
				throw new FileOperationException("Unable to obtain: File already locked");
		}

		public void CopyFile(CFile dest, CFile src, int principalID) {
			CopyMoveFile(dest, src, false, principalID);	
		}

		public void MoveFile(CFile dest, CFile src, int principalID) {
			CopyMoveFile(dest, src, true, principalID);
		}

		protected void CopyMoveFile(CFile dest, CFile src, bool move, int principalID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand; 

			if (!src.IsDirectory()) {
				myCommand = new SqlCommand("ipbased.fd_CopyFile", myConnection);
				myCommand.CommandType = CommandType.StoredProcedure;
				SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
				parameter.Value = src.ID;
				myCommand.Parameters.Add(parameter);
				parameter = new SqlParameter("@PrincipalID", SqlDbType.Int, 4);
				parameter.Value = principalID;
				myCommand.Parameters.Add(parameter);
				parameter = new SqlParameter("@DestPath", SqlDbType.NVarChar, CFile.MaxPath);
				parameter.Value = dest.FullPath;
				myCommand.Parameters.Add(parameter);
				parameter = new SqlParameter("@Move", SqlDbType.Bit, 1);
				parameter.Value = move;
				myCommand.Parameters.Add(parameter);
				parameter = new SqlParameter("@Lock", SqlDbType.Bit, 1);
				parameter.Value = 1;
				myCommand.Parameters.Add(parameter);
				SqlParameter retval = new SqlParameter("@retval", SqlDbType.Int, 4);
				retval.Direction = ParameterDirection.ReturnValue;
				myCommand.Parameters.Add(retval);

				myConnection.Open();
				myCommand.ExecuteNonQuery();
				int rval = (int) retval.Value;
				if (rval < 0) {
					myConnection.Close();
					throw new FileOperationException("Unable to copy/move because the file is locked");
				}
			}
			else {
				myCommand = new SqlCommand("ipbased.fd_CopyDirectory", myConnection);
				myCommand.CommandType = CommandType.StoredProcedure;
				SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
				parameter.Value = src.ID;
				myCommand.Parameters.Add(parameter);
				parameter = new SqlParameter("@DestID", SqlDbType.Int, 4);
				parameter.Value = dest.ID;
				myCommand.Parameters.Add(parameter);
				parameter = new SqlParameter("@PrincipalID", SqlDbType.Int, 4);
				parameter.Value = principalID;
				myCommand.Parameters.Add(parameter);
				parameter = new SqlParameter("@Move", SqlDbType.Bit, 1);
				parameter.Value = move;
				myCommand.Parameters.Add(parameter);
				
				myConnection.Open();
				SqlDataReader reader = myCommand.ExecuteReader();
				if (!reader.Read()) throw new Exception("Unbelievable exception!");
				
				int condition = (int) reader["condition"];
				int culprit = (int) reader["culprit"];
				string errormsg;

				if (condition < 0) {
					errormsg = String.Format("Unable to copy/move because file: {0} is locked",
						GetFile(culprit).FullPath);
					myConnection.Close();
					throw new FileOperationException(errormsg);
				}
			}		
			myConnection.Close();
		}

		public void DeleteFile(CFile file, int principalID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand; 

			myCommand = new SqlCommand("ipbased.fd_DeleteFile", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = file.ID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@PrincipalID", SqlDbType.Int, 4);
			parameter.Value = principalID;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) throw new Exception("Unbelievable exception!");
				
			int condition = (int) reader["condition"];
			int culprit = (int) reader["culprit"];
			string errormsg;

			if (condition < 0) {
				errormsg = String.Format("Unable to delete because file: {0} is locked",
					GetFile(culprit).Alias);
				myConnection.Close();
				throw new FileOperationException(errormsg);
			}

			myConnection.Close();
		}

		/// <summary>
		/// Create a file record
		/// </summary>
		public void CreateFile(CFile file) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_CreateFile", myConnection);

			myCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter parameter = new SqlParameter("@Path", SqlDbType.NVarChar, CFile.MaxPath);
			parameter.Value = file.Path;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Name", SqlDbType.NVarChar, CFile.MaxPath);
			parameter.Value = file.Name;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Type", SqlDbType.Int, 4);
			parameter.Value = file.Type;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Readonly", SqlDbType.Bit, 1);
			parameter.Value = file.ReadOnly;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) throw new Exception("File system integrity lost!");
			
			FormFileFromSqlReader(reader, file);
			myConnection.Close();	
		}

		/// <summary>
		/// Gets all the assignments for a given course in admin mode
		/// </summary>
		public Assignment.AssignmentList GetCourseAssignments(int courseID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("SELECT * FROM CourseAsstView WHERE courseID=@CourseID", myConnection);
			
			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			
			Assignment.AssignmentList assts = new Assignment.AssignmentList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Assignment asst = new Assignment();
				FormEntityFromReader(reader, asst);
				assts.Add(asst);
			}
			myConnection.Close();
			
			return assts;
		}

		/// <summary>
		/// Gets all the sections in a course
		/// </summary>
		public Section.SectionList GetCourseSections(int courseID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("SELECT * FROM SectionsView WHERE courseID=@CourseID", 
				myConnection);
			
			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			
			Section.SectionList secs = new Section.SectionList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Section sec = new Section();
				FormEntityFromReader(reader, sec);
				secs.Add(sec);
			}
			myConnection.Close();
			
			return secs;
		}

		/// <summary>
		/// Get all sections a user belongs to
		/// </summary>
		public Section.SectionList GetUserSections(int courseID, string username) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetUserSections", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Username", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			
			Section.SectionList secs = new Section.SectionList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Section sec = new Section();
				FormEntityFromReader(reader, sec);
				secs.Add(sec);
			}
			myConnection.Close();	
			return secs;
		}

		/// <summary>
		/// Gets all the settings of a given category for this course
		/// </summary>
		public Setting.SettingList GetCategoricalCourseSettings(int courseID, int categoryID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_GetCourseSettings", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			
			parameter = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
			parameter.Value = categoryID;
			myCommand.Parameters.Add(parameter);

			Setting.SettingList mems = new Setting.SettingList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Setting mem = new Setting();
				FormEntityFromReader(reader, mem);
				mems.Add(mem);
			}
			myConnection.Close();
			
			return mems;
		}

		/// <summary>
		/// Gets all the settings for a particular assignment and category type in this course
		/// </summary>
		public Setting.SettingList GetAssignmentCategoricalSettings(int categoryID, int asstID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_GetAssignmentSettings", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			
			SqlParameter parameter = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
			parameter.Value = categoryID;
			myCommand.Parameters.Add(parameter);

			parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = asstID;
			myCommand.Parameters.Add(parameter);

			Setting.SettingList mems = new Setting.SettingList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Setting mem = new Setting();
				FormEntityFromReader(reader, mem);
				mems.Add(mem);
			}
			myConnection.Close();
			
			return mems;
		}


		/// <summary>
		/// Gets all the user in the course
		/// </summary>
		public User.UserList GetCourseMembers(int courseID, IProviderTransaction tran) {
			SqlConnection myConnection;
			if (tran == null)
				myConnection = new SqlConnection(Globals.DataConnectionString);
			else
				myConnection = (tran as SqlProviderTransaction).Connection;
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_GetCourseMembers", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			if (tran != null) myCommand.Transaction = (tran as SqlProviderTransaction).Transaction;
			
			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			
			User.UserList users = new User.UserList();
			if (tran == null) myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				User mem = new User();
				FormEntityFromReader(reader, mem);
				users.Add(mem);
			}
			reader.Close();
			if (tran == null) myConnection.Close();
			
			return users;
		}

		/// <summary>
		/// Gets the course members that match the given username
		/// </summary>
		public User.UserList GetCourseMembersByUsername(string username, int courseID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_GetCourseMembersLikeUserName", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			
			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			
			User.UserList users = new User.UserList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				User mem = new User();
				FormEntityFromReader(reader, mem);
				users.Add(mem);
			}
			reader.Close();
			myConnection.Close();		
			return users;
		}

		/// <summary>
		/// Gets the course members that match the given username
		/// </summary>
		public User.UserList GetCourseMembersByLastname(string lastname, int courseID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_GetCourseMembersLikeLastName", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			
			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@LastName", SqlDbType.NVarChar, 50);
			parameter.Value = lastname;
			myCommand.Parameters.Add(parameter);
			
			User.UserList users = new User.UserList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				User mem = new User();
				FormEntityFromReader(reader, mem);
				users.Add(mem);
			}
			reader.Close();
			myConnection.Close();		
			return users;
		}

		/// <summary>
		/// Gets principal permission
		/// </summary>
		public bool CheckPermission(int principalID, int courseID, string perm, int entid, string enttype,
			IProviderTransaction tran) {
			SqlConnection myConnection;
			if (tran == null)
				myConnection = new SqlConnection(Globals.DataConnectionString);
			else
				myConnection = (tran as SqlProviderTransaction).Connection;
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_CheckPrinPermission", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			if (tran != null) myCommand.Transaction = (tran as SqlProviderTransaction).Transaction;
			
			SqlParameter parameter = new SqlParameter("@PrincipalID", SqlDbType.Int, 4);
			parameter.Value = principalID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Perm", SqlDbType.NVarChar, 50);
			parameter.Value = perm;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@EntityID", SqlDbType.Int, 4);
			parameter.Value = entid;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@EntityType", SqlDbType.NVarChar, 50);
			parameter.Value = enttype;
			myCommand.Parameters.Add(parameter);
			
			if (tran == null) myConnection.Open();
			int count = Convert.ToInt32(myCommand.ExecuteScalar());
			if (tran == null) myConnection.Close();
			
			return (count > 0);
		}

		/// <summary>
		/// Gets all the user in the course
		/// </summary>
		public bool CheckPermission(string username, int courseID, string perm, int entid, string enttype,
									IProviderTransaction tran) {
			SqlConnection myConnection;
			if (tran == null)
				myConnection = new SqlConnection(Globals.DataConnectionString);
			else
				myConnection = (tran as SqlProviderTransaction).Connection;
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_CheckPermission", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			if (tran != null) myCommand.Transaction = (tran as SqlProviderTransaction).Transaction;
			
			SqlParameter parameter = new SqlParameter("@Username", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Perm", SqlDbType.NVarChar, 50);
			parameter.Value = perm;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@EntityID", SqlDbType.Int, 4);
			parameter.Value = entid;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@EntityType", SqlDbType.NVarChar, 50);
			parameter.Value = enttype;
			myCommand.Parameters.Add(parameter);
			
			if (tran == null) myConnection.Open();
			int count = Convert.ToInt32(myCommand.ExecuteScalar());
			if (tran == null) myConnection.Close();
			
			return (count > 0);
		}

		/// <summary>
		/// Gets all the user in the course
		/// </summary>
		public CourseRole GetUserCourseRole(string username, int courseID, IProviderTransaction tran) {
			SqlConnection myConnection;
			if (tran == null)
				myConnection = new SqlConnection(Globals.DataConnectionString);
			else
				myConnection = (tran as SqlProviderTransaction).Connection;
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_GetUserCourseRole", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			if (tran != null) myCommand.Transaction = (tran as SqlProviderTransaction).Transaction;
			
			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Username", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			
			CourseRole role = new CourseRole();
			if (tran == null) myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { if (tran == null) myConnection.Close(); reader.Close(); return null; }
			FormEntityFromReader(reader, role);
			reader.Close();
			if (tran == null) myConnection.Close();
			
			return role;
		}

		/// <summary>
		/// Get permissions for a given typr
		/// </summary>
		public Permission.PermissionList GetTypePermissions(string etype) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM PermissionView WHERE type=@Type", myConnection);
			
			SqlParameter parameter = new SqlParameter("@Type", SqlDbType.NVarChar, 50);
			parameter.Value = etype;
			myCommand.Parameters.Add(parameter);
			
			Permission.PermissionList perms = new Permission.PermissionList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Permission perm = new Permission();
				FormEntityFromReader(reader, perm);
				perms.Add(perm);
			}
			reader.Close();
			myConnection.Close();
			
			return perms;
		}

		/// <summary>
		/// Gets all the user in the course
		/// </summary>
		public CourseRole.CourseRoleList GetCourseRoles(int courseID, IProviderTransaction tran) {
			SqlConnection myConnection;
			if (tran == null)
				myConnection = new SqlConnection(Globals.DataConnectionString);
			else
				myConnection = (tran as SqlProviderTransaction).Connection;
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM CourseRoles WHERE courseID=@CourseID", myConnection);
			if (tran != null) myCommand.Transaction = (tran as SqlProviderTransaction).Transaction;
			
			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			
			CourseRole.CourseRoleList mems = new CourseRole.CourseRoleList();
			if (tran == null) myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				CourseRole mem = new CourseRole();
				FormEntityFromReader(reader, mem);
				mems.Add(mem);
			}
			reader.Close();
			if (tran == null) myConnection.Close();
			
			return mems;
		}

		/// <summary>
		/// Get course annoucements
		/// </summary>
		public Announcement.AnnouncementList GetCourseAnnouncements(int courseID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM AnnouncementView WHERE courseID=@CourseID", myConnection);
			
			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			
			Announcement.AnnouncementList mems = new Announcement.AnnouncementList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Announcement mem = new Announcement();
				FormEntityFromReader(reader, mem);
				mems.Add(mem);
			}
			myConnection.Close();
			
			return mems;
		}

		/// <summary>
		/// Get course annoucements
		/// </summary>
		public bool ChangePassword(string username, string password) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("UPDATE Users SET password=@Password WHERE username=@Username", 
				myConnection);
			
			SqlParameter parameter = new SqlParameter("@Password", SqlDbType.NVarChar, 50);
			parameter.Value = password;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Username", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
			
			return true;
		}

		public bool CreateUser(User user, string password, IProviderTransaction tran) {

			SqlConnection myConnection;
			if (tran == null)
				myConnection = new SqlConnection(Globals.DataConnectionString);
			else
				myConnection = (tran as SqlProviderTransaction).Connection;
			SqlCommand myCommand = new SqlCommand("ipbased.fd_CreateNewUser", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			if (tran != null) myCommand.Transaction = (tran as SqlProviderTransaction).Transaction;

			SqlParameter parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
			parameter.Value = user.UserName;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@PassWord", SqlDbType.NVarChar, 50);
			parameter.Value = password;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50);
			parameter.Value = user.FirstName;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@LastName", SqlDbType.NVarChar, 50);
			parameter.Value = user.LastName;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
			parameter.Value = user.Email;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Admin", SqlDbType.Bit, 1);
			parameter.Value = user.Admin;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@VerifyKey", SqlDbType.NVarChar, 50);
			parameter.Value = user.VerifyKey;
			myCommand.Parameters.Add(parameter);

			//run the query
			if (tran == null) myConnection.Open();
			user.PrincipalID = Convert.ToInt32(myCommand.ExecuteScalar());
			if (tran == null) myConnection.Close();
			
			return true;
		}

		public bool CreateSession(Guid guid, string username, string address) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_CreateSession", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
			parameter.Value = guid;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Username", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Address", SqlDbType.NVarChar, 50);
			parameter.Value = address;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool CreateGroup(Group group) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_CreateGroup", myConnection);

			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@GroupName", SqlDbType.NVarChar, 150);
			parameter.Value = group.Name;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Creator", SqlDbType.NVarChar, 50);
			parameter.Value = group.Creator;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = group.AsstID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			group.PrincipalID = Convert.ToInt32(myCommand.ExecuteScalar());
			myConnection.Close();			

			return true;
		}

		public bool CreateSection(Section section) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_CreateSection", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@SectionName", SqlDbType.NVarChar, 50);
			parameter.Value = section.Name;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Owner", SqlDbType.NVarChar, 50);
			parameter.Value = section.Owner;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = section.CourseID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			section.ID = Convert.ToInt32(myCommand.ExecuteScalar());
			myConnection.Close();			

			return true;
		}

		public bool CreateSubjLineAffect(int resID, int line) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("INSERT INTO ResultsCodeLines (resID, line) VALUES " +
							   "(@ResID, @Line)", myConnection);
			
			SqlParameter parameter = new SqlParameter("@ResID", SqlDbType.Int, 4);
			parameter.Value = resID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Line", SqlDbType.Int, 4);
			parameter.Value = line;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool CreateSubjResult(SubjResult result) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_CreateSubjResult", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@RubricID", SqlDbType.Int, 4);
			parameter.Value = result.RubricID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@SubID", SqlDbType.Int, 4);
			parameter.Value = result.SubmissionID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = result.FileID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Line", SqlDbType.Int, 4);
			parameter.Value = result.Line;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Type", SqlDbType.Int, 4);
			parameter.Value = result.SubjType;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Comment", SqlDbType.Text, result.Comment.Length);
			parameter.Value = result.Comment;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Points", SqlDbType.Float, 8);
			parameter.Value = result.Points;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Grader", SqlDbType.NVarChar, 50);
			parameter.Value = result.Grader;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			result.ID = Convert.ToInt32(myCommand.ExecuteScalar());
			myConnection.Close();			

			return true;
		}

		public bool CreateAutoResult(AutoResult result) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_CreateAutoResult", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@EvalID", SqlDbType.Int, 4);
			parameter.Value = result.EvalID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@SubID", SqlDbType.Int, 4);
			parameter.Value = result.SubmissionID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Result", SqlDbType.Text, result.XmlResult.Length);
			parameter.Value = result.XmlResult;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Grader", SqlDbType.NVarChar, 50);
			parameter.Value = result.Grader;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Points", SqlDbType.Float, 8);
			parameter.Value = result.Points;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Success", SqlDbType.Int, 4);
			parameter.Value = result.Success;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Comp", SqlDbType.Float);
			parameter.Value = result.CompetitionScore;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			result.ID = Convert.ToInt32(myCommand.ExecuteScalar());
			myConnection.Close();			

			return true;
		}

		public bool CreateAutoJob(AutoJob job) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_CreateAutoJob", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
			parameter.Value = job.JobName;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Creator", SqlDbType.NVarChar, 50);
			parameter.Value = job.JobCreator;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = job.AsstID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			job.ID = Convert.ToInt32(myCommand.ExecuteScalar());
			myConnection.Close();			

			return true;
		}

		public bool CreateAutoJobTest(int jobID, int evalID, int subID, bool onsubmit) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("INSERT INTO AutoJobTests " +
				"(jobID, evalID, subID, testerIP, testerDesc, status, onsubmit) " +
				@"VALUES (@JobID, @EvalID, @SubID,'','',0,@OnSubmit)" , myConnection);
			
			SqlParameter parameter = new SqlParameter("@JobID", SqlDbType.Int, 4);
			parameter.Value = jobID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@EvalID", SqlDbType.Int, 4);
			parameter.Value = evalID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@SubID", SqlDbType.Int, 4);
			parameter.Value = subID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@OnSubmit", SqlDbType.Int, 4);
			parameter.Value = onsubmit;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool DeleteAutoJob(int jobID, IProviderTransaction tran) {

			SqlConnection myConnection;
			if (tran == null)
				myConnection = new SqlConnection(Globals.DataConnectionString);
			else
				myConnection = (tran as SqlProviderTransaction).Connection;
			SqlCommand myCommand = new SqlCommand("ipbased.fd_DeleteAutoJob", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			if (tran != null) myCommand.Transaction = (tran as SqlProviderTransaction).Transaction;

			SqlParameter parameter = new SqlParameter("@JobID", SqlDbType.Int, 4);
			parameter.Value = jobID;
			myCommand.Parameters.Add(parameter);
			
			if (tran == null) myConnection.Open();
			myCommand.ExecuteNonQuery();
			if (tran == null) myConnection.Close();			
			return true;
		}

		public bool DeleteAutoJobTest(int jobID, int evalID, int subID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_DeleteAutoJobTest", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@JobID", SqlDbType.Int, 4);
			parameter.Value = jobID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@EvalID", SqlDbType.Int, 4);
			parameter.Value = evalID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@SubID", SqlDbType.Int, 4);
			parameter.Value = subID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			
			return true;
		}

		public bool CreateAutoEvaluation(AutoEvaluation eval) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_CreateAutoEval", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@Creator", SqlDbType.NVarChar, 50);
			parameter.Value = eval.Creator;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@IsBuild", SqlDbType.Int, 4);
			parameter.Value = eval.IsBuild;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Tool", SqlDbType.NVarChar, 50);
			parameter.Value = eval.RunTool;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@ToolArguments", SqlDbType.NVarChar, 100);
			parameter.Value = eval.RunToolArgs;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@TimeLimit", SqlDbType.Int, 4);
			parameter.Value = eval.TimeLimit;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = eval.AsstID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@RunOnSubmit", SqlDbType.Bit, 1);
			parameter.Value = eval.RunOnSubmit;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Competitive", SqlDbType.Bit, 1);
			parameter.Value = eval.Competitive;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Manager", SqlDbType.Int, 4);
			parameter.Value = eval.Manager;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@ToolVersion", SqlDbType.NVarChar, 50);
			parameter.Value = eval.ToolVersion;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@ResType", SqlDbType.NVarChar, 50);
			parameter.Value = eval.ResultType;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@ToolVersioning", SqlDbType.Int, 4);
			parameter.Value = eval.ToolVersioning;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			eval.ID = Convert.ToInt32(myCommand.ExecuteScalar());
			myConnection.Close();			

			return true;
		}

		public bool UpdateRubricSubPoints(int rubID, int subID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_UpdateRubricSubPoints", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@RubricID", SqlDbType.Int);
			parameter.Value = rubID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@SubID", SqlDbType.Int);
			parameter.Value = subID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool UpdateRubricEntry(Rubric rub) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_UpdateRubricEntry", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@Points", SqlDbType.Float, 8);
			parameter.Value = rub.Points;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Description", SqlDbType.Text, rub.Description.Length);
			parameter.Value = rub.Description;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
			parameter.Value = rub.Name;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@RubID", SqlDbType.Int, 4);
			parameter.Value = rub.ID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@AllowNeg", SqlDbType.Bit);
			parameter.Value = rub.AllowNegativePoints;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool UpdateSubjResult(SubjResult res) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_UpdateSubjResult", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@Points", SqlDbType.Float, 8);
			parameter.Value = res.Points;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Comment", SqlDbType.Text, res.Comment.Length);
			parameter.Value = res.Comment;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@SubjType", SqlDbType.Int, 4);
			parameter.Value = res.SubjType;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@ResID", SqlDbType.Int, 4);
			parameter.Value = res.ID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool UpdateCannedResponse(CannedResponse can) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("UPDATE CannedComments SET " +
				"points = @Points, comment = @Comment, " +
				"type = @Type WHERE ID = @CanID", myConnection);
			
			SqlParameter parameter = new SqlParameter("@Points", SqlDbType.Float, 8);
			parameter.Value = can.Points;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Comment", SqlDbType.Text, can.Comment.Length);
			parameter.Value = can.Comment;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Type", SqlDbType.Int, 4);
			parameter.Value = can.Type;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@CanID", SqlDbType.Int, 4);
			parameter.Value = can.ID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool CreateRubric(int asstID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_CreateRubric", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = asstID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool CreateRubricEntry(string name, 
									  string desc, int parentID, double points, int evalID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_CreateRubricEntry", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@ParentID", SqlDbType.Int, 4);
			parameter.Value = parentID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Points", SqlDbType.Float, 8);
			parameter.Value = points;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Description", SqlDbType.Text, desc.Length);
			parameter.Value = desc;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
			parameter.Value = name;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@EvalID", SqlDbType.Int, 4);
			parameter.Value = evalID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool CreateCannedResponse(int rubricID, string comment, double points, int type) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("INSERT INTO CannedComments " +
				"(rubricID, points, comment, type) " +
				"VALUES " +
				"(@RubricID, @Points, @Comment, @Type)", 
				myConnection);
			
			SqlParameter parameter = new SqlParameter("@RubricID", SqlDbType.Int, 4);
			parameter.Value = rubricID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Points", SqlDbType.Float, 8);
			parameter.Value = points;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Comment", SqlDbType.Text, comment.Length);
			parameter.Value = comment;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Type", SqlDbType.Int, 4);
			parameter.Value = type;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public Components.Submission.SubmissionList GetAssignmentSubmissions(int asstID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);

			SqlCommand myCommand =
				new SqlCommand("ipbased.fd_GetAsstSubmissions", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = asstID;
			myCommand.Parameters.Add(parameter);

			Components.Submission.SubmissionList subs = new Components.Submission.SubmissionList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Components.Submission sub = new Components.Submission();
				FormEntityFromReader(reader, sub);
				subs.Add(sub);
			}
			myConnection.Close();

			return subs;
		}

		public Rubric GetAssignmentRubric(int asstID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM RubricForest " +
								"WHERE AsstID = @AsstID AND parentID = -1", 
				myConnection);
	
			SqlParameterCollection sparams = myCommand.Parameters;

			sparams.Add(new SqlParameter("@AsstID", SqlDbType.Int, 4));
			sparams["@AsstID"].Value = asstID;

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			Rubric rub = new Rubric();
			FormEntityFromReader(reader, rub);
			myConnection.Close();

			return rub;
		}

		public Announcement GetAnnouncementInfo(int annID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM AnnouncementView WHERE ID = @AnnID", 
				myConnection);
	
			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@AnnID", SqlDbType.Int, 4));
			sparams["@AnnID"].Value = annID;

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			Announcement ann = new Announcement();
			FormEntityFromReader(reader, ann);
			myConnection.Close();

			return ann;
		}

		public Session GetSessionInfo(Guid guid) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM SessionView WHERE guid=@Guid", myConnection);
	
			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@Guid", SqlDbType.UniqueIdentifier));
			sparams["@Guid"].Value = guid;

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			Session ses = new Session();
			FormEntityFromReader(reader, ses);
			myConnection.Close();

			return ses;
		}

		public Rubric GetRubricInfoFromEval(int evalID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM RubricForestView WHERE evalID = @EvalID", 
				myConnection);
	
			SqlParameterCollection sparams = myCommand.Parameters;

			sparams.Add(new SqlParameter("@EvalID", SqlDbType.Int, 4));
			sparams["@EvalID"].Value = evalID;

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			Rubric rub = new Rubric();
			FormEntityFromReader(reader, rub);
			myConnection.Close();

			return rub;
		}

		public bool GetRubricPoints(int rubID, int subID, out double points) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetRubricPoints", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			SqlParameterCollection sparams = myCommand.Parameters;

			sparams.Add(new SqlParameter("@RubricID", SqlDbType.Int));
			sparams.Add(new SqlParameter("@SubID", SqlDbType.Int));
			sparams["@RubricID"].Value = rubID;
			sparams["@SubID"].Value = subID;

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			points = 0;
			if (!reader.Read()) { myConnection.Close(); return false; }
			int success = (int) reader["status"];
			points = Convert.ToDouble(reader["points"]);
			myConnection.Close();

			return (success == 1);
		}

		public Rubric GetRubricInfo(int rubID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM RubricForestView WHERE ID = @RubID", 
				myConnection);
	
			SqlParameterCollection sparams = myCommand.Parameters;

			sparams.Add(new SqlParameter("@RubID", SqlDbType.Int, 4));
			sparams["@RubID"].Value = rubID;

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			Rubric rub = new Rubric();
			FormEntityFromReader(reader, rub);
			myConnection.Close();

			return rub;
		}

		public Evaluation GetEvalInfo(int evalID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetEvalInfo", myConnection);
	
			myCommand.CommandType = CommandType.StoredProcedure;
			SqlParameterCollection sparams = myCommand.Parameters;

			sparams.Add(new SqlParameter("@EvalID", SqlDbType.Int, 4));
			sparams["@EvalID"].Value = evalID;

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			Evaluation eval = FormEvalFromReader(reader);
			myConnection.Close();

			return eval;
		}

		public AutoEvaluation GetAutoEvalInfoByZone(int zoneID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetEvalInfoByZone", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
	
			SqlParameterCollection sparams = myCommand.Parameters;

			sparams.Add(new SqlParameter("@ZoneID", SqlDbType.Int, 4));
			sparams["@ZoneID"].Value = zoneID;

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			AutoEvaluation eval = (AutoEvaluation) FormEvalFromReader(reader);
			myConnection.Close();

			return eval;
		}

		public CourseRole GetCourseRoleInfo(int principalID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM CourseRoles WHERE principalID=@PrinID", 
				myConnection);

			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@PrinID", SqlDbType.Int));
			sparams["@PrinID"].Value = principalID;

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			CourseRole res = new CourseRole();
			FormEntityFromReader(reader, res);
			myConnection.Close();

			return res;
		}

		public CourseRole GetCourseRoleInfo(string role, int courseID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM CourseRoles WHERE name=@Name AND courseID=@CourseID", 
					myConnection);

			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50));
			sparams["@Name"].Value = role;
			sparams.Add(new SqlParameter("@CourseID", SqlDbType.Int, 4));
			sparams["@CourseID"].Value = courseID;

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			CourseRole res = new CourseRole();
			FormEntityFromReader(reader, res);
			myConnection.Close();

			return res;
		}
		
		public Result GetResultInfo(int resID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_GetResultInfo", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@ResID", SqlDbType.Int, 4));
			sparams["@ResID"].Value = resID;

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			Result res = FormResFromReader(reader);
			myConnection.Close();

			return res;
		}

		public bool UpdateAutoEvaluation(AutoEvaluation eval) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_UpdateAutoEval", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@EvalID", SqlDbType.Int, 4);
			parameter.Value = eval.ID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@IsBuild", SqlDbType.Int, 4);
			parameter.Value = eval.IsBuild;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@ZoneID", SqlDbType.Int, 4);
			parameter.Value = eval.ZoneID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Tool", SqlDbType.NVarChar, 50);
			parameter.Value = eval.RunTool;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@ToolArguments", SqlDbType.NVarChar, 100);
			parameter.Value = eval.RunToolArgs;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@RunOnSubmit", SqlDbType.Bit, 1);
			parameter.Value = eval.RunOnSubmit;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@TimeLimit", SqlDbType.Int, 4);
			parameter.Value = eval.TimeLimit;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Competitive", SqlDbType.Bit, 1);
			parameter.Value = eval.Competitive;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@ToolVersion", SqlDbType.NVarChar, 50);
			parameter.Value = eval.ToolVersion;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@ToolVersioning", SqlDbType.Int, 4);
			parameter.Value = eval.ToolVersioning;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool InviteUser(string invitee, string invitor, int groupID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_InviteUser", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@Invitee", SqlDbType.NVarChar, 50);
			parameter.Value = invitee;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Invitor", SqlDbType.NVarChar, 50);
			parameter.Value = invitor;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@GroupID", SqlDbType.Int, 4);
			parameter.Value = groupID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool CreateSubmission(Components.Submission sub) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_CreateSubmission", myConnection);
	
			myCommand.CommandType = CommandType.StoredProcedure;
			SqlParameterCollection sparams = myCommand.Parameters;

			sparams.Add(new SqlParameter("@LocationID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@AsstID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@PrincipalID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@Status", SqlDbType.Int, 4));
			
			sparams["@LocationID"].Value = sub.LocationID;
			sparams["@AsstID"].Value = sub.AsstID;
			sparams["@PrincipalID"].Value = sub.PrincipalID;
			sparams["@Status"].Value = sub.Status;

			myConnection.Open();
			sub.ID = Convert.ToInt32(myCommand.ExecuteScalar());
			myConnection.Close();

			return true;
		}

		public bool CreateAnnouncement(Announcement annou) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_CreateAnnounce", myConnection);
	
			myCommand.CommandType = CommandType.StoredProcedure;
			SqlParameterCollection sparams = myCommand.Parameters;

			sparams.Add(new SqlParameter("@Poster", SqlDbType.NVarChar, 50));
			sparams.Add(new SqlParameter("@Desc", SqlDbType.NText, annou.Description.Length));
			sparams.Add(new SqlParameter("@CourseID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 200));
			
			sparams["@Poster"].Value = annou.Poster;
			sparams["@Desc"].Value = annou.Description;
			sparams["@CourseID"].Value = annou.CourseID;
			sparams["@Title"].Value = annou.Title;

			myConnection.Open();
			annou.ID = Convert.ToInt32(myCommand.ExecuteScalar());
			myConnection.Close();

			return true;
		}

		public bool UpdateAnnouncement(Announcement announce) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("UPDATE Announcements SET title = @Title, content = @Description WHERE ID = @ID", 
				myConnection);
			
			SqlParameterCollection sparams = myCommand.Parameters;

			sparams.Add(new SqlParameter("@ID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 200));
			sparams.Add(new SqlParameter("@Description", SqlDbType.NText, announce.Description.Length));
			
			sparams["@ID"].Value = announce.ID;
			sparams["@Title"].Value = announce.Title;
			sparams["@Description"].Value = announce.Description;
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		
			return true;
		}

		public bool DeleteAnnouncement(int announcementID) {
	
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("DELETE FROM Announcements WHERE ID = @ID", myConnection);

			SqlParameter parameter = new SqlParameter("@ID", SqlDbType.Int, 4);
			parameter.Value = announcementID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public Group GetGroupInfo(int groupid) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM GroupView WHERE principalID = @GroupID", myConnection);

			SqlParameter parameterUsername = new SqlParameter("@GroupID", SqlDbType.Int, 4);
			parameterUsername.Value = groupid;
			myCommand.Parameters.Add(parameterUsername);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			
			Group group = new Group();
			FormEntityFromReader(reader, group);
			myConnection.Close();

			return group;
		}

		public bool GetCourseInfo(int courseid, Course course) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("SELECT * FROM Courses WHERE ID = @CourseID", myConnection);

			SqlParameter parameterUsername = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameterUsername.Value = courseid;
			myCommand.Parameters.Add(parameterUsername);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return false; }
			
			FormEntityFromReader(reader, course);
			myConnection.Close();

			return true;
		}

		public string GetUserPassword(string username) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT password FROM Users WHERE username=@Username", myConnection);

			SqlParameter parameterUsername = new SqlParameter("@Username", SqlDbType.NVarChar, 50);
			parameterUsername.Value = username;
			myCommand.Parameters.Add(parameterUsername);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			
			string password = (string) reader["password"];
			myConnection.Close();

			return password;
		}

		public bool GetUserInfo(string username, User user, IProviderTransaction tran) {
			
			SqlConnection myConnection;
			if (tran == null)
				myConnection = new SqlConnection(Globals.DataConnectionString);
			else
				myConnection = (tran as SqlProviderTransaction).Connection;
			SqlCommand myCommand = new SqlCommand("SELECT * FROM Users WHERE username=@Username", 
				myConnection);
			if (tran != null) myCommand.Transaction = (tran as SqlProviderTransaction).Transaction;

			SqlParameter parameterUsername = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
			parameterUsername.Value = username;
			myCommand.Parameters.Add(parameterUsername);
			
			if (tran == null) myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { 
				if (tran == null) myConnection.Close(); 
				reader.Close();
				return false; 
			}
			FormEntityFromReader(reader, user);
			reader.Close();
			if (tran == null) myConnection.Close();
			
			return true;
		}

		public CannedResponse GetCannedInfo(int canID) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);

			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM CannedComments WHERE ID = @CanID", myConnection);
			
			SqlParameter parameter= new SqlParameter("@CanID", SqlDbType.Int, 4);
			parameter.Value = canID;
			myCommand.Parameters.Add(parameter);
			
			CannedResponse can = new CannedResponse();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			FormEntityFromReader(reader, can);
			myConnection.Close();
			
			return can;
		}

		public bool GetPrincipalInfo(int pid, Principal principal) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);

			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetPrincipalInfo", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter= new SqlParameter("@Pid", SqlDbType.Int, 4);
			parameter.Value = pid;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return false; }
			FormEntityFromReader(reader, principal);
			myConnection.Close();
			
			return true;
		}

		/// <summary>
		/// Get file object corresponding to absolute path
		/// </summary>
		public CFile GetFile(string path) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM Files WHERE fileName=@Name AND filePath=@Path", 
				myConnection);

			SqlParameter parameter = new SqlParameter("@Path", SqlDbType.NVarChar, CFile.MaxPath);	
			string dirname = Path.GetDirectoryName(path);
			if (dirname == null || dirname.Length == 0)
				parameter.Value = path;
			else
				parameter.Value = dirname;

			myCommand.Parameters.Add(parameter);
			// Add Parameters to SPROC
			parameter = new SqlParameter("@Name", SqlDbType.NVarChar, CFile.MaxName);

			string filename = Path.GetFileName(path);
			if (filename == null || filename.Length == 0)
				parameter.Value = path;
			else
				parameter.Value = filename;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			CFile file = new CFile();
			FormFileFromSqlReader(reader, file);
			reader.Close();
			myConnection.Close();

			return file;
		}

		/// <summary>
		/// Get all files
		/// </summary>
		public CFile.FileList GetAllFiles() {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM Files", myConnection);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			CFile.FileList files = new CFile.FileList();
			while (reader.Read()) {
				CFile file = new CFile();
				FormFileFromSqlReader(reader, file);
				files.Add(file);
			}
			reader.Close();
			myConnection.Close();

			return files;
		}

		public bool AuthorizeFile(int principalID, CFile file, FileAction action) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_CheckFilePermission", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@PrincipalID", SqlDbType.Int);
			parameter.Value = principalID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = file.ID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@FileAction", SqlDbType.Int, 4);
			parameter.Value = (int)action;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			int retval = (int) myCommand.ExecuteScalar();
			myConnection.Close();

			return (retval > 0);
		}

		/// <summary>
		/// Update a file permission
		/// </summary>
		public void UpdateFilePermission(CFile file, CFilePermission perm) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_UpdateFilePermission", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = file.ID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@PrincipalID", SqlDbType.Int, 4);
			parameter.Value = perm.PrincipalID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@FileAction", SqlDbType.Int, 4);
			parameter.Value = perm.Action;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Grant", SqlDbType.Bit);
			parameter.Value = perm.Grant;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}

		/// <summary>
		/// Get file object corresponding to ID
		/// </summary>
		public CFile GetFile(int id) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM Files WHERE ID = @FileID", myConnection);

			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = id;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			CFile file = new CFile();
			FormFileFromSqlReader(reader, file);
			reader.Close();
			myConnection.Close();

			return file;
		}

		public Course.CourseList GetCourses() {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("SELECT * FROM Courses ORDER BY available DESC", myConnection);

			Course.CourseList courses = new Course.CourseList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Course course = new Course();
				FormEntityFromReader(reader, course);
				courses.Add(course);
			}
			myConnection.Close();

			return courses;
		}

		public User.UserList GetAllUsers() {	
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand(
				"SELECT * FROM Users ORDER BY lastName ASC", myConnection);
			
			User.UserList users = new User.UserList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				User user = new User();
				FormEntityFromReader(reader, user);
				users.Add(user);
			}
			myConnection.Close();

			return users;
		}

		public AutoJob.AutoJobList GetAllJobs() {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand(
				"ipbased.fd_GetAutoJobs", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			AutoJob.AutoJobList jobs = new AutoJob.AutoJobList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				AutoJob job = new AutoJob();
				FormEntityFromReader(reader, job);
				jobs.Add(job);
			}
			myConnection.Close();

			return jobs;
		}

		public AutoJob.AutoJobList GetAllAsstJobs(int asstID, IProviderTransaction tran) {
		
			SqlConnection myConnection;
			if (tran == null)
				myConnection = new SqlConnection(Globals.DataConnectionString);
			else
				myConnection = (tran as SqlProviderTransaction).Connection;
			SqlCommand myCommand = new SqlCommand(
				"SELECT * FROM AutoJobsView WHERE asstID=@AsstID", myConnection);
			if (tran != null) myCommand.Transaction = (tran as SqlProviderTransaction).Transaction;

			SqlParameter parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = asstID;
			myCommand.Parameters.Add(parameter);

			AutoJob.AutoJobList jobs = new AutoJob.AutoJobList();
			if (tran == null) myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				AutoJob job = new AutoJob();
				FormEntityFromReader(reader, job);
				jobs.Add(job);
			}
			reader.Close();
			if (tran == null) myConnection.Close();

			return jobs;
		}

		public AutoJob.AutoJobList GetUserAsstJobs(string username, int asstID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand(
				"SELECT * FROM AutoJobsView WHERE creator=@Creator AND asstID=@AsstID", myConnection);

			SqlParameter parameter = new SqlParameter("@Creator", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = asstID;
			myCommand.Parameters.Add(parameter);

			AutoJob.AutoJobList jobs = new AutoJob.AutoJobList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				AutoJob job = new AutoJob();
				FormEntityFromReader(reader, job);
				jobs.Add(job);
			}
			myConnection.Close();

			return jobs;
		}

		public AutoJobTest.AutoJobTestList GetSubAutoJobTests(int subID) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand(
				"ipbased.fd_GetSubAutoJobTests", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@SubID", SqlDbType.Int, 4);
			parameter.Value = subID;
			myCommand.Parameters.Add(parameter);

			AutoJobTest.AutoJobTestList jobs = new AutoJobTest.AutoJobTestList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				AutoJobTest job = new AutoJobTest();
				FormEntityFromReader(reader, job);
				jobs.Add(job);
			}
			myConnection.Close();

			return jobs;
		}

		public AutoJobTest.AutoJobTestList GetAllAutoJobTests() {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand(
				"ipbased.fd_GetAllAutoJobTests", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			AutoJobTest.AutoJobTestList jobs = new AutoJobTest.AutoJobTestList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				AutoJobTest job = new AutoJobTest();
				FormEntityFromReader(reader, job);
				jobs.Add(job);
			}
			myConnection.Close();

			return jobs;
		}

		public AutoJobTest.AutoJobTestList GetAutoJobTests(int jobID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand(
				"ipbased.fd_GetAutoJobTests", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@JobID", SqlDbType.Int, 4);
			parameter.Value = jobID;
			myCommand.Parameters.Add(parameter);

			AutoJobTest.AutoJobTestList jobs = new AutoJobTest.AutoJobTestList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				AutoJobTest job = new AutoJobTest();
				FormEntityFromReader(reader, job);
				jobs.Add(job);
			}
			myConnection.Close();

			return jobs;
		}

		public AutoJobTest ClaimJob(string ipaddress, string desc) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand(
				"ipbased.fd_GetNextAutoJob", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@TesterIP", SqlDbType.NVarChar, 50);
			parameter.Value = ipaddress;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@TesterDesc", SqlDbType.NVarChar, 50);
			parameter.Value = desc;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { 
				reader.Close();
				myConnection.Close();
				return null; 
			}
			AutoJobTest job = new AutoJobTest();
			FormEntityFromReader(reader, job);
			reader.Close();
			myConnection.Close();

			return job;
		}

		public Backup.BackupList GetBackups(int courseID) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand(
				"ipbased.fd_GetBackups", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);

			Backup.BackupList bax = new Backup.BackupList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Backup bak = new Backup();
				FormEntityFromReader(reader, bak);
				bax.Add(bak);
			}
			myConnection.Close();

			return bax;
		}

		public Components.Submission.SubmissionList GetAllSubmissions() {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand(
				"SELECT * FROM Submissions", myConnection);

			Components.Submission.SubmissionList subs = new Components.Submission.SubmissionList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Components.Submission sub = new Components.Submission();
				FormEntityFromReader(reader, sub);
				subs.Add(sub);
			}
			myConnection.Close();

			return subs;
		}

		public Components.Submission GetSubmissionInfo(int subID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);

			SqlCommand myCommand = new SqlCommand("SELECT * FROM Submissions WHERE ID = @SubID", 
				myConnection);

			SqlParameter parameter = new SqlParameter("@SubID", SqlDbType.Int, 4);
			parameter.Value = subID;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { 
				reader.Close();
				myConnection.Close();
				return null; 
			}

			Components.Submission sub = new Components.Submission();
			FormEntityFromReader(reader, sub);
			reader.Close();
			myConnection.Close();

			return sub;
		}

		public Components.Submission GetSubmissionInfoFromDirID(int dirID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);

			SqlCommand myCommand = new SqlCommand("SELECT * FROM Submissions WHERE directoryID = @FileID", 
				myConnection);

			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = dirID;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { 
				reader.Close();
				myConnection.Close();
				return null; 
			}

			Components.Submission sub = new Components.Submission();
			FormEntityFromReader(reader, sub);
			reader.Close();
			myConnection.Close();

			return sub;
		}

		public Activity.ActivityList GetObjectActivity(int objID, int type) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("SELECT * FROM ActivityLogView WHERE objID = @ObjID AND type=@Type ORDER BY acttime DESC", 
				myConnection);

			SqlParameter parameter = new SqlParameter("@ObjID", SqlDbType.Int);
			parameter.Value = objID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Type", SqlDbType.Int);
			parameter.Value = type;
			myCommand.Parameters.Add(parameter);


			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			Activity.ActivityList acts = new Activity.ActivityList();	
			while (reader.Read()) { 
				Activity act = new Activity();
				FormEntityFromReader(reader, act);
				acts.Add(act);
			}
			reader.Close();
			myConnection.Close();

			return acts;
		}

		public Result.ResultList GetFileSubjResults(int fileID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("SELECT * FROM SubjResultsView WHERE fileID = @FileID", 
				myConnection);

			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = fileID;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			Result.ResultList ress = new Result.ResultList();
			while (reader.Read()) { 
				Result res = FormResFromReader(reader);
				ress.Add(res);
			}
			reader.Close();
			myConnection.Close();

			return ress;
		}

		public bool GetSubjResultLines(SubjResult res) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM FileCommentView WHERE resID = @ResID", 
				myConnection);

			SqlParameter parameter = new SqlParameter("@ResID", SqlDbType.Int, 4);
			parameter.Value = res.ID;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			res.LinesAffected.Clear();
			while (reader.Read()) 
				res.LinesAffected.Add((int) reader["line"]);
			myConnection.Close();

			return true;
		}

		public Result.ResultList GetSubmissionResults(int subID, string type) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetSubResults", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@SubID", SqlDbType.Int, 4);
			parameter.Value = subID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Type", SqlDbType.NVarChar, 50);
			parameter.Value = type;
			myCommand.Parameters.Add(parameter);

			Result.ResultList ress = new Result.ResultList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Result res = FormResFromReader(reader);
				ress.Add(res);
			}
			myConnection.Close();

			return ress;
		}

		public Result.ResultList GetRubricResults(int rubID, int subID, string type) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetRubricResults", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@RubricID", SqlDbType.Int, 4);
			parameter.Value = rubID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@SubID", SqlDbType.Int, 4);
			parameter.Value = subID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Type", SqlDbType.NVarChar, 50);
			parameter.Value = type;
			myCommand.Parameters.Add(parameter);

			Result.ResultList ress = new Result.ResultList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Result res = FormResFromReader(reader);
				ress.Add(res);
			}
			myConnection.Close();

			return ress;
		}

		public Components.Submission.SubmissionList GetUserSubmissions(string username, int id, bool course) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);

			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetUserSubmissions", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@DescID", SqlDbType.Int, 4);
			parameter.Value = id;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Course", SqlDbType.Bit, 1);
			parameter.Value = course;
			myCommand.Parameters.Add(parameter);

			Components.Submission.SubmissionList subs = new Components.Submission.SubmissionList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Components.Submission sub = new Components.Submission();
				FormEntityFromReader(reader, sub);
				subs.Add(sub);
			}
			myConnection.Close();

			return subs;
		}

		public Rubric.RubricList GetRubricChildren(int rubID) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM RubricForestView WHERE parentID = @RubID " +
								"ORDER BY evalID, points", 
				myConnection);

			SqlParameter parameter = new SqlParameter("@RubID", SqlDbType.Int, 4);
			parameter.Value = rubID;
			myCommand.Parameters.Add(parameter);

			Rubric.RubricList rubs = new Rubric.RubricList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Rubric rub = new Rubric();
				FormEntityFromReader(reader, rub);
				rubs.Add(rub);
			}
			myConnection.Close();

			return rubs;
		}

		public CannedResponse.CannedResponseList GetRubricCannedResponses(int rubID) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM CannedComments WHERE rubricID = @RubID", 
				myConnection);

			SqlParameter parameter = new SqlParameter("@RubID", SqlDbType.Int, 4);
			parameter.Value = rubID;
			myCommand.Parameters.Add(parameter);

			CannedResponse.CannedResponseList cans = new CannedResponse.CannedResponseList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				CannedResponse can = new CannedResponse();
				FormEntityFromReader(reader, can);
				cans.Add(can);
			}
			myConnection.Close();

			return cans;
		}

		public Components.Submission.SubmissionList GetPrincipalSubmissions(int pid, int asstid) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);

			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetPrincipalSubmissions", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter parameter = new SqlParameter("@Pid", SqlDbType.Int, 4);
			parameter.Value = pid;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = asstid;
			myCommand.Parameters.Add(parameter);

			Components.Submission.SubmissionList subs = new Components.Submission.SubmissionList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Components.Submission sub = new Components.Submission();
				FormEntityFromReader(reader, sub);
				subs.Add(sub);
			}
			myConnection.Close();

			return subs;
		}

		public bool DeleteEvalDependency(int evalID, int depID) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("DELETE FROM EvaluationDeps WHERE " +
				"evalID = @EvalID AND depID = @DepID", myConnection);
			
			SqlParameter parameter = new SqlParameter("@EvalID", SqlDbType.Int, 4);
			parameter.Value = evalID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@DepID", SqlDbType.Int, 4);
			parameter.Value = depID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool CreateEvalDependency(int evalID, int depID) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("INSERT INTO EvaluationDeps (evalID, depID) VALUES " +
							   "(@EvalID, @DepID)", myConnection);
			
			SqlParameter parameter = new SqlParameter("@EvalID", SqlDbType.Int, 4);
			parameter.Value = evalID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@DepID", SqlDbType.Int, 4);
			parameter.Value = depID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool CreateCourse(Course course) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_CreateNewCourse", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@CourseName", SqlDbType.NVarChar, 150);
			parameter.Value = course.Name;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@CourseNumber", SqlDbType.NVarChar, 50);
			parameter.Value = course.Number;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			course.ID = Convert.ToInt32(myCommand.ExecuteScalar());
			myConnection.Close();			

			return true;
		}

		public bool CreateCourseRole(CourseRole role) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_CreateRole", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
			parameter.Value = role.Name;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = role.CourseID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Staff", SqlDbType.Bit, 1);
			parameter.Value = role.Staff;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			role.PrincipalID = Convert.ToInt32(myCommand.ExecuteScalar());
			myConnection.Close();			

			return true;
		}

		public bool AssignPermission(int principalID, string perm, string etype, int entityID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_AssignPermission", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@PrincipalID", SqlDbType.Int, 4);
			parameter.Value = principalID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Perm", SqlDbType.NVarChar, 50);
			parameter.Value = perm;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@EntityType", SqlDbType.NVarChar, 50);
			parameter.Value = etype;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@EntityID", SqlDbType.Int, 4);
			parameter.Value = entityID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool DenyPermission(int principalID, string perm, string etype, int entityID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_DenyPermission", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@PrincipalID", SqlDbType.Int, 4);
			parameter.Value = principalID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Perm", SqlDbType.NVarChar, 50);
			parameter.Value = perm;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@EntityType", SqlDbType.NVarChar, 50);
			parameter.Value = etype;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@EntityID", SqlDbType.Int, 4);
			parameter.Value = entityID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;
		}

		public bool CreateCourseMember(string username, int courseID, string role, IProviderTransaction tran) {

			SqlConnection myConnection;
			if (tran == null)
				myConnection = new SqlConnection(Globals.DataConnectionString);
			else
				myConnection = (tran as SqlProviderTransaction).Connection;
			SqlCommand myCommand = new SqlCommand(
				"ipbased.fd_AddUserToCourse", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			if (tran != null) myCommand.Transaction = (tran as SqlProviderTransaction).Transaction;
			
			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Role", SqlDbType.NVarChar, 50);
			parameter.Value = role;
			myCommand.Parameters.Add(parameter);
			
			if (tran == null) myConnection.Open();
			myCommand.ExecuteNonQuery();
			if (tran == null) myConnection.Close();			

			return true;			
		}

		public bool CreateGroupMember(Invitation invite) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand(
				"ipbased.fd_AcceptInvite", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@InviteID", SqlDbType.Int, 4);
			parameter.Value = invite.ID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;	
		}

		public bool CreateSectionMember(int sectionID, string username, bool switchu) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand(
				"ipbased.fd_AddUserToSection", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@SectionID", SqlDbType.Int, 4);
			parameter.Value = sectionID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Switch", SqlDbType.Bit, 1);
			parameter.Value = switchu;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();			

			return true;	
		}

		public bool UpdateCourse(Course course) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("UPDATE Courses SET courseName = @Name, " + 
								"courseNumber = @Number, contentID = @ContentID, available = @Avail WHERE ID = @ID", 
				myConnection);
			
			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@ID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@Number", SqlDbType.NVarChar, 200));
			sparams.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 200));
			sparams.Add(new SqlParameter("@ContentID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@Avail", SqlDbType.Bit));
			
			sparams["@ID"].Value = course.ID;
			sparams["@Number"].Value = course.Number;
			sparams["@Name"].Value = course.Name;
			sparams["@ContentID"].Value = course.ContentID;
			sparams["@Avail"].Value = course.Available;
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool UpdateCourseRole(string username, int courseID, string role, IProviderTransaction tran) {
			SqlConnection myConnection;
			if (tran == null)
				myConnection = new SqlConnection(Globals.DataConnectionString);
			else
				myConnection = (tran as SqlProviderTransaction).Connection;
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_UpdateCourseRole", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			if (tran != null) myCommand.Transaction = (tran as SqlProviderTransaction).Transaction;
			
			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@CourseID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 50));
			sparams.Add(new SqlParameter("@Role", SqlDbType.NVarChar, 50));
			
			sparams["@CourseID"].Value = courseID;
			sparams["@Username"].Value = username;
			sparams["@Role"].Value = role;
			
			if (tran == null) myConnection.Open();
			myCommand.ExecuteNonQuery();
			if (tran == null) myConnection.Close();
			return true;
		}

		public bool UpdateSubmission(Submission sub) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("UPDATE Submissions SET directoryID=@LocID, " +
							   "subTime = @SubTime, status = @Status WHERE ID = @SubID", 
					myConnection);

			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@LocID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@SubTime", SqlDbType.DateTime, 8));
			sparams.Add(new SqlParameter("@SubID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@Status", SqlDbType.Int, 4));
			
			sparams["@LocID"].Value = sub.LocationID;
			sparams["@SubTime"].Value = sub.Creation;
			sparams["@SubID"].Value = sub.ID;
			sparams["@Status"].Value = sub.Status;
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool UpdateSection(Section sec) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_UpdateSection", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@SectionID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50));
			sparams.Add(new SqlParameter("@Owner", SqlDbType.NVarChar, 50));
			
			sparams["@SectionID"].Value = sec.ID;
			sparams["@Name"].Value = sec.Name;
			sparams["@Owner"].Value = sec.Owner;
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool UpdateUser(User user, IProviderTransaction tran) {
			SqlConnection myConnection;
			if (tran == null)
				myConnection = new SqlConnection(Globals.DataConnectionString);
			else
				myConnection = (tran as SqlProviderTransaction).Connection;
			SqlCommand myCommand = 
				new SqlCommand("UPDATE Users SET " + 
							   "firstName=@FirstName, " +
							   "lastName=@LastName, " +
							   "verifykey=@VerifyKey, " +
							   "email=@Email WHERE username = @Username", myConnection);
			if (tran != null) myCommand.Transaction = (tran as SqlProviderTransaction).Transaction;

			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 50));
			sparams.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 50));
			sparams.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50));
			sparams.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 50));
			sparams.Add(new SqlParameter("@VerifyKey", SqlDbType.NVarChar, 50));
			
			sparams["@FirstName"].Value = user.FirstName;
			sparams["@LastName"].Value = user.LastName;
			sparams["@Email"].Value = user.Email;
			sparams["@Username"].Value = user.UserName;
			sparams["@VerifyKey"].Value = user.VerifyKey;
			
			if (tran == null) myConnection.Open();
			myCommand.ExecuteNonQuery();
			if (tran == null) myConnection.Close();

			return true;
		}

		public bool UpdateAssignment(Assignment asst) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_UpdateAssignment", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@AsstID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 200));
			sparams.Add(new SqlParameter("@ContentID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@StuRelease", SqlDbType.Bit, 1));
			sparams.Add(new SqlParameter("@ResRelease", SqlDbType.Bit, 1));
			sparams.Add(new SqlParameter("@DueDate", SqlDbType.DateTime, 8));
			sparams.Add(new SqlParameter("@Format", SqlDbType.NText));

			sparams["@AsstID"].Value = asst.ID;
			sparams["@Description"].Value = asst.Description;
			sparams["@ContentID"].Value = asst.ContentID;
			sparams["@StuRelease"].Value = asst.StudentRelease;
			sparams["@ResRelease"].Value = asst.StudentSubmit;
			sparams["@DueDate"].Value = asst.DueDate;
			if (asst.Format == null || asst.Format.Length == 0)
				sparams["@Format"].Value = DBNull.Value;
			else
				sparams["@Format"].Value = asst.Format;

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		
			return true;
		}

		/// <summary>
		/// Update a course setting
		/// </summary>
		public bool UpdateCourseSetting(Setting mySetting){

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("UPDATE CourseSettings SET settingValue = @SettingValue WHERE ID = @ID", 
				myConnection);
			
			SqlParameterCollection sparams = myCommand.Parameters;

			sparams.Add(new SqlParameter("@ID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@SettingValue", SqlDbType.Text, 16));

			sparams["@ID"].Value = mySetting.ID;
			sparams["@SettingValue"].Value = mySetting.Value;
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		
			return true;
		}
		
		/// <summary>
		/// Update an assignment setting
		/// </summary>
		public bool UpdateAssignmentSetting(Setting mySetting){

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("UPDATE AssignmentSettings SET settingValue = @SettingValue WHERE ID = @ID", 
				myConnection);
			
			SqlParameterCollection sparams = myCommand.Parameters;

			sparams.Add(new SqlParameter("@ID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@SettingValue", SqlDbType.Text, 16));

			sparams["@ID"].Value = mySetting.ID;
			sparams["@SettingValue"].Value = mySetting.Value;
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		
			return true;
		}

		public bool CreateAssignment(Assignment asst) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_CreateAssignment", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameterCollection sparams = myCommand.Parameters;

			sparams.Add(new SqlParameter("@CourseID", SqlDbType.Int, 4));
			sparams.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 200));
			sparams.Add(new SqlParameter("@DueDate", SqlDbType.DateTime, 8));
			sparams.Add(new SqlParameter("@Creator", SqlDbType.NVarChar, 50));
			sparams.Add(new SqlParameter("@AsstID", SqlDbType.Int, 4));
			sparams.Add(new	SqlParameter("@Format", SqlDbType.NText));

			sparams["@CourseID"].Value = asst.CourseID;
			sparams["@Description"].Value = asst.Description;
			sparams["@DueDate"].Value = asst.DueDate;
			sparams["@Creator"].Value = asst.Creator;
			sparams["@AsstID"].Direction = ParameterDirection.Output;
			sparams["@Format"].Value = asst.Format;

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			asst.ID = (int) sparams["@AsstID"].Value;
			myConnection.Close();

			return true;
		}

		public bool CreateActivity(Activity act) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_CreateActivity", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 50));
			sparams.Add(new SqlParameter("@Type", SqlDbType.Int));
			sparams.Add(new SqlParameter("@Desc", SqlDbType.NText));
			sparams.Add(new SqlParameter("@ObjID", SqlDbType.Int));

			sparams["@Username"].Value = act.Username;
			sparams["@Type"].Value = act.Type;
			sparams["@Desc"].Value = act.Description;
			sparams["@ObjID"].Value = act.ObjectID;

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool CreateBackup(Backup bak) {
			
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_CreateBackup", 
				myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameterCollection sparams = myCommand.Parameters;
			sparams.Add(new SqlParameter("@Creator", SqlDbType.NVarChar, 50));
			sparams.Add(new SqlParameter("@BackUp", SqlDbType.NVarChar, 250));
			sparams.Add(new SqlParameter("@DataFile", SqlDbType.NVarChar, 100));
			sparams.Add(new SqlParameter("@CourseID", SqlDbType.Int, 4));

			sparams["@Creator"].Value = bak.Creator;
			sparams["@BackUp"].Value = bak.BackedUp;
			sparams["@DataFile"].Value = bak.FileLocation;
			sparams["@CourseID"].Value = bak.CourseID;

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool GetAssignmentInfo(int asstID, Assignment asst) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT * FROM AssignmentsView WHERE ID=@AsstID", myConnection);

			SqlParameter parameterUsername = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameterUsername.Value = asstID;
			myCommand.Parameters.Add(parameterUsername);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return false; }

			FormEntityFromReader(reader, asst);
			myConnection.Close();

			return true;
		}

		public Group.GroupList GetAssignmentGroups(int asstID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetAsstGroups", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = asstID;
			myCommand.Parameters.Add(parameter);
			
			Group.GroupList groups = new Group.GroupList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Group group = new Group();
				FormEntityFromReader(reader, group);
				groups.Add(group);
			}
			reader.Close();
			myConnection.Close();
			return groups;
		}

		public Evaluation.EvaluationList GetAssignmentEvals(int asstID, string type) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetAsstEvals", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = asstID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Type", SqlDbType.NVarChar, 50);
			parameter.Value = type;
			myCommand.Parameters.Add(parameter);
			
			Evaluation.EvaluationList evals = new Evaluation.EvaluationList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Evaluation eval = (Evaluation) FormEvalFromReader(reader);
				evals.Add(eval);
			}

			reader.Close();
			myConnection.Close();
			return evals;
		}

		public Evaluation.EvaluationList GetEvalDependencies(int evalID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_GetEvalDependencies", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@EvalID", SqlDbType.Int, 4);
			parameter.Value = evalID;
			myCommand.Parameters.Add(parameter);
			
			Evaluation.EvaluationList evals = new Evaluation.EvaluationList();
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			while (reader.Read()) {
				Evaluation eval = (Evaluation) FormEvalFromReader(reader);
				evals.Add(eval);
			}
			reader.Close();
			myConnection.Close();
			
			return evals;
		}

		public bool DeleteAssignment(int asstID) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_DeleteAssignment", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@AsstID", SqlDbType.Int, 4);
			parameter.Value = asstID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool DeleteRubricEntry(int rubID) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_DeleteRubricEntry", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@RubID", SqlDbType.Int, 4);
			parameter.Value = rubID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool DeleteCannedResponse(int canID) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("DELETE FROM CannedComments WHERE ID = @CanID", myConnection);

			SqlParameter parameter = new SqlParameter("@CanID", SqlDbType.Int, 4);
			parameter.Value = canID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool DeleteSession(Guid guid) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("DELETE FROM Sessions WHERE guid=@Guid", myConnection);

			SqlParameter parameter = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
			parameter.Value = guid;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool DeleteEval(int evalID) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_DeleteEval", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@EvalID", SqlDbType.Int, 4);
			parameter.Value = evalID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool DeleteResult(int resID) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_DeleteResult", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@ResID", SqlDbType.Int, 4);
			parameter.Value = resID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool DeleteInvitation(Invitation invite) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("DELETE FROM GroupInvitations WHERE ID = @ID", myConnection);

			SqlParameter parameter = new SqlParameter("@ID", SqlDbType.Int, 4);
			parameter.Value = invite.ID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool DeleteGroupMember(string username, Group group) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_DeleteGroupMember", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@GroupID", SqlDbType.Int, 4);
			parameter.Value = group.PrincipalID;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool DeleteCourseMember(string username, int courseID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_DeleteCourseMember", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
			return true;
		}

		public bool DeleteSectionMember(string username, int sectionID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_DeleteSectionMember", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
			parameter.Value = username;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@SectionID", SqlDbType.Int, 4);
			parameter.Value = sectionID;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool DeleteSection(int sectionID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_DeleteSection", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@SectionID", SqlDbType.Int, 4);
			parameter.Value = sectionID;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool DeleteSubmission(int subID) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("DELETE FROM Submissions WHERE ID=@SubID", myConnection);

			SqlParameter parameter = new SqlParameter("@SubID", SqlDbType.Int, 4);
			parameter.Value = subID;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool DeleteCourse(int courseID) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_DeleteCourse", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value = courseID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		public bool DeleteGroup(int groupID) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_DeleteGroup", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@GroupID", SqlDbType.Int, 4);
			parameter.Value = groupID;
			myCommand.Parameters.Add(parameter);
			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return true;
		}

		/// <summary>
		/// Get a setting from an ID
		/// </summary>
		public Setting GetCourseSetting(int courseID, string settingName){
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("ipbased.fd_GetCourseSettingByName", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			
			SqlParameter parameter = new SqlParameter("@SettingName", SqlDbType.NVarChar, 50);
			parameter.Value = settingName;
			myCommand.Parameters.Add(parameter);

			parameter = new SqlParameter("@CourseID", SqlDbType.Int, 4);
			parameter.Value =courseID;
			myCommand.Parameters.Add(parameter);
			
			
			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) { myConnection.Close(); return null; }
			
			Setting oneSetting = new Setting();
		    FormEntityFromReader(reader, oneSetting);
			myConnection.Close();
			
			return oneSetting;
				
		}

		public Setting.Category.CategoryList GetSettingCategories(){
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = new SqlCommand("SELECT * FROM SettingCategories", myConnection);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			Setting.Category.CategoryList catList = new Setting.Category.CategoryList();
			while (reader.Read()){
				Setting.Category oneCat = new Setting.Category();
				FormEntityFromReader(reader, oneCat);
				catList.Add(oneCat);
			}
			return catList;
		}

		private void FormEntityFromReader(SqlDataReader reader, DataComponent comp) {
			for (int i = 0; i < reader.FieldCount; i++) {
				string fname = reader.GetName(i);
				if (comp.FieldExists(fname)) {
					object val = reader[i];
					if (!(val is DBNull))
						comp[fname] = Convert.ChangeType(val, reader[i].GetType());
				}
			}
		}
		
		private Evaluation FormEvalFromReader(SqlDataReader reader) {	
			Evaluation eval=null;
			string etype = (string) reader[Evaluation.TYPE_FIELD];
			int i;

			if (etype == Evaluation.AUTO_TYPE) 
				eval = new AutoEvaluation();
			
			for (i = 0; i < reader.FieldCount; i++) {
				string fname = reader.GetName(i);
				if (eval.FieldExists(fname)) {
					object val = reader[i];
					if (!(val is DBNull))
						eval[fname] = Convert.ChangeType(val, reader[i].GetType());
				}
			}
			
			return eval;
		}

		private Result FormResFromReader(SqlDataReader reader) {
			
			Result res=null;
			string rtype = (string) reader[Result.TYPE_FIELD];
			int i;

			if (rtype == Result.AUTO_TYPE) 
				res = new AutoResult();
			else if (rtype == Result.SUBJ_TYPE)
				res = new SubjResult();
			
			for (i = 0; i < reader.FieldCount; i++) {
				string fname = reader.GetName(i);
				if (res.FieldExists(fname)) {
					object val = reader[i];
					if (!(val is DBNull))
						res[fname] = Convert.ChangeType(val, reader[i].GetType());
				}
			}
			
			return res;
		}
	}
}