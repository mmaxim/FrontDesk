using System;
using System.Collections;

using FrontDesk.Data.Filesys;
using FrontDesk.Components;
using FrontDesk.Components.Filesys;
using FrontDesk.Components.Evaluation;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Evaluation data access component
	/// </summary>
	public class Evaluations : DataAccessComponent {

		public Evaluations(AuthorizedIdent ident) : base(ident) { }

		/// <summary>
		/// Compare two AutoResult objects that contain valid competitive scores
		/// </summary>
		class CompResultComparer : IComparer {

			public int Compare(object x, object y) {	
				AutoResult ax = (AutoResult) x;
				AutoResult ay = (AutoResult) y;

				if (ax.CompetitionScore > ay.CompetitionScore)
					return -1;
				else if (ax.CompetitionScore == ay.CompetitionScore)
					return 0;
				else
					return 1;
			}
		}

		/// <summary>
		/// Dependency graph class
		/// </summary>
		public class DependencyGraph {

			//Node class
			protected class Node {

				private Evaluation m_eval;
				private bool m_marked=false;

				public Node(Evaluation eval, bool marked) { 
					m_eval = eval; m_marked = marked; 
				}

				public override bool Equals(object obj) {
					return (m_eval.ID == (obj as Node).Eval.ID);
				}

				public override int GetHashCode() {
					return m_eval.ID;
				}

				public Evaluation Eval {
					get { return m_eval; }
					set { m_eval = value; }
				}

				public bool Marked {
					get { return m_marked; }
					set { m_marked = value; }
				}
			}

			public DependencyGraph(Evaluation eval, AuthorizedIdent ident) {
				m_eval = eval; m_ident = ident;
			}		

			private Evaluation m_eval;
			private AuthorizedIdent m_ident;

			public Evaluation Eval {
				get { return m_eval; }
			}

			public Evaluation.EvaluationList GetBuildOrder() {
		
				//Build graph
				Hashtable graph = new Hashtable();
				Node root = BuildGraph(new Evaluations(m_ident), m_eval, graph);

				//Order
				ArrayList sort = new ArrayList();
				SortGraph(root, graph, sort);

				//Convert
				int i;
				Evaluation.EvaluationList border = new Evaluation.EvaluationList();
				for (i = sort.Count-1; i > 0; i--)
					border.Add(((Node)sort[i]).Eval);
			
				return border;
			}

			private void SortGraph(Node root, Hashtable graph, ArrayList sort) {

				//Return on marked node
				if (root.Marked) return;

				//Add root
				sort.Add(root);
				root.Marked = true;

				//Run on adjacents
				ArrayList adjs = (ArrayList) graph[root];
				foreach (Node adj in adjs) 
					SortGraph(adj, graph, sort);	
			}

			private Node BuildGraph(Evaluations evals, Evaluation eval, Hashtable adjlists) {

				//Create node in graph if not there
				Node enode = new Node(eval, false);
				if (!adjlists.ContainsKey(enode)) 
					adjlists.Add(enode, new ArrayList());

				//Create edges
				Evaluation.EvaluationList deps = evals.GetDependencies(eval.ID);
				foreach (Evaluation dep in deps) {

					//Get node in graph
					Node dnode = BuildGraph(evals, dep, adjlists);

					//Create edge
					((ArrayList)adjlists[enode]).Add(dnode);
				}

				return enode;
			}

		}

		/// <summary>
		/// Get extended information about an automatic test
		/// Direct Provider layer call
		/// </summary>
		public Evaluation GetInfo(int evalID) {
			return m_dp.GetEvalInfo(evalID);
		}

		/// <summary>
		/// Get rubric entry for this evaluation
		/// Direct Provider layer call
		/// </summary>
		public Rubric GetRubric(int evalID) {
			return m_dp.GetRubricInfoFromEval(evalID);
		}

		/// <summary>
		/// Get info about a eval from its zone ID
		/// Direct Provider layer call
		/// </summary>
		public AutoEvaluation GetAutoInfoByZone(int zoneID) {
			return m_dp.GetAutoEvalInfoByZone(zoneID);
		}

		/// <summary>
		/// Get the results of a competitive evaluation
		/// </summary>
		public Result.ResultList GetCompetitionResults(int evalID, out Hashtable subhash) {

			subhash = new Hashtable();
			Evaluation eval = GetInfo(evalID);
			int rubID = GetRubric(evalID).ID;

			//Get all results for the evaluation
			Result.ResultList ress = new Rubrics(m_ident).GetResults(rubID);

			//Get all subs for the assignment
			Components.Submission.SubmissionList subs = new Assignments(m_ident).GetSubmissions(eval.AsstID);
			
			//Load unique subs into hash table
			Principals prinda = new Principals(m_ident);
			foreach (Components.Submission sub in subs)
				if (!subhash.Contains(sub.PrincipalID) &&
					prinda.GetLatestSubmission(sub.PrincipalID, eval.AsstID).ID == sub.ID) {
					subhash[sub.PrincipalID] = sub;
				}
			Components.Submission[] usubs = new Components.Submission[subhash.Count];
			subhash.Values.CopyTo(usubs, 0);
			subhash.Clear();
			foreach (Components.Submission sub in usubs)
				subhash[sub.ID] = sub;

			//Run through results and delete any repetitive ones
			Result.ResultList fress = new Result.ResultList();
			foreach (Result res in ress) 
				if (((AutoResult)res).Success != AutoResult.CRITICALLYFLAWED &&
					((AutoResult)res).Success != AutoResult.DEPFAIL &&
					subhash.Contains(res.SubmissionID))
					fress.Add(res);
	
			//Sort by competitive score
			fress.Sort(new CompResultComparer());

			return fress;
		}

		/// <summary>
		/// Create the evaluation
		/// Inserts the test files into the file system
		/// Create the record for the auto evaluation
		/// </summary>
		public bool CreateAuto(AutoEvaluation eval, IExternalSource zone) {

			//Create the record for the val
			m_dp.CreateAutoEvaluation(eval);
			eval.CourseID = new Assignments(m_ident).GetInfo(eval.AsstID).CourseID;

			return UpdateAuto(eval, zone);
		}	

		/// <summary>
		/// Create an evaluation dependency
		/// </summary>
		public bool CreateDependency(int evalID, int depID) {

			//Check for self-deps
			if (!CheckDepLoop(evalID, depID))
				return false;

			return m_dp.CreateEvalDependency(evalID, depID);
		}

		/// <summary>
		/// Delete a dependency
		/// Direct Provider layer call
		/// </summary>
		public bool DeleteDependency(int evalID, int depID) {
			return m_dp.DeleteEvalDependency(evalID, depID);
		}

		private bool CheckDepLoop(int oevalID, int depID) {
			
			Evaluation.EvaluationList deps = GetDependencies(depID);
			foreach (Evaluation dep in deps) {
				if (dep.ID == oevalID)
					return false;
				else if (!CheckDepLoop(oevalID, dep.ID)) 
					return false;
			}
			return true;
		}

		/// <summary>
		/// Return the dependencies for a given evaluation
		/// Direct Provider layer call
		/// </summary>
		public Evaluation.EvaluationList GetDependencies(int evalID) {
			return m_dp.GetEvalDependencies(evalID);
		}

		/// <summary>
		/// Check to see if an evaluation depends on another
		/// </summary>
		public bool DependsOn(int evalID, int depID) {
			Evaluation.EvaluationList evals = GetDependencies(evalID);
			foreach (Evaluation eval in evals)
				if (eval.ID == depID)
					return true;
			return false;
		}

		/// <summary>
		/// Update an automatic evaluation
		/// </summary>
		public bool UpdateAuto(AutoEvaluation eval, IExternalSource esrc) {

			//Check version
			if (!ValidateVersion(eval.ToolVersion))
				throw new DataAccessException("Illegal version number");

			eval.ZoneID = CommitTestSource(eval, esrc);

			return m_dp.UpdateAutoEvaluation(eval);
		}

		private bool ValidateVersion(string version) {
			if (version == null || version.Length == 0) return true;
			foreach (char c in version) {
				if ((c < '0' || c > '9') && c != '.')
					return false;
			}
			return true;
		}

		private int CommitTestSource(AutoEvaluation eval, IExternalSource zone) {
			
			FileSystem fs = new FileSystem(m_ident);

			//Make sure toplevel zone directory exists
			CFile zonedir = fs.GetFile(@"c:\zones");
			if (null == zonedir)
				zonedir = fs.CreateDirectory(@"c:\zones", true, null);

			//Build file perms
			CFilePermission.FilePermissionList perms = new CFilePermission.FilePermissionList();
			CourseRole.CourseRoleList staff = new Courses(m_ident).GetTypedRoles(eval.CourseID, true, null);
			foreach (CourseRole role in staff)
				perms.AddRange(CFilePermission.CreateFullAccess(role.PrincipalID));

			//Create zone directory
			string zpath = @"c:\zones\" + eval.ID;
			CFile ezonedir;
			if (null == (ezonedir = fs.GetFile(zpath))) {
				ezonedir = fs.CreateDirectory(zpath, false, perms);
				ezonedir.Alias = eval.Name; ezonedir.SpecType = CFile.SpecialType.TEST;
				fs.UpdateFileInfo(ezonedir, false);
			}
			fs.ImportData(zpath, zone, false, true); //Import the data

			return ezonedir.ID;
		}

		/// <summary>
		/// Remove an auto evaluation (takes results with it)
		/// </summary>
		public bool Delete(int evalID) {
			
			Evaluation eval = GetInfo(evalID);

			//take evaluation
			m_dp.DeleteEval(evalID);

			//Delete zone files
			if (eval.Type == Evaluation.AUTO_TYPE) { 
				FileSystem fs = new FileSystem(m_ident);
				AutoEvaluation aeval = eval as AutoEvaluation;
				CFile zdir = fs.GetFile(aeval.ZoneID);
				if (null != zdir)
					fs.DeleteFile(zdir);
			}

			return true;
		}

	}
}
