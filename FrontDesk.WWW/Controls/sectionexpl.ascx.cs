using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Collections;
using Microsoft.Web.UI.WebControls;

using FrontDesk.Pages;
using FrontDesk.Components;
using FrontDesk.Data.Access;
using FrontDesk.Common;

namespace FrontDesk.Controls {

	/// <summary>
	///	Section explorer user control
	/// </summary>
	public class SectionExplorer : Pagelet {
		protected System.Web.UI.HtmlControls.HtmlGenericControl divSection;

		protected Microsoft.Web.UI.WebControls.TreeView tvSection;

		public SectionExplorer() {
			ViewState["showchecks"] = false;
			ViewState["principals"] = new ArrayList();
			ViewState["restrict"] = -1;
		}

		private void Page_Load(object sender, EventArgs e) {
			int sectionID = Convert.ToInt32(HttpContext.Current.Request.Params["SectionID"]);
			
			if (!IsPostBack)
				BindSectionTree(sectionID);
		}

		public Unit Height {
			get { return tvSection.Height; }
			set { tvSection.Height = value; divSection.Style["HEIGHT"] = value.ToString(); }
		}

		public Unit Width {
			get { return tvSection.Width; }
			set { tvSection.Width = value; divSection.Style["WIDTH"] = value.ToString(); }
		}

		public bool ShowChecks {
			get { return Convert.ToBoolean(ViewState["showchecks"]); }
			set { ViewState["showchecks"] = value; }
		}

		public ArrayList Principals {
			get { return (ArrayList) ViewState["principals"]; }
			set { ViewState["principals"] = value; }
		}

		public int RestrictAsst {
			get { return (int) ViewState["restrict"]; }
			set { ViewState["restrict"] = value; }
		}

		private void BindSectionTree(int sectionID) {
			int courseID = Convert.ToInt32(HttpContext.Current.Request.Params["CourseID"]);
			AddSections(courseID);
			AddAllGroups(courseID);
			AddAllUsers(courseID);
		}

		private void AddAllGroups(int courseID) {
			
			TreeNode groups = AddNode(tvSection.Nodes, false, "All Submission Groups", "attributes/group.jpg", null);
			Assignment.AssignmentList assts = (new Courses(Globals.CurrentIdentity)).GetAssignments(courseID);
			foreach (Assignment asst in assts) 
				AddNode(groups.Nodes, false, asst.Description, "attributes/folder.gif", "attributes/folderopen.gif",
					"AsstG " + asst.ID);
		}

		private void AddAllUsers(int courseID) {

			TreeNode courses = AddNode(tvSection.Nodes, true, "All Users", "attributes/group.jpg", 
									   "Uallusers");
			User.UserList mems = new Courses(Globals.CurrentIdentity).GetMembers(courseID, null);
			foreach (User mem in mems) {
				TreeNode root = AddNode(courses.Nodes, true,
					mem.FullName + " (Username: " + mem.UserName + ")" , 
					"attributes/user.jpg", "Uname " + mem.UserName);

				AddNode(root.Nodes, false, "Sections", "attributes/folder.gif", "attributes/folderopen.gif",
					"USects " + mem.UserName);
				AddNode(root.Nodes, false, "Submission Groups", "attributes/folder.gif", "attributes/folderopen.gif",
					"UGrous " + mem.UserName);
			}
		}

		private void AddSections(int courseID) {
			Section.SectionList sections = (new Courses(Globals.CurrentIdentity)).GetSections(courseID);
			foreach (Section section in sections) 
				AddNode(tvSection.Nodes, true, section.Name + " (Owner: " + section.Owner + ")",
					"attributes/group.jpg", "Section " + section.ID);
		}

		private void tvSection_Expand(object sender, TreeViewClickEventArgs e) {
			TreeNode node = tvSection.GetNodeFromIndex(e.Node);
			if (node.Nodes.Count == 0) {
				if (node.NodeData.IndexOf("Section") == 0)
					AddSectionMembers(node);
				else if (node.NodeData.IndexOf("USects") == 0)
					AddUserSections(node);
				else if (node.NodeData.IndexOf("UGrous") == 0)
					AddUserGroups(node);
				else if (node.NodeData.IndexOf("GrouAsst") == 0)
					AddAsstGroups(node);
				else if (node.NodeData.IndexOf("Group") == 0)
					AddGroupMembers(node);
				else if (node.NodeData.IndexOf("AsstG") == 0)
					AddAllAsstGroups(node);
			}
		}

		private void AddAllAsstGroups(TreeNode node) {
			string[] tokens = node.NodeData.Split(" ".ToCharArray());
			int asstID = Convert.ToInt32(tokens[1]);

			Group.GroupList groups = (new Assignments(Globals.CurrentIdentity)).GetGroups(asstID);
			foreach (Group group in groups) 
				AddNode(node.Nodes, true, group.Name, "attributes/group.jpg", "Group " + group.PrincipalID);
		}

		private void AddGroupMembers(TreeNode node) {
			string[] tokens = node.NodeData.Split(" ".ToCharArray());
			int groupID = Convert.ToInt32(tokens[1]);

			User.UserList mems = (new Groups(Globals.CurrentIdentity)).GetMembership(groupID);
			foreach (User user in mems) {

				TreeNode root = AddNode(node.Nodes, false, user.FullName + " (Username: " + user.UserName + ")",
					"attributes/user.jpg", "Uname " + user.UserName);

				AddNode(root.Nodes, false, "Sections", "attributes/folder.gif", "attributes/folderopen.gif",
					"USects " + user.UserName);
				AddNode(root.Nodes, false, "Submission Groups", "attributes/folder.gif", "attributes/folderopen.gif",
					"UGrous " + user.UserName);
			}
		}

		private void AddAsstGroups(TreeNode node) {
			string[] tokens = node.NodeData.Split(" ".ToCharArray());
			int asstID = Convert.ToInt32(tokens[1]);
			string username = tokens[2];

			Group.GroupList groups = (new Users(Globals.CurrentIdentity)).GetGroups(username, asstID);
			foreach (Group group in groups) 
				AddNode(node.Nodes, true, group.Name, "attributes/group.jpg", "Group " + group.PrincipalID);
		}

		private void AddUserGroups(TreeNode node) {	
			string[] tokens = node.NodeData.Split(" ".ToCharArray());
			string username = tokens[1];
			int courseID = Convert.ToInt32(HttpContext.Current.Request.Params["CourseID"]);
			int resid = RestrictAsst;

			Assignment.AssignmentList assts = (new Courses(Globals.CurrentIdentity)).GetAssignments(courseID);
			foreach (Assignment asst in assts) 
				if (resid < 0 || resid == asst.ID)
					AddNode(node.Nodes, false, asst.Description, "attributes/folder.gif", "attributes/folderopen.gif",
						"GrouAsst " + asst.ID + " " + username);	
		}

		private void AddUserSections(TreeNode node) {
			string[] tokens = node.NodeData.Split(" ".ToCharArray());
			string username = tokens[1];
	
			int courseID = Convert.ToInt32(HttpContext.Current.Request.Params["CourseID"]);
			Section.SectionList sections = (new Users(Globals.CurrentIdentity)).GetSections(username, courseID);

			foreach (Section section in sections) 
				AddNode(node.Nodes, true, section.Name + " (Owner: " + section.Owner + ")",
					"attributes/group.jpg", "Section " + section.ID);
		}

		private void AddSectionMembers(TreeNode node) {

			string[] tokens = node.NodeData.Split(" ".ToCharArray());
			int sectionID = Convert.ToInt32(tokens[1]);

			User.UserList mems = (new Sections(Globals.CurrentIdentity)).GetMembers(sectionID);
			foreach (User user in mems) {

				TreeNode root = AddNode(node.Nodes, true, user.FullName + " (Username: " + user.UserName + ")",
					"attributes/user.jpg", "Uname " + user.UserName);

				AddNode(root.Nodes, false, "Sections", "attributes/folder.gif", "attributes/folderopen.gif",
					"USects " + user.UserName);
				AddNode(root.Nodes, false, "Submission Groups", "attributes/folder.gif", "attributes/folderopen.gif",
					"UGrous " + user.UserName);
			}
		}

		private TreeNode AddNode(TreeNodeCollection par, bool chked, string text, string image, string nodedata) {
			return AddNode(par, chked, text, image, null, nodedata);
		}

		private TreeNode AddNode(TreeNodeCollection par, bool chked, string text, string image, string expimage,
			string nodedata) {
			TreeNode root = new TreeNode();
			root.Text = text;
			root.ImageUrl = image;
			root.ExpandedImageUrl = expimage;
			root.Expandable = ExpandableValue.Always;
			root.NodeData = nodedata;
			root.CheckBox = ShowChecks && chked;
			par.Add(root);
			return root;
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
			this.tvSection.Expand += new Microsoft.Web.UI.WebControls.ClickEventHandler(this.tvSection_Expand);
			this.tvSection.Check += new Microsoft.Web.UI.WebControls.ClickEventHandler(this.tvSection_Check);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void tvSection_Check(object sender, TreeViewClickEventArgs e) {
			
			TreeNode node = tvSection.GetNodeFromIndex(e.Node);
			Users users = new Users(Globals.CurrentIdentity);
			int courseID = Convert.ToInt32(HttpContext.Current.Request.Params["CourseID"]);
			string[] tokens;

			if (node.NodeData.IndexOf("Uname") == 0) {
				tokens = node.NodeData.Split(" ".ToCharArray());
				int pid = users.GetInfo(tokens[1], null).PrincipalID;
				ArrayList prins = Principals;
				if (node.Checked) 
					prins.Add(pid);
				else 
					prins.Remove(pid);
				
				Principals = prins;
			}
			else if (node.NodeData.IndexOf("Group") == 0) {
				tokens = node.NodeData.Split(" ".ToCharArray());
				int pid = Convert.ToInt32(tokens[1]);
				ArrayList prins = Principals;
				if (node.Checked) 
					prins.Add(pid);
				else 
					prins.Remove(pid);
				
				Principals = prins;
			}
			else if (node.NodeData.IndexOf("Uallusers") == 0) {
				User.UserList mems = new Courses(Globals.CurrentIdentity).GetMembers(courseID, null);
				ArrayList prins = Principals;
				
				foreach (TreeNode cnode in node.Nodes)
					cnode.Checked = node.Checked;
				
				foreach (User mem in mems) {
					if (node.Checked)
						prins.Add(mem.PrincipalID);
					else
						prins.Remove(mem.PrincipalID);
				}

				node.Expanded = true;
				Principals = prins;
			}
			else if (node.NodeData.IndexOf("Section") == 0) {
				tokens = node.NodeData.Split(" ".ToCharArray());
				User.UserList susers = (new Sections(Globals.CurrentIdentity)).GetMembers(
					Convert.ToInt32(tokens[1]));
				
				if (node.Nodes.Count == 0)
					AddSectionMembers(node);
				
				foreach (TreeNode cnode in node.Nodes)
					cnode.Checked = node.Checked;

				ArrayList prins = Principals;
				foreach (User user in susers) {
					if (node.Checked)
						prins.Add(user.PrincipalID);
					else
						prins.Remove(user.PrincipalID);
				}

				node.Expanded = true;
				Principals = prins;
			}
			
		}

	}
}
