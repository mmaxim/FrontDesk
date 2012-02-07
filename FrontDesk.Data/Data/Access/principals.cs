using System;

using FrontDesk.Components;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {
	/// <summary>
	/// Summary description for principals.
	/// </summary>
	public class Principals : DataAccessComponent {

		public Principals(AuthorizedIdent ident) : base(ident) { }

		/// <summary>
		/// Returns a principal object with bare principal info
		/// Direct Provider layer call
		/// </summary>
		public Principal GetInfo(int pid) {
			
			Principal prin = new Principal();
			m_dp.GetPrincipalInfo(pid, prin);
			
			return prin;
		}

		/// <summary>
		/// Get principal submissions
		/// Direct Provider layer call
		/// </summary>
		public Components.Submission.SubmissionList GetSubmissions(int pid, int asstID) {
			return m_dp.GetPrincipalSubmissions(pid, asstID);
		}

		/// <summary>
		/// Get graded principal submissions
		/// </summary>
		public Components.Submission.SubmissionList GetGradedSubmissions(int pid, int asstID) {
			Submission.SubmissionList subs = m_dp.GetPrincipalSubmissions(pid, asstID);
			Submission.SubmissionList gsubs = new Submission.SubmissionList();
			foreach (Submission sub in subs)
				if (sub.Status == Submission.GRADED)
					gsubs.Add(sub);
			return gsubs;
		}

		/// <summary>
		/// Filter out defunct submissions, null if no non-defunct submissions exist
		/// </summary>
		private Components.Submission GetLatestNonDefunct(Components.Submission.SubmissionList subs) {
			foreach (Components.Submission sub in subs) {
				if (sub.Status != Components.Submission.DEFUNCT)
					return sub;
			}
			return null;
		}

		/// <summary>
		/// Get latest graded submission
		/// </summary>
		public Components.Submission GetLatestGradedSubmission(int pid, int asstID) {
			Components.Submission.SubmissionList subs = GetGradedSubmissions(pid, asstID);
			if (subs.Count > 0)
				return GetLatestNonDefunct(subs);
			else
				return null;
		}

		/// <summary>
		/// Return the latest submission of the user
		/// </summary>
		public Components.Submission GetLatestSubmission(int pid, int asstID) {

			Components.Submission.SubmissionList subs = GetSubmissions(pid, asstID);
			if (subs.Count > 0)
				return GetLatestNonDefunct(subs);
			else
				return null;
		}
	}
}
