using System;
using System.Threading;
using System.Net;
using System.Diagnostics;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.IO;
using System.Data;
using System.Collections;

using FrontDesk.Common;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Testing.Logging;
using FrontDesk.Testing.Zones;
using FrontDesk.Testing.Authentication;
using FrontDesk.Tools;
using FrontDesk.Security;
using FrontDesk.Services;

namespace FrontDesk.Testing {
	
	public delegate void StartJobEventHandler(object sender, EventArgs args);
	public delegate void EndJobEventHandler(object sender, EventArgs args);

	/// <summary>
	/// Main handler for finding tests to run
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class TestCenter : Component {

		private static TestCenter m_instance=null;
		private int m_proced=0;
		private string m_ipaddress="127.0.0.1", m_desc="A testing center";
		private Thread m_worker;
		private bool m_shutdown;
		private AuthorizedIdent m_ident = AuthorizedIdent.NoOne;
		private Status m_status = Status.IDLE;
		private static TestLogger m_logger =
			new TestLogger("testlog.txt", 20);

		public enum Status { IDLE, QUEUED, RUNNING };

		protected TestCenter() { 
			m_logger.Log("Testing Center starting up...");
			m_ipaddress = Globals.CurrentIP;
		}

		public static TestCenter GetInstance() {
			if (m_instance == null)
				return (m_instance = new TestCenter());
			else
				return m_instance;
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				//if (m_ident.Name != "__noone")
				//	FileSystem.GetInstance(m_ident, false).Dispose();
			}
			base.Dispose (disposing);
		}

		public static TestLogger Logger {
			get { return m_logger; }
		}

		public Status CenterStatus {
			get { return m_status; }
		}

		public string IPAddress {
			get { return m_ipaddress; }
			set { m_ipaddress = value; }
		}

		public string Description {
			get { return m_desc; }
			set { m_desc = value; }
		}

		public AuthorizedIdent Identity {
			get { return m_ident; }
		}

		public bool Authenticate(string username, string password) {

			//Run auth through the auth svc to get an identity
			AuthenticateService authsvc = new AuthenticateService();
			try {
				authsvc.Url = String.Format("http://{0}/FrontDeskServices/authsvc.asmx", 
					TestConfig.AuthenticationAddress);
				Authentication.ServiceTicket tik = authsvc.Authenticate(username, password);
				FrontDesk.Services.ServiceTicket ticket = new FrontDesk.Services.ServiceTicket();
				ticket.HostAddress = tik.HostAddress;
				ticket.Username = tik.Username;
				ticket.Ident = tik.Ident;
				m_ident = AuthorizedIdent.Create(ticket);
				if (m_ident != null) {
					m_logger.Log("Authentication for: " + username + 
						" succeeded.");
					return true;
				}
				else {
					m_logger.Log("Authentication for: " + username + 
						" failed.", TestLogger.LogType.ERROR);
					return false;
				}
			} catch (Exception er) {
				m_logger.Log("Error during authentication: MESSAGE: " + er.Message, 
					TestLogger.LogType.ERROR);
				return false;
			}
		}

		/// <summary>
		/// Start harvesting tests
		/// </summary>
		public void StartTestWorker() {
			m_worker = new Thread(new ThreadStart(TestWorker));
			m_shutdown = false;
			m_status = Status.QUEUED;
			m_logger.Log("Worker thread started. Looking for jobs...");
			m_worker.Start();
		}

		public void ShutdownTestWorker(bool wait) {
			if (m_status != Status.IDLE) {
				m_shutdown = true;
				if (wait)
					m_worker.Join(TimeSpan.FromSeconds(30));
				m_worker.Abort();
			}
			m_logger.Log("Worker thread shutdown");
			m_status = Status.IDLE;
		}

		/// <summary>
		/// Main worker thread for the testing center
		/// </summary>
		public void TestWorker() {

			AutoJobTest job;
			bool bsuc;
			Evaluations evals = new Evaluations(m_ident);
			AutoJobs jobs = new AutoJobs(m_ident);
			ZoneService testsvc = new ZoneService("auto", m_ident, m_logger);
			ZoneService stusvc = new ZoneService("stu", m_ident, m_logger);
			Submissions subs = new Submissions(m_ident);

			while (!m_shutdown) {
		
				//Get job
				try {
					job = jobs.Claim(m_ipaddress, m_desc);
					if (job != null) {

						m_status = Status.RUNNING;

						Submission sub = subs.GetInfo(job.SubmissionID);
						m_logger.Log(String.Format("Claimed job: JOB: {0} EVAL: {1} SUB: {2}",
							job.JobName, job.AutoEval.Name, new Principals(m_ident).GetInfo(sub.PrincipalID).Name));
				
						m_logger.Log("Synchronizing eval and student zones");
						//Sync test zone
						Zone tzone = testsvc.Synchronize(job.AutoEval);

						//Sync stu zone
						Zone szone = stusvc.Synchronize(sub);

						//Copy stu zone into test zone
						testsvc.CopyZone(tzone, szone);
					
						//Create dep graph and run deps
						m_logger.Log("Beginning dependency running");
						Evaluations.DependencyGraph dg = 
							new Evaluations.DependencyGraph(job.AutoEval, m_ident);
						string faildep, xmloutput="";
						if (null != (faildep = RunDependencies(tzone, testsvc, dg))) {
							xmloutput = FormErrorXml(AutoResult.DEPFAIL, 
								"Test unable to run, dependency: " +
								faildep + " failed to complete successfully!", 
								job.AutoEval.Points);
							m_logger.Log("Dependency fail (" + faildep + "), not running main test", 
								TestLogger.LogType.WARNING);
						} else {
							//Run test and gather result
							m_logger.Log("Starting run of test");
							if (job.AutoEval.IsBuild)
								xmloutput = RunBuildTest(tzone, job.AutoEval, out bsuc);
							else
								xmloutput = RunTest(tzone, job.AutoEval);
						}

						//Post result
						xmloutput = Globals.PurifyZeroes(xmloutput);
						if (!PostResult(job, xmloutput))
							m_logger.Log("Error logging result", TestLogger.LogType.ERROR);
						else
							m_logger.Log("Test completed, result stored");

						//Clear the job out
						jobs.FinishTest(job);
					}
				} catch (Exception er) {
					m_logger.Log("Unexpected and fatal error during testing: MESSAGE: " + er.Message, TestLogger.LogType.ERROR);
				}
				
				m_status = Status.QUEUED;
				Thread.Sleep(TimeSpan.FromSeconds(5));
			}
		}

		protected bool PostResult(AutoJobTest job, string xmloutput) {
			Results resda = new Results(m_ident);		
			if (job.AutoEval.ResultType == Result.AUTO_TYPE) {
				new Activities(m_ident).Create(job.JobCreator, Activity.SUBMISSION, job.SubmissionID,
					"Result posted for evaluation: " + job.AutoEval.Name);
				if (!job.OnSubmit) {			
					return resda.CreateAuto(
						job.AutoEval.ID, job.JobCreator, job.SubmissionID, xmloutput);
				} 
				else {
					Components.Submission sub = new Submissions(m_ident).GetInfo(job.SubmissionID);
					new EmailWizard(m_ident).SendByPrincipal(sub.PrincipalID, 
						"FrontDesk Submission Results: " + job.AutoEval.Name, 
						ConvertXmlToText(xmloutput, job.AutoEval.CourseID, job.AutoEval.AsstID));
					m_logger.Log("Result emailed to submitter");
					if (job.AutoEval.Competitive) {
						m_logger.Log("Competitive pre-test result stored");
						return resda.CreateAuto(
							job.AutoEval.ID, job.JobCreator, job.SubmissionID, xmloutput);
					}
					else
						return true;
				}
			} else {
				SubjResult.SubjResultList ress = 
					ParseSubjXmlResults(xmloutput, new Submissions(m_ident).GetInfo(job.SubmissionID));
				Rubric rub = new Evaluations(m_ident).GetRubric(job.AutoEval.ID);
				new Rubrics(m_ident).ClearResults(rub.ID, job.SubmissionID);
				foreach (SubjResult res in ress) 
					resda.CreateSubj(job.SubmissionID, rub.ID, res.Comment, 
						res.FileID, res.Line, res.Points, new ArrayList(), res.SubjType);
				
				return true;
			}
		}

		protected SubjResult.SubjResultList ParseSubjXmlResults(string xmlresults, Submission sub) {
			SubjResult.SubjResultList ress = new SubjResult.SubjResultList();
			XPathNavigator xnav = new XmlWizard().GetXPathNavigator(xmlresults);
			FileSystem fs = new FileSystem(m_ident);
			CFile zone = fs.GetFile(sub.LocationID);

			xnav.MoveToFirstChild(); xnav.MoveToFirstChild();
			XPathNavigator comments = xnav.Clone();
			while (comments.MoveToNext()) {
				SubjResult res = new SubjResult();
				XPathNavigator comment = comments.Clone();
				comment.MoveToFirstChild();
				
				string subjtype = comment.Value; comment.MoveToNext();
				switch (subjtype) {
				case "Warning":
					res.SubjType = Rubric.WARNING;
					break;
				case "Error":
					res.SubjType = Rubric.ERROR;
					break;
				case "Good":
					res.SubjType = Rubric.GOOD;
					break;
				};

				res.Points = Convert.ToDouble(comment.Value); comment.MoveToNext();
				res.Comment = comment.Value; comment.MoveToNext();

				string filename = comment.Value; comment.MoveToNext();
				if (filename.StartsWith(@".\"))
					filename = filename.Substring(2, filename.Length-2);
				CFile file = fs.GetFile(Path.Combine(zone.FullPath, filename));
				if (file != null) {
					res.FileID = file.ID;
					res.Line = Convert.ToInt32(comment.Value);
					ress.Add(res);
				}
			}

			return ress;
		}

		protected string ConvertXmlToText(string xmloutput, int courseID, int asstID) {
			
			DataSet dsres = new DataSet();
			string textoutput="This is an auto-generated message, please do not reply.\n\n";

			//Read Xml
			try {
				MemoryStream memstream = 
					new MemoryStream(System.Text.Encoding.ASCII.GetBytes(xmloutput));
				dsres.ReadXml(memstream);
			} catch (Exception) { return xmloutput; }
			
			DataTable restbl = dsres.Tables["Result"];
			DataTable errortbl = dsres.Tables["Error"];
			DataTable failtbl = dsres.Tables["Failure"];

			//Course and assignment
			Course course = new Courses(m_ident).GetInfo(courseID);
			Assignment asst = new Assignments(m_ident).GetInfo(asstID);
			textoutput += "Course: " + course.Name + "\n";
			textoutput += "Assignment: " + asst.Description + "\n\n";

			//Get success or failure
			string sucstr = (string) restbl.Rows[0]["Success"];
			if (sucstr == "criticallyflawed" || sucstr ==  "flawed")
				textoutput += "There were errors with this evaluation!";
			else if (sucstr == "depfail")
				textoutput += "Dependency failed. Evaluation not run.";
			else if (sucstr == "flawless")
				textoutput += "Evaluation successful!";
			textoutput += "\n\n";

			//Get failures
			if (failtbl != null)
				foreach (DataRow row in failtbl.Rows) {
					textoutput += "Failure: " + row["Name"] + "\n";
					textoutput += "\tDetails: " + row["Message"] + "\n\n";
				}

			//Add up error points
			if (errortbl != null)
				foreach (DataRow row in errortbl.Rows) {
					textoutput += "Error: " + row["Name"] + "\n";
					textoutput += "\tDetails: " + row["Message"] + "\n\n";
				}
			
			textoutput += "Messages: " + restbl.Rows[0]["Msg"] + "\n";
			textoutput += "Time: " + restbl.Rows[0]["Time"] + "s\n";
			textoutput += "Test Count: " + restbl.Rows[0]["Count"] + "\n";
			textoutput += "API: " + restbl.Rows[0]["API"] + "\n\n";

			textoutput += "Please contact the course staff if you feel there is a problem " +
				"with your results.";

			return textoutput;
		}
		
		protected string RunDependencies(Zone tzone, ZoneService testsvc, 
									   Evaluations.DependencyGraph dg) {

			Evaluation.EvaluationList border = dg.GetBuildOrder();
			bool suc;

			//Copy zones first
			foreach (AutoEvaluation eval in border) {
				Zone ezone = testsvc.Synchronize(eval);
				testsvc.CopyZone(tzone, ezone);
			}

			//Run the deps
			foreach (AutoEvaluation eval in border) {	

				m_logger.Log("Running Dep: " + eval.Name);
				if (eval.IsBuild)
					RunBuildTest(tzone, eval, out suc);
				else {
					string xmlout = RunTest(tzone, eval);
					if (eval.ResultType == Result.AUTO_TYPE) {
						AutoResult res = new AutoResult();
						res.XmlResult = xmlout;

						suc = (res.Success != AutoResult.CRITICALLYFLAWED);
					} else
						suc = true;
				}

				if (!suc)
					return eval.Name;
			}

			return null;
		}

		protected string RunBuildTest(Zone zone, AutoEvaluation job, out bool suc) {

			int retval=-1;
			ExternalToolFactory extfact = ExternalToolFactory.GetInstance();

			IExternalTool exttool = extfact.CreateExternalTool(job.RunTool);
			exttool.Arguments = job.RunToolArgs;

			DateTime begin = DateTime.Now;
			try {
				retval = exttool.Execute(zone.LocalPath, job.TimeLimit*1000);
			} catch (ToolExecutionException er) {
				suc = false;
				m_logger.Log("Error: " + er.Message, TestLogger.LogType.ERROR);
				return FormErrorXml(AutoResult.CRITICALLYFLAWED, er.Message, job.Points);
			} catch (Exception) {
				suc = false;
				string ermsg = "Unexpected tool execution error. Contact administrator";
				m_logger.Log("Error: " + ermsg, TestLogger.LogType.ERROR);
				return FormErrorXml(AutoResult.CRITICALLYFLAWED, ermsg, job.Points);
			}

			DateTime end = DateTime.Now;

			int time = Convert.ToInt32(end.Subtract(begin).TotalSeconds);

			suc = (retval == 0);
			return FormBuildXml(suc, time,
				String.Format("{0}\n\n{1}", exttool.Output, exttool.ErrorOutput),
				job.Points);
		}

		protected string RunTest(Zone zone, AutoEvaluation job) {

			ExternalToolFactory extfact = ExternalToolFactory.GetInstance();

			//Get interface to external running tool
			IExternalTool exttool = extfact.CreateExternalTool(job.RunTool);
			exttool.Arguments = job.RunToolArgs;

			//Execute the test with the tool
			try {
				if (exttool.Execute(zone.LocalPath, job.TimeLimit*1000) != 0)
					return FormErrorXml(AutoResult.CRITICALLYFLAWED, 
						exttool.ErrorOutput, job.Points);
				else
					return ValidateResult(exttool.Output);
			} catch (ToolExecutionException er) {
				m_logger.Log("Error: " + er.Message, TestLogger.LogType.ERROR);
				return FormErrorXml(AutoResult.CRITICALLYFLAWED, er.Message, job.Points);
			} catch (ResultFormatException er) {
				m_logger.Log("Error: " + er.Message, TestLogger.LogType.ERROR);
				return FormErrorXml(AutoResult.CRITICALLYFLAWED, er.Message, job.Points);
			} catch (Exception) {
				string ermsg = "Unexpected tool execution error. Contact administrator";
				m_logger.Log("Error: " + ermsg, TestLogger.LogType.ERROR);
				return FormErrorXml(AutoResult.CRITICALLYFLAWED, ermsg, job.Points);
			}	
		}

		protected string ValidateResult(string result) {

			XmlWizard xmlwiz = new XmlWizard();

			if (xmlwiz.ValidateXml(result, "result.xsd") ||
				xmlwiz.ValidateXml(result, "subjresult.xsd"))
				return result;
			else
				throw new ResultFormatException("Xml result description does not conform to the FrontDesk XSD specification.\n" +
						" Please use a FrontDesk toolkit when designing tests, or debug the current test " +
					    "to conform with FrontDesk specifications. \nError: " + xmlwiz.GetLastError() + 
						"\n\n<![CDATA[ XML: " + result + "]]>");
		}

		protected string FormErrorXml(int flaw, string msg, double epoints) {
			string xmlerror = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n";

			xmlerror += "<Result xmlns=\"urn:frontdesk-result\">\n";
			if (flaw == AutoResult.CRITICALLYFLAWED)
				xmlerror += "\t<Success>criticallyflawed</Success>\n";
			else if (flaw == AutoResult.DEPFAIL)
				xmlerror += "\t<Success>depfail</Success>\n";
			else if (flaw == AutoResult.FLAWED)
				xmlerror += "\t<Success>flawed</Success>\n";

			xmlerror += "\t<Time>??</Time>\n";
			xmlerror += "\t<Count>??</Count>\n";
			xmlerror += "\t<API>FrontDesk Test Fail Recovery</API>\n";

			xmlerror += "\t<Error>\n";
			xmlerror += "\t\t<Name>Execution Error</Name>\n";
			xmlerror += "\t\t<Points>" + epoints + "</Points>\n";
			xmlerror += "\t\t<Message>" + msg + "</Message>\n";
			xmlerror += "\t</Error>\n";

			xmlerror += "\t<Msg>" + msg + "</Msg>\n";

			xmlerror += "</Result>\n";

			return xmlerror;
		}

		protected string FormBuildXml(bool suc, int time, string msg, double epoints) {
			string xmlerror = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n";

			xmlerror += "<Result xmlns=\"urn:frontdesk-result\">\n";
			if (suc)
				xmlerror += "\t<Success>flawless</Success>\n";
			else
				xmlerror += "\t<Success>criticallyflawed</Success>\n";

			xmlerror += "\t<Time>" + time + "</Time>\n";
			xmlerror += "\t<Count>1</Count>\n";
			xmlerror += "\t<API>FrontDesk Build System</API>\n";

			string emsg = msg;
			if (msg.Length > 300) 
				emsg = msg.Substring(0, 300) + " ... (truncated)";

			if (!suc) {
				xmlerror += "\t<Error>\n";
				xmlerror += "\t\t<Name>Compile Error</Name>\n";
				xmlerror += "\t\t<Points>" + epoints + "</Points>\n";
				xmlerror += "\t\t<Message>" + emsg + "</Message>\n";
				xmlerror += "\t</Error>\n";
			}

			xmlerror += "\t<Msg>" + emsg + "</Msg>\n";
			xmlerror += "</Result>\n";

			return xmlerror;
		}
	}

	public class ResultFormatException : Exception {
		public ResultFormatException() : base("Tester result format incorrect!") { }
		public ResultFormatException(string msg) : base(msg) { }
	}
}
