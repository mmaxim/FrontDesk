using System;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Configuration;
using System.Web.Security;
using System.IO;
using Microsoft.Web.UI.WebControls;

using FrontDesk.Common;
using FrontDesk.Pages;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Filesys;
using FrontDesk.Data.Access;
using FrontDesk.Tools;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	///		Summary description for filebrowser.
	/// </summary>
	public class FileBrowserPagelet : Pagelet {

		protected Microsoft.Web.UI.WebControls.Toolbar tbActions;
		protected System.Web.UI.WebControls.Label lblClipboard;
		protected Microsoft.Web.UI.WebControls.TreeView tvFiles;
		protected System.Web.UI.WebControls.Label lblMessages;
		protected System.Web.UI.WebControls.DataGrid dgFiles;
		protected System.Web.UI.WebControls.Button cmdUpload;
		protected System.Web.UI.HtmlControls.HtmlInputFile fiUpload;
		protected System.Web.UI.WebControls.HyperLink hypClose;

		[Serializable]
		protected class ClipBoard {

			public ClipBoard() {
				m_files = new ArrayList();
				m_move = false;
			}

			public ClipBoard(ArrayList files, bool move) {
				m_files = files; m_move = move;
			}

			public ArrayList FileIDs {
				get { return m_files; }
				set { m_files = value; }
			}

			public CFile.FileList Files {
				get {
					FileSystem fs = new FileSystem(Globals.CurrentIdentity);
					CFile.FileList files = new CFile.FileList();
					foreach (int fileid in m_files)
						files.Add(fs.GetFile(fileid));
					return files;
				}	
			}

			public bool Move {
				get { return m_move; }
				set { m_move = value; }
			}

			private ArrayList m_files;
			private bool m_move;
		}

		private CFile m_curdir;

		private void Page_Load(object sender, System.EventArgs e) {
		
			lblMessages.Text = "<font color=\"#000000\"><b>Messages:</b></font> ";
			(tbActions.Items[0] as ToolbarButton).ButtonClick += new ToolbarItemEventHandler(cmdNewFolder_ButtonClick);
			(tbActions.Items[2] as ToolbarButton).ButtonClick += new ToolbarItemEventHandler(cmdView_ButtonClick);
			(tbActions.Items[3] as ToolbarButton).ButtonClick += new ToolbarItemEventHandler(cmdCut_ButtonClick);
			(tbActions.Items[4] as ToolbarButton).ButtonClick += new ToolbarItemEventHandler(cmdCopy_ButtonClick);
			(tbActions.Items[5] as ToolbarButton).ButtonClick += new ToolbarItemEventHandler(cmdPaste_ButtonClick);
			(tbActions.Items[6] as ToolbarButton).ButtonClick += new ToolbarItemEventHandler(cmdDelete_ButtonClick);
			(tbActions.Items[8] as ToolbarButton).ButtonClick += new ToolbarItemEventHandler(cmdReload_ButtonClick);
			(tbActions.Items[10] as ToolbarButton).ButtonClick += new ToolbarItemEventHandler(cmdSelectAll_ButtonClick);
			(tbActions.Items[11] as ToolbarButton).ButtonClick += new ToolbarItemEventHandler(cmdSelectNone_ButtonClick);

			if (Request.Params["SubID"] != null)
				ViewState["subID"] = Convert.ToInt32(Request.Params["SubID"]);

			if (!IsPostBack)
				LoadFileBrowser();
		}

		protected ArrayList m_roots = new ArrayList();

		private int GetSubID(TreeNode node) {
			
			CFile file = new FileSystem(Globals.CurrentIdentity).GetFile(node.NodeData);
			if (file.SpecType == CFile.SpecialType.SUBMISSION)
				return new Submissions(Globals.CurrentIdentity).GetInfoByDirectoryID(file.ID).ID;

			if (node.Parent != null && node.Parent is TreeNode)
				return GetSubID((TreeNode)node.Parent);

			return -1;
		}

		private string GetCurrentPath() {
			return (string) ViewState["gridpath"];
		}

		private void BindFileGrid() {
			
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			string path = GetCurrentPath();
			if (path == null) return;

			CFile file = fs.GetFile(path);
			CFile.FileList dirlist;
			try {
				dirlist = fs.ListDirectory(file);
			} catch (CustomException er) {
				dirlist = new CFile.FileList();
				DisplayMessage(er.Message);
			}

			dirlist.Insert(0, file);

			m_curdir = file;
			dgFiles.DataSource = dirlist;
			dgFiles.DataBind();
		}

		public void AddDirectoryRoot(string path) {
			m_roots.Add(path);
		}

		public void LoadFileBrowser() {
			int i=0;
			tvFiles.Nodes.Clear();
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			foreach(string droot in m_roots) {
				CFile dirroot = fs.GetFile(droot);

				TreeNode root = new TreeNode();
				root.Text = dirroot.Alias;
				root.ImageUrl = GetFolderIcon(dirroot);
				root.NodeData = dirroot.FullPath;
				root.Expandable = ExpandableValue.Always;
				tvFiles.Nodes.Add(root);

				if (i == 0 && ViewState["gridpath"] == null) {
					ViewState["gridpath"] = dirroot.FullPath;
					ExpandTreeNode(root);
				}
				++i;
			}			

			BindFileGrid();
			BindClipBoard();
		}

		private void AddToTreeNode(TreeNode node, CFile.FileList dirlist) {
		
			foreach (CFile file in dirlist) {
				if (file.IsDirectory()) {
					TreeNode item = new TreeNode();
					item.Text = file.Alias;
					item.ImageUrl = GetFolderIcon(file);
					item.ExpandedImageUrl = GetExpandedFolderIcon(file);
					item.Expandable = ExpandableValue.Always;
					item.NodeData = file.FullPath;
					node.Nodes.Add(item);
				}
			}
		}

		private string GetFolderIcon(CFile file) {
			if (file.SpecType == CFile.SpecialType.SUBMISSION)
				return "attributes/filebrowser/subfolder.gif";
			else
				return "attributes/filebrowser/folder.gif";
		}

		private string GetExpandedFolderIcon(CFile file) {
			if (file.SpecType == CFile.SpecialType.SUBMISSION)
				return "attributes/filebrowser/subfolder.gif";
			else
				return "attributes/filebrowser/folderopen.gif";
		}

		public string GetFileImage(CFile file) {
			if (file.IsDirectory())
				return "../../" + GetFolderIcon(file);
			else {
				;
				string imgpath;
				try {
					imgpath = FileBrowserImageFactory.GetInstance().GetExtensionImage(
						Path.GetExtension(file.Name));
				} catch (Exception) {
					imgpath=null;
				}
				if (imgpath == null)
					return "../../attributes/filebrowser/misc.gif";
				else
					return "../../attributes/filebrowser/" + imgpath;
			}
		}

		protected void BindClipBoard() {
			ClipBoard cb = (ClipBoard) ViewState["clipboard"];
			if (cb == null)
				lblClipboard.Text = "<b>Clipboard:</b> Empty";
			else {
				string cstr = cb.Files.ToString();
				if (cstr.Length > 50)
					cstr = cstr.Substring(0, 50) + " ...";
				lblClipboard.Text = String.Format("<b>Clipboard:</b> <b>Cut:</b> {0} <b>Files:</b> {1}",
					cb.Move, cstr);
			}
		}
	
		protected bool cmdCut_ButtonClick(object sender, EventArgs e) {
			CopyCutHandler(true);
			return true;
		}

		protected bool cmdCopy_ButtonClick(object sender, EventArgs e) {
			CopyCutHandler(false);
			return true;
		}

		protected void CopyCutHandler(bool cut) {

			ArrayList selfiles = new ArrayList();
			CheckBox chkSelect;
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);

			foreach (DataGridItem item in dgFiles.Items) {
				if (null != (chkSelect = (CheckBox)item.FindControl("chkSelect"))) {
					if (chkSelect.Checked) 
						selfiles.Add((int)dgFiles.DataKeys[item.ItemIndex]);
				}
			}
		
			if (selfiles.Count > 0)
				ViewState["clipboard"] = new ClipBoard(selfiles, cut);
			else
				DisplayMessage("No files selected. To cut/copy files check the boxes on their left");

			BindClipBoard();
		}

		protected void DisplayMessage(string msg) {
			lblMessages.Visible = true;
			lblMessages.Text = "<font color=\"#000000\"><b>Messages:</b></font> " + msg;
		}

		protected bool cmdPaste_ButtonClick(object sender, EventArgs ea) {
			TreeNode destnode;
			ClipBoard cb;
			FileSystem fs;

			cb = (ClipBoard) ViewState["clipboard"];
			if (cb == null) {
				DisplayMessage("Clipboard empty. You must cut/copy files before pasting has an effect");	
				return true;
			}

			fs = new FileSystem(Globals.CurrentIdentity);
			destnode = tvFiles.GetNodeFromIndex(tvFiles.SelectedNodeIndex);
			CFile dest = fs.GetFile((string)ViewState["gridpath"]);

			try {
				if (cb.Move)
					fs.MoveFiles(dest, cb.Files, false);
				else
					fs.CopyFiles(dest, cb.Files, false);
			}
			catch (FileOperationException e) {
				DisplayMessage(e.Message);
				return false;
			}

			UpdateTreeNode(destnode, true);	
			BindFileGrid();
			ViewState["clipboard"] = null;
			BindClipBoard();
			
			return true;
		}

		private CFile.FileList GetSelectedFiles() {
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			CheckBox chkSelect;
			CFile.FileList selfiles = new CFile.FileList();
			foreach (DataGridItem item in dgFiles.Items) {
				if (null != (chkSelect = (CheckBox)item.FindControl("chkSelect"))) 
					if (chkSelect.Checked) 
						selfiles.Add(fs.GetFile((int)dgFiles.DataKeys[item.ItemIndex]));
			}
			return selfiles;
		}

		protected bool cmdDelete_ButtonClick(object sender, EventArgs ev) {
		
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);		
			TreeNode destnode;
			string selstr;
		
			CFile.FileList selfiles = GetSelectedFiles();
			destnode = tvFiles.GetNodeFromIndex(tvFiles.SelectedNodeIndex);
			selstr = destnode.GetNodeIndex();
			
			if (selfiles.Count == 0) {
				DisplayMessage("No files selected to delete. Select files by checking the boxes to their left");
				return false;
			}

			try {
				fs.DeleteFiles(selfiles);
			}
			catch (FileOperationException e) {
				DisplayMessage("Error: " + e.Message);
				return false;
			}

			UpdateTreeNode(destnode, true);

			tvFiles.SelectedNodeIndex = selstr;
			BindFileGrid();
			return true;
		}

		protected bool cmdReload_ButtonClick(object sender, EventArgs ea) {
			BindFileGrid();
			return true;
		}

		protected bool cmdSelectAll_ButtonClick(object sender, EventArgs ea) {
			CheckBox chkSelect;
			foreach (DataGridItem item in dgFiles.Items) {
				if (null != (chkSelect = (CheckBox)item.FindControl("chkSelect")))
					if (chkSelect.Enabled)
						chkSelect.Checked = true;
			}
			return true;
		}

		protected bool cmdNewFolder_ButtonClick(object sender, EventArgs ea) {
		
			try {
				new FileSystem(Globals.CurrentIdentity).CreateDirectory(
						Path.Combine(GetCurrentPath(), "New_Folder"), false, null);
			} catch (DataAccessException er) {
				DisplayMessage("Error: " + er.Message);
			} catch (FileOperationException er) {
				DisplayMessage("Error: " + er.Message);
			}

			UpdateTreeNode(tvFiles.GetNodeFromIndex(tvFiles.SelectedNodeIndex), true);
			BindFileGrid();
			return true;
		}

		protected bool cmdSelectNone_ButtonClick(object sender, EventArgs ea) {
			CheckBox chkSelect;
			foreach (DataGridItem item in dgFiles.Items) {
				if (null != (chkSelect = (CheckBox)item.FindControl("chkSelect"))) 
					chkSelect.Checked = false;
			}
			return true;
		}

		protected void UpdateTreeNode(TreeNode node, bool fullreload) {
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			CFile dir;

			dir = fs.GetFile(node.NodeData);
			if (fullreload) node.Nodes.Clear();
			if (node.Nodes.Count == 0) {
				
				CFile.FileList dirlist;
				try {
					dirlist = fs.ListDirectory(dir);
				} catch (CustomException er) {
					DisplayMessage(er.Message); return;
				}

				if (dirlist.Count > 0)
					AddToTreeNode(node, dirlist);
			}
		}

		protected void ExpandTreeNode(TreeNode node) {
			node.Expanded = true;
			UpdateTreeNode(node, false);
		}

		protected void tvFiles_Expand(object sender, TreeViewClickEventArgs e) {
			TreeNode selnode = tvFiles.GetNodeFromIndex(e.Node);
			UpdateTreeNode(selnode, false);
		}

		protected void tvFiles_SelectedIndexChange(object sender, TreeViewSelectEventArgs e) {
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);

			TreeNode selnode = tvFiles.GetNodeFromIndex(e.NewNode);
			ViewState["gridpath"] = fs.GetFile(selnode.NodeData).FullPath;

			BindFileGrid();
		}

		private void dgFiles_ItemDataBound(object sender, DataGridItemEventArgs e) {
			System.Web.UI.WebControls.Image img;
			ImageButton imgDownload;
			TextBox txtName;
			LinkButton lnkName, lnkRename;
			Label lblSize;
			CheckBox chkSelect;

			if (null != (lblSize = e.Item.FindControl("lblSize") as Label)) {
	
				lnkName = (LinkButton) e.Item.FindControl("lnkName");
				img = (System.Web.UI.WebControls.Image) e.Item.FindControl("FileImage");
				imgDownload = (ImageButton) e.Item.FindControl("imgDownload");
				lnkRename = (LinkButton) e.Item.FindControl("lnkRename");
				chkSelect = (CheckBox) e.Item.FindControl("chkSelect");

				CFile file = e.Item.DataItem as CFile;
				if (img != null)
					img.ImageUrl = GetFileImage(file);	

				int kb = file.Size/1000;
				if (kb > 0)
					lblSize.Text = String.Format("{0} KB", kb);
				else
					lblSize.Text = String.Format("{0} bytes", file.Size);
		
				img = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgReadOnly");
				if (!file.ReadOnly)
					img.Visible = false;
				else
					img.ImageUrl = "../../attributes/filebrowser/ro.gif";
				
				img = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgLock");
				if (new FileSystem(Globals.CurrentIdentity).IsLocked(file))
					img.ImageUrl = "../../attributes/filebrowser/lock.gif";
				else
					img.Visible = false;

				if (imgDownload != null) {
					imgDownload.Attributes.Add("onClick", 
						@"window.open('Controls/Filesys/dlfile.aspx?FileID=" + file.ID + 
						@"', '"+file.ID+@"', 'width=770, height=580')");
				}

				if (lnkName != null) {
					if (m_curdir.ID != file.ID)
						lnkName.Text = file.Alias;
					else {
						lnkRename.Enabled = false;
						chkSelect.Enabled = false;
						lnkName.Text = ".";
					}
					if (!file.IsDirectory()) {
						int subID;
						if (0 > (subID = GetSubID(tvFiles.GetNodeFromIndex(tvFiles.SelectedNodeIndex)))) {
							lnkName.Attributes.Add("onClick", 
								@"window.open('Controls/Filesys/viewfile.aspx?FileIDs=" + file.ID + 
								@"', '"+file.ID+@"', 'width=770, height=580')");
						} else {
							lnkName.Attributes.Add("onClick", 
								@"window.open('Controls/Filesys/viewfile.aspx?SubID="+subID+
								"&FileIDs=" + file.ID + 
								@"', '"+file.ID+@"', 'width=770, height=580')");
						}
						lnkName.CommandName = "PopUp";
					}	
				}
			}
			if (null != (txtName = (TextBox) e.Item.FindControl("txtName"))) {
				CFile file = (CFile) e.Item.DataItem;
				txtName.Text = file.Name;
			}
		}

		private void FileCommand(object source, DataGridCommandEventArgs e) {
			CFile file = new FileSystem(Globals.CurrentIdentity).GetFile(
				(int)dgFiles.DataKeys[e.Item.ItemIndex]);
			if (file.IsDirectory()) {
				TreeNode curnode = tvFiles.GetNodeFromIndex(tvFiles.SelectedNodeIndex);
				foreach (TreeNode node in curnode.Nodes) {
					if (node.NodeData == file.FullPath) {
						ExpandTreeNode(node);
						ViewState["gridpath"] = node.NodeData;
						try {
							tvFiles.SelectedNodeIndex = node.GetNodeIndex();
						} catch (Exception) {
							ExpandTreeNode((TreeNode)node.Parent);
							tvFiles.SelectedNodeIndex = node.GetNodeIndex();
						}
						BindFileGrid();
						break;
					}
				}
			}
		}

		private void dgFiles_ItemCommand(object source, DataGridCommandEventArgs e) {
			if (e.CommandName == "File") 
				FileCommand(source, e);
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
			this.tvFiles.Expand += new Microsoft.Web.UI.WebControls.ClickEventHandler(this.tvFiles_Expand);
			this.tvFiles.SelectedIndexChange += new Microsoft.Web.UI.WebControls.SelectEventHandler(this.tvFiles_SelectedIndexChange);
			this.dgFiles.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgFiles_ItemCommand);
			this.dgFiles.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgFiles_CancelCommand);
			this.dgFiles.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgFiles_EditCommand);
			this.dgFiles.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgFiles_UpdateCommand);
			this.dgFiles.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgFiles_ItemDataBound);
			this.cmdUpload.Click += new System.EventHandler(this.cmdUpload_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void dgFiles_EditCommand(object source, DataGridCommandEventArgs e) {
			dgFiles.EditItemIndex = e.Item.ItemIndex;
			BindFileGrid();
		}

		private void dgFiles_UpdateCommand(object source, DataGridCommandEventArgs e) {

			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			TextBox txtName = e.Item.FindControl("txtName") as TextBox;

			int fileID = (int) dgFiles.DataKeys[e.Item.ItemIndex];
			CFile file = fs.GetFile(fileID);

			if (file.Alias == file.Name)
				file.Name = txtName.Text;
			file.Alias = txtName.Text;

			try {
				fs.UpdateFileInfo(file, false);
			} catch (FileOperationException er) {
				DisplayMessage(er.Message);
			}

			dgFiles.EditItemIndex = -1;
			UpdateTreeNode(tvFiles.GetNodeFromIndex(tvFiles.SelectedNodeIndex), true);
			BindFileGrid();
		}

		private void dgFiles_CancelCommand(object source, DataGridCommandEventArgs e) {
			dgFiles.EditItemIndex = -1;
			BindFileGrid();
		}

		private bool cmdView_ButtonClick(object sender, EventArgs e) {
			string script = "<script>", filestr="";
			CFile.FileList selfiles = GetSelectedFiles();
			int subID, mainID;

			if (selfiles.Count == 0) {
				DisplayMessage("No files selected to delete. Select files by checking the boxes to their left");
				return false;
			}

			mainID = selfiles[0].ID;
			foreach (CFile file in selfiles)
				filestr += file.ID + "|";

			if (0 > (subID = GetSubID(tvFiles.GetNodeFromIndex(tvFiles.SelectedNodeIndex)))) {
				script +=
					@"window.open('Controls/Filesys/viewfile.aspx?FileIDs=" + filestr + 
					@"', '"+mainID+@"', 'width=770, height=580')";
			} else {
				script += 
					@"window.open('Controls/Filesys/viewfile.aspx?SubID="+subID+
					"&FileIDs=" + filestr + 
					@"', '"+mainID+@"', 'width=770, height=580')";
			}
			script += "</script>";
	
			Page.RegisterClientScriptBlock("mike", script);
			return true;
		}

		private void cmdUpload_Click(object sender, System.EventArgs e) {
			
			if (fiUpload.PostedFile.FileName.Length == 0) {
				DisplayMessage("Please select a file to upload");
				return;
			}

			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			string name = Path.GetFileName(fiUpload.PostedFile.FileName);
			SingleFileSource sfs = new SingleFileSource(name);

			try {
				sfs.CreateSource(fiUpload.PostedFile.InputStream);
				fs.ImportData(GetCurrentPath(), sfs, true, false);
			} catch (FileOperationException er) {
				DisplayMessage(er.Message);
			}

			BindFileGrid();
		}
	}
}
