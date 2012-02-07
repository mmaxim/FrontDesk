using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

using FrontDesk.Pages;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Components;
using FrontDesk.Components.Filesys;
using FrontDesk.Tools;
using FrontDesk.Common;

namespace FrontDesk.Controls.Submission{

	/// <summary>
	///	CVS submission system
	/// </summary>
	public class CVSSubmissionControl : SubmissionSystemControl, ISubmissionSystemControl {
		protected System.Web.UI.WebControls.TextBox txtModule;
		protected System.Web.UI.WebControls.TextBox txtServer;
		protected System.Web.UI.WebControls.TextBox txtRepository;
		protected System.Web.UI.WebControls.TextBox txtUsername;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected CustomButton.ClickOnceButton cmdSubmit;

		private void Page_Load(object sender, System.EventArgs e) {
			if (cmdSubmit.Enabled)
				lblError.Visible = false;
		}

		public void FinishInit() {
			cmdSubmit.Enabled = new Assignments(Globals.CurrentIdentity).IsSubmissionAvailable(AssignmentID);
			if (!cmdSubmit.Enabled)
				PageError("This assignment is not available for student submissions, therefore you are unable to submit " +
					"at this time. Please check back later when course staff has enabled student submissions.");
		}

		public string Name {
			get { return "CVS Submission System"; }
		}

		private bool Validate() {
			return (txtModule.Text.Length > 0 &&
					txtServer.Text.Length > 0 &&
					txtRepository.Text.Length > 0 &&
					txtUsername.Text.Length > 0);
		}

		private string FormCVSRoot(string server, string username, string repos) {
			return String.Format(":pserver:{0}@{1}:{2}", username, server, repos);
		}

		private void cmdSubmit_Click(object sender, System.EventArgs e) {
			lblError.Visible = false;

			//Sanity check
			if (!Validate()) {
				PageError("Missing a field, please try again.");
				return;
			}

			//Set up the submission process
			string cvsroot = FormCVSRoot(txtServer.Text, txtUsername.Text, txtRepository.Text);
			SubmitCommand scmd = new SubmitCommand(cvsroot, txtModule.Text, txtPassword.Text, 
				AssignmentID, SubmitterPrincipalID, new Submissions(Globals.CurrentIdentity));

			//Load global context for threads
			Globals.Context = HttpContext.Current;

			//Do the submission
			ViewState["submitting"] = true;
		//	((MasterPage)Page).ExecuteLongTask(scmd);
			scmd.Finished += new ICommandEventHandler(scmd_Finished);
			scmd.Execute();
		}

		protected class SubmitCommand : ICommand {
			
			private string m_cvsroot, m_module, m_target, m_password;
			private int m_asstID, m_prinID;
			private Submissions m_subda;

			public SubmitCommand(string cvsroot, string module, string password, 
				int asstID, int prinID, Submissions subda) {
				m_cvsroot=cvsroot; m_module=module; 
				m_password = password;
				m_asstID=asstID; m_prinID=prinID;
				m_subda = subda;
			}

			public void Execute() {

				IExternalSource extsrc;
				ICommandEventArgs args = new ICommandEventArgs();

				//Run CVS to get the files
				CVSTool cvs = new CVSTool();
				try {
					extsrc = cvs.Checkout(m_cvsroot, m_module, m_password, out m_target);
				} catch (Exception er) {
					args.Exception = er;
					Finish(args); return;
				}

				//Sanity check on CVS
				if (extsrc == null) {
					args.Exception = new ToolExecutionException("Failure during execution of CVS");
					Finish(args); return;
				}

				//Create the submission
				Submissions subda = new Submissions(Globals.CurrentIdentity);
				Components.Submission sub = null;
				try {
					sub = m_subda.Create(m_asstID, m_prinID, extsrc);
				} catch (Exception er) {
					args.Exception = er;
					Finish(args); return;
				}

				//Finish up
				args.ReturnValue = sub;
				Finish(args);				
			}

			private void Finish(ICommandEventArgs args) {
				Directory.Delete(m_target, true);
				if (Finished != null)
					Finished(this, args);
			}

			public event ICommandEventHandler Finished;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmdSubmit.Click += new System.EventHandler(this.cmdSubmit_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void scmd_Finished(object sender, ICommandEventArgs args) {
			if (args.Exception != null)
				PageError(args.Exception.Message);
			else
				FireSubEvent(args);
		}
	}
}
