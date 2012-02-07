using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Microsoft.Web.UI.WebControls;

using System.IO;
using System.Collections;

using FrontDesk.Pages;
using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Tools;

namespace FrontDesk.Controls.Matrix {

	/// <summary>
	/// Assignment details view
	/// </summary>
	public class AssignmentView : Pagelet, IMatrixControl {
		protected System.Web.UI.WebControls.TextBox txtAsstName;
		protected System.Web.UI.WebControls.TextBox txtAsstDueDate;
		protected System.Web.UI.WebControls.Button cmdAsstUpdate;
		protected System.Web.UI.WebControls.Label lblAsstError;
		protected System.Web.UI.WebControls.CheckBox chkAvailable;
		protected System.Web.UI.WebControls.CheckBox chkEvaluation;
		protected Microsoft.Web.UI.WebControls.TreeView tvFormat;
		protected System.Web.UI.WebControls.Label lblContentID;
		protected Microsoft.Web.UI.WebControls.Toolbar tbActions;
		protected System.Web.UI.WebControls.Label lblCourseID;
        
		//locations of toolbar elements
		public const int TB_FOLDER=0, TB_FILE=1, TB_DELETE=2, TB_SAVE=3;
		protected System.Web.UI.WebControls.TextBox txtFileName;
		protected System.Web.UI.WebControls.Label lblFormatError;
		protected System.Web.UI.WebControls.Button cmdFormatUpdate;

		private void Page_Load(object sender, System.EventArgs e) {
			lblAsstError.Visible = lblFormatError.Visible = false;
			(tbActions.Items[TB_FOLDER] as Microsoft.Web.UI.WebControls.ToolbarButton).ButtonClick += 
				new Microsoft.Web.UI.WebControls.ToolbarItemEventHandler(tb_CreateFolder);
			(tbActions.Items[TB_FILE] as Microsoft.Web.UI.WebControls.ToolbarButton).ButtonClick += 
				new Microsoft.Web.UI.WebControls.ToolbarItemEventHandler(tb_CreateFile);
			(tbActions.Items[TB_DELETE] as Microsoft.Web.UI.WebControls.ToolbarButton).ButtonClick += 
				new Microsoft.Web.UI.WebControls.ToolbarItemEventHandler(tb_Delete);
			(tbActions.Items[TB_SAVE] as Microsoft.Web.UI.WebControls.ToolbarButton).ButtonClick += 
				new Microsoft.Web.UI.WebControls.ToolbarItemEventHandler(tb_Save);
		}

		private void PageAsstError(string msg) {
			lblAsstError.Text = msg;
			lblAsstError.Visible = true;
		}

		private void PageFormatError(string msg) {
			lblFormatError.Text = msg;
			lblFormatError.Visible = true;
		}

		private void BindAsstView(int asstID) {

			Assignment asst = new Assignments(Globals.CurrentIdentity).GetInfo(asstID);

			txtAsstName.Text = asst.Description;
			txtAsstDueDate.Text = asst.DueDate.ToString();
			chkAvailable.Checked = asst.StudentRelease;
			chkEvaluation.Checked = asst.StudentSubmit;
			lblContentID.Text = asst.ContentID.ToString();
			lblCourseID.Text = asst.CourseID.ToString();
			
			DataSet zonedesc = new DataSet();
			MemoryStream memstream = 
				new MemoryStream(System.Text.Encoding.ASCII.GetBytes(asst.Format));
			zonedesc.ReadXml(memstream);
			
			LoadFormatView(zonedesc, "");
		}

		public void LoadFormatView(DataSet zonefiles, string imgPrefix){
			tvFormat.Nodes.Clear();
			DataTable inodes = zonefiles.Tables["File"];
			
			Hashtable dirHash = new Hashtable();
				
			foreach (DataRow row in inodes.Rows){
				string type = (string)row["type"];
				string fullPath = (string)row["name"];
				string dirName = Path.GetDirectoryName(fullPath);
				string itemName = Path.GetFileName(fullPath);
				if (itemName=="."){
					TreeNode root = AddFolderNode(tvFormat.Nodes, ".", imgPrefix);
					dirHash["."] = root;				
				}else{
					if (type=="dir"){
						dirHash[fullPath] = AddFolderNode(((TreeNode)dirHash[dirName]).Nodes, itemName, imgPrefix); 
					}else{
						AddFileNode(((TreeNode)dirHash[dirName]).Nodes, itemName, imgPrefix); 
					}
				}
			}

			TreeNode curnode = tvFormat.GetNodeFromIndex(tvFormat.SelectedNodeIndex);
			SyncToolbar(curnode);
			txtFileName.Text = curnode.Text;
		}

		private TreeNode AddFolderNode(TreeNodeCollection par, string folderName, string imgPrefix){
			return AddNode(par, folderName, imgPrefix + "attributes/folder.gif", imgPrefix + "attributes/folderopen.gif", "dir", true);	
		}

		private TreeNode AddFileNode(TreeNodeCollection par, string fileName, string imgPrefix){
			return AddNode(par, fileName, imgPrefix + "attributes/book.gif", imgPrefix + "attributes/book.gif", "file", false);	
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
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
		private void InitializeComponent() {
			this.cmdAsstUpdate.Click += new System.EventHandler(this.cmdAsstUpdate_Click);
			this.tvFormat.SelectedIndexChange += new Microsoft.Web.UI.WebControls.SelectEventHandler(this.tvFormat_SelectedIndexChange);
			this.cmdFormatUpdate.Click += new System.EventHandler(this.cmdFormatUpdate_Click);;
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["asstid"] = ap.ID;
			BindAsstView(ap.ID);
		}

		private void cmdAsstUpdate_Click(object sender, EventArgs e) {

			Assignment asst = new Assignment();

			asst.ID = (int) ViewState["asstid"];
			asst = new Assignments(Globals.CurrentIdentity).GetInfo(asst.ID);
			asst.Description = txtAsstName.Text;
			asst.DueDate = Convert.ToDateTime(txtAsstDueDate.Text);
			asst.ContentID = Convert.ToInt32(lblContentID.Text);
			asst.StudentRelease = chkAvailable.Checked;
			asst.StudentSubmit = chkEvaluation.Checked;
			asst.CourseID = Convert.ToInt32(lblCourseID.Text);

			try {
				new Assignments(Globals.CurrentIdentity).Update(asst);
			} catch (DataAccessException er) {
				PageAsstError(er.Message);
			}

			Refresh(this, new RefreshEventArgs("", true, false));
		}

		private void cmdFormatUpdate_Click(object sender, EventArgs e) {
			TreeNode node = GetCurrentNode();
		
			if (node.Text != ".")
				node.Text=txtFileName.Text;	
			else
				PageFormatError("Error: Cannot update the name of the root element");
		}

		private TreeNode AddNode(TreeNodeCollection parentsChildren, string text, string image, string expimage,
			string nodedata, bool expandable) {
			TreeNode root = new TreeNode();
			root.Text = text;
			root.ImageUrl = image;
			root.ExpandedImageUrl = expimage;
			root.Expanded = true;
			
			if (expandable)
				root.Expandable = ExpandableValue.Always;
			root.NodeData = nodedata;
			parentsChildren.Add(root);
			return root;
		}

		private string GetPathForCurrentNode(TreeNode node) {
			
			string str="";
			TreeNode tmp = node;
			if (tmp.Text=="."){
				return ".";
			}
			while (tmp!=null){
				str = tmp.Text + str;
				tmp = (TreeNode)tmp.Parent;
				str = "\\" + str;
				if (tmp.Text=="."){
					str = "."+str;
					break;
				}
			}
			return str;
		}

		private string GenerateXMLForStructure(TreeNode rootNode){
			string start = GetXMLForCurrentNode(rootNode);
			foreach (TreeNode child in rootNode.Nodes){
				start += GenerateXMLForStructure(child);
			}
			return start;
		}
		private string GetXMLForCurrentNode(TreeNode node) {
			string xmlString = "<File><type>"+node.NodeData+"</type><name>"+GetPathForCurrentNode(node)+"</name></File>";
			return xmlString;
		}

		private TreeNode GetCurrentNode() {
			return tvFormat.GetNodeFromIndex(tvFormat.SelectedNodeIndex);
		}

		private bool tb_CreateFolder(object sender, EventArgs e){
			TreeNode node = AddFolderNode(GetCurrentNode().Nodes, "NewFolder", "");	
			tvFormat.SelectedNodeIndex = node.GetNodeIndex();
			SyncToolbar(node);
			txtFileName.Text = node.Text;

			return true;
		}
		
		private bool tb_CreateFile(object sender, EventArgs e){
			TreeNode node = AddFileNode(GetCurrentNode().Nodes, "NewFile.txt", "");
			tvFormat.SelectedNodeIndex = node.GetNodeIndex();
			SyncToolbar(node);
			txtFileName.Text = node.Text;

			return true;
		}

		private bool tb_Delete(object sender, EventArgs e){
			TreeNode node = GetCurrentNode();
			TreeNode parent = (TreeNode)node.Parent;
			parent.Nodes.Remove(node);

			TreeNode curnode = tvFormat.GetNodeFromIndex(tvFormat.SelectedNodeIndex);
			SyncToolbar(curnode);
			txtFileName.Text = curnode.Text;

			return true;
		}

		private bool tb_Save(object sender, EventArgs e){
			//needs to navigate in order thru the current tree and write
			//the structure to and xml string which is then written to the database
			//the order of forming this final xml is important since it determines
			//the order in which nodes are created in the tree
			TreeNode rootNode = tvFormat.Nodes[0];
			string xmlString = GenerateXMLForStructure(rootNode);
			Assignment asst = new Assignments(Globals.CurrentIdentity).GetInfo((int)ViewState["asstid"]);
			asst.Format = "<Root>"+xmlString+"</Root>";
			new Assignments(Globals.CurrentIdentity).Update(asst);
			
			return true;
		}

		private void SyncToolbar(TreeNode node) {
			if (node.NodeData=="file") {
				//grey the buttons out
				(tbActions.Items[TB_FOLDER] as ToolbarButton).Enabled=false;
				(tbActions.Items[TB_FOLDER] as ToolbarButton).ImageUrl="attributes/foldergray.gif";

				(tbActions.Items[TB_FILE] as ToolbarButton).Enabled=false;
				(tbActions.Items[TB_FILE] as ToolbarButton).ImageUrl="attributes/bookgray.gif";
			}
			else {
				//reenable
				(tbActions.Items[TB_FOLDER] as ToolbarButton).Enabled=true;
				(tbActions.Items[TB_FOLDER] as ToolbarButton).ImageUrl="attributes/folder.gif";

				(tbActions.Items[TB_FILE] as ToolbarButton).Enabled=true;
				(tbActions.Items[TB_FILE] as ToolbarButton).ImageUrl="attributes/book.gif";
			}
		}

		private void tvFormat_SelectedIndexChange(object sender, Microsoft.Web.UI.WebControls.TreeViewSelectEventArgs e){
			TreeNode node = tvFormat.GetNodeFromIndex(e.NewNode);

			SyncToolbar(node);
			txtFileName.Text = node.Text;		
		}

	}

}
