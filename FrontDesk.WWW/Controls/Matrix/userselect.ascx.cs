using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Collections;

using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Tools;

namespace FrontDesk.Controls.Matrix {

	/// <summary>
	///	Select users for evaluation
	/// </summary>
	public class UserSelectView : UserControl, IMatrixControl {
		protected System.Web.UI.WebControls.DataGrid dgUsers;
		protected System.Web.UI.WebControls.Label lblEvaluate;
		protected System.Web.UI.WebControls.LinkButton lnkSecExpl;
		protected Microsoft.Web.UI.WebControls.MultiPage mpViews;
		protected System.Web.UI.WebControls.Button cmdReset;
		protected System.Web.UI.WebControls.Button cmdEvaluate;

		private void Page_Load(object sender, System.EventArgs e) {
			lblEvaluate.Visible = false;
		}

		private int GetAsstID() {
			return (int) ViewState["asstID"];
		}

		private void BindData() {
			Courses courseda = new Courses(Globals.CurrentIdentity);
			Assignment asst = new Assignments(Globals.CurrentIdentity).GetInfo(GetAsstID());
			Course course = courseda.GetInfo(asst.CourseID);
			
			Section.SectionList sections = courseda.GetSections(course.ID);
			CourseMember.CourseMemberList mems = courseda.GetMembers(course.ID, null);

			ArrayList secmems = sections;
			secmems.AddRange(mems);

			dgUsers.DataSource = secmems;
			dgUsers.DataBind();
		
			cmdEvaluate.Enabled = asst.ResultRelease;
			lblEvaluate.Visible = !asst.ResultRelease;

			lnkSecExpl.Attributes.Add("onClick", 
				@"window.open('sectionexpl.aspx?CourseID=" + course.ID + 
				@"', '"+ course.ID+@"', 'width=430, height=530')");
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
			this.cmdEvaluate.Click += new EventHandler(cmdEvaluate_Click);
			this.dgUsers.PageIndexChanged += new DataGridPageChangedEventHandler(dgUsers_PageIndexChanged);
			this.dgUsers.ItemDataBound += new DataGridItemEventHandler(dgUsers_ItemDataBound);
			this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["asstID"] = ap.ID;
			BindData();
		}

		private string HarvestSelectedUsers() {
			string userstr="";
			Label lblType, lblID;
			Sections sectda = new Sections(Globals.CurrentIdentity);
			Users userda = new Users(Globals.CurrentIdentity);
			foreach (DataGridItem row in dgUsers.Items) {
				if ((row.FindControl("chkSelect") as CheckBox).Checked) {
					lblType = (Label) row.FindControl("lblType");
					lblID = (Label) row.FindControl("lblID");
					string sid = lblID.Text;
					if (lblType.Text == "S") {
						int id = Convert.ToInt32(sid);
						User.UserList users = sectda.GetMembers(id);
						foreach (User user in users)
							userstr += user.UserName + "|";
					}
					else
						userstr += sid + "|";
				}
			}
			return userstr;
		}

		private void cmdEvaluate_Click(object sender, System.EventArgs e) {
			
			string userstr = HarvestSelectedUsers();

			mpViews.SelectedIndex = 1;
			Refresh(this, new RefreshEventArgs(userstr, false));
		}

		private void dgUsers_ItemDataBound(object sender, DataGridItemEventArgs e) {
			Label lblName, lblProgress, lblType, lblID;
			System.Web.UI.WebControls.Image imgStatus, imgType;
			CheckBox chkSelect;

			if (null != (lblName = (Label) e.Item.FindControl("lblName"))) {
				lblProgress = (Label) e.Item.FindControl("lblProgress");
				lblType = (Label) e.Item.FindControl("lblType");
				lblID = (Label) e.Item.FindControl("lblID");
				imgStatus = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgStatus");
				imgType = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgType");
				chkSelect = (CheckBox) e.Item.FindControl("chkSelect");

				if (e.Item.DataItem is Section) {
					BindSectionItem(e.Item.DataItem as Section,
						lblName, lblProgress, imgStatus, imgType, chkSelect);
					lblType.Text = "S";
					lblID.Text = (e.Item.DataItem as Section).ID.ToString();
				}
				else {
					BindUserItem(e.Item.DataItem as CourseMember, 
						lblName, lblProgress, imgStatus, imgType, chkSelect);
					lblType.Text = "U";
					lblID.Text = (e.Item.DataItem as CourseMember).User.UserName;
				}
			}
		}

		private void BindSectionItem(Section sec, Label lblName, Label lblProgress,
			System.Web.UI.WebControls.Image imgStatus, 
			System.Web.UI.WebControls.Image imgType, CheckBox chkSelect) {
			
			imgType.ImageUrl = "../../attributes/group.gif";
			lblName.Text = sec.Name;
			chkSelect.Checked = false;

			int prg = GetSectionProgress(sec);
			lblProgress.Text = prg.ToString() + "%";

			if (prg == 100) 
				imgStatus.ImageUrl = "../../attributes/subgrade.gif";
			else
				imgStatus.Visible = false;
		}

		private int GetSectionProgress(Section sec) {

			Users userda = new Users(Globals.CurrentIdentity);
			User.UserList users = new Sections(Globals.CurrentIdentity).GetMembers(sec.ID);
			int done=0, count=0;
			foreach (User user in users) {
				Components.Submission sub = userda.GetLatestAsstSubmission(user.UserName, GetAsstID());
				if (sub != null) {
					count++;
					if (sub.Status == Components.Submission.GRADED)
						done++;
				}
			}
			
			return Math.Min(100, Math.Max(0, (int)(((double)done)/((double)count)*100.0)));
		}

		private void BindUserItem(CourseMember mem, Label lblName, Label lblProgress,
			System.Web.UI.WebControls.Image imgStatus, 
			System.Web.UI.WebControls.Image imgType, CheckBox chkSelect) {

			User user = mem.User;
			imgType.ImageUrl = "../../attributes/user.gif";
			lblName.Text = user.FullName + "(" + user.UserName + ")";
			
			Users userda = new Users(Globals.CurrentIdentity);
			Components.Submission sub = userda.GetLatestAsstSubmission(user.UserName, GetAsstID());
			if (sub == null) {
				imgStatus.ImageUrl = "../../attributes/nosub.gif";
				lblProgress.Text = "N/A";
				chkSelect.Enabled = false;
			}
			else {
				switch (sub.Status) {
					case Components.Submission.GRADED:
						imgStatus.ImageUrl = "../../attributes/subgrade.gif";
						lblProgress.Text = "100%";
						break;
					case Components.Submission.INPROGRESS:
						imgStatus.ImageUrl = "../../attributes/clock.gif";
						lblProgress.Text = "??%";
						break;
					case Components.Submission.UNGRADED:
						imgStatus.ImageUrl = "../../attributes/sub.gif";
						lblProgress.Text = "0%";
						break;
				}
			}
		}

		private void dgUsers_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {
			dgUsers.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}

		private void cmdReset_Click(object sender, EventArgs e) {
			mpViews.SelectedIndex = 0;
			Refresh(this, new RefreshEventArgs("", false));
		}
	}
}
