using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Common;
using FrontDesk.Data.Access;
using FrontDesk.Controls.Matrix;
using FrontDesk.Pages;

namespace FrontDesk.Pages.Pagelets {

	/// <summary>
	///	Student course main page
	/// </summary>
	public class StudentCoursePagelet : Pagelet {

		protected CourseMatrixControl ucRubric;

		private void Page_Load(object sender, System.EventArgs e) {
			if (!IsPostBack)
				BindData();
		}

		private void BindData() {
			int courseID = Convert.ToInt32(Request.Params["CourseID"]);
			
			ucRubric.StudentMode = true;
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
