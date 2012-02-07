using System;
using System.IO;
using System.Collections;

using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Common;
using FrontDesk.Components.Evaluation;
using FrontDesk.Components.Filesys;

namespace FrontDesk.Tools {

	public class JUnitToolException : CustomException {
		public JUnitToolException() : base("Error running JUnit tool") { }
		public JUnitToolException(string msg) : base(msg) { }
	}

	/// <summary>
	/// JUnit Tool
	/// </summary>
	public class JUnitTool {

		public JUnitTool() { }

		public string Discover(AutoEvaluation eval, out double points, out int time, out int count) {

			//Get Perl
			IExternalTool perl = ExternalToolFactory.GetInstance().CreateExternalTool("Perl",
				"5.0", ExternalToolFactory.VersionCompare.ATLEAST);
			if (perl == null) 
				throw new JUnitToolException(
					"Unable to find Perl v5.0 or later. Please check the installation or contact the administrator");
	
			//Get all files on the disk
			string tpath = ExportToTemp(eval);

			//Run disco program
			perl.Arguments = "jdisco.pl i";
			perl.Execute(tpath);
			Directory.Delete(tpath, true);

			//Validate XML
			string xmltests = perl.Output;
			XmlWizard xmlwiz = new XmlWizard();
			if (!xmlwiz.ValidateXml(xmltests, Path.Combine(Globals.WWWDirectory, "Xml/testsuite.xsd")))
				throw new JUnitToolException("Invalid JUnit Test Suite. Check to make sure the test suite conforms to FrontDesk standards");
	
			//Write XML
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			CFile zone = fs.GetFile(eval.ZoneID);
			string tspath = Path.Combine(zone.FullPath, "__testsuite.xml");
			CFile xmldesc = fs.GetFile(tspath);
			if (xmldesc == null)
				xmldesc = fs.CreateFile(tspath, false, null);
			xmldesc.Data = xmltests.ToCharArray();
			fs.Edit(xmldesc);
			fs.Save(xmldesc);

			//Copy disco program over
			CFile.FileList dfiles = new CFile.FileList();
			dfiles.Add(fs.GetFile(@"c:\system\junit\jdisco.pl"));
			dfiles.Add(fs.GetFile(@"c:\system\junit\JUnitDiscover.class"));
			dfiles.Add(fs.GetFile(@"c:\system\junit\JUnitDiscover$ClassFileFilter.class"));
			fs.CopyFiles(zone, dfiles, true);

			//Get suite metadata
			GetSuiteInfo(xmltests, out points, out time, out count);

			//Punt all previous results
			RemoveResults(eval);
			
			return xmltests;
		}

		private void RemoveResults(AutoEvaluation eval) {
			Rubric rub = new Evaluations(Globals.CurrentIdentity).GetRubric(eval.ID);
			Result.ResultList ress = new Rubrics(Globals.CurrentIdentity).GetResults(rub.ID);
			Results resda = new Results(Globals.CurrentIdentity);
			foreach (Result res in ress)
				resda.Delete(res.ID);
		}

		public string ReDiscover(AutoEvaluation eval, out double points, out int time, out int count) {
	
			string xmltests;
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);

			CFile zone = fs.GetFile(eval.ZoneID);
			CFile xmldesc = fs.GetFile(Path.Combine(zone.FullPath, "__testsuite.xml"));
			if (xmldesc == null)
				throw new JUnitToolException("No proper JUnit Test Suite uploaded");

			fs.LoadFileData(xmldesc);
			xmltests = new string(xmldesc.Data);
		
			GetSuiteInfo(xmltests, out points, out time, out count);
			
			return xmltests;
		}

		private void GetSuiteInfo(string xmltests, out double points, out int time, out int count) {
			ArrayList tests = new XmlWizard().GetXSDTests(xmltests);
			points = 0; time = 5; count = tests.Count;
			foreach (XmlWizard.XSDTest test in tests) {
				points += test.Points; time += test.Time;
			}
		}

		private string ExportToTemp(AutoEvaluation eval) {

			FileSystem fs = new FileSystem(Globals.CurrentIdentity);

			string tpath = Path.Combine(Globals.TempDirectory, Globals.CurrentUserName);
			try { Directory.Delete(tpath); } catch (Exception) { }
			Directory.CreateDirectory(tpath);

			IExternalSink tsink = new OSFileSystemSink();
			tsink.CreateSink("");

			//Export eval files (JUnit test suite)
			try {
				fs.ExportData(tpath, fs.GetFile(eval.ZoneID), tsink, false);
			} catch (Exception er) {
				string mike = er.Message;
			}

			//Export jdisco program
			try {
				fs.ExportData(tpath, fs.GetFile(@"c:\system\junit"), tsink, false);
			} catch (Exception er) {
				string mike = er.Message;
			}
		
			return tpath;
		}	

	}
}
