using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Microsoft.Web.UI.WebControls;

using FrontDesk.Pages;
using FrontDesk.Data.Filesys;
using FrontDesk.Data.Access;
using FrontDesk.Components.Filesys;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Controls.Filesys;
using FrontDesk.Controls;
using FrontDesk.Controls.Submission;
using FrontDesk.Common;

namespace FrontDesk.Controls {

	//Event hook
	public delegate void RubricViewSelectEventHandler(object sender, RubricViewSelectEventArgs args);
	public delegate void RubricViewSelectResultEventHandler(object sender, RubricViewSelectResultEventArgs args);

	/// <summary>
	///	Rubric view control
	/// </summary>
	public class RubricViewControl : Pagelet {
		protected System.Web.UI.HtmlControls.HtmlGenericControl divMain;

		public event RubricViewSelectEventHandler RubricSelect;
		public event RubricViewSelectResultEventHandler ResultSelect;

		protected Microsoft.Web.UI.WebControls.TreeView tvRubric;

		public RubricViewControl() {
			ViewState["repAuto"] = false;
			ViewState["expandSubj"] = true;
			ViewState["width"] = "95%";
			ViewState["height"] = "130px";
		}

		private void Page_Load(object sender, System.EventArgs e) {
			divMain.Style["WIDTH"] = Width;
			divMain.Style["HEIGHT"] = Height;
			tvRubric.Height = new Unit("95%");
		}

		public string Width {
			get { return (string) ViewState["width"]; }
			set { ViewState["width"] = value; }
		}

		public string Height {
			get { return (string) ViewState["height"]; }
			set { ViewState["height"] = value; }
		}

		public bool ExpandSubj {
			get { return (bool) ViewState["expandSubj"]; }
			set { ViewState["expandSubj"] = value; }
		}

		public bool RepressAutos {
			get { return (bool) ViewState["repAuto"]; }
			set { ViewState["repAuto"] = value; }
		}

		private int GetSubID() {
			return (int) ViewState["subID"];
		}

		public void InitRubric(Rubric rub, int subID, string imgPrefix) {
			tvRubric.Nodes.Clear();

			ViewState["subID"] = subID;

			TreeNode root = AddRubricNode(tvRubric.Nodes, rub, imgPrefix);
			LoadTreeView(root, imgPrefix);
		}

		private TreeNode AddSubjResultNode(TreeNodeCollection par, SubjResult res, string imgPrefix) {
			string name = String.Format("{0} ({1} points)", res.Comment, res.Points);
			return AddNode(par, name, imgPrefix + "attributes/book.gif", imgPrefix + "attributes/book.gif", "Result " + res.ID, 
				false);
		}

		private TreeNode AddRubricNode(TreeNodeCollection par, Rubric rub, string imgPrefix) {
			
			string img, expimage, text;
			bool expand;
			
			if (new Rubrics(Globals.CurrentIdentity).IsHeading(rub)) {
				if (rub.ParentID < 0)
					text = String.Format("Total - {0}/{1} Points", 
						new Submissions(Globals.CurrentIdentity).GetPoints(GetSubID()), 
						rub.Points);
				else
					text = rub.Name;
				img = imgPrefix + "attributes/folder.gif";
				expimage = imgPrefix + "attributes/folderopen.gif";
				expand = true;
			}
			else {		
				text = GetResultPoints(rub);
				if (rub.EvalID < 0) {
					img = imgPrefix + "attributes/book.gif";
					expimage = imgPrefix + "attributes/book.gif";
					expand = ExpandSubj;
				}
				else {
					img = imgPrefix + "attributes/cyl.gif";
					expimage = imgPrefix + "attributes/cyl.gif";
					expand = false;
				}
			}

			return AddNode(par, text, img, expimage, "Rubric " + rub.ID.ToString(), expand);
		}

		private TreeNode AddNode(TreeNodeCollection par, string text, string image, string expimage,
			string nodedata, bool expandable) {
			TreeNode root = new TreeNode();
			root.Text = text;
			root.ImageUrl = image;
			root.ExpandedImageUrl = expimage;
			root.Expanded = true;
			if (expandable)
				root.Expandable = ExpandableValue.Always;
			root.NodeData = nodedata;
			par.Add(root);
			return root;
		}

		public void UpdateRubric() {
			TreeNode node = tvRubric.GetNodeFromIndex(tvRubric.SelectedNodeIndex);
			Rubrics rubda = new Rubrics(Globals.CurrentIdentity);

			Rubric rub = rubda.GetInfo(GetRubricID(node));
			node.Text = GetResultPoints(rub);

			TreeNode root = tvRubric.Nodes[0];
			rub = rubda.GetInfo(GetRubricID(root));
			root.Text =
				String.Format("Total - {0}/{1} Points", 
				new Submissions(Globals.CurrentIdentity).GetPoints(GetSubID()), 
				rub.Points);
		}

		public int GetCurrentRubricID() {
			return GetRubricID(tvRubric.GetNodeFromIndex(tvRubric.SelectedNodeIndex));
		}

		private int GetRubricID(TreeNode node) {
			return Convert.ToInt32(node.NodeData.Split(" ".ToCharArray())[1]);
		}

		private int GetResultID(TreeNode node) {
			return GetRubricID(node);
		}

		private string GetResultPoints(Rubric rub) {
			Result.ResultList ress = 
				new Rubrics(Globals.CurrentIdentity).GetResults(rub.ID, GetSubID());
			if (ress.Count == 0 && rub.EvalID >= 0)
				return String.Format("{0} (Ungraded, {1} points)", rub.Name, rub.Points);

			return String.Format("{0} ({1}/{2} points)", rub.Name, 
				new Rubrics(Globals.CurrentIdentity).GetPoints(rub.ID, GetSubID()), rub.Points); 
		}

		private void LoadTreeView(TreeNode node, string imgPrefix) {
			Rubric.RubricList rubs = 
				new Rubrics(Globals.CurrentIdentity).GetChildren(GetRubricID(node));
			foreach (Rubric rub in rubs) {
				TreeNode rnode;
				if (RepressAutos && rub.EvalID >= 0)
					continue;
				else
					rnode = AddRubricNode(node.Nodes, rub, imgPrefix);
				LoadTreeView(rnode, imgPrefix);
			}

			//Add subjective results in under books
			if (ExpandSubj) {
				Result.ResultList ress = new Rubrics(Globals.CurrentIdentity).GetResults(GetRubricID(node), GetSubID());
				foreach (Result res in ress) 
					if (res is SubjResult)
						AddSubjResultNode(node.Nodes, (SubjResult)res, imgPrefix);
			}
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
			this.tvRubric.SelectedIndexChange += new Microsoft.Web.UI.WebControls.SelectEventHandler(this.tvRubric_SelectedIndexChange);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void tvRubric_SelectedIndexChange(object sender, TreeViewSelectEventArgs e) {
			TreeNode node = tvRubric.GetNodeFromIndex(e.NewNode);
			if (node.NodeData.Split(" ".ToCharArray())[0] == "Rubric") {
				int rubID = GetRubricID(node);
				Rubric rub = new Rubrics(Globals.CurrentIdentity).GetInfo(rubID);
				if (RubricSelect != null)
					RubricSelect(this, new RubricViewSelectEventArgs(rub));
			} else {
				int resID = GetResultID(node);
				Result res = new Results(Globals.CurrentIdentity).GetInfo(resID);
				if (ResultSelect != null)
					ResultSelect(this, new RubricViewSelectResultEventArgs(res));
			}
		}
	}

	public class RubricViewSelectEventArgs : EventArgs {
		public RubricViewSelectEventArgs(Rubric rub) { 
			SelectedRubric = rub;
		}
		public Rubric SelectedRubric;
	}

	public class RubricViewSelectResultEventArgs : EventArgs {
		public RubricViewSelectResultEventArgs(Result res) { 
			SelectedResult = res;
		}
		public Result SelectedResult;
	}
}
