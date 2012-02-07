using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Common;
using FrontDesk.Data.Filesys;
using FrontDesk.Components.Filesys;
using FrontDesk.Pages;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	///	Text file viewer control 
	/// </summary>
	public class TextFileViewer : Pagelet, IFileViewer {

		protected System.Web.UI.WebControls.TextBox txtData;

		private void Page_Load(object sender, System.EventArgs e) {
			txtData.ReadOnly = true;
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
			txtData.ReadOnly = false;
		}

		public void UnEdit() {
			txtData.ReadOnly = true;
		}

		public void LoadFile(CFile file) {
			new FileSystem(Globals.CurrentIdentity).LoadFileData(file);
			ViewState["fileid"] = file.ID;
			txtData.Text = new String(file.Data);
		}

		public bool Editable {
			get { return true; }
		}

		public int FileID {
			get { return (int) ViewState["fileid"]; }
		}

		public string Data {
			get { return txtData.Text; }
		}
	}
}
