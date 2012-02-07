using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.IO;
using System.Threading;
using System.Security.Principal;

using FrontDesk.Pages;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Components;
using FrontDesk.Components.Filesys;
using FrontDesk.Tools;
using FrontDesk.Common;

namespace FrontDesk.Controls.Submission {

	/// <summary>
	///	Archive submission system
	/// </summary>
	public class ArchiveSubmissionControl : SubmissionSystemControl, ISubmissionSystemControl {

		protected System.Web.UI.HtmlControls.HtmlInputFile fileUpload;
		protected CustomButton.ClickOnceButton cmdSubmit;
		protected System.Web.UI.WebControls.Label lblTypes;

		private void Page_Load(object sender, System.EventArgs e) {
			
			if (cmdSubmit.Enabled)
				lblError.Visible = false;
			
			if (!IsPostBack) {
				string[] types = ArchiveToolFactory.GetInstance().ListKeys();
				foreach (string type in types)
					lblTypes.Text += type + " ";
			}
		}

		public void FinishInit() {
			cmdSubmit.Enabled = new Assignments(Globals.CurrentIdentity).IsSubmissionAvailable(AssignmentID);
			if (!cmdSubmit.Enabled)
				PageError("This assignment is not available for student submissions, therefore you are unable to submit " +
				      "at this time. Please check back later when course staff has enabled student submissions.");
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

		private void cmdSubmit_Click(object sender, System.EventArgs e) {
			lblError.Visible = false;
			HttpPostedFile upfile = fileUpload.PostedFile;
			if (upfile.ContentLength == 0) {
				PageError("Please specify a file to upload");
				return;
			}

			SubmitCommand scmd = 
				new SubmitCommand(new Submissions(Globals.CurrentIdentity), upfile,
				AssignmentID, SubmitterPrincipalID);

			//Load global context for threads
			Globals.Context = HttpContext.Current;

			ViewState["submitting"] = true;
		//	((MasterPage)Page).ExecuteLongTask(scmd);
			
			scmd.Finished += new ICommandEventHandler(scmd_Finished);
			scmd.Execute();		
		}

		public class SubmitCommand : ICommand {

			private HttpPostedFile m_upfile;
			private int m_asstID, m_prinID;
			private Submissions m_subda;

			public SubmitCommand(Submissions subda, HttpPostedFile upfile, int asstID, int prinID) {
				m_upfile=upfile; m_asstID=asstID; m_prinID=prinID; m_subda = subda;
			}

			public void Execute() {
				IExternalSource extsource;
				ICommandEventArgs args = new ICommandEventArgs();

				//Unpack archive
				Components.Submission sub=null;
				try {
					extsource = CreateSource(m_upfile);

					//Create submission
					sub = m_subda.Create(m_asstID, m_prinID, extsource);
				} catch (Exception er) {
					args.Exception = er;
				}

				args.ReturnValue = sub;
				if (Finished != null)
					Finished(this, args);
			}

			private IExternalSource CreateSource(HttpPostedFile upfile) {
				string ext = Path.GetExtension(upfile.FileName);
				IArchiveTool etool = 
					ArchiveToolFactory.GetInstance().CreateArchiveTool(ext);

				(etool as IExternalSource).CreateSource(upfile.InputStream);

				return (etool as IExternalSource);
			}

			public event ICommandEventHandler Finished;
		}

		private void scmd_Finished(object sender, ICommandEventArgs args) {
			if (args.Exception != null)
				PageError(args.Exception.Message);
			else
				FireSubEvent(args);
		}

		public string Name {
			get { return "Archive Submission System"; }
		}
	}
}
