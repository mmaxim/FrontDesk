using System;
using System.Data;
using System.IO;
using System.Collections;
using System.ComponentModel;

using FrontDesk.Components;
using FrontDesk.Components.Filesys;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Filesys.Provider;
using FrontDesk.Data.Provider;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys.DataService;
using FrontDesk.Common;
using FrontDesk.Security;

namespace FrontDesk.Data.Filesys {

	/// <summary>
	/// File system access component
	/// All consistency checking happens here (Except locks)
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class FileSystem : Component, IProvidee, IFileSystemProvidee {

		protected IFileSystemProvider m_fs;
		protected IDataProvider m_dp;
		protected AuthorizedIdent m_ident=null;
		protected User m_user;
		protected static Hashtable m_fdss = new Hashtable();
		protected static FileSystem m_instance;

		public FileSystem(AuthorizedIdent ident, bool local) {
			Init(ident, local);
		}

		public FileSystem(AuthorizedIdent ident) {
			Init(ident, false);
		}

		private void Init(AuthorizedIdent ident, bool local) {
			DataProviderFactory.GetInstance(this);
			m_ident = ident;
			m_user = new Users(m_ident).GetInfo(m_ident.Name, null);

			if (local || Globals.FSLocal)
				FileSystemProviderFactory.GetInstance(this);
			else
				FileServiceInit(ident, false);
		}

		public void Acquire(IDataProvider provider) {
			m_dp=provider;
		}

		void IFileSystemProvidee.Acquire(IFileSystemProvider provider) {
			m_fs=provider;
		}

		private void FileServiceInit(AuthorizedIdent ident, bool reload) {
			if (reload || m_fdss[ident.Name] == null) {
				FileDataWebGateway fds = new FileDataWebGateway();
				fds.Url = String.Format("http://{0}/FrontDeskServices/filedataservice.asmx",
					Globals.FileSystemAddress);
				string password = new Users(null).GetPassword(ident.Name);
				ServiceTicket tik = fds.Authenticate(ident.Name, password);
				fds.ServiceTicketValue = tik;

				m_fdss[m_ident.Name] = fds;
			}
		}

		~FileSystem() {
			Dispose(false);
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
	/*			FileDataWebGateway fds = (FileDataWebGateway) m_fdss[m_ident.Name];
				if (fds != null)
					fds.Logout();*/
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Authorize a user on a file for an action
		/// </summary>
		protected bool Authorize(CFile file, FileAction action) {
			return Authorize(m_user.PrincipalID, file, action, m_user);
		}

		/// <summary>
		/// Authorize a user on a file for an action
		/// </summary>
		protected bool Authorize(string username, CFile file, FileAction action) {
			User user = new Users(m_ident).GetInfo(username, null);
			return Authorize(user.PrincipalID, file, action, user);
		}

		/// <summary>
		/// Authorize a user on a file for an action
		/// </summary>
		protected bool Authorize(int principalID, CFile file, FileAction action, User hint) {
			//Hack for system path
			if (file.Path.StartsWith(@"c:\system"))
				return true;
			else if (hint != null) {
				if (hint.Admin) return true;
				else return m_dp.AuthorizeFile(principalID, file, action);
			}
			else {
				//Check for a user
				Principal prin = new Principals(m_ident).GetInfo(principalID);
				if (prin.Type == Principal.USER) {
					User user = new Users(m_ident).GetInfo(prin.Name, null);
					if (user.Admin) return true;
				}

				return m_dp.AuthorizeFile(principalID, file, action);
			}
		}

		public bool IsSubdir(CFile left, CFile right) {
			return (right.FullPath.StartsWith(left.FullPath));
		}

		/// <summary>
		/// Get all the files in the directory
		/// </summary>
		public CFile.FileList ListDirectory(CFile file) {
		
			//Check permission
			if (!Authorize(file, FileAction.READ))
				throw new FileOperationException("Permission denied on file: " + file.Name + " for Operation: LIST");
			
			return m_dp.ListDirectory(file);
		}

		/// <summary>
		/// Lock a file for editing
		/// </summary>
		public CFileLock Edit(CFile file) {
			
			//Check for readonly 
			if (file.ReadOnly)
				throw new FileOperationException("Cannot edit a readonly file");

			//Authorize
			if (!Authorize(file, FileAction.WRITE))
				throw new FileOperationException("Permission denied on action: WRITE");

			//Attempt to get the lock
			return m_dp.ObtainLock(file, m_user.PrincipalID);
		}

		/// <summary>
		/// Save changes to file data
		/// </summary>
		public bool Save(CFile file) {
			
			CFileLock flock;

			//Release the lock
			if (null == (flock = m_dp.GetLockByFileID(file.ID)))
				throw new FileOperationException("File was never locked, cannot save");

			//Authorize
			if (!Authorize(file, FileAction.WRITE))
				throw new FileOperationException("Permission denied on action: WRITE");

			//Write the data
			CommitData(file);
	
			//Update mod times
			file.FileModified = DateTime.Now; file.Size = file.RawData.Length;
			m_dp.SyncFile(file);
			PercolateModified(file, file.FileModified);

			m_dp.ReleaseLock(flock);

			return true;
		}

		private void CommitData(CFile file) {
			FileDataWebGateway fds = (FileDataWebGateway) m_fdss[m_ident.Name];
			if (fds != null) {
				try {
					fds.CommitData(file.ID, file.RawData);
				} catch (Exception) {
					FileServiceInit(m_ident, true);
					fds = (FileDataWebGateway) m_fdss[m_ident.Name];
					try {
						fds.CommitData(file.ID, file.RawData);
					} catch (Exception) {
						throw new FileOperationException("Unable to connect to the file system");
					}
				}
			}
			else
				m_fs.CommitData(file);
		}
		
		/// <summary>
		/// Release a lock on a file
		/// </summary>
		public bool UnLock(CFile file) {
			
			CFileLock flock;

			if (null == (flock = m_dp.GetLockByFileID(file.ID)))
				throw new FileOperationException("File was never locked, cannot release");

			m_dp.ReleaseLock(flock);

			return true;
		}

		/// <summary>
		/// Synchronize the file against the system
		/// </summary>
		public bool UpdateFileInfo(CFile file, bool modupdate) {

			//Authorize
			if (!Authorize(file, FileAction.WRITE))
				throw new FileOperationException("Permission denied on action: WRITE");

			//Check name
			if (!ValidateFileName(file.Name))
				throw new FileOperationException("Invalid file name: " + file.Name);

			//Rename checks
			CFile ofile = GetFile(file.ID);
			if (file.FullPath != ofile.FullPath) {
				//Dup check
				if (GetFile(file.FullPath) != null)
					throw new FileOperationException("Cannot rename file, one already exists with same name");
			}

			m_dp.SyncFile(file);
			if (modupdate)
				PercolateModified(file, file.FileModified);
			
			//Rename move
			if (file.FullPath != ofile.FullPath && file.IsDirectory()) {
				CFile.FileList dirfiles = ListDirectory(ofile);
				try {
					MoveFiles(file, dirfiles, false);
				} catch (FileOperationException er) {
					//Undo the rename
					m_dp.SyncFile(ofile);
					throw er;
				}
			}

			return true;
		}

		private bool ValidateFileName(string name) {
			string badchars = "\\,:'\"`/";
			foreach (char c in badchars)
				if (name.IndexOf(c) >= 0)
					return false;
			return true;
		}

		private void PercolateModified(CFile file, DateTime mod) {
			if (file != null && file.FullPath != @"c:\") {
				
				file.FileModified = mod;
				m_dp.SyncFile(file);

				//Check special directories
				if (file.SpecType == CFile.SpecialType.SUBMISSION) {
					Submissions subda = new Submissions(Globals.CurrentIdentity);
					Components.Submission sub = subda.GetInfoByDirectoryID(file.ID);
					if (sub != null) {
						//Check to see if a staff member modded
						CourseRole role = new Courses(Globals.CurrentIdentity).GetRole(
							m_ident.Name, new Assignments(Globals.CurrentIdentity).GetInfo(sub.AsstID).CourseID, null);
						
						//Student mods update the submission time, staff mods don't...
						if (!role.Staff) {
							sub.Creation = mod;
							sub.Status = Components.Submission.UNGRADED;
							subda.Update(sub, new EmptySource());

							//Log this in sub log
							new Activities(m_ident).Create(m_ident.Name, Activity.SUBMISSION, sub.ID, 
								"Updated submission time due to student modification of files");
						}
					}
				} else if (file.SpecType == CFile.SpecialType.TEST) {
					Evaluations evalda = new Evaluations(Globals.CurrentIdentity);
					AutoEvaluation eval = evalda.GetAutoInfoByZone(file.ID);
					if (eval != null) {
						eval.ZoneModified = mod;
						evalda.UpdateAuto(eval, new EmptySource());
					}
				}
			
				CFile par = GetFile(file.Path);
				PercolateModified(par, mod);
			}
		}
	
		/// <summary>
		/// Create a new directory
		/// </summary>
		public void ImportData(string name, string path, IExternalSource extsource, bool modupdate, bool over) {
			ImportData(Path.Combine(path,name), extsource, modupdate, over);
		}

		/// <summary>
		/// Transfer a FS into another compatible FS
		/// </summary>
		public void CopyFileSystem(IFileSystemProvider dest) {
			CFile.FileList files = GetAllFiles();
			foreach (CFile file in files) {
				if (!file.IsDirectory()) {
					dest.CreateFile(file);
					LoadFileData(file);
					dest.CommitData(file);
				}
			}
		}

		/// <summary>
		/// Create a new directory
		/// </summary>
		public void ImportData(string fullpath, IExternalSource extsource, bool modupdate, bool over) {
			ExternalFile efile;
			
			CFile idir = GetFile(fullpath);
			if (!idir.IsDirectory())
				throw new FileOperationException("Cannot import data into a non-directory");
			
			//Authorize
			if (!Authorize(idir, FileAction.WRITE))
				throw new FileOperationException("Permission denied on action: WRITE");

			//Do the import
			try {
				while (null != (efile = extsource.NextFile())) {			
					string fpath = Path.Combine(fullpath, efile.Path);
					CFile file;	
					if (efile.Directory) {
						try {
							file = CreateDirectory(fpath, false, null, false);
						} catch (FileExistsException) { }
					}
					else {
						if (over) {
							if (null == (file = GetFile(fpath)))
								file = CreateFile(fpath, false, null, false);
						} else
							file = CreateFile(fpath, false, null, false);
					
						//Read file
						MemoryStream memstr = new MemoryStream();
						int size = 4096; byte[] data = new byte[size];
						while (true) {
							size = efile.DataStream.Read(data, 0, data.Length);
							if (size > 0) 
								memstr.Write(data, 0, size);
							else 
								break;	
						}
						//Commit data to database
						memstr.Seek(0, SeekOrigin.Begin);
						data = Globals.ReadStream(memstr, (int) memstr.Length);
						file.RawData = data; file.Size = data.Length;
						m_dp.SyncFile(file);
						CommitData(file);
					}
					extsource.CloseFile(efile);
				}
				extsource.CloseSource();
			} catch (Exception er) {
				throw new FileOperationException("Error during the import of the external source. If this source is an archive, please make sure that the format is supported by FrontDesk: MESSAGE: " + er.Message);
			}

			if (modupdate)
				PercolateModified(idir, DateTime.Now);
		}

		/// <summary>
		/// Export data to the specified external sink
		/// </summary>
		public DataSet ExportData(string prefix, string name, string path, IExternalSink extsink, bool exportfp) {
			return ExportData(prefix, Path.Combine(path,name), extsink, exportfp);
		}

		/// <summary>
		/// Export data to the specified external sink
		/// </summary>
		public DataSet ExportData(string prefix, string fullpath, IExternalSink extsink, bool exportfp) {
			return ExportData(prefix, m_dp.GetFile(fullpath), extsink, exportfp);
		}

		/// <summary>
		/// Export data to the specified external sink
		/// </summary>
		public DataSet ExportData(string prefix, int id, IExternalSink extsink, bool exportfp) {
			return ExportData(prefix, m_dp.GetFile(id), extsink, exportfp);
		}

		public DataSet ExportData(string prefix, CFile dir, IExternalSink extsink, 
								  bool exportfp) {

			//Init dataset descriptor
			DataSet expdata = new DataSet();
			expdata.DataSetName = "ExportData";
			expdata.Tables.Add("Export"); expdata.Tables.Add("File");
			expdata.Tables["Export"].Columns.Add("Mod");
			expdata.Tables["Export"].Columns.Add("ID");
			expdata.Tables["File"].Columns.Add("type");
			expdata.Tables["File"].Columns.Add("path");
			DataRow toprow = expdata.Tables["Export"].NewRow();
			toprow["Mod"] = dir.FileModified.ToString();
			toprow["ID"] = dir.ID.ToString();
			expdata.Tables["Export"].Rows.Add(toprow);

			//Do the export
			ExportData(expdata, prefix, dir, extsink, exportfp, "");
			
			return expdata;
		}

		/// <summary>
		/// Export data to the specified external sink
		/// </summary>
		public void ExportData(DataSet expdata, string prefix, CFile dir, IExternalSink extsink, 
							   bool exportfp, string relpath) {
			
			if (!dir.IsDirectory()) 
				throw new FileOperationException("Cannot export a single file");

			//Authorize
			if (!Authorize(dir, FileAction.READ))
				throw new FileOperationException("Permission denied for operation: READ");

			CFile.FileList dirlist = ListDirectory(dir);
			foreach (CFile file in dirlist) {

				string epath;
			
				if (prefix.Length > 0 && prefix[prefix.Length-1] != '\\')
					prefix += @"\";
				if (exportfp)
					epath=prefix + GetExportPath(file);
				else 
					epath=prefix + relpath + file.Name;

				DataRow dirrow = expdata.Tables["File"].NewRow();
				if (!exportfp) dirrow["path"]=relpath + file.Name; else dirrow["path"]=epath;

				ExternalFile extfile = new ExternalFile();
				extfile.Path = epath;
				
				if (file.IsDirectory()) {	
		
					dirrow["type"]="dir"; 
					expdata.Tables["File"].Rows.Add(dirrow);
					
					//Create the directory
					extfile.Directory = true;
					extsink.PutFile(extfile);

					ExportData(expdata, prefix, file, extsink, exportfp, relpath+file.Name+@"\");
				}
				else {

					new FileSystem(m_ident).LoadFileData(file);
					extfile.Size = file.RawData.Length;
					extfile.DataStream = new MemoryStream(file.RawData, 0, file.RawData.Length);

					//File row
					dirrow["type"]="file"; 
					expdata.Tables["File"].Rows.Add(dirrow);

					extsink.PutFile(extfile);
				}
			}
	
		}

		private string GetExportPath(CFile file) {
			
			string path = file.FullPath.Substring(3, file.FullPath.Length-3);
			string[] dirs = path.Split(@"\".ToCharArray());
			string aggdir="";
			int i;

			path="";
			for (i = 0; i < dirs.Length-1; i++) {
				aggdir = Path.Combine(aggdir, dirs[i]);
				CFile fdir = GetFile(@"c:\" + aggdir);

				if (fdir == null)
					path += dirs[i] + @"\";
				else
					path += fdir.Alias + @"\";
			}
			path += file.Alias;

			path = Globals.PurifyString(path);

			return path;
		}

		/// <summary>
		/// Copy a file into another file
		/// dest must be a directory
		/// </summary>
		public bool CopyFile(CFile dest, CFile src, bool overwrite) {
			//Destination must be a directory
			if (!dest.IsDirectory())
				throw new FileOperationException("Destination of a copy must be a directory");
			//Check to make sure one is copying onto themselves
			if (src.ID == dest.ID)
				throw new FileOperationException("Destination and source cannot be the same file: " +
					src.FullPath);
			
			//Check to make sure a file is not there with same name
			CFile.FileList dirlist = m_dp.ListDirectory(dest);
			foreach (CFile dirmem in dirlist)
				if (dirmem.Name == src.Name) {
					if (!overwrite)
						throw new FileOperationException("File with same already exists in destination");
					else
						DeleteFile(dirmem);
				}

			//Update the times
			PercolateModified(src, DateTime.Now);
			PercolateModified(dest, DateTime.Now);

			//Do the copy
			m_dp.CopyFile(dest, src, m_user.PrincipalID);
			CopyFileData("", Path.Combine(dest.FullPath, src.Name), src.FullPath);

			return true;
		}

		protected void CopyFileData(string dir, string dbase, string sbase) {
			string spath = Path.Combine(sbase, dir);
			CFile sfile = GetFile(spath);

			if (sfile.IsDirectory()) 
				foreach (CFile dirmem in ListDirectory(sfile))
					CopyFileData(Path.Combine(dir, dirmem.Name), dbase, sbase);
			else {
				string dpath = Path.Combine(dbase, dir);
				CFile dfile = GetFile(dpath);

				//Copy file
				FileDataWebGateway fds = (FileDataWebGateway) m_fdss[m_ident.Name];
				if (fds != null) {
					try {
						fds.CreateFile(sfile.ID);
						fds.CopyFile(sfile.ID, dfile.ID);
					} catch (Exception) {
						FileServiceInit(m_ident, true);
						fds = (FileDataWebGateway) m_fdss[m_ident.Name];
						try {
							fds.CreateFile(sfile.ID);
							fds.CopyFile(sfile.ID, dfile.ID);
						} catch (Exception) {
							throw new FileOperationException("Unable to connect to the file system");
						}
					}
				} else {
					m_fs.CreateFile(sfile);
					m_fs.CopyFile(sfile, dfile);
				}
			}
		}

		/// <summary>
		/// Copy multiple files into a desintation directory
		/// </summary>
		public bool CopyFiles(CFile dest, CFile.FileList files, bool overwrite) {
			ArrayList flocks = new ArrayList();
	
			foreach (CFile src in files)
				CopyFile(dest, src, overwrite);

			return true;
		}

		/// <summary>
		/// Copy a file into another file
		/// dest must be a directory
		/// </summary>
		public bool MoveFile(CFile dest, CFile src, bool overwrite) {

			//Destination must be a directory
			if (!dest.IsDirectory())
				throw new FileOperationException("Destination of a copy must be a directory");

			//Check to make sure one is copying onto themselves
			if (src.ID == dest.ID)
				throw new FileOperationException("Destination and source cannot be the same file: " +
					src.FullPath);

			//Cannot move a readonly file
			if (src.ReadOnly)
				throw new FileOperationException("Cannot move a readonly file");

			//Check to make sure a file is not there with same name
			CFile.FileList dirlist = m_dp.ListDirectory(dest);
			foreach (CFile dirmem in dirlist)
				if (dirmem.Name == src.Name) {
					if (!overwrite)
						throw new FileOperationException("File with same already exists in destination");
					else
						DeleteFile(dirmem);
				}

			//Update the times
			PercolateModified(src, DateTime.Now);
			PercolateModified(dest, DateTime.Now);

			//Get the user
			User user = new Users(m_ident).GetInfo(m_ident.Name, null);

			//Do the copy
			m_dp.MoveFile(dest, src, user.PrincipalID);

			return true;
		}

		/// <summary>
		/// Is the file locked
		/// </summary>
		public bool IsLocked(CFile file) {
			return (null != m_dp.GetLockByFileID(file.ID));
		}

		/// <summary>
		/// Return the lock for this file
		/// If the file loses the lock, null is returned
		/// </summary>
		public CFileLock GetLockInfo(CFile file) {
			return m_dp.GetLockByFileID(file.ID);
		}

		/// <summary>
		/// Copy multiple files into a desintation directory
		/// </summary>
		public bool MoveFiles(CFile dest, CFile.FileList files, bool overwrite) {
			foreach (CFile src in files)
				MoveFile(dest, src, overwrite);
			return true;
		}

		/// <summary>
		/// Copy multiple files into a desintation directory
		/// </summary>
		public bool DeleteFiles(CFile.FileList files) {
			foreach (CFile src in files)
				DeleteFile(src);
			return true;
		}

		/// <summary>
		/// Delete a file
		/// </summary>
		public bool DeleteFile(CFile file) {
			
			//Read only is undeletable
			if (file.ReadOnly)
				throw new FileOperationException("Cannot delete a readonly file");

			//Authorize
			if (!Authorize(file, FileAction.DELETE))
				throw new FileOperationException("Permission denied on action: DELETE");

			//Get user
			User user = new Users(m_ident).GetInfo(m_ident.Name, null);

			//Check for lock
			if (IsLocked(file))
				throw new FileOperationException("File cannot be deleted because it is locked");

			//Check for any subjective results
			if (new Results(m_ident).GetFileResults(file.ID).Count > 0)
				throw new FileOperationException("File cannot be deleted because there are results attached to it");

			//Get parent and update times
			CFile par = GetParent(file);
			if (par != null && par.FullPath != @"c:\")
				PercolateModified(par, DateTime.Now);

			DeleteDirData(file);
			m_dp.DeleteFile(file, user.PrincipalID);

			return true;
		}

		private void DeleteDirData(CFile dir) {
			if (dir.IsDirectory()) {
				CFile.FileList dirlist = ListDirectory(dir);
				foreach (CFile file in dirlist) {
					if (file.IsDirectory())
						DeleteDirData(file);
					else
						DeleteFileData(file);
				}
			} else
				DeleteFileData(dir);
		}

		private void DeleteFileData(CFile file) {
			FileDataWebGateway fds = (FileDataWebGateway) m_fdss[m_ident.Name];
			if (fds != null) {
				try {
					fds.DeleteFile(file.ID);
				} catch (Exception) {
					FileServiceInit(m_ident, true);
					fds = (FileDataWebGateway) m_fdss[m_ident.Name];
					try {
						fds.DeleteFile(file.ID);
					} catch (Exception) {
						throw new FileOperationException("Unable to connect to the file system");
					}
				}
			}
			else
				m_fs.DeleteFile(file);
		}

		/// <summary>
		/// Set permissions for a given file
		/// </summary>
		public void SetPermissions(CFile file, CFilePermission perm) {
			CFilePermission.FilePermissionList perms = new CFilePermission.FilePermissionList();
			perms.Add(perm);

			SetPermissions(file, perms);
		}

		/// <summary>
		/// Set permissions for a given file
		/// </summary>
		public void SetPermissions(CFile file, CFilePermission.FilePermissionList perms) {
			
			//Authorize
			if (!Authorize(file, FileAction.ADMIN))
				throw new FileOperationException("Permission denied for action: ADMIN");

			SetPermissionsInt(file, perms);
		}

		/// <summary>
		/// Private functionality to override admin security check
		/// </summary>
		private void SetPermissionsInt(CFile file, CFilePermission.FilePermissionList perms) {
			foreach (CFilePermission perm in perms) 
				m_dp.UpdateFilePermission(file, perm);
		}

		/// <summary>
		/// Get permissions for a given file
		/// </summary>
		public CFilePermission.FilePermissionList GetPermissions(CFile file, int principalID) {
			//Check perms
			CFilePermission.FilePermissionList perms = new CFilePermission.FilePermissionList();
			if (Authorize(principalID, file, FileAction.READ, null)) 
				perms.Add(new CFilePermission(principalID, FileAction.READ, true));
			if (Authorize(principalID, file, FileAction.WRITE, null))
				perms.Add(new CFilePermission(principalID, FileAction.WRITE, true));
			if (Authorize(principalID, file, FileAction.DELETE, null))
				perms.Add(new CFilePermission(principalID, FileAction.DELETE, true));

			return perms;
		}

		/// <summary>
		/// Recreate the file permissions table
		/// </summary>
		public void RecoverBaseFilePermissions() {

			//Get all submissions
			Components.Submission.SubmissionList subs = new Submissions(m_ident).GetAll();
			Assignments asstda = new Assignments(m_ident);
			Courses courseda = new Courses(m_ident);
			CFilePermission.FilePermissionList full;
			
			foreach (Components.Submission sub in subs) {
				CFile subdir = GetFile(sub.LocationID);
				int courseID = asstda.GetInfo(sub.AsstID).CourseID;
				
				//Give staff access
				CourseRole.CourseRoleList staff = courseda.GetTypedRoles(courseID, true, null);
				foreach (CourseRole role in staff) {
					full = CFilePermission.CreateFullAccess(role.PrincipalID);	
					SetPermissionsInt(subdir, full);
				}

				//Give sub principal access
				full = CFilePermission.CreateOprFullAccess(sub.PrincipalID);
				SetPermissionsInt(subdir, full);
			}

			//Do content
			Course.CourseList courses = courseda.GetAll();
			foreach (Course course in courses) {
				CFile cont = GetFile(course.ContentID);
				
				//Give staff access
				CourseRole.CourseRoleList staff = courseda.GetTypedRoles(course.ID, true, null);
				foreach (CourseRole role in staff) {
					full = CFilePermission.CreateFullAccess(role.PrincipalID);	
					SetPermissionsInt(cont, full);
				}
				//Give students read access
				CourseRole.CourseRoleList stu = courseda.GetTypedRoles(course.ID, false, null);
				foreach (CourseRole role in stu) {
					full = new CFilePermission.FilePermissionList();
					full.Add(new CFilePermission(role.PrincipalID, FileAction.READ, true));
					SetPermissionsInt(cont, full);
				}

				//Give staff  and stuaccess to asst content
				Assignment.AssignmentList assts = courseda.GetAssignments(course.ID);
				foreach (Assignment asst in assts) {
					CFile acont = GetFile(asst.ContentID);
					foreach (CourseRole role in staff) {
						full = CFilePermission.CreateFullAccess(role.PrincipalID);	
						SetPermissionsInt(acont, full);
					}
					foreach (CourseRole role in stu) {
						full = new CFilePermission.FilePermissionList();
						full.Add(new CFilePermission(role.PrincipalID, FileAction.READ, true));
						SetPermissionsInt(acont, full);
					}
				}
			}
		}

		/// <summary>
		/// Return file information
		/// Direct Provider layer call
		/// </summary>
		public CFile GetFile(int id) {
			return m_dp.GetFile(id);
		}

		/// <summary>
		/// Load data for a file
		/// </summary>
		public bool LoadFileData(CFile file) {
			if (file.IsDirectory())
				return false;

			//Authorize
			if (!Authorize(file, FileAction.READ))
				throw new FileOperationException("Permission denied for action: READ");

			//Connect to data service
			FileDataWebGateway fds = (FileDataWebGateway) m_fdss[m_ident.Name];
			if (fds != null) {
				DataEnvelope data;
				try {
					data = fds.LoadFileData(file.ID);
				} catch (Exception) {
					FileServiceInit(m_ident, true);
					fds = (FileDataWebGateway) m_fdss[m_ident.Name];
					try {
						data = fds.LoadFileData(file.ID);
					} catch (Exception) {
						throw new FileOperationException("Unable to connect to the file system or file not found");
					}
				}
				if (data.Size > 0)
					file.RawData = data.Data;
				else
					file.RawData = new byte[0];
			} else
				m_fs.FetchData(file);

			return true;
		}

		/// <summary>
		/// Get all files in the system
		/// Direct Provider layer call
		/// </summary>
		public CFile.FileList GetAllFiles() {
			return m_dp.GetAllFiles();
		}

		/// <summary>
		/// Return file information
		/// Direct Provider layer call
		/// </summary>
		public CFile GetFile(string file) {
			return m_dp.GetFile(file);
		}

		/// <summary>
		/// Get the parent directory on the file
		/// Direct Provider layer call
		/// </summary>
		public CFile GetParent(CFile file) {
			return m_dp.GetFileParent(file);
		}

		private void CreateFileData(CFile file) {
			FileDataWebGateway fds = (FileDataWebGateway) m_fdss[m_ident.Name];
			if (fds != null) { 
				try {
					fds.CreateFile(file.ID); 
				} catch (Exception) {
					FileServiceInit(m_ident, true);
					fds = (FileDataWebGateway) m_fdss[m_ident.Name];
					try {
						fds.CreateFile(file.ID); 
					} catch (Exception) {
						throw new FileOperationException("Unable to connect to the file system");
					}
				}
			}
			else
				m_fs.CreateFile(file);
		}

		/// <summary>
		/// Create a new file
		/// </summary>
		public CFile CreateFile(string fullpath, bool ronly, CFilePermission.FilePermissionList perms) {
			return CreateFile(fullpath, ronly, perms, true);
		}

		/// <summary>
		/// Create a new file
		/// </summary>
		public CFile CreateFile(string fullpath, bool ronly, CFilePermission.FilePermissionList perms, bool modupdate) {

			CFile file = new CFile();

			fullpath = PurifyPath(fullpath);
			string dirname = Path.GetDirectoryName(fullpath);
			if (dirname == null || dirname.Length == 0) file.Path = fullpath;
			else file.Path = dirname;
			string filename = Path.GetFileName(fullpath);
			if (filename == null || filename.Length == 0) file.Name = fullpath;
			else file.Name = filename;

			file.Type = CFile.FileType.FILE;
			file.ReadOnly = ronly;

			//Check name
			if (!ValidateFileName(file.Name))
				throw new FileOperationException("Invalid file name: "  + file.Name);

			//Check to see if a file exists here
			if (GetFile(file.FullPath) != null)
				throw new FileExistsException("Create file failed: File already exists");

			//Check to make sure parent exists
			CFile par = GetFile(file.Path);
			if (par == null && file.Path != @"c:\")
				CreateDirectory(file.Path, ronly, null, modupdate);

			m_dp.CreateFile(file);
			if (perms != null) SetPermissionsInt(file, perms);
			CreateFileData(file);

			//Get parent and update times
			par = GetParent(file);
			if (modupdate && par != null && par.FullPath != @"c:\")
				PercolateModified(par, DateTime.Now);

			return file;
		}

		/// <summary>
		/// Create a new directory
		/// </summary>
		public CFile CreateDirectory(string fullpath, bool ronly, CFilePermission.FilePermissionList perms) {
			return CreateDirectory(fullpath, ronly, perms, true);
		}

		/// <summary>
		/// Create a new directory
		/// </summary>
		public CFile CreateDirectory(string fullpath, bool ronly, CFilePermission.FilePermissionList perms, bool modupdate) {

			CFile file = new CFile();

			fullpath = PurifyPath(fullpath);
			string dirname = Path.GetDirectoryName(fullpath);
			if (dirname == null || dirname.Length == 0) file.Path = fullpath;
			else file.Path = dirname;
			string filename = Path.GetFileName(fullpath);
			if (filename == null || filename.Length == 0) file.Name = fullpath;
			else file.Name = filename;

			file.Type = CFile.FileType.DIRECTORY;
			file.ReadOnly = ronly;

			//Check name
			if (!ValidateFileName(file.Name))
				throw new FileOperationException("Invalid file name: " + file.Name);

			//Check to see if a file exists here
			if (GetFile(file.FullPath) != null)
				throw new FileExistsException("Create file failed: Directory already exists");

			//Check to make sure parent exists
			CFile par = GetFile(file.Path);
			if (par == null && file.Path != @"c:\")
				CreateDirectory(file.Path, ronly, null, modupdate);

			m_dp.CreateFile(file);
			if (perms != null)
				SetPermissionsInt(file, perms);

			//Get parent and update times
			par = GetParent(file);
			if (modupdate && par != null && par.FullPath != @"c:\")
				PercolateModified(par, DateTime.Now);

			return file;
		}

		protected string PurifyPath(string path) {
			path.Replace('/', '\\');
			if (path.EndsWith("/"))
				return path.Substring(0, path.Length-1);
			else
				return path;
		}
	}

	/// <summary>
	/// A file operation error
	/// </summary>
	public class FileOperationException : CustomException {
		public FileOperationException(string msg) : base(msg) { }	
	}

	public class FileExistsException : FileOperationException {
		public FileExistsException(string msg) : base(msg) { }
	}
}
