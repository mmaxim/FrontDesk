using System;
using System.Collections;
using System.IO;

namespace FrontDesk.Testing.Logging {

	/// <summary>
	/// Testing logger 
	/// </summary>
	
	public class LogEventArgs : EventArgs {
		
		private string m_log="";
		private DateTime m_logtime;
		private TestLogger.LogType m_type = TestLogger.LogType.INFORMATION;

		public LogEventArgs() { }
		public LogEventArgs(string log, TestLogger.LogType type) { 
			m_log = log; m_type = type;
			m_logtime = DateTime.Now;
		}

		public string Message {
			get { return m_log; }
		}

		public TestLogger.LogType Type {
			get { return m_type; }
		}

		public DateTime Time {
			get { return m_logtime; }
		}
	}

	public class TestLogger : IDisposable {

		public TestLogger(string filename, int latesize) { 
			m_logfile = new StreamWriter(filename, true);
			m_latesize = latesize;
		}

		//Types of log entries
		public enum LogType { INFORMATION, WARNING, ERROR, RESULT };

		//Log event
		public delegate void LogEventHandler(object sender, LogEventArgs args);
		public event LogEventHandler LogEnterred; 

		//All log entries
		protected ArrayList m_latest = new ArrayList();
		protected StreamWriter m_logfile;
		protected int m_latesize;

		public void Dispose() {
			m_logfile.Close();
		}

		public bool Log(string strlog) {
			return Log(strlog, LogType.INFORMATION);
		}

		public bool Log(string strlog, LogType type) {
			
			LogEventArgs logent = new LogEventArgs(strlog, type);
			
			//Add to list and fire event
			m_latest.Add(logent);

			string logline = String.Format("[{0}] TYPE: {1} MESSAGE: {2}",
				logent.Time, logent.Type, logent.Message);
			m_logfile.WriteLine(logline);
			m_logfile.Flush();
			
			LogEnterred(this, logent);
			return true;
		}

		private void AddToLatest(LogEventArgs logent) {
			
			//Remove from latest at top
			if (m_latest.Count > m_latesize)
				m_latest.RemoveAt(m_latest.Count-1);

			m_latest.Insert(0, logent);
		}
	}
}
