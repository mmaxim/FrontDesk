using System;
using System.Data;
using System.IO;

using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Filesys;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Summary description for submissions.
	/// </summary>
	public class Submissions : DataAccessComponent {
		
		public Submissions(AuthorizedIdent ident) : base(ident) { }

		protected void Log(string msg, int subID) {
			LogActivity(msg, Activity.SUBMISSION, subID);
		}	

		/// <summary>
		/// Create a new submission
		/// </summary>
		public Components.Submission Create(int asstid, int principalID, IExternalSource files) {

			//TODO: Verify parameters
			Components.Submission sub = new Components.Submission();
			sub.AsstID = asstid;
			sub.PrincipalID = principalID;
			sub.Status = Submission.UNGRADED;

			//Check for locked for evaluation		
			if (!new Assignments(m_ident).IsSubmissionAvailable(asstid)) 
				throw new DataAccessException("Submission is locked. No more submissions are being accepted. Please contact course staff to proceed");
	
			//Create submission
			m_dp.CreateSubmission(sub);
			
			//Update to commit files
			sub = GetInfo(sub.ID);
			try {
				Update(sub, files);
			} catch (DataAccessException er) {
				UnsafeDelete(sub.ID);
				throw er;
			}

			//Queue autosubmit tests
			QueueSubmitTests(sub);

			//Log submission
			Assignment asst = new Assignments(m_ident).GetInfo(asstid);
			Log("User submitted " + asst.Description + " successfully", sub.ID);

			return sub;
		}

		private void QueueSubmitTests(Components.Submission sub) {
			
			int asstID = sub.AsstID;
			string strlog="";
			Evaluation.EvaluationList tests = new Assignments(m_ident).GetSubmitAutoEvals(asstID);
			if (tests.Count == 0) return;

			//Queue up pretests
			AutoJobs jobda = new AutoJobs(m_ident);
			AutoJob job = jobda.Create(m_ident.Name + " submission", asstID);
			foreach (Evaluation eval in tests) {
				jobda.CreateTest(job.ID, sub.ID, eval.ID, true);	
				strlog += eval.Name + " ";
			}
			
			//Log queueing
			Log("Pretests queued: " + strlog, sub.ID); 
		}

		/// <summary>
		/// Delete a submission
		/// </summary>
		public bool Delete(int subID) {
			
			Submission sub = GetInfo(subID);
			Assignment asst = new Assignments(m_ident).GetInfo(sub.AsstID);
			int locid = sub.LocationID; 

			//Check permission
			Authorize(asst.CourseID, Permission.ASSIGNMENT, "delsub", asst.ID, null);

			//Kill
			DoDelete(subID, locid);

			return true;
		}

		private void UnsafeDelete(int subID) {	
			Submission sub = GetInfo(subID);
			DoDelete(subID, sub.LocationID);
		}

		private void DoDelete(int subID, int locid) {

			FileSystem fs = new FileSystem(m_ident);

			//Take results
			Results resultda = new Results(m_ident);
			Result.ResultList ress = GetResults(subID);
			foreach (Result res in ress)
				resultda.Delete(res.ID);

			//Take any tests queued
			AutoJobs jobda = new AutoJobs(m_ident);
			AutoJobTest.AutoJobTestList tests = jobda.GetSubTests(subID);
			foreach (AutoJobTest test in tests)
				jobda.FinishTest(test);

			//Take the submission record
			m_dp.DeleteSubmission(subID);
			
			//Take the files
			CFile subdir = fs.GetFile(locid);
			if (subdir != null)
				fs.DeleteFile(subdir);
		}

		/// <summary>
		/// Get all submissions from all courses
		/// Direct Provider layer call
		/// </summary>
		public Components.Submission.SubmissionList GetAll() {
			return m_dp.GetAllSubmissions();
		}

		/// <summary>
		/// Get extended information about a submission
		/// Direct Provider layer call
		/// </summary>
		public Submission GetInfo(int subID) {
			return m_dp.GetSubmissionInfo(subID);
		}

		/// <summary>
		/// Return the submission from the dir ID
		/// Direct Provider layer call
		/// </summary>
		public Submission GetInfoByDirectoryID(int dirID) {
			return m_dp.GetSubmissionInfoFromDirID(dirID);
		}

		/// <summary>
		/// Get all results for a submission
		/// </summary>
		public Result.ResultList GetResults(int subID) {
			Result.ResultList ress = 
				m_dp.GetSubmissionResults(subID, Result.AUTO_TYPE);

			ress.AddRange(m_dp.GetSubmissionResults(subID, Result.SUBJ_TYPE));

			return ress;
		}

		/// <summary>
		/// Get earned points for a submission
		/// </summary>
		public double GetPoints(int subID) {
			Rubric arub = new Assignments(m_ident).GetRubric(GetInfo(subID).AsstID);

			return new Rubrics(m_ident).GetPoints(arub.ID, subID);
		}

		/// <summary>
		/// Get activity for the specified submission
		/// Direct Provider layer call
		/// </summary>
		public Activity.ActivityList GetActivity(int subID) {
			return m_dp.GetObjectActivity(subID, Activity.SUBMISSION);
		}

		/// <summary>
		/// Load submission directory with new files, updates time
		/// </summary>
		public bool Update(Submission sub, IExternalSource files) {
			FileSystem fs = new FileSystem(m_ident);
			bool markcmp, unmarkcmp, defunct;

			//Get old sub
			Components.Submission oldsub = GetInfo(sub.ID);
			markcmp = (oldsub.Status == Components.Submission.UNGRADED &&
					  sub.Status == Components.Submission.GRADED);
			unmarkcmp = (oldsub.Status == Components.Submission.GRADED &&
						 sub.Status == Components.Submission.UNGRADED);
			defunct = (oldsub.Status != Components.Submission.DEFUNCT &&
						sub.Status == Components.Submission.DEFUNCT);

			//Make sure toplevel zone directory exists
			CFile subdir = fs.GetFile(@"c:\subs");
			if (null == subdir)
				subdir = fs.CreateDirectory(@"c:\subs", true, null, false);

			//Build file perms
			CFilePermission.FilePermissionList perms = new CFilePermission.FilePermissionList();
			int courseID = new Assignments(m_ident).GetInfo(sub.AsstID).CourseID;
			CourseRole.CourseRoleList staff = new Courses(m_ident).GetTypedRoles(courseID, true, null);
			foreach (CourseRole role in staff) 
				perms.AddRange(CFilePermission.CreateFullAccess(role.PrincipalID));	
			perms.AddRange(CFilePermission.CreateOprFullAccess(sub.PrincipalID));

			//Create zone directory
			CFile esubdir;
			string zpath = @"c:\subs\" + sub.ID;
			if (null == (esubdir = fs.GetFile(zpath))) {
				esubdir = fs.CreateDirectory(zpath, false, perms, false);
				esubdir.SpecType = CFile.SpecialType.SUBMISSION;
				string name = new Principals(m_ident).GetInfo(sub.PrincipalID).Name;
				esubdir.Alias = String.Format("{0}: {1}", 
					name, GetNextSubmission(sub.AsstID, sub.PrincipalID)); 
				fs.UpdateFileInfo(esubdir, false);
			}	
			//Update sub entry
			sub.LocationID = esubdir.ID;
			m_dp.UpdateSubmission(sub);


			//Load files
			try {
				fs.ImportData(zpath, files, false, false); //Import the data
			} catch (Exception) {
				throw new DataAccessException("Invalid external file source. This means the system does " +
					"not understand how to extract files from the source. Please create a valid source");
			}

			//Verify submission structure
			VerifyFormat(sub.AsstID, zpath);

			//Log
			if (markcmp)
				Log("User [" + m_ident.Name + "] marked submission " + esubdir.Alias + " completed", sub.ID);
			else if (unmarkcmp)
				Log("User [" + m_ident.Name + "] marked submission " + esubdir.Alias + " incomplete", sub.ID);
			else if (defunct)
				Log("User [" + m_ident.Name + "] marked submission " + esubdir.Alias + " defunct", sub.ID);

			return true;
		}

		private void VerifyFormat(int asstID, string zpath) {
			FileSystem fs = new FileSystem(m_ident);
			
			DataSet zonedesc = new DataSet();
			string xmlString = new Assignments(m_ident).GetInfo(asstID).Format;
			if (xmlString == null || xmlString.Length == 0)
				return;
			MemoryStream memstream = 
				new MemoryStream(System.Text.Encoding.ASCII.GetBytes(xmlString));
			zonedesc.ReadXml(memstream);
			DataTable inodes = zonedesc.Tables["File"];
			
			foreach (DataRow row in inodes.Rows){
				string name = (string)row["name"];
				if (name != ".")
					if (null == fs.GetFile(Path.Combine(zpath, name.Substring(2))))
						throw new DataAccessException("Error: Your archive was missing the file: "+ name + " that is required by this assignment. You must resubmit with the correct files.");			
			}
		}

		protected int GetNextSubmission(int asstID, int pid) {
			Components.Submission.SubmissionList subdata = 
				new Principals(m_ident).GetSubmissions(pid, asstID);
			
			return subdata.Count;
		}
	}
}
