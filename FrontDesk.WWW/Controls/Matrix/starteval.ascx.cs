using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Collections;

using FrontDesk.Pages;
using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Tools;

/*
  			this.cmdReset.Click += new EventHandler(cmdReset_Click);
			this.cmdCancel.Click += new EventHandler(cmdCancel_Click);
			this.cmdEvaluate.Click += new EventHandler(cmdEvaluate_Click);
			this.cmdUserEvaluate.Click += new EventHandler(cmdUserEvaluate_Click);
			this.dgSections.PageIndexChanged += new DataGridPageChangedEventHandler(dgSections_PageIndexChanged);
			this.dgSections.ItemDataBound += new DataGridItemEventHandler(dgSections_ItemDataBound);
			this.cmdSkip.Click += new System.EventHandler(this.cmdSkip_Click);
			this.lstTests.SelectedIndexChanged += new System.EventHandler(this.lstTests_SelectedIndexChanged);
			this.cmdSubmit.Click += new System.EventHandler(this.cmdSubmit_Click);
            this.dgSections.ItemCommand += new DataGridCommandEventHandler(dgSections_ItemCommand);
			this.dgUserSearch.ItemDataBound += new DataGridItemEventHandler(dgUserSearch_ItemDataBound);
			this.cmdSearch.Click +=new EventHandler(cmdSearch_Click);
			this.tsUsers.SelectedIndexChange += new System.EventHandler(this.tsUsers_SelectedIndexChange);
			this.dgUserSearch.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgUserSearch_ItemDataBound);
			this.dgUserSearch.PageIndexChanged += new DataGridPageChangedEventHandler(dgUserSearch_PageIndexChanged);
			
			<Columns>
			<asp:TemplateColumn>
				<ItemStyle Width="10px"></ItemStyle>
				<ItemTemplate>
					<asp:CheckBox Runat="server" ID="chkSelect" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Status">
				<ItemStyle Width="10px"></ItemStyle>
				<ItemTemplate>
					<asp:Image Runat="server" ID="imgStatus" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Submissions">
				<ItemStyle Width="10px"></ItemStyle>
				<ItemTemplate>
					<asp:Label runat="server" ID="lblNumSubmissions" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Name">
				<ItemTemplate>
					<asp:Image Runat="server" ID="imgType" ImageAlign="AbsMiddle" />
					<asp:Label runat="server" ID="lblName"></asp:Label>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn Visible="False">
				<ItemTemplate>
					<asp:Label runat="server" ID="lblType"></asp:Label>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn Visible="False" HeaderText="Group/UserID">
				<ItemTemplate>
					<asp:Label runat="server" ID="lblID"></asp:Label>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
*/


namespace FrontDesk.Controls.Matrix {

	/// <summary>
	///	Select users for evaluation
	/// </summary>
	public class StartEvaluationView : Pagelet, IMatrixControl {
		
		protected System.Web.UI.WebControls.DataGrid dgSections;
		protected System.Web.UI.WebControls.DataGrid dgUserSearch;

		protected System.Web.UI.WebControls.Label lblEvaluate;
		protected System.Web.UI.WebControls.Label lblPrinStr;
		protected System.Web.UI.WebControls.LinkButton lnkSecExpl;
		protected Microsoft.Web.UI.WebControls.MultiPage mpViews;
		protected System.Web.UI.WebControls.Button cmdReset;
		protected System.Web.UI.WebControls.Button cmdCancel;
		protected System.Web.UI.WebControls.Button cmdSkip;

		//searching for users
		protected System.Web.UI.WebControls.Button cmdSearch;
		protected System.Web.UI.WebControls.TextBox txtSearch;
		protected System.Web.UI.WebControls.DropDownList drpSearchMethod;

		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.ListBox lstTests;
		protected System.Web.UI.WebControls.Button cmdSubmit;
		protected System.Web.UI.WebControls.ListBox lstOrder;
		protected System.Web.UI.WebControls.Label lblAutoError;
		protected System.Web.UI.WebControls.Button cmdEvaluate;
		protected System.Web.UI.WebControls.Button cmdUserEvaluate;
		protected Microsoft.Web.UI.WebControls.TabStrip tsUsers;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divUsers;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divSearch;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divSections;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divAuto;

		private void Page_Load(object sender, System.EventArgs e) {
			lblEvaluate.Visible = false;
			lblAutoError.Visible = false;
		}

		private void PageAutoError(string msg) {
			lblAutoError.Text = msg;
			lblAutoError.Visible = true;
		}

		private int GetAsstID() {
			return (int) ViewState["asstID"];
		}

		private string GetGroups() {
			return (string) ViewState["Groups"];
		}

		private int GetCourseID() {
			return (int) ViewState["courseID"];
		}

		private void BindAutoData() {
			Evaluation.EvaluationList evals = 
				new Assignments(Globals.CurrentIdentity).GetAutoEvals(GetAsstID());

			lstTests.DataSource = evals;
			lstTests.DataTextField = Evaluation.NAME_FIELD;
			lstTests.DataValueField = Evaluation.ID_FIELD;
			lstTests.DataBind();
		}

		private void lstTests_SelectedIndexChanged(object sender, System.EventArgs e) {
			
			ArrayList evals = GetTests();
			Evaluations aevals = new Evaluations(Globals.CurrentIdentity);

			lstOrder.Items.Clear();
			foreach (int evalID in evals) {
				Evaluation eval = aevals.GetInfo(evalID);
				Evaluations.DependencyGraph dg = 
					new Evaluations.DependencyGraph(eval, Globals.CurrentIdentity);
				
				Evaluation.EvaluationList order = dg.GetBuildOrder();
				foreach (Evaluation oeval in order) 
					lstOrder.Items.Add(oeval.Name);
			
				lstOrder.Items.Add(eval.Name);
			}
		}

		private void BindGroups(User.UserList users){
			//Load the groups into viewstate
			ArrayList groups = new ArrayList();
			new UserGrouper().Group(users, groups, 9);
			string vsrecord="";
			foreach (UserGrouper.UserGroup group in groups) 
				vsrecord += group.LowerBound + " " + group.UpperBound + "\n";
			ViewState["Groups"] = vsrecord;

			dgUserSearch.VirtualItemCount = 9*groups.Count;
		//	dgUserSearch.CurrentPageIndex = 0;
		}

		public void BindSearchData(User.UserList users) {

			int courseID = new Assignments(Globals.CurrentIdentity).GetInfo(GetAsstID()).CourseID;

			//Load the groups into viewstate
			if (users.Count > 0) {
				string[] bounds = GetGroups().Split(
					"\n".ToCharArray())[dgUserSearch.CurrentPageIndex].Split(" ".ToCharArray());
				new UserGrouper().Regroup(bounds[1], bounds[0], users);
			}

			dgUserSearch.DataSource = users;
			dgUserSearch.DataBind();
		}

		private void BindSections() {
			Courses courseda = new Courses(Globals.CurrentIdentity);
			Section.SectionList sections = courseda.GetSections(GetCourseID());

			dgSections.DataSource = sections;
			dgSections.DataBind();
		}

		private void BindSelectData() {
			
			//Bind the correct tab
			switch (tsUsers.SelectedIndex) {
			case 0:
				BindSections();
				divSections.Visible = true;
				divUsers.Visible = false;
				break;
			case 1:
				BindGroups(new User.UserList());
				BindSearchData(new User.UserList());
				divSections.Visible = false;
				divUsers.Visible = true;
				divSearch.Visible = true;
				break;
			case 2:
				User.UserList mems = new Courses(Globals.CurrentIdentity).GetMembers(GetCourseID(), null);
				BindGroups(mems);
				BindSearchData(mems);
				divSections.Visible = false;
				divUsers.Visible = true;
				divSearch.Visible = false;
				break;
			}

			int courseID = GetCourseID();
			lnkSecExpl.Attributes.Add("onClick", 
				@"window.open('sectionexpl.aspx?CourseID=" + courseID + 
				@"', '"+ courseID+@"', 'width=430, height=530')");
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
			this.cmdReset.Click += new EventHandler(cmdReset_Click);
			this.cmdCancel.Click += new EventHandler(cmdCancel_Click);
			this.cmdEvaluate.Click += new EventHandler(cmdEvaluate_Click);
			this.cmdUserEvaluate.Click += new EventHandler(cmdUserEvaluate_Click);
			this.dgSections.PageIndexChanged += new DataGridPageChangedEventHandler(dgSections_PageIndexChanged);
			this.dgSections.ItemDataBound += new DataGridItemEventHandler(dgSections_ItemDataBound);
			this.cmdSkip.Click += new System.EventHandler(this.cmdSkip_Click);
			this.lstTests.SelectedIndexChanged += new System.EventHandler(this.lstTests_SelectedIndexChanged);
			this.cmdSubmit.Click += new System.EventHandler(this.cmdSubmit_Click);
			this.dgSections.ItemCommand += new DataGridCommandEventHandler(dgSections_ItemCommand);
			this.dgUserSearch.ItemDataBound += new DataGridItemEventHandler(dgUserSearch_ItemDataBound);
			this.cmdSearch.Click +=new EventHandler(cmdSearch_Click);
			this.tsUsers.SelectedIndexChange += new System.EventHandler(this.tsUsers_SelectedIndexChange);
			this.dgUserSearch.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgUserSearch_ItemDataBound);
			this.dgUserSearch.ItemCreated += new DataGridItemEventHandler(dgUserSearch_ItemCreated);
			this.dgUserSearch.PageIndexChanged += new DataGridPageChangedEventHandler(dgUserSearch_PageIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["asstID"] = ap.ID;

			Courses courseda = new Courses(Globals.CurrentIdentity);
			Assignment asst = new Assignments(Globals.CurrentIdentity).GetInfo(ap.ID);
			
			ViewState["courseID"] = asst.CourseID;

			BindSelectData();
		}

		private string[] HarvestSelectedUsers(System.Web.UI.WebControls.DataGrid sgrid) {
			string userstr="", prinstr="";
			Label lblType, lblID;
			Sections sectda = new Sections(Globals.CurrentIdentity);
			Users userda = new Users(Globals.CurrentIdentity);
			foreach (DataGridItem row in sgrid.Items) {
				if ((row.FindControl("chkSelect") as CheckBox).Checked) {
					lblType = (Label) row.FindControl("lblType");
					lblID = (Label) row.FindControl("lblID");
					string sid = lblID.Text;
					if (lblType.Text == "S") {
						int id = Convert.ToInt32(sid);
						User.UserList users = sectda.GetMembers(id);
						foreach (User user in users) {
							userstr += user.UserName + "|";
							prinstr += user.PrincipalID.ToString() + " ";
						}
					}
					else {
						userstr += sid + "|";
						User user = userda.GetInfo(sid, null);
						prinstr += user.PrincipalID.ToString() + " ";
					}
				}
			}
			
			string[] retarr = new string[2];
			retarr[0] = userstr; retarr[1] = prinstr;
			return retarr;
		}

		private void cmdEvaluate_Click(object sender, System.EventArgs e) {
			
			string[] retstrs = HarvestSelectedUsers(dgSections);

			mpViews.SelectedIndex = 2;
			Refresh(this, new RefreshEventArgs(retstrs[0], false, false));

			lblPrinStr.Text = retstrs[1];
			BindAutoData();
		}

		private void cmdUserEvaluate_Click(object sender, System.EventArgs e) {
			
			string[] retstrs = HarvestSelectedUsers(dgUserSearch);

			mpViews.SelectedIndex = 2;
			Refresh(this, new RefreshEventArgs(retstrs[0], false, false));

			lblPrinStr.Text = retstrs[1];
			BindAutoData();
		}


		private void lnkUserSearch_Click(object sender, System.EventArgs e) {
			mpViews.SelectedIndex = 3;	
		}

		private void dgSections_ItemCommand(object sender, DataGridCommandEventArgs e){
			
			string refreshString = "";
			string prinstr = "";
			int status = Components.Submission.UNGRADED;;
			User.UserList users=null;
			Label lblID = (Label) e.Item.FindControl("lblID");

			if (e.CommandName=="UnGraded")
				status = Components.Submission.UNGRADED;
			else if (e.CommandName == "Graded")
				status = Components.Submission.GRADED;

			users = new Sections(Globals.CurrentIdentity).GetStudentsBySubStatus(
				Convert.ToInt32(lblID.Text), GetAsstID(), status);
			
			if (users!=null){
				if (users.Count!=0){
					foreach (User user in users){
						refreshString += user.UserName + "|";
						prinstr += user.PrincipalID.ToString() + " ";
					}
					mpViews.SelectedIndex = 2;
					Refresh(this, new RefreshEventArgs(refreshString, false, false));
					lblPrinStr.Text = prinstr;
					BindAutoData();
				}
			}
		}

		private void dgSections_ItemDataBound(object sender, DataGridItemEventArgs e) {
			Label lblName,lblStudents, lblSubmissions, lblType, lblID;
			LinkButton lnkGraded, lnkUnGraded;
			System.Web.UI.WebControls.Image imgStatus, imgType;
			CheckBox chkSelect;

			if (null != (lblName = (Label) e.Item.FindControl("lblName"))) {
				
				//statistics to understand the progress of this section
				lblStudents = (Label) e.Item.FindControl("lblStudents");
				lblSubmissions = (Label) e.Item.FindControl("lblSubmissions");
				lnkGraded = (LinkButton) e.Item.FindControl("lnkGraded");
				lnkUnGraded = (LinkButton) e.Item.FindControl("lnkUnGraded");

				lblType = (Label) e.Item.FindControl("lblType");
				lblID = (Label) e.Item.FindControl("lblID");
				imgStatus = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgStatus");
				imgType = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgType");
				chkSelect = (CheckBox) e.Item.FindControl("chkSelect");

				if (e.Item.DataItem is Section) {
					BindSectionItem(e.Item.DataItem as Section,
						lblName, lnkGraded, lnkUnGraded, lblStudents, lblSubmissions, imgStatus, imgType, chkSelect);
					lblType.Text = "S";
					lblID.Text = (e.Item.DataItem as Section).ID.ToString();
				}
				else {
					BindUserItem(e.Item.DataItem as User,
						lblName, lblSubmissions, lnkGraded, imgStatus, imgType, chkSelect);
					lblType.Text = "U";
					lblID.Text = (e.Item.DataItem as User).UserName;
				}
			}
		}

		private void BindSectionItem(Section sec, Label lblName, 
			LinkButton lnkGraded, 
			LinkButton lnkUnGraded,
			Label lblStudents,
			Label lblSubmissions,
			System.Web.UI.WebControls.Image imgStatus, 
			System.Web.UI.WebControls.Image imgType, CheckBox chkSelect) {
			
			imgType.ImageUrl = "../../attributes/group.gif";
			lblName.Text = sec.Name;
			chkSelect.Checked = false;

			Section.SectionProgress progress = 
				new Sections(Globals.CurrentIdentity).GetGradingProgress(sec.ID, GetAsstID());

			int gradedperc = 
				Math.Min(100, Math.Max(0, (int)(((double)progress.TotalGraded / ((double)(progress.TotalSubmissions)))*100.0)));
			lnkGraded.Text = progress.TotalGraded.ToString() + " (" + gradedperc + "%)";
			lnkUnGraded.Text = (progress.TotalSubmissions - progress.TotalGraded).ToString() 
				+ " (" + (100-gradedperc).ToString() + "%)";
			lblStudents.Text = progress.TotalStudents.ToString();
			lblSubmissions.Text = progress.TotalSubmissions.ToString();

			if (progress.TotalGraded == progress.TotalSubmissions) 
				imgStatus.ImageUrl = "../../attributes/subgrade.gif";
			else
				imgStatus.Visible = false;
		}

		private ArrayList GetTests() {

			ArrayList tests = new ArrayList();
			foreach (ListItem item in lstTests.Items) 
				if (item.Selected)
					tests.Add(Convert.ToInt32(item.Value));
	
			return tests;
		}

		private void BindUserItem(User user, Label lblName, Label numSubmissions, LinkButton lnkProgress,
			System.Web.UI.WebControls.Image imgStatus, 
			System.Web.UI.WebControls.Image imgType, CheckBox chkSelect) {

			imgType.ImageUrl = "../../attributes/user.gif";
			lblName.Text = user.FullName + "(" + user.UserName + ")";
			if (lnkProgress == null) lnkProgress = new LinkButton();

			Principals prinda = new Principals(Globals.CurrentIdentity);
			Components.Submission sub = prinda.GetLatestSubmission(user.PrincipalID, GetAsstID());
			int numsubs = prinda.GetSubmissions(user.PrincipalID, GetAsstID()).Count;
			numSubmissions.Text = numsubs.ToString();

			if (sub == null) {
				imgStatus.ImageUrl = "../../attributes/nosub.gif";
				lnkProgress.Text = "N/A";
				chkSelect.Enabled = false;
			}
			else {
				switch (sub.Status) {
					case Components.Submission.GRADED:
						imgStatus.ImageUrl = "../../attributes/subgrade.gif";
						lnkProgress.Text = "100%";
						break;
					case Components.Submission.INPROGRESS:
						imgStatus.ImageUrl = "../../attributes/clock.gif";
						lnkProgress.Text = "??%";
						break;
					case Components.Submission.UNGRADED:
						imgStatus.ImageUrl = "../../attributes/sub.gif";
						lnkProgress.Text = "0%";
						break;
				}
			}
		}

		private ArrayList GetPrincipals() {
			string prinstr = lblPrinStr.Text;
			string[] sprins = prinstr.Split(" ".ToCharArray());
			ArrayList prins = new ArrayList();
			foreach (string sprin in sprins) 
				if (sprin.Length > 0)
					prins.Add(Convert.ToInt32(sprin));
			return prins;
		}	

		private void cmdSubmit_Click(object sender, EventArgs e) {
		
			ArrayList prins = GetPrincipals();
			ArrayList tests = GetTests();
			AutoJobs jobs = new AutoJobs(Globals.CurrentIdentity);
			Principals aprins = new Principals(Globals.CurrentIdentity);

			//Check for nothing
			if (tests.Count == 0 || prins.Count == 0) {
				PageAutoError("Must select at least one user and one test to create an auto job");
				return;
			}

			AutoJob job = jobs.Create(txtName.Text, GetAsstID());
			foreach (int prin in prins) {
				foreach (int evalid in tests) {
					try {
						Components.Submission sub = 
							aprins.GetLatestSubmission(prin, GetAsstID());	
						if (sub != null)
							jobs.CreateTest(job.ID, sub.ID, evalid, false);				
					} catch (DataAccessException er) {
						PageAutoError(er.Message);
						return;
					}
				}
			}		
	  
			mpViews.SelectedIndex = 1;
		}

		private void dgSections_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {
			dgSections.CurrentPageIndex = e.NewPageIndex;
			BindSelectData();
		}

		private void cmdReset_Click(object sender, EventArgs e) {
			mpViews.SelectedIndex = 0;
			Refresh(this, new RefreshEventArgs(CourseMatrixControl.REFRESH_RESET, false, false));
		}

		private void cmdCancel_Click(object sender, EventArgs e) {
			mpViews.SelectedIndex = 0;
			Refresh(this, new RefreshEventArgs(CourseMatrixControl.REFRESH_RESET, false, false));
		}

		private void cmdSkip_Click(object sender, EventArgs e) {
			mpViews.SelectedIndex = 1;
		}

		
		private void cmdSearch_Click(object sender, EventArgs e){
			User.UserList foundUsers = null;
			//to get the course id, you can use the current assignment
			Assignment asst = new Assignments(Globals.CurrentIdentity).GetInfo(GetAsstID());
			if (drpSearchMethod.SelectedValue == "username")
				foundUsers = new Courses(Globals.CurrentIdentity).GetMembersByUsername(txtSearch.Text, asst.CourseID);
			else if (drpSearchMethod.SelectedValue == "lastname")
				foundUsers = new Courses(Globals.CurrentIdentity).GetMembersByLastname(txtSearch.Text, asst.CourseID);
		
			//clear the text box and then fill the dialog box
			txtSearch.Text="";
			BindGroups(foundUsers);
			BindSearchData(foundUsers);
		}

		private void tsUsers_SelectedIndexChange(object sender, System.EventArgs e) {
			BindSelectData();
		}

		private void dgUserSearch_ItemDataBound(object sender, DataGridItemEventArgs e) {
			Label lblName, lblType, lblID, lblNumSubmissions;
			System.Web.UI.WebControls.Image imgStatus, imgType;
			CheckBox chkSelect;

			if (null != (lblName = (Label) e.Item.FindControl("lblName"))) {
				lblType = (Label) e.Item.FindControl("lblType");
				lblID = (Label) e.Item.FindControl("lblID");
				imgStatus = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgStatus");
				imgType = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgType");
				lblNumSubmissions = (Label) e.Item.FindControl("lblNumSubmissions");

				chkSelect = (CheckBox) e.Item.FindControl("chkSelect");

				BindUserItem(e.Item.DataItem as User,
					lblName, lblNumSubmissions, null, imgStatus, imgType, chkSelect);
				lblType.Text = "U";
				lblID.Text = (e.Item.DataItem as User).UserName;
				
			}
		}

		private void dgUserSearch_ItemCreated(object sender, DataGridItemEventArgs e) {
			//Take the pager over
			if (e.Item.ItemType == ListItemType.Pager) {
				TableCell pager = e.Item.Cells[0];
				string[] groups = GetGroups().Split("\n".ToCharArray());

				//Change the names of the numbers to group descs
				foreach (Control c in pager.Controls) {
					if (c is LinkButton) {
						LinkButton lc = (LinkButton) c;
						if (lc.Text != "...") {
							string[] bounds = groups[Convert.ToInt32(lc.Text)-1].Split(" ".ToCharArray());
							lc.Text = bounds[1].ToUpper() + "-" + bounds[0].ToUpper();
						}
					} else if (c is Label) {
						Label lc = (Label) c;
						if (lc.Text != "...") {
							string[] bounds = groups[Convert.ToInt32(lc.Text)-1].Split(" ".ToCharArray());
							lc.Text = bounds[1].ToUpper() + "-" + bounds[0].ToUpper();
						}
					}
				}
			}
		}

		private void dgUserSearch_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {
			dgUserSearch.CurrentPageIndex = e.NewPageIndex;
			BindSelectData();
		}
	}
}
