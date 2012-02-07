using System;
using System.IO;
using System.Xml;

using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Components.Filesys;
using FrontDesk.Tools;
using FrontDesk.Data.Filesys;
using FrontDesk.Data.Provider;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Assignment data access component
	/// </summary>
	public class Assignments : DataAccessComponent {

		public Assignments(AuthorizedIdent ident) : base(ident) { }

		protected void Authorize(int courseID, string perm, int entityID, IProviderTransaction tran) {
			Authorize(courseID, Permission.ASSIGNMENT, perm, entityID, tran);
		}

		protected void Log(string msg, int asstID) {
			LogActivity(msg, Activity.ASSIGNMENT, asstID);
		}	

		/// <summary>
		/// Get assignment info
		/// Results in direct Provider layer call
		/// </summary>
		public Assignment GetInfo(int asstID) {
			Assignment asst = new Assignment();
			m_dp.GetAssignmentInfo(asstID, asst);
			return asst;
		}

		/// <summary>
		/// Update the assignment
		/// </summary>
		public bool Update(Assignment asst) {
			
			//Check various permissions
			Authorize(asst.CourseID, "update", asst.ID, null);
			Assignment oldasst = GetInfo(asst.ID);
			if (oldasst.StudentRelease != asst.StudentRelease)
				Authorize(asst.CourseID, "mrkavail", asst.ID, null);
			if (oldasst.StudentSubmit != asst.StudentSubmit)
				Authorize(asst.CourseID, "stusubmit", asst.ID, null);

			return m_dp.UpdateAssignment(asst);
		}

		/// <summary>
		/// Return all the submission groups for this assignment
		/// Direct Provider layer call
		/// </summary>
		public Group.GroupList GetGroups(int asstID) {
			return m_dp.GetAssignmentGroups(asstID);
		}

		/// <summary>
		/// Get all evaluations for the assignment
		/// </summary>
		public Evaluation.EvaluationList GetEvals(int asstID) {
			Evaluation.EvaluationList evals = new Evaluation.EvaluationList();
			evals.AddRange(GetAutoEvals(asstID));

			return evals;
		}

		/// <summary>
		/// Get all evaluations for the assignment that are competitive
		/// </summary>
		public Evaluation.EvaluationList GetCompetitions(int asstID) {
			Evaluation.EvaluationList evals = GetEvals(asstID);
			Evaluation.EvaluationList comps = new Evaluation.EvaluationList();
			foreach (Evaluation eval in evals)
				if (eval.Competitive)
					comps.Add(eval);
			return comps;
		}

		/// Get the auto evals for an assignment
		/// Direct Provider layer call
		/// </summary>
		public Evaluation.EvaluationList GetAutoEvals(int asstID) {
			return m_dp.GetAssignmentEvals(asstID, Evaluation.AUTO_TYPE);
		}

		/// <summary>
		/// Get auto evals that are to be run during submission
		/// </summary>
		public Evaluation.EvaluationList GetSubmitAutoEvals(int asstID) {
			Evaluation.EvaluationList allevals = GetAutoEvals(asstID);
			Evaluation.EvaluationList sevals = new Evaluation.EvaluationList();
			foreach (Evaluation eval in allevals)
				if (eval.RunOnSubmit)
					sevals.Add(eval);
			return sevals;
		}

		/// <summary>
		/// Get auto jobs for this asst
		/// Direct Provider layer call
		/// </summary>
		public AutoJob.AutoJobList GetAutoJobs(int asstID, IProviderTransaction tran) {
			return m_dp.GetAllAsstJobs(asstID, tran);
		}

		/// <summary>
		/// Get the subjective evaluation for this assignment
		/// </summary>
		public Rubric GetRubric(int asstID) {
			Rubric rub = m_dp.GetAssignmentRubric(asstID);
			if (rub == null) {
				new Rubrics(m_ident).Create(asstID);
				rub = m_dp.GetAssignmentRubric(asstID);
			}	
			return rub;
		}

		/// <summary>
		/// Get all submissions for an assignment
		/// Direct Provider layer call
		/// </summary>
		public Components.Submission.SubmissionList GetSubmissions(int asstID) {
			return m_dp.GetAssignmentSubmissions(asstID);
		}

		/// <summary>
		/// Delete the assignment
		/// </summary>
		public bool Delete(int asstID) {
		
			FileSystem fs = new FileSystem(m_ident);
			Submissions subda = new Submissions(m_ident);
			Evaluations evalda = new Evaluations(m_ident);
			Results resultda = new Results(m_ident);
			Groups groupda = new Groups(m_ident);
			AutoJobs jobda = new AutoJobs(m_ident);

			Assignment asst = GetInfo(asstID);

			//Check permission
			Authorize(asst.CourseID, "delete", asstID, null);

			//Take auto jobs
			IProviderTransaction tran = m_dp.BeginTransaction();
			AutoJob.AutoJobList jobs = GetAutoJobs(asstID, tran);
			foreach (AutoJob job in jobs)
				jobda.Finish(job.ID, tran);
			m_dp.CommitTransaction(tran);

			//Take submissions and results
			Components.Submission.SubmissionList allsubs = GetSubmissions(asstID);
			foreach (Components.Submission sub in allsubs) 	
				subda.Delete(sub.ID);
			
			//Take rubric
			Rubric rub = GetRubric(asstID);
			new Rubrics(m_ident).Delete(rub.ID);

			//Take evaluations
			Evaluation.EvaluationList allevals = GetEvals(asstID);
			foreach (Evaluation eval in allevals)
				evalda.Delete(eval.ID);

			//Take groups
			Group.GroupList groups = GetGroups(asstID);
			foreach (Group group in groups)
				groupda.Delete(group.PrincipalID, asstID);

			//Take assignment
			m_dp.DeleteAssignment(asstID);

			//Take content
			CFile content = fs.GetFile(asst.ContentID);
			fs.DeleteFile(content);

			//Log
			Log("Deleted assignment: " + asst.Description, asst.ID);
			
			return true;
		}

		public string Backup(int asstID) {

			string zfile, wzfile;
			
			Assignment asst = GetInfo(asstID);

			//Check permission
			Authorize(asst.CourseID, "createbackup", asstID, null);

			//Create our external sink file
			IExternalSink extsink = 
				ArchiveToolFactory.GetInstance().CreateArchiveTool(".zip") as IExternalSink;

			zfile = Globals.PurifyString(asst.Description)+
				DateTime.Now.Hour+DateTime.Now.Minute+DateTime.Now.Second+".zip";
			wzfile = Globals.BackupDirectoryName + "/" + zfile;
			zfile = Path.Combine(Globals.BackupDirectory, zfile);
			extsink.CreateSink(zfile);

			//Backup Info
			//Backup Results

			//Back up submissions
			Users users = new Users(m_ident);
			User.UserList mems = new Courses(m_ident).GetMembers(asst.CourseID, null);

			foreach (User mem in mems) 
				users.Backup(mem.UserName, asst.CourseID, asstID, extsink);

			extsink.CloseSink();

			new Backups(m_ident).Create(asst.Description, wzfile, asst.CourseID);

			return zfile;
		}

		/// <summary>
		/// Check to see if submission is available for current user
		/// </summary>
		public bool IsSubmissionAvailable(int asstID) {
			Assignment asst = GetInfo(asstID);
			CourseRole role = new Courses(m_ident).GetRole(m_ident.Name, asst.CourseID, null);

			return (role.Staff || asst.StudentSubmit);
		}

		/// <summary>
		/// Create an assignment
		/// </summary>
		public int Create(int courseID, string creator, string desc, DateTime duedate) {
			
			//Check perm
			Authorize(courseID, Permission.COURSE, "createasst", courseID, null);
			
			Assignment asst = new Assignment();
			asst.CourseID = courseID;
			asst.Creator = creator;
			asst.Description = desc;
			asst.DueDate = duedate;
			asst.Format = Assignment.DEFAULT_FORMAT;
		
			//Create
			m_dp.CreateAssignment(asst);

			//Setup default permissions
			CreatePermissions(asst.ID, courseID, Permission.ASSIGNMENT);

			//Setup default file permissions
			Courses courseda = new Courses(m_ident);
			CFilePermission.FilePermissionList perms = new CFilePermission.FilePermissionList();
			CourseRole.CourseRoleList staff = courseda.GetTypedRoles(courseID, true, null);
			CourseRole.CourseRoleList stu = courseda.GetTypedRoles(courseID, false, null);
			foreach (CourseRole role in staff) 
				perms.AddRange(CFilePermission.CreateFullAccess(role.PrincipalID));	
			
			foreach (CourseRole role in stu) 
				perms.Add(new CFilePermission(role.PrincipalID, FileAction.READ, true));

			//Create content area
			FileSystem fs = new FileSystem(m_ident);
			CFile cdir = fs.CreateDirectory(@"c:\acontent\" + asst.ID, false, perms, false);
			asst.ContentID = cdir.ID;
			Update(asst);	
	
			//Log
			Log("Created assignment: " + desc, asst.ID);
			
			return asst.ID;
		}
	}
}
