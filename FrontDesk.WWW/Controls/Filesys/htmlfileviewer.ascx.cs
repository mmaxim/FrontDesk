using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using FrontDesk.Common;
using FrontDesk.Pages;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Filesys;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	///	HTML File viewer control
	/// </summary>
	public class HTMLFileViewer : Pagelet, IFileViewer {
		protected System.Web.UI.HtmlControls.HtmlGenericControl divHtml;
		protected System.Web.UI.WebControls.TextBox txtData;

		private void Page_Load(object sender, EventArgs e) {
		
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public void Edit() {
			divHtml.Visible = false;

			int fileid = (int) ViewState["fileid"];
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			CFile file = fs.GetFile(fileid); fs.LoadFileData(file);
			
			txtData.Text = new String(file.Data);
			txtData.Visible = true;
		}

		public void UnEdit() {
			txtData.Visible = false;
			divHtml.Visible = true;
		}

		public string Data {
			get { return txtData.Text; }
		}

		public int FileID {
			get { return (int) ViewState["fileid"]; }
		}

		public void LoadFile(CFile file) {
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
	
			//Load data
			fs.LoadFileData(file);

			txtData.Text = new string(file.Data);
			divHtml.InnerHtml = new string(file.Data);

			ViewState["fileid"] = file.ID;
		}

		public bool Editable {
			get { return true; }
		}
	}
}
