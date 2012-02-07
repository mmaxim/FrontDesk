using System;
using System.Collections;

using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Provider;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {
	
	/// <summary>
	/// Results access component
	/// </summary>
	public class Results : DataAccessComponent {

		public Results(AuthorizedIdent ident) : base(ident) { }

		protected void Authorize(int courseID, string perm, int entityID, IProviderTransaction tran) {
			Authorize(courseID, Permission.ASSIGNMENT, perm, entityID, tran);
		}

		/// <summary>
		/// Create a auto result.
		/// </summary>
		public bool CreateAuto(int evalID, string grader, int subID, string result) {

			//Check permission
			Submissions subac = new Submissions(m_ident);
			Components.Submission sub = subac.GetInfo(subID);
			Assignment asst = new Assignments(m_ident).GetInfo(sub.AsstID);
			Authorize(asst.CourseID, "createauto", asst.ID, null);

			AutoResult res = new AutoResult();
			res.EvalID = evalID; res.Grader = grader;
			res.SubmissionID = subID; res.XmlResult = result;

			//Clear out all results for this evaluation
			Submission.SubmissionList subs = 
				new Principals(m_ident).GetSubmissions(sub.PrincipalID, sub.AsstID);

			//Delete all old results
			foreach (Submission s in subs) {
				Result.ResultList ress = subac.GetResults(s.ID);
				foreach (Result r in ress) {
					if (r.Type == Result.AUTO_TYPE) {
						AutoResult ar = r as AutoResult;
						if (ar.EvalID == evalID)
							Delete(ar.ID);
					}
				}
			}

			return m_dp.CreateAutoResult(res);
		}

		/// <summary>
		/// Create a subjective result
		/// </summary>
		public bool CreateSubj(int subID, int rubricID, string comment, int fileid, int line, 
							   double points, ArrayList lines, int type) {
			
			//Check permission
			Components.Submission sub = 
				new Submissions(m_ident).GetInfo(subID);
			Assignment asst = new Assignments(m_ident).GetInfo(sub.AsstID);
			Authorize(asst.CourseID, "createsubj", asst.ID, null);

			SubjResult res = new SubjResult();
			res.SubjType = type; res.RubricID = rubricID;
			res.Comment = comment; res.FileID = fileid; res.Line = line;
			res.Grader = m_ident.Name; res.SubmissionID = subID;
			res.Points = points;

			//Create result
			m_dp.CreateSubjResult(res);

			//Attach lines
			foreach (int l in lines)
				m_dp.CreateSubjLineAffect(res.ID, l);

			return true;
		}

		/// <summary>
		/// Update a subjective result
		/// </summary>
		public bool UpdateSubj(SubjResult res) {

			//Check permission
			Components.Submission sub = 
				new Submissions(m_ident).GetInfo(res.SubmissionID);
			Assignment asst = new Assignments(m_ident).GetInfo(sub.AsstID);
			Authorize(asst.CourseID, "updatesubj", asst.ID, null);

			return m_dp.UpdateSubjResult(res);
		}

		/// <summary>
		/// Delete a result
		/// </summary>
		public bool Delete(int resID) {

			//Check permission
			Result res = GetInfo(resID);
			Components.Submission sub = new Submissions(m_ident).GetInfo(res.SubmissionID);
			Assignment asst = new Assignments(m_ident).GetInfo(sub.AsstID);
			Authorize(asst.CourseID, "deletesubj", asst.ID, null);

			return m_dp.DeleteResult(resID);
		}

		/// <summary>
		/// Get result information from id
		/// Direct Provider layer call
		/// </summary>
		public Result GetInfo(int resID) {
			return m_dp.GetResultInfo(resID);
		}

		/// <summary>
		/// Get the lines affected by the result
		/// </summary>
		public Result.ResultList GetFileResults(int fileID) {
			
			//Postulate result list
			Result.ResultList ress = m_dp.GetFileSubjResults(fileID);

			//Fill in lines
			foreach (SubjResult res in ress)
				m_dp.GetSubjResultLines(res);

			return ress;
		}
	}
}
