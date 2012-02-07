using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Pages;
using FrontDesk.Components;
using FrontDesk.Data.Access;
using FrontDesk.Common;

namespace FrontDesk.Controls.Matrix {

	/// <summary>
	///	Course announcements pagelet
	/// </summary>
	public class AnnouncementView : Pagelet, IMatrixControl {

		protected System.Web.UI.WebControls.Button cmdCreate;
		protected System.Web.UI.WebControls.TextBox txtDesc;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.LinkButton DeleteLink;
		protected System.Web.UI.WebControls.Label lblPreview;
		protected System.Web.UI.WebControls.Label lblCourseID;
		protected System.Web.UI.WebControls.Label lblPoster;
		protected System.Web.UI.WebControls.Label lblDate;
		protected System.Web.UI.WebControls.Button cmdPreview;

		private void Page_Load(object sender, EventArgs e) {	
			lblError.Visible = false;
		}

		private int GetCurrentID() {
			return (int) ViewState["annid"];
		}

		private void BindData(int annID) {
			
			Announcement ann =
				new Announcements(Globals.CurrentIdentity).GetInfo(annID);

			txtTitle.Text = ann.Title;
			txtDesc.Text = HTMLWizard.BRToLineBreak(ann.Description);
			lblCourseID.Text = ann.CourseID.ToString();

			lblDate.Text = "Post Date: " + ann.Created;
			lblPoster.Text = "Poster: <b>" + ann.Poster + "</b>";
			lblPreview.Text = ann.Description;
		}

		private void cmdCreate_Click(object sender, System.EventArgs e) {

			int courseID = Convert.ToInt32(lblCourseID.Text);
			Announcement ann = new Announcement();

			ann.ID = GetCurrentID();
			ann.Description = txtDesc.Text;
			ann.Title = txtTitle.Text;
			ann.CourseID = courseID;
			try {
				(new Announcements(Globals.CurrentIdentity)).Update(ann);
			} catch (DataAccessException er) {
				lblError.Text = er.Message;
				lblError.Visible = true;
			}

			BindData(GetCurrentID());
			Refresh(this, new RefreshEventArgs());
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
			this.cmdCreate.Click += new System.EventHandler(this.cmdCreate_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["annid"] = ap.ID;
			BindData(ap.ID);
		}

	}
}
