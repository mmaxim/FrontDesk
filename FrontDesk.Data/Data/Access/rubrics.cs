using System;

using FrontDesk.Components;
using FrontDesk.Components.Filesys;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Filesys;
using FrontDesk.Data.Provider;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Rubric data access component
	/// </summary>
	public class Rubrics : DataAccessComponent {

		public Rubrics(AuthorizedIdent ident) : base(ident) { }

		protected void Authorize(int courseID, string perm, int entityID, IProviderTransaction tran) {
			Authorize(courseID, Permission.ASSIGNMENT, perm, entityID, tran);
		}

		protected void Log(string msg, int asstID) {
			LogActivity(msg, Activity.ASSIGNMENT, asstID);
		}	

		/// <summary>
		/// Create an assignment level rubric
		/// Direct Provider layer call
		/// </summary>
		public bool Create(int asstID) {
			return m_dp.CreateRubric(asstID);
		}

		/// <summary>
		/// Return is the rubric entry is a heading
		/// </summary>
		public bool IsHeading(Rubric rub) {
			return (rub.Points < 0 || rub.ParentID == -1);
		}

		/// <summary>
		/// Create a new rubric entry
		/// </summary>
		public bool CreateItem(int parent, string name, string desc) {
			return CreateItem(parent, name, desc, -1, -1);
		}
		
		/// <summary>
		/// Return an asst rubric in the form of a list
		/// </summary>
		public Rubric.RubricList Flatten(Rubric rub) {
			Rubric.RubricList rublist = new Rubric.RubricList();
			FlattenRubric(rublist, rub);
			return rublist;
		}

		/// <summary>
		/// Sort in somewhat topological order
		/// </summary>
		private void FlattenRubric(Rubric.RubricList rublist, Rubric rub) {	
			Rubric.RubricList children = GetChildren(rub.ID);
			foreach (Rubric child in children)
				FlattenRubric(rublist, child);

			if (!IsHeading(rub))
				rublist.Add(rub);
		}

		/// <summary>
		/// Create a new rubric entry
		/// </summary>
		public bool CreateItem(int parent, string name, string desc, double points, int evalID) {

			//check permission
			Rubric rub = GetInfo(parent);
			Assignment asst = new Assignments(m_ident).GetInfo(rub.AsstID);
			Authorize(asst.CourseID, "createrubric", asst.ID, null);

			//log
			Log("Created rubric entry: " + name, asst.ID);
			return m_dp.CreateRubricEntry(name, desc, parent, points, evalID);
		}

		/// <summary>
		/// Create a canned response for a rubric
		/// </summary>
		public bool AddCannedResponse(int rubricID, string comment, double points, int type) {
			
			//check permission
			Rubric rub = GetInfo(rubricID);
			Assignment asst = new Assignments(m_ident).GetInfo(rub.AsstID);
			Authorize(asst.CourseID, "createrubric", asst.ID, null);
			
			return m_dp.CreateCannedResponse(rubricID, comment, points, type);
		}

		/// <summary>
		/// Update a rubric entry
		/// </summary>
		public bool Update(Rubric rub) {

			//check permission
			Assignment asst = new Assignments(m_ident).GetInfo(rub.AsstID);
			Authorize(asst.CourseID, "updaterubric", asst.ID, null);

			//Rename zone on entry rename
			if (rub.EvalID >= 0) {
				FileSystem fs = new FileSystem(m_ident);
				int zoneID = 
					(new Evaluations(m_ident).GetInfo(rub.EvalID) as AutoEvaluation).ZoneID;
				CFile zdir = fs.GetFile(zoneID);
				zdir.Alias = rub.Name;
				fs.UpdateFileInfo(zdir, false);
			}

			//Update entry
			m_dp.UpdateRubricEntry(rub);

			//Retally points and update root entry
			Rubric root = new Assignments(m_ident).GetRubric(rub.AsstID);
			root.Points = RetallyPoints(root);
			m_dp.UpdateRubricEntry(root);

			//Log
			Log("Updated rubric entry: " + rub.Name, rub.AsstID);

			return true;
		}

		/// <summary>
		/// Retally the points
		/// </summary>
		private double RetallyPoints(Rubric rub) {
			double points=0.0;

			if (rub.Points >= 0.0 && rub.ParentID >= 0)
				points += rub.Points;

			Rubric.RubricList chil = GetChildren(rub.ID);
			foreach (Rubric crub in chil)
				points += RetallyPoints(crub);

			return points;
		}

		/// <summary>
		/// Update a canned response
		/// </summary>
		public bool UpdateCannedResponse(CannedResponse can) {

			//check permission
			Rubric rub = GetInfo(can.RubricID);
			Assignment asst = new Assignments(m_ident).GetInfo(rub.AsstID);
			Authorize(asst.CourseID, "updaterubric", asst.ID, null);

			return m_dp.UpdateCannedResponse(can);
		}

		/// <summary>
		/// Delete a rubric entry
		/// </summary>
		public bool Delete(int rubID) {	
	
			//check permission
			Rubric rub = GetInfo(rubID);
			Assignment asst = new Assignments(m_ident).GetInfo(rub.AsstID);
			Authorize(asst.CourseID, "deleterubric", asst.ID, null);

			//Delete children
			Rubric.RubricList chil = GetChildren(rubID);
			foreach (Rubric c in chil)
				Delete(c.ID);

			//Delete any underlying evaluation
			if (rub.EvalID >= 0)
				new Evaluations(m_ident).Delete(rub.EvalID);

			//Delete entry
			m_dp.DeleteRubricEntry(rubID);

			//Retally points and update root entry
			if (rub.ParentID >= 0) {
				Rubric root = new Assignments(m_ident).GetRubric(rub.AsstID);
				root.Points = RetallyPoints(root);
				m_dp.UpdateRubricEntry(root);
			}

			//Log
			Log("Deleted rubric entry: " + rub.Name, rub.AsstID);

			return true;
		}

		/// <summary>
		/// Destroy a canned response
		/// Direct Provier layer call
		/// </summary>
		public bool RemoveCannedResponse(int canID) {
			return m_dp.DeleteCannedResponse(canID);
		}

		/// <summary>
		/// Get information about a canned response
		/// Direct Provider layer call
		/// </summary>
		public CannedResponse GetCannedInfo(int canID) {
			return m_dp.GetCannedInfo(canID);
		}

		/// <summary>
		/// Get rubric from ID
		/// Direct Provider layer call
		/// </summary>
		public Rubric GetInfo(int rubID) {
			return m_dp.GetRubricInfo(rubID);
		}

		/// <summary>
		/// Clear all results for a submission and rubric
		/// </summary>
		public bool ClearResults(int rubID, int subID) {
			Results resda = new Results(m_ident);
			Result.ResultList oldress = GetResults(rubID, subID);
			foreach (Result oldres in oldress)
				resda.Delete(oldres.ID);
			return true;
		}

		/// <summary>
		/// Get the results for a rubric entry
		/// Direct Provider layer call
		/// </summary>
		public Result.ResultList GetResults(int rubID, int subID) {
			Result.ResultList ress = m_dp.GetRubricResults(rubID, subID, Result.SUBJ_TYPE);
			ress.AddRange(m_dp.GetRubricResults(rubID, subID, Result.AUTO_TYPE));

			return ress;
		}

		/// <summary>
		/// Get all results for all submissions for a rubric ID
		/// </summary>
		public Result.ResultList GetResults(int rubID) {
			return GetResults(rubID, -1);
		}

		public void SynchronizePoints() {
			Courses courseda = new Courses(m_ident);
			Assignments asstda = new Assignments(m_ident);
			int total=0;

			Course.CourseList courses = courseda.GetAll();
			foreach (Course course in courses) {
				Assignment.AssignmentList assts = courseda.GetAssignments(course.ID);
				foreach (Assignment asst in assts) {
					Rubric arub = asstda.GetRubric(asst.ID);
					Rubric.RubricList rubs = Flatten(arub);
					Components.Submission.SubmissionList subs = asstda.GetSubmissions(asst.ID);
					foreach (Rubric rub in rubs) {
						foreach (Components.Submission sub in subs) {
							m_dp.UpdateRubricSubPoints(rub.ID, sub.ID);
							total++;
						}
					}
				}
			}

			System.Diagnostics.Trace.WriteLine(total);
		}

		/// <summary>
		/// Get earned points for a rubric
		/// Direct Provider layer call
		/// </summary>
		public double GetPoints(int rubID, int subID) {

			double points;

			//Check for an entry, if none, compute, otherwise return
			if (!m_dp.GetRubricPoints(rubID, subID, out points)) {
				Rubric trub = GetInfo(rubID);
				Rubric.RubricList rubs = Flatten(trub);
				foreach (Rubric rub in rubs)
					m_dp.UpdateRubricSubPoints(rub.ID, subID);
				m_dp.GetRubricPoints(rubID, subID, out points);
			}

			return points;
		}

		/// <summary>
		/// Get children of rubric entry
		/// Direct Provider layer call
		/// </summary>
		public Rubric.RubricList GetChildren(int rubID) {
			return m_dp.GetRubricChildren(rubID);
		}

		/// <summary>
		/// Get a list of canned responses for the rubric entry
		/// Direct Provider layer call
		/// </summary>
		public CannedResponse.CannedResponseList GetCannedResponses(int rubID) {
			return m_dp.GetRubricCannedResponses(rubID);
		}
	}
}
