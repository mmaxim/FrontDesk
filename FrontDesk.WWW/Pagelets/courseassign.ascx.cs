using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Microsoft.Web.UI.WebControls;

using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Tools;
using FrontDesk.Controls.Matrix;
using FrontDesk.Pages;

namespace FrontDesk.Pages.Pagelets {

	/// <summary>
	///	Subjective grading evaluation creation
	/// </summary>
	public class CourseAssignPagelet : Pagelet {
		protected System.Web.UI.WebControls.Image Image1;
		protected Microsoft.Web.UI.WebControls.MultiPage mpViews;
		protected System.Web.UI.WebControls.Button cmdAsstUpdate;
		protected System.Web.UI.WebControls.LinkButton lnkAsstDelete;
		protected System.Web.UI.WebControls.TextBox txtAsstName;
		protected System.Web.UI.WebControls.TextBox txtAsstUrl;
		protected System.Web.UI.WebControls.TextBox txtAsstDueDate;
		protected System.Web.UI.WebControls.LinkButton lnkAsstBackup;
		protected Label lblAsstID;
		protected System.Web.UI.WebControls.Label lblAsstError;
		protected System.Web.UI.WebControls.HyperLink hypAsstAutoTest;
		protected System.Web.UI.WebControls.Label lblAsstBackup;
		protected CourseMatrixControl ucRubric;

		private void Page_Load(object sender, System.EventArgs e) {
			if (!IsPostBack)
				BindData();
		}

		private void BindData() {
			int courseID = Convert.ToInt32(Request.Params["CourseID"]);
			
			ucRubric.AddRoot(new Courses(Globals.CurrentIdentity).GetInfo(courseID));
			ucRubric.BindData();
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
	}
}
