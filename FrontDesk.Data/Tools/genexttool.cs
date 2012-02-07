using System;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace FrontDesk.Tools {

	/// <summary>
	/// Generic external tool handler
	/// </summary>
	public class GenericExternalTool : IExternalTool {
		
		public GenericExternalTool(string path, string version) { 
			m_toolpath = path; m_version = version;
		}

		protected string m_toolpath, m_toolargs, m_stdout="", m_stderr="", m_version;
		protected string m_stdin="";
		protected Process m_proc;
	
		public string Path {
			get { return m_toolpath; }
			set { m_toolpath = value; }
		}

		public string Arguments {
			get { return m_toolargs; }
			set { m_toolargs = value; }
		}

		public string Version {
			get { return m_version; }
		}

		public string Output {
			get { return m_stdout; }
		}

		public string Input {
			set { m_stdin = value; }
		}		

		public string ErrorOutput {
			get { return m_stderr; }
		}

		public int Execute() {
			return Execute(null, -1);
		}

		public int Execute(string wrkdir) {
			return Execute(wrkdir, -1);
		}

		public int Execute(int waittime) {
			return Execute(null, waittime);
		}

		public void ReadStdOutStream() {
			m_stdout = m_proc.StandardOutput.ReadToEnd();
		}

		public void ReadStdErrStream() {
			m_stderr = m_proc.StandardError.ReadToEnd();
		}

		public int Execute(string wrkdir, int waittime) {
			
			ProcessStartInfo si = new ProcessStartInfo();

			si.FileName = Path;
			si.Arguments = Arguments;
			si.RedirectStandardOutput = true;
			si.RedirectStandardError = true;
			si.RedirectStandardInput = (m_stdin.Length > 0);
			si.WorkingDirectory = wrkdir;
			si.UseShellExecute = false;
			si.CreateNoWindow = true;

			//Start the process
			m_proc = new Process(); 
			try {
				m_proc.StartInfo = si;
				m_proc.Start();
			} catch (Exception) {
				throw new ToolExecutionException("Tool failed to execute. Check the path");
			}

			//Start stream readers
			Thread stdout = new Thread(new ThreadStart(ReadStdOutStream));
			Thread stderr = new Thread(new ThreadStart(ReadStdErrStream));
			stdout.Start(); stderr.Start(); 

			//Fire off to stdin
			if (m_stdin.Length > 0) {
				StreamWriter stdin = m_proc.StandardInput;
				stdin.AutoFlush = true;
				stdin.Write(m_stdin + System.Environment.NewLine);
				stdin.Close();
			}

			//Kill the process
			if (waittime >= 0) {
				if (!m_proc.WaitForExit(waittime)) {
					m_proc.Kill();
					stdout.Join(); stderr.Join(); 
					throw new ToolExecutionException(
						"Tool failed to execute within time limit!");
				}
			}
			else
				m_proc.WaitForExit();

			stdout.Join(); stderr.Join(); 

			return m_proc.ExitCode;
		}
	}
}
