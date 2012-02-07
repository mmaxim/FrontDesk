using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using Microsoft.Web.UI.WebControls;

using FrontDesk.Pages;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Common;
using FrontDesk.Data.Access;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Filesys;
using FrontDesk.Controls;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	/// Summary description for viewfile.
	/// </summary>
	public class ViewFilePage : MasterPage {
		protected System.Web.UI.WebControls.Button cmdEdit;
		protected System.Web.UI.WebControls.Button cmdSave;
		protected System.Web.UI.WebControls.Button cmdCancel;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label lblLockInfo;
		protected System.Web.UI.HtmlControls.HtmlImage imgLock;
		protected System.Web.UI.WebControls.PlaceHolder phFileViewer;
		protected System.Web.UI.WebControls.Button cmdUnlock;
		protected System.Web.UI.WebControls.Button cmdCreate;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divView;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divGrade;
		protected System.Web.UI.WebControls.DropDownList ddlComments;
		protected System.Web.UI.WebControls.TextBox txtLines;
		protected System.Web.UI.WebControls.TextBox txtCustom;
		protected System.Web.UI.WebControls.DropDownList ddlType;
		protected System.Web.UI.WebControls.TextBox txtPoints;
		protected System.Web.UI.WebControls.HyperLink hypClose;
		protected System.Web.UI.WebControls.Label lblCommentError;
		protected Microsoft.Web.UI.WebControls.TabStrip tsFiles;
		protected RubricViewControl ucRubric;
		protected IFileViewer[] m_viewers;
		protected System.Web.UI.WebControls.PlaceHolder phFileViewer0;
		protected System.Web.UI.WebControls.PlaceHolder phFileViewer1;
		protected System.Web.UI.WebControls.PlaceHolder phFileViewer2;
		protected System.Web.UI.WebControls.PlaceHolder phFileViewer3;
		protected System.Web.UI.WebControls.PlaceHolder phFileViewer4;
		protected System.Web.UI.WebControls.PlaceHolder phFileViewer5;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divView1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divView2;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divView3;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divView4;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divView5;
		protected Microsoft.Web.UI.WebControls.MultiPage mpFiles;

		public const int MAXFILES = 5;

		[Serializable]
		class FileWindow {
			public FileWindow() { }	
		
			public int Low, High;
			public int[] FileIDs;
			public string[] FilePaths;
		};	

		private void Page_Load(object sender, System.EventArgs e) {
			string sfiles = Request.Params["FileIDs"];
			CFile.FileList files = GetFiles(sfiles);
			FileWindow window = new FileWindow();
			int i, oldtsindex;
			
			imgLock.Visible = false;
			lblError.Visible = lblCommentError.Visible = false;
			
			// Setup events
			ucRubric.RubricSelect += new RubricViewSelectEventHandler(this.ucRubric_RubricSelect);
			ucRubric.ResultSelect += new RubricViewSelectResultEventHandler(ucRubric_ResultSelect);

			//Setup initial window
			//if (!IsPostBack) {
				window.Low = 0; window.High = Math.Min(MAXFILES, files.Count); 
				window.FileIDs = new int[files.Count];
				window.FilePaths = new string[files.Count];
				for (i = 0; i < window.High; i++) {
					window.FileIDs[i] = files[i].ID;
					window.FilePaths[i] = files[i].FullPath;
				}	
				ViewState["window"] = window;
			//}

			oldtsindex = tsFiles.SelectedIndex;
			SetupFileWindow(window);
			tsFiles.SelectedIndex = oldtsindex;
			
			ucRubric.RubricSelect += new RubricViewSelectEventHandler(ucRubric_RubricSelect);
			if (!IsPostBack && Request.Params["SubID"] != null) {
				int subID = Convert.ToInt32(Request.Params["SubID"]);
				ViewState["SubID"] = Convert.ToInt32(Request.Params["SubID"]);
				
				mpFiles.Height = Unit.Pixel(325);
				divGrade.Style["TOP"] = "395px";
				divGrade.Visible = true;

				Components.Submission sub =
					new Submissions(Globals.CurrentIdentity).GetInfo(subID);

				Rubric rub = new Assignments(Globals.CurrentIdentity).GetRubric(sub.AsstID);
				ucRubric.RepressAutos = true;
				ucRubric.InitRubric(rub, subID, "../../");		
				ucRubric.Width = "53%";
				ucRubric.Height = "180px";
			} else if (!IsPostBack)
				divGrade.Visible = false;
			
			if (!IsPostBack)
				BindData(window);		
		}

		private void SetupFileWindow(FileWindow window) {			
			int i, vi=0;
			m_viewers = new IFileViewer[MAXFILES];

			//Get viewers and load tabs
			tsFiles.Items.Clear();
			for (i = window.Low; i < window.High; i++) {
				int fileID;
				string fileName;

				fileID = window.FileIDs[i];
				fileName = window.FilePaths[i];

				Tab tab = new Tab();
				tab.Text = Path.GetFileName(fileName);
				tsFiles.Items.Add(tab);
				
				try {
					m_viewers[vi++] = LoadFileControl(fileID, fileName);
				} catch (FileDownloadException er) {
					//Redirect if we only have 1 file
					if (window.High - window.Low == 1)
						Response.Redirect("dlfile.aspx?FileID="+er.FileID, true);
					else
						vi++;
				}
			}
			//Load up the placeholder controls
			LoadPlaceHolders(m_viewers);
		}

		private void LoadPlaceHolders(IFileViewer[] viewers) {
			int i;
			for (i = 0; i < MAXFILES; i++) {
				string ctrlname = "phFileViewer" + i.ToString();
				PlaceHolder ph = (PlaceHolder) FindControl(ctrlname);
				if (viewers[i] != null)
					ph.Controls.Add(viewers[i] as Control);
			}
		}

		private IFileViewer LoadFileControl(int fileID, string fileName) {
			IFileViewer fileViewer;
			string cname;
			try {
				cname = 
					FileViewerFactory.GetInstance().CreateFileViewer(Path.GetExtension(fileName));
				cname = Path.Combine("../..", cname); 
			} catch (Exception) {
				throw new FileDownloadException("Unreadable", fileID);
			}
			fileViewer = Page.LoadControl(cname) as IFileViewer;

			return fileViewer;
		}

		private CFile.FileList GetFiles(string files) {
			string[] tokens = files.Split("|".ToCharArray());
			CFile.FileList flist = new CFile.FileList();
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			foreach (string sfile in tokens)
				if (sfile.Length > 0)
					flist.Add(fs.GetFile(Convert.ToInt32(sfile)));
			return flist;
		}

		private FileWindow GetWindow() {
			return (FileWindow) ViewState["window"];
		}

		private IFileViewer GetCurrentViewer() {
			FileWindow window = GetWindow();
			int curview = tsFiles.SelectedIndex;
			return m_viewers[curview-window.Low];
		}

		private bool IsGradeMode() {
			return (ViewState["SubID"] != null);
		}

		private int GetSubID() {
			return (int) ViewState["SubID"];
		}

		private void BindData(FileWindow window) {
			IFileViewer viewer;
			int curfile = tsFiles.SelectedIndex;
			int i;
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			for (i = window.Low; i < window.High; i++) {
				CFile file = fs.GetFile(window.FileIDs[i]);
				viewer = m_viewers[i-window.Low];
				
				if (viewer != null)
					viewer.LoadFile(file);
			}

			viewer = GetCurrentViewer();
			cmdEdit.Enabled = (viewer.Editable);
		}

		private void DisplayLockInfo(CFile file) {
			CFileLock flock = new FileSystem(Globals.CurrentIdentity).GetLockInfo(file);
		
			lblLockInfo.Text = "File Locked: <b>User: </b>" + flock.UserName + 
				" <b>Since:</b> " + flock.Creation;

			imgLock.Visible = true;
			lblLockInfo.Visible = true;
			cmdUnlock.Visible = true;
		}

		private void PageError(string msg) {
			lblError.Text = msg;
			lblError.Visible = true;
		}

		private void PageCommentError(string msg) {
			lblCommentError.Text = msg;
			lblCommentError.Visible = true;
		}

		private bool CheckLock(CFile file) {
			if (new FileSystem(Globals.CurrentIdentity).IsLocked(file)) {
				DisplayLockInfo(file);
				return false;
			} else {
				imgLock.Visible = cmdUnlock.Visible = lblLockInfo.Visible = false;
				return true;
			}
		}

		private void cmdEdit_Click(object sender, System.EventArgs e) {
			IFileViewer fileViewer = GetCurrentViewer();
			int fileID = fileViewer.FileID;
			CFile file = new FileSystem(Globals.CurrentIdentity).GetFile(fileID);
			
			//Check to see if this file is locked
			if (!CheckLock(file)) {
				BindData(GetWindow());
				return;
			}
			
			try {
				new FileSystem(Globals.CurrentIdentity).Edit(file);
			} catch (FileOperationException er) {
				PageError(er.Message);
				return;
			}

			fileViewer.Edit();
			cmdSave.Enabled = cmdCancel.Enabled = true;
			cmdEdit.Enabled = false;
			CheckLock(file);
		}

		private void cmdSave_Click(object sender, System.EventArgs e) {
			IFileViewer fileViewer = GetCurrentViewer();
			int fileID = fileViewer.FileID;
			CFile file = new FileSystem(Globals.CurrentIdentity).GetFile(fileID);
			new FileSystem(Globals.CurrentIdentity).LoadFileData(file);
			try {	
				file.Data = fileViewer.Data.ToCharArray();
				new FileSystem(Globals.CurrentIdentity).Save(file);
			} catch (FileOperationException er) {
				PageError(er.Message);
			}
			
			fileViewer.LoadFile(file);
			fileViewer.UnEdit();
			cmdSave.Enabled = cmdCancel.Enabled = false;
			cmdEdit.Enabled = true;

			CheckLock(file);
			BindData(GetWindow());
		}

		private void cmdCancel_Click(object sender, System.EventArgs e) {
			GiveUpLock();
		}

		private void cmdUnlock_Click(object sender, System.EventArgs e) {
			GiveUpLock();
		}

		private void GiveUpLock() {
			IFileViewer fileViewer = GetCurrentViewer();
			int fileID = fileViewer.FileID;
			CFile file = new FileSystem(Globals.CurrentIdentity).GetFile(fileID);

			try {
				new FileSystem(Globals.CurrentIdentity).UnLock(file);
			} catch (FileOperationException er) {
				PageError(er.Message);
			}
	
			fileViewer.UnEdit();
			cmdSave.Enabled = cmdCancel.Enabled = false;
			cmdEdit.Enabled = true;
			CheckLock(file);
			BindData(GetWindow());
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.cmdEdit.Click += new System.EventHandler(this.cmdEdit_Click);
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			this.cmdUnlock.Click += new System.EventHandler(this.cmdUnlock_Click);
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			this.cmdCreate.Click += new System.EventHandler(this.cmdCreate_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private CannedResponse GetCannedResponse() {
			int canID = Convert.ToInt32(ddlComments.SelectedItem.Value);
			if (canID < 0) 
				return null;
			else
				return new Rubrics(Globals.CurrentIdentity).GetCannedInfo(canID);
		}

		private ArrayList ParseLineGroup(string range) {
			
			ArrayList lines = new ArrayList();
			string[] tokens = range.Split("-".ToCharArray());
			int begin, end;
			try {
				begin = Convert.ToInt32(tokens[0]);
				end = Convert.ToInt32(tokens[1]);
			} catch (Exception) {
				throw new LineParseException("Lines affected line in incorrect format due to: " + range);
			}
			for (int i = begin; i <= end; i++)
				lines.Add(i);

			return lines;
		}

		private ArrayList ParseLinesAffected(string line) {
			
			ArrayList lines = new ArrayList();
			string[] tokens = line.Split(",".ToCharArray());
			foreach (string token in tokens) {
				if (token.IndexOf("-") >= 0)
					lines.AddRange(ParseLineGroup(token));
				else {
					try {
						lines.Add(Convert.ToInt32(token));
					} catch (Exception) {
						throw new LineParseException("Lines affected line in incorrect format due to: " + token);
					}
				}
			}
			return lines;
		}

		private void cmdCreate_Click(object sender, System.EventArgs e) {
			
			string comment;
			double points;
			IFileViewer fileViewer = GetCurrentViewer();
			int fileID = fileViewer.FileID, type;
			int rubID = ucRubric.GetCurrentRubricID();
			CannedResponse can = GetCannedResponse();
			ArrayList lines;
			string parserrmsg="";
			
			//Try to parse line string
			try {
				lines = ParseLinesAffected(txtLines.Text);
			} catch (Exception er) {
				lines = null;
				parserrmsg = er.Message;
			}

			if (lines == null)
				PageCommentError(parserrmsg);
			else if (lines.Count == 0)
				PageCommentError("Must enter at least one line affected"); 
			else {
				if (can == null) {
					comment= txtCustom.Text;
					type = Convert.ToInt32(ddlType.SelectedValue);
					points = Convert.ToDouble(txtPoints.Text);
				} else {
					comment = can.Comment;
					type = can.Type;
					points = can.Points;
				}
				try {
					new Results(Globals.CurrentIdentity).CreateSubj(GetSubID(), rubID, comment, 
						fileID, (int)lines[0], points, lines, type);
				} catch (DataAccessException er) {
					PageCommentError(er.Message);
				} catch (LineParseException er) {
					PageCommentError(er.Message);
				}
			}

			ucRubric.UpdateRubric();
			BindData(GetWindow());	
		}

		private void ucRubric_RubricSelect(object sender, RubricViewSelectEventArgs args) {

			Rubrics rubda = new Rubrics(Globals.CurrentIdentity);
			Rubric rub = args.SelectedRubric;
			
			ddlType.Items.Clear();
			ddlComments.Items.Clear();
			txtLines.Text = ""; txtCustom.Text = ""; txtPoints.Text = "";
			if (rubda.IsHeading(rub)) 
				cmdCreate.Enabled = false;
			else {
				CannedResponse.CannedResponseList cans = rubda.GetCannedResponses(rub.ID);
				foreach (CannedResponse can in cans) {
					string canstr = can.Comment.Substring(0, Math.Min(80, can.Comment.Length));
					if (canstr.Length == 80)
						canstr += " ...";
					ListItem item = new ListItem(canstr, can.ID.ToString());
					ddlComments.Items.Add(item);
				}
				ddlComments.Items.Add(new ListItem("Custom", "-1"));
				cmdCreate.Enabled = true;

				ddlType.Items.Add(new ListItem("Error", Rubric.ERROR.ToString()));
				ddlType.Items.Add(new ListItem("Warning", Rubric.WARNING.ToString()));
				ddlType.Items.Add(new ListItem("Good", Rubric.GOOD.ToString()));
			}
			
			BindData(GetWindow());
		}

		class LineParseException : Exception {
			public LineParseException() : base("Error parsing line") { }
			public LineParseException(string msg) : base(msg) { }
		}

		class FileDownloadException : Exception {
			public FileDownloadException() : base("Cannot download file in multiple file view") { }
			public FileDownloadException(string msg, int fileID) : base(msg) { FileID = fileID; }

			public int FileID;
		}

		private void ucRubric_ResultSelect(object sender, RubricViewSelectResultEventArgs args) {
			ddlType.Items.Clear();
			ddlComments.Items.Clear();
			txtLines.Text = ""; txtCustom.Text = ""; txtPoints.Text = "";

			cmdCreate.Enabled = false;
			BindData(GetWindow());
		}
	}
}
