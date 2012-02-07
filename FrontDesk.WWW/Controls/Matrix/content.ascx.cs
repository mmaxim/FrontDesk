using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.IO;

using FrontDesk.Pages;
using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Filesys;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Tools;
using FrontDesk.Controls.Filesys;

/*
this.cmdDataUpload.Click += new EventHandler(cmdDataUpload_Click);
this.cmdUrlUpload.Click += new EventHandler(cmdUrlUpload_Click);
this.cmdUpdate.Click += new EventHandler(cmdUpdate_Click);
this.lnkDownload.Click += new System.EventHandler(this.lnkDownload_Click);
this.rdbData.CheckedChanged += new System.EventHandler(this.rdbData_CheckedChanged);
this.rdbLink.CheckedChanged += new System.EventHandler(this.rdbLink_CheckedChanged);
			*/

namespace FrontDesk.Controls.Matrix {

	/// <summary>
	///	Content view control
	/// </summary>
	public class ContentView : Pagelet, IMatrixControl {

		protected System.Web.UI.WebControls.Button cmdUpdate;
		protected System.Web.UI.WebControls.LinkButton lnkEdit;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divData;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divPerms;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divUpload;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.TextBox txtDesc;
		protected System.Web.UI.WebControls.TextBox txtType;
		protected System.Web.UI.WebControls.LinkButton lnkDownload;
		protected System.Web.UI.HtmlControls.HtmlInputFile ufContent;
		protected System.Web.UI.WebControls.Label lblUpError;
		protected System.Web.UI.HtmlControls.HtmlGenericControl iDirections;
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.RadioButton rdbData;
		protected System.Web.UI.WebControls.RadioButton rdbLink;
		protected System.Web.UI.WebControls.Button cmdDataUpload;
		protected Microsoft.Web.UI.WebControls.MultiPage mpViews;
		protected System.Web.UI.WebControls.TextBox txtUrl;
		protected System.Web.UI.WebControls.Button cmdUrlUpload;
		protected System.Web.UI.WebControls.Label lblLinkError;
		protected FilePermissionsControl ucFilePerms;

		private void Page_Load(object sender, System.EventArgs e) {
			lblError.Visible = lblUpError.Visible = lblLinkError.Visible = false;
		}

		private void PageLinkError(string msg) {
			lblLinkError.Text = msg;
			lblLinkError.Visible = true;
		}

		private void PageError(string msg) {
			lblError.Visible = true;
			lblError.Text = msg;
		}

		private void PageUpError(string msg) {
			lblUpError.Visible = true;
			lblUpError.Text = msg;
		}

		private int GetFileID() {
			return (int) ViewState["fileID"];
		}

		private bool GetStudentMode() {
			return (bool) ViewState["stumode"];
		}

		private void BindData() {

			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			CFile file = fs.GetFile(GetFileID());	
	
			if (!GetStudentMode()) {
				divPerms.Visible = true;
				ucFilePerms.FileID = file.ID;
				ucFilePerms.BindData();
			} else
				divPerms.Visible = false;

			cmdUpdate.Visible = iDirections.Visible = divUpload.Visible = !GetStudentMode();
			txtName.Text = file.Alias;
			if (file.IsDirectory()) {
				divData.Visible = false;
				txtType.Enabled = false;
				txtType.Text = "Folder";
			}
			else {
				divData.Visible = true;

				txtType.Enabled = true;
				txtType.Text = Path.GetExtension(file.Name);
				txtDesc.Text = file.Description;

				if (txtType.Text == ".url") {
					fs.LoadFileData(file);
					string url = new string(file.Data);
					txtUrl.Text = url;
					lnkEdit.Attributes.Add("onClick", 
						@"window.open('" + url + "', '"+"Mike"+@"', 'width=800, height=600 " +
						@", scrollbars=yes, menubar=yes, resizable=yes, status=yes, toolbar=yes')");
					lnkDownload.Enabled = false;
					rdbData.Checked = false; rdbLink.Checked = true;
				} else {
					txtUrl.Text = "";
					rdbData.Checked = true; rdbLink.Checked = false;
					lnkEdit.Attributes.Add("onClick", 
						@"window.open('Controls/Filesys/viewfile.aspx?FileID=" + file.ID + 
						@"', '"+file.ID+@"', 'width=770, height=580')");
					lnkDownload.Attributes.Add("onClick", 
						@"window.open('Controls/Filesys/dlfile.aspx?FileID=" + file.ID + 
						@"', '"+file.ID+@"', 'width=770, height=580')");
					lnkDownload.Enabled = true;
				}
			}

			txtType.Enabled = txtDesc.Enabled = txtName.Enabled =
				!GetStudentMode();
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
			this.cmdDataUpload.Click += new EventHandler(cmdDataUpload_Click);
			this.cmdUrlUpload.Click += new EventHandler(cmdUrlUpload_Click);
			this.cmdUpdate.Click += new EventHandler(cmdUpdate_Click);
			this.lnkDownload.Click += new System.EventHandler(this.lnkDownload_Click);
			this.rdbData.CheckedChanged += new System.EventHandler(this.rdbData_CheckedChanged);
			this.rdbLink.CheckedChanged += new System.EventHandler(this.rdbLink_CheckedChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["fileID"] = ap.ID;
			ViewState["stumode"] = ap.StudentMode;
			BindData();
		}

		private void lnkDownload_Click(object sender, System.EventArgs e) {
			
		}

		private void cmdUrlUpload_Click(object sender, System.EventArgs e) {
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			CFile file = fs.GetFile(GetFileID());

			try {
				//Save Url data
				fs.Edit(file);
				file.Data = txtUrl.Text.ToCharArray();
				fs.Save(file);
				//Update file
				file.Name = file.ID + ".url";
				fs.UpdateFileInfo(file, false);
			} catch (Exception er) {
				PageLinkError(er.Message);
			}

			BindData();

			if (Refresh != null)
				Refresh(this, new RefreshEventArgs("", true, false));
		}

		private void rdbData_CheckedChanged(object sender, System.EventArgs e) {
			if (rdbData.Checked)
				mpViews.SelectedIndex = 0;
		}

		private void rdbLink_CheckedChanged(object sender, System.EventArgs e) {
			if (rdbLink.Checked)
				mpViews.SelectedIndex = 1;
		}

		private void cmdDataUpload_Click(object sender, EventArgs e) {	
			if (ufContent.PostedFile.ContentLength > 0) {	
				FileSystem fs = new FileSystem(Globals.CurrentIdentity);
				CFile file = fs.GetFile(GetFileID());

				try {
					fs.Edit(file);

					byte[] content = Globals.ReadStream(
						ufContent.PostedFile.InputStream, ufContent.PostedFile.ContentLength);
					file.RawData = content;
					fs.Save(file);

					file.Name = file.ID + Path.GetFileName(ufContent.PostedFile.FileName);
					fs.UpdateFileInfo(file, false);
				} catch (Exception er) {
					PageUpError(er.Message);
				}

				if (Refresh != null)
					Refresh(this, new RefreshEventArgs("", true, false));
			}
			else
				PageUpError("Must specify a local file to upload");

			BindData();
		}

		private void cmdUpdate_Click(object sender, EventArgs e) {
			FileSystem fs;
			CFile file = (fs = new FileSystem(Globals.CurrentIdentity)).GetFile(GetFileID());

			file.Alias = txtName.Text; file.Description = txtDesc.Text;
			string tyext = txtType.Text;
			if (!tyext.StartsWith("."))
				tyext = "." + tyext;
			file.Name = Path.GetFileNameWithoutExtension(file.Name) + tyext;

			try {
				fs.UpdateFileInfo(file, false);
			} catch (DataAccessException er) {
				PageError(er.Message);
			} catch (FileOperationException er) {
				PageError(er.Message);
			}

			if (Refresh != null)
				Refresh(this, new RefreshEventArgs("", true, false));
		}
	}
}
