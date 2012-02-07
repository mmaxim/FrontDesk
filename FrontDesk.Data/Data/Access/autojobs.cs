using System;

using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Provider;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Auto jobs accessor
	/// </summary>
	public class AutoJobs : DataAccessComponent {

		public AutoJobs(AuthorizedIdent ident) : base(ident) { }
	
		protected void Authorize(int courseID, string perm, int entityID, IProviderTransaction tran) {
			Authorize(courseID, Permission.ASSIGNMENT, perm, entityID, tran);
		}

		/// <summary>
		/// Get all active jobs
		/// Direct Provider layer call
		/// </summary>
		public AutoJob.AutoJobList GetAll() {
			return m_dp.GetAllJobs();
		}

		/// <summary>
		/// Get the current users asst auto jobs
		/// Direct Provider layer call
		/// </summary>
		public AutoJob.AutoJobList GetUserAsstJobs(int asstID) {
			return m_dp.GetUserAsstJobs(m_ident.Name, asstID);
		}

		/// <summary>
		/// Return a list of tests for the current job
		/// Direct Provider layer call
		/// </summary>
		public AutoJobTest.AutoJobTestList GetTests(int jobID) {
			return m_dp.GetAutoJobTests(jobID);
		}

		/// <summary>
		/// Claim a job (used by testing centers)
		/// Direct Provider layer call
		/// </summary>
		public AutoJobTest Claim(string ipaddress, string desc) {
			return m_dp.ClaimJob(ipaddress, desc);
		}

		/// <summary>
		/// Clear job from queue and actives
		/// Direct Provider layer call
		/// </summary>
		public bool Finish(int jobID, IProviderTransaction tran) {
			return m_dp.DeleteAutoJob(jobID, tran);
		}

		/// <summary>
		/// Finish a test from a job
		/// Direct Provider layer call
		/// </summary>
		public bool FinishTest(AutoJobTest test) {
			return m_dp.DeleteAutoJobTest(test.JobID, test.EvalID, test.SubmissionID);
		}

		/// <summary>
		/// Create a new job in the queue
		/// </summary>
		public AutoJob Create(string name, int asstID) {

			Assignment asst = new Assignments(m_ident).GetInfo(asstID);
		//	Authorize(asst.CourseID, "createjob", asstID, null);

			AutoJob job = new AutoJob();
			job.JobName = name;
			job.JobCreator = m_ident.Name;
			job.AsstID = asstID;
			m_dp.CreateAutoJob(job);

			return job;
		}

		/// <summary>
		/// Get the queue position of an auto job test
		/// </summary>
		public int GetQueuePosition(AutoJobTest test) {
			AutoJobTest.AutoJobTestList tests = GetAllTests();
			int i;
			for (i = 0; i < tests.Count; i++) {
				if (tests[i].JobID == test.JobID &&
					tests[i].SubmissionID == test.SubmissionID &&
					tests[i].EvalID == test.EvalID)
					return i+1;
			}
			return -1;
		}

		/// <summary>
		/// Get the size of the job queue
		/// Direct Provider layer call
		/// </summary>
		public AutoJobTest.AutoJobTestList GetAllTests() {
			return m_dp.GetAllAutoJobTests();
		}

		/// <summary>
		/// Get all auto tests on a submissions
		/// Direct Provider layer call
		/// </summary>
		public AutoJobTest.AutoJobTestList GetSubTests(int subID) {
			return m_dp.GetSubAutoJobTests(subID);
		}

		/// <summary>
		/// Create a test for a job
		/// </summary>
		public bool CreateTest(int jobID, int subID, int evalID, bool onsubmit) {

			Components.Submission sub = new Submissions(m_ident).GetInfo(subID);
			Assignment asst = new Assignments(m_ident).GetInfo(sub.AsstID);
			Evaluation eval = new Evaluations(m_ident).GetInfo(evalID);
	//		Authorize(asst.CourseID, "createjob", asst.ID, null);

			//Log that the student is being auto run
			new Activities(m_ident).Create(m_ident.Name, Activity.SUBMISSION, subID,
				"Started auto-evaluation: " + eval.Name);

			return m_dp.CreateAutoJobTest(jobID, evalID, subID, onsubmit);			
		}
	}
}
