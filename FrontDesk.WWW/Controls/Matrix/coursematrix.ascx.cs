using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Microsoft.Web.UI.WebControls;
using System.IO;

using FrontDesk.Pages;
using FrontDesk.Common;
using FrontDesk.Controls.Filesys;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Tools;

namespace FrontDesk.Controls.Matrix {

	//Hooks for ppl wanting to change view
	public delegate void RefreshEventHandler(object sender, RefreshEventArgs args);

	/// <summary>
	///	Rubric View user control
	/// </summary>
	public class CourseMatrixControl : Pagelet {
		protected System.Web.UI.WebControls.Button cmdCancel;
		protected System.Web.UI.WebControls.Label lblError;
		protected Microsoft.Web.UI.WebControls.TreeView tvRubric;

		public const string REFRESH_RESET = "___reset";

		public const int RUBRICVIEW = 0,ASSTVIEW=1,ANNVIEW=2,SUBSYSVIEW=3,
			STUCOURSEVIEW=4,GROUPVIEW=5,EVALUATIONVIEW=6,SUBJFEEDBACKVIEW=7,
			AUTOSYSVIEW=8,AUTOJOBVIEW=9,SECTIONFOLDERVIEW=10,SECTIONVIEW=11,
			BACKUPSVIEW=12,USRCOURSERPTVIEW=13,GRPCOURSERPTVIEW=14,
			GRPASSTRPTVIEW=15,CONTENTVIEW=16,PERMVIEW=17,COMPETITIONVIEW=18,
			AGGREGATEVIEW=19, MAXVIEWS=20;

		public const int AUTOMATIC=0, CANNED=1, HEADING=2, ASSIGNMENT=3, COURSE=4, FOLDER=5, DOCUMENT=6;
		public const int ANNOUNCEMENT=11, ANNFOLDER=12, SUBFOLDER=13, GROUPFOLDER=14, RESULTFOLDER=15;
		public const int RESSUB=16, FEEDBACK=17, AUTOSYS=18, AUTOJOB=19, SECTIONFOLDER=20;
		public const int SUBJUSER=21, SECTION=22, BACKUPS=23, USERGROUP=24, USER=25, REPORT=26, ALLUSERS=27;
		public const int PERMISSION=28, USERSUB=29, JUNIT=30, CHECKSTYLE=31, EVALUATION=32, COMPETITION=33;
		public const int AGGREGATE=34;

		public const int TB_ASST=0, TB_FOLDER=3, TB_DOCUMENT=4, TB_ANN=5, TB_CHECKSTYLE=8,
			TB_CANNED=7, TB_AUTO=10, TB_JUNIT=9, TB_HEADING=11, TB_BACKUP=13, TB_DELETE=14, TB_SECTION=1;

		protected Microsoft.Web.UI.WebControls.Toolbar tbActions;
		protected Microsoft.Web.UI.WebControls.MultiPage mpViews;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divRubric;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divRight;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divMain;
		protected IMatrixControl[] m_views = new IMatrixControl[MAXVIEWS];

		public CourseMatrixControl() {
			if (ViewState["stumode"] == null) 
				ViewState["stumode"] = false;
		}

		private void Page_Load(object sender, System.EventArgs e) {

			(tbActions.Items[TB_ASST] as ToolbarButton).ButtonClick += 
				new ToolbarItemEventHandler(tb_CreateAsst);
			(tbActions.Items[TB_FOLDER] as ToolbarButton).ButtonClick += 
				new ToolbarItemEventHandler(tb_CreateFolder);
			(tbActions.Items[TB_DOCUMENT] as ToolbarButton).ButtonClick += 
				new ToolbarItemEventHandler(tb_CreateDocument);
			(tbActions.Items[TB_CANNED] as ToolbarButton).ButtonClick +=
				new ToolbarItemEventHandler(tb_CreateSubj);
			(tbActions.Items[TB_AUTO] as ToolbarButton).ButtonClick += 
				new ToolbarItemEventHandler(tb_CreateAuto);
			(tbActions.Items[TB_HEADING] as ToolbarButton).ButtonClick += 
				new ToolbarItemEventHandler(tb_CreateHeading);
			(tbActions.Items[TB_ANN] as ToolbarButton).ButtonClick += 
				new ToolbarItemEventHandler(tb_CreateAnnouncement);
			(tbActions.Items[TB_SECTION] as ToolbarButton).ButtonClick += 
				new ToolbarItemEventHandler(tb_CreateSection);
			(tbActions.Items[TB_BACKUP] as ToolbarButton).ButtonClick += 
				new ToolbarItemEventHandler(tb_CreateBackup);
			(tbActions.Items[TB_DELETE] as ToolbarButton).ButtonClick += 
				new ToolbarItemEventHandler(tb_Delete);
			(tbActions.Items[TB_JUNIT] as ToolbarButton).ButtonClick += 
				new ToolbarItemEventHandler(tb_CreateJUnit);
			(tbActions.Items[TB_CHECKSTYLE] as ToolbarButton).ButtonClick += 
				new ToolbarItemEventHandler(tb_CreateCheckStyle);

			ToolbarButton tbDelete = (tbActions.Items[TB_DELETE] as ToolbarButton);

			if (StudentMode) {
				divRubric.Style["TOP"] = "5px";
				divRight.Style["TOP"] = "5px";
				divMain.Style["HEIGHT"] = "515px";
			}

			lblError.Visible = false;
			LoadViews();
		}

		public bool StudentMode {
			get { return (bool) ViewState["stumode"]; }
			set { 
				if (value) {
					tbActions.Visible = false;  
				} else {
					tbActions.Visible = true; 
				}
				ViewState["stumode"] = value; 
			}
		}

		private void LoadViews() {
			m_views[RUBRICVIEW] = (IMatrixControl) FindControl("ucRubric");
			m_views[ASSTVIEW] = (IMatrixControl) FindControl("ucAsst");
			m_views[ANNVIEW] = (IMatrixControl) FindControl("ucAnn");
			m_views[SUBSYSVIEW] = (IMatrixControl) FindControl("ucSubsys");
			m_views[STUCOURSEVIEW] = (IMatrixControl) FindControl("ucStuCourse");
			m_views[GROUPVIEW] = (IMatrixControl) FindControl("ucGroup");
			m_views[EVALUATIONVIEW] = (IMatrixControl) FindControl("ucFeedback");
			m_views[SUBJFEEDBACKVIEW] = (IMatrixControl) FindControl("ucSubjFeedback");
			m_views[AUTOSYSVIEW] = (IMatrixControl) FindControl("ucAutoSys");
			m_views[AUTOJOBVIEW] = (IMatrixControl) FindControl("ucAutoJob");
			m_views[SECTIONFOLDERVIEW] = (IMatrixControl) FindControl("ucCourseSect");
			m_views[SECTIONVIEW] = (IMatrixControl) FindControl("ucSection");
			m_views[BACKUPSVIEW] = (IMatrixControl) FindControl("ucBackups");
			m_views[USRCOURSERPTVIEW] = (IMatrixControl) FindControl("ucUserCourseReport");
			m_views[GRPCOURSERPTVIEW] = (IMatrixControl) FindControl("ucGroupCourseReport");
			m_views[GRPASSTRPTVIEW] = (IMatrixControl) FindControl("ucGroupAsstReport");
			m_views[CONTENTVIEW] = (IMatrixControl) FindControl("ucContent");
			m_views[PERMVIEW] = (IMatrixControl) FindControl("ucPerms");
			m_views[COMPETITIONVIEW] = (IMatrixControl) FindControl("ucCompete");
			m_views[AGGREGATEVIEW] = (IMatrixControl) FindControl("ucAggregate");
			foreach (IMatrixControl c in m_views) 
				c.Refresh += new RefreshEventHandler(views_Refresh);
		}

		private void PageError(string msg) {
			lblError.Text = msg;
			lblError.Visible = true;
		}

		public void AddRoot(Course course) {
			TreeNode root = AddCourseNode(tvRubric.Nodes, course);

			LoadNode(root); root.Expanded = true;
			if (tvRubric.Nodes.Count == 1) {
				LoadViews();
				ActivateNodeView(root);
			}
		}

		public void BindData() {
			TreeNode node = tvRubric.GetNodeFromIndex(tvRubric.SelectedNodeIndex);
			SyncToolbar(GetNodeType(node));
		}

		public TreeNode GetCurrentParent() {
			TreeNode cur = tvRubric.GetNodeFromIndex(tvRubric.SelectedNodeIndex);

			if (cur.Parent is TreeView) return null;
			else return (TreeNode) cur.Parent;
		}

		public int GetCurrentParentID() {
			
			TreeNode par = GetCurrentParent();
			if (par == null) return -1;

			int rubID = Convert.ToInt32(par.NodeData);
			Rubric rub = new Rubrics(Globals.CurrentIdentity).GetInfo(rubID);

			return rub.ID;
		}

		public TreeNode GetCurrentNode() {
			return tvRubric.GetNodeFromIndex(tvRubric.SelectedNodeIndex);
		}

		private int GetCourseID() {
			return Convert.ToInt32(Request.Params["CourseID"]);
		}

		public int GetCurrentID() {
			TreeNode cur = GetCurrentNode();

			int rubID = Convert.ToInt32(cur.NodeData);
			Rubric rub = new Rubrics(Globals.CurrentIdentity).GetInfo(rubID);

			return rub.ID;
		}

		private bool tb_CreateAnnouncement(object sender, EventArgs e) {
			
			TreeNode node = GetCurrentNode();
			int courseID = Convert.ToInt32(Request.Params["CourseID"]);
			try {
				new Announcements(Globals.CurrentIdentity).Create(Globals.CurrentUserName, 
					"New Announcement", "Fill in announcement text here!", courseID);
			} catch (DataAccessException er) {
				PageError(er.Message);
				return false;
			}

			LoadNode(node);
			node.Expanded = true;

			return true;
		}

		private bool tb_CreateAsst(object sender, EventArgs e) {
			
			TreeNode node = GetCurrentNode();
			int courseID = Convert.ToInt32(Request.Params["CourseID"]);
			int asstID=0;
			Assignments asstda = new Assignments(Globals.CurrentIdentity);
			try {
				asstID = new Assignments(Globals.CurrentIdentity).Create(courseID, 
					Globals.CurrentUserName, "New Assignment", DateTime.Now);
			} catch (DataAccessException er) {
				PageError(er.Message);
				return false;
			}

			LoadCourseNode(node, courseID);
			node.Expanded = true;

			return true;
		}

		private int GetCurrentFileID() {

			TreeNode node = GetCurrentNode();
			int ntype = GetNodeType(node);
			int id = Convert.ToInt32(node.NodeData.Split(" ".ToCharArray())[1]);
			
			switch (ntype) {
			case FOLDER:
				return id;
			case COURSE:
				return new Courses(Globals.CurrentIdentity).GetInfo(id).ContentID;
			case ASSIGNMENT:
				return new Assignments(Globals.CurrentIdentity).GetInfo(id).ContentID;
			}

			return -1;
		}

		private bool tb_CreateDocument(object sender, EventArgs e) {
	
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			CFile par = fs.GetFile(GetCurrentFileID());
			string path = par.FullPath + @"\" + "newdoc.txt";

			try {
				CFile doc = fs.CreateFile(path, false, null);
				doc.Alias = "New Document"; 
				doc.Name = doc.ID + doc.Name;
				fs.UpdateFileInfo(doc, false);
			} catch (FileOperationException er) {
				PageError(er.Message);
				return false;
			}

			TreeNode node = GetCurrentNode();
			LoadNode(node);
			node.Expanded = true;

			return true;
		}

		private bool tb_CreateFolder(object sender, EventArgs e) {
	
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			CFile par = fs.GetFile(GetCurrentFileID());
			string path = par.FullPath + @"\" + "newfolder";

			try {
				CFile fold = fs.CreateDirectory(path, false, null);
				fold.Alias = "New Folder"; 
				fold.Name = fold.ID + fold.Name;
				fs.UpdateFileInfo(fold, false);
			} catch (FileOperationException er) {
				PageError(er.Message);
				return false;
			}

			TreeNode node = GetCurrentNode();
			LoadNode(node);
			node.Expanded = true;

			return true;
		}

		private bool tb_CreateHeading(object sender, EventArgs e) {
			CreateRubricEntry(HEADING);
			return true;
		}

		private bool tb_CreateAuto(object sender, EventArgs e) {
			CreateRubricEntry(AUTOMATIC);
			return true;
		}

		private bool tb_CreateSubj(object sender, EventArgs e) {
			CreateRubricEntry(CANNED);
			return true;
		}

		private bool tb_CreateSection(object sender, EventArgs e) {

			int courseID = Convert.ToInt32(Request.Params["CourseID"]);
			string name = "New Section";
			string owner = Globals.CurrentUserName;

			try {
				(new Sections(Globals.CurrentIdentity)).Create(name, owner, courseID);
			} catch (DataAccessException er) {
				PageError(er.Message);
				return false;
			}

			TreeNode node = GetCurrentNode();
			LoadNode(node);
			node.Expanded = true;

			ActivateNodeView(node);
			return true;
		}

		private bool tb_CreateBackup(object sender, EventArgs e) {
			TreeNode node = GetCurrentNode();
			int type = GetNodeType(node);
			int id = GetNodeIndex(node);
			switch (type) {
			case COURSE:
				try {
					new Courses(Globals.CurrentIdentity).Backup(id);
				} catch (DataAccessException er) {
					PageError(er.Message);
					return false;
				} catch (FileOperationException fer) {
					PageError(fer.Message);
					return false;
				}			
				break;
			case ASSIGNMENT:
				try {
					new Assignments(Globals.CurrentIdentity).Backup(id);
				} catch (DataAccessException er) {
					PageError(er.Message);
					return false;
				} catch (FileOperationException fer) {
					PageError(fer.Message);
					return false;
				}	
				break;
			};

			PageError("Backup created successfully!");
			return true;
		}

		private bool tb_CreateCheckStyle(object sender, EventArgs e) {
			CreateRubricEntry(CHECKSTYLE);
			return true;
		}

		private bool tb_CreateJUnit(object sender, EventArgs e) {
			CreateRubricEntry(JUNIT);
			return true;
		}

		private bool tb_Delete(object sender, EventArgs e) {

			TreeNode node;
			int ty = GetNodeType(node = GetCurrentNode());

			switch (ty) {
			case USER:
				try {
					new Courses(Globals.CurrentIdentity).RemoveUser(
						node.NodeData.Split(" ".ToCharArray())[1], GetCourseID());
				} catch (DataAccessException er) {
					PageError(er.Message);
				} catch (FileOperationException er) {
					PageError(er.Message);
				}
				break;
			case USERSUB:
				try {
					int subID = Convert.ToInt32(node.NodeData.Split(" ".ToCharArray())[2]);
					new Submissions(Globals.CurrentIdentity).Delete(subID);
				} catch (DataAccessException er) {
					PageError(er.Message);
				} catch (FileOperationException er) {
					PageError(er.Message);
				}
				break;
			case ASSIGNMENT:
				try {
					new Assignments(Globals.CurrentIdentity).Delete(GetNodeIndex(node));
				} catch (DataAccessException er) {
					PageError(er.Message); 
				} catch (FileOperationException er) {
					PageError(er.Message);
				}
				break;
			case ANNOUNCEMENT:
				try {
					new Announcements(Globals.CurrentIdentity).Delete(GetNodeIndex(node));
				} catch (DataAccessException er) {
					PageError(er.Message);
				}
				break;
			case SECTION:
				try {
					new Sections(Globals.CurrentIdentity).Delete(GetNodeIndex(node));
				} catch (DataAccessException er) {
					PageError(er.Message);
				}
				break;
			case HEADING:
			case CANNED:
			case AUTOMATIC:
				Rubric rub = new Rubrics(Globals.CurrentIdentity).GetInfo(GetNodeIndex(node));
				if (rub.ParentID < 0) 
					PageError("Cannot delete the root rubric");
				else {
					try {
						new Rubrics(Globals.CurrentIdentity).Delete(rub.ID);
					} catch (DataAccessException er) {
						PageError(er.Message); 
					} catch (FileOperationException er) {
						PageError(er.Message);
					}
				}
				break;
			case FOLDER:
			case DOCUMENT:
				FileSystem fs = new FileSystem(Globals.CurrentIdentity);
				CFile file = fs.GetFile(GetNodeIndex(node));

				try {
					fs.DeleteFile(file);
				} catch (DataAccessException er) {
					PageError(er.Message); 
				} catch (FileOperationException er) {
					PageError(er.Message);
				}
				break;
			}

			LoadNode((TreeNode)node.Parent);
			ActivateNodeView((TreeNode)node.Parent);

			return true;
		}

		private TreeNode AddAsstNode(TreeNodeCollection par, Assignment asst) {
			return AddNode(par, asst.Description, "attributes/asst.gif", "attributes/asst.gif",
				"Asst " + asst.ID, true);
		}

		private TreeNode AddPermNode(TreeNodeCollection par, int id) {
			return AddNode(par, "Permissions", "attributes/key.gif", "attributes/key.gif",
				"Perm " + id, false);
		}

		private TreeNode AddAnnFolderNode(TreeNodeCollection par, int courseID) {
			return AddNode(par, "Announcements", "attributes/ann.gif", "attributes/ann.gif",
				"AnnFolder " + courseID, true);
		}

		private TreeNode AddFeedbackNode(TreeNodeCollection par, int asstID) {
			return AddNode(par, "Feedback System", "attributes/glass.gif", "attributes/glass.gif",
				"Feedback " + asstID, true);
		}

		private TreeNode AddEvaluationNode(TreeNodeCollection par, int asstID) {
			return AddNode(par, "Start Evaluation", "attributes/glass.gif", "attributes/glass.gif",
				"Evaluation " + asstID, false);
		}

		private TreeNode AddCompetitionNode(TreeNodeCollection par, int asstID) {
			return AddNode(par, "Competitions", "attributes/compete.gif", "attributes/compete.gif",
				"Competition " + asstID, false);
		}

		private TreeNode AddAutoSysNode(TreeNodeCollection par, int asstID) {
			return AddNode(par, "Pending Automatic Jobs", "attributes/glassyellow.gif", "attributes/glassyellow.gif",
				"AutoSys " + asstID, true);
		}

		private TreeNode AddAutoJobNode(TreeNodeCollection par, AutoJob job) {
			return AddNode(par, job.JobName, "attributes/cyl.gif", "attributes/cyl.gif",
				"AutoJob " + job.ID, false);
		}

		private TreeNode AddSubjUserNode(TreeNodeCollection par, User user, int asstID, Components.Submission sub, 
										 bool expand) {
			string imageurl;
			string desc;

			switch (sub.Status) {
			case Components.Submission.GRADED:
				imageurl = "attributes/subgrade.gif"; break;
			case Components.Submission.DEFUNCT:
				imageurl = "attributes/skull.gif"; break;
			default:
				imageurl = "attributes/sub.gif"; break;
			};

			CFile sdir = new FileSystem(Globals.CurrentIdentity).GetFile(sub.LocationID);
			if (sdir == null)
				desc = "*Error*";
			else
				desc = sdir.Alias;
			
			if (expand) desc = "(" + user.UserName + ") " + desc; //Make top level more clear
			return AddNode(par, desc, imageurl, imageurl,
				"SubjUser " + user.UserName + " " + asstID.ToString() + " " + sub.ID, expand);
		}

		private TreeNode AddAggregateNode(TreeNodeCollection par, int asstID, string agglist) {
			string imageurl = "attributes/sub.gif";
			return AddNode(par, "All Users", imageurl, imageurl, "Aggregate " + asstID + " " + agglist, false); 
		}

		private TreeNode AddUserSubNode(TreeNodeCollection par, string username, Components.Submission sub) {
			bool graded = (sub.Status == Components.Submission.GRADED);
			string desc;
			CFile sdir = new FileSystem(Globals.CurrentIdentity).GetFile(sub.LocationID);
			if (sdir == null)
				desc = "*Error*";
			else
				desc = sdir.Alias;
			string imageurl = (graded) ? "attributes/subgrade.gif" : "attributes/sub.gif";
			return AddNode(par, desc, imageurl, imageurl,
				"UserSub " + username + " " + sub.ID.ToString(), false);
		}

		private TreeNode AddSubFolderNode(TreeNodeCollection par, int asstID) {
			return AddNode(par, "Submission System", "attributes/sub.gif", "attributes/sub.gif",
				"SubFolder " + asstID, false);
		}

		private TreeNode AddResSubNode(TreeNodeCollection par, Components.Submission sub) {
			CFile subdir = new FileSystem(Globals.CurrentIdentity).GetFile(sub.LocationID);
			return AddNode(par, subdir.Alias, "attributes/subgrade.gif", "attributes/subgrade.gif",
				"ResSub " + sub.ID, false);
		}

		private TreeNode AddSectionFolderNode(TreeNodeCollection par, int courseID, string title) {
			return AddNode(par, title, "attributes/group.gif", "attributes/group.gif",
				"SectionFolder " + courseID, true);
		}

		private TreeNode AddReportNode(TreeNodeCollection par, int sectionID) {
			return AddNode(par, "Grade Report", "attributes/graph.gif", "attributes/graph.gif",
				"Report " + sectionID, false);
		}

		private TreeNode AddSectionNode(TreeNodeCollection par, Section sect) {
			return AddNode(par, sect.Name, "attributes/group.gif", "attributes/group.gif",
				"Section " + sect.ID, true);
		}

		private TreeNode AddUserGroupNode(TreeNodeCollection par, int id, UserGrouper.UserGroup group) {
			string title = String.Format("Students ({0}-{1})",
				UserGrouper.UserGroup.Display(group.UpperBound), 
				UserGrouper.UserGroup.Display(group.LowerBound));
			
			return AddNode(par, title, "attributes/group.gif", "attributes/group.gif",
				"UserGroup " + id + " " + group.UpperBound + " " + group.LowerBound, true);
		}

		private TreeNode AddAllUsersNode(TreeNodeCollection par, int courseID) {
			return AddNode(par, "All Students", "attributes/group.gif", "attributes/group.gif",
				"AllUsers " + courseID, true);
		}

		private TreeNode AddUserNode(TreeNodeCollection par, User user, int asstID, bool expand) {
			return AddNode(par, user.FullName + " (" + user.UserName + ")",
				"attributes/user.gif", "attributes/user.gif",
				"User " + user.UserName + " " + asstID, expand);
		}

		private TreeNode AddGroupFolderNode(TreeNodeCollection par, int asstID) {
			return AddNode(par, "Groups", "attributes/group.gif", "attributes/group.gif",
				"GroupFolder " + asstID, false);
		}

		private TreeNode AddResFolderNode(TreeNodeCollection par, int asstID) {
			return AddNode(par, "Results", "attributes/result.gif", "attributes/result.gif",
				"ResFolder " + asstID, true);
		}

		private TreeNode AddAnnNode(TreeNodeCollection par, Announcement ann) {
			return AddNode(par, ann.Title, "attributes/ann.gif", "attributes/ann.gif",
				"Ann " + ann.ID, false);
		}

		private TreeNode AddFolderNode(TreeNodeCollection par, CFile dir) {
			return AddNode(par, dir.Alias, "attributes/folder.gif", "attributes/folderopen.gif",
				"Folder " + dir.ID, true);
		}

		private TreeNode AddBackupsNode(TreeNodeCollection par, int courseID) {
			string img = "attributes/backup.gif";
			return AddNode(par, "Backups", img, img, "Backups " + courseID, false);
		}

		private TreeNode AddDocumentNode(TreeNodeCollection par, CFile doc) {
			string img = new FileBrowserPagelet().GetFileImage(doc).Substring(6);
			return AddNode(par, doc.Alias, img, img, "Document " + doc.ID, false);
		}

		private TreeNode AddCourseNode(TreeNodeCollection par, Course course) {
			return AddNode(par, course.Name, "attributes/course.gif", "attributes/course.gif",
				"Course " + course.ID, true);
		}

		private TreeNode AddRubricNode(TreeNodeCollection par, Rubric rub) {
			
			string img, expimage, text;
			bool expand;

			if (new Rubrics(Globals.CurrentIdentity).IsHeading(rub)) {
				if (rub.ParentID < 0)
					text = String.Format("Rubric - {0} Points", rub.Points);
				else
					text = rub.Name;
				img = "attributes/folder.gif";
				expimage = "attributes/folderopen.gif";
				expand = true;
			}
			else {		
				text = String.Format("{0} ({1} points)", rub.Name, rub.Points);
				
				if (rub.EvalID < 0) {
					img = "attributes/book.gif";
					expimage = "attributes/book.gif";
				}
				else {
					Evaluation eval = new Evaluations(Globals.CurrentIdentity).GetInfo(rub.EvalID);
					if (eval.Manager == Evaluation.JUNIT_MANAGER) {
						img = "attributes/cylj.gif";
						expimage = "attributes/cylj.gif";
					}
					else if (eval.Manager == Evaluation.CHECKSTYLE_MANAGER) {
						img = "attributes/bookcs.gif";
						expimage = "attributes/bookcs.gif";
					} else {
						img = "attributes/cyl.gif";
						expimage = "attributes/cyl.gif";
					}
				}
				expand = false;
			}

			return AddNode(par, text, img, expimage, "Rubric " + rub.ID.ToString(), expand);
		}

		private TreeNode AddNode(TreeNodeCollection par, string text, string image, string expimage,
			string nodedata, bool expandable) {
			TreeNode root = new TreeNode();
			root.Text = text; 
			root.ImageUrl = image;
			root.ExpandedImageUrl = expimage;
			if (expandable)
				root.Expandable = ExpandableValue.Always;
			root.NodeData = nodedata;
			par.Add(root);
			return root;
		}

		private void tvRubric_Expand(object sender, TreeViewClickEventArgs e) {
			TreeNode node = tvRubric.GetNodeFromIndex(e.Node);
			LoadNode(node);
		}

		private void LoadNode(TreeNode node) {
			string[] tokens = node.NodeData.Split(" ".ToCharArray());
			if (tokens[0] == "Course")
				LoadCourseNode(node, Convert.ToInt32(tokens[1]));
			else if (tokens[0] == "Asst")
				LoadAsstNode(node, Convert.ToInt32(tokens[1]));
			else if (tokens[0] == "Rubric")
				LoadRubricNode(node, Convert.ToInt32(tokens[1]));
			else if (tokens[0] == "Folder")
				LoadFolderNode(node, Convert.ToInt32(tokens[1]));
			else if (tokens[0] == "AnnFolder")
				LoadAnnFolderNode(node, Convert.ToInt32(tokens[1]));
			else if (tokens[0] == "ResFolder")
				LoadResFolderNode(node, Convert.ToInt32(tokens[1]));
			else if (tokens[0] == "Evaluation")
				LoadEvaluationNode(node, Convert.ToInt32(tokens[1]));
			else if (tokens[0] == "Feedback")
				LoadFeedbackNode(node, Convert.ToInt32(tokens[1]));
			else if (tokens[0] == "AutoSys")
				LoadAutoSysNode(node, Convert.ToInt32(tokens[1]));
			else if (tokens[0] == "SectionFolder")
				LoadSectionFolderNode(node, Convert.ToInt32(tokens[1]));
			else if (tokens[0] == "UserGroup")
				LoadUserGroupNode(node, Convert.ToInt32(tokens[1]), tokens[2], tokens[3]);
			else if (tokens[0] == "Section")
				LoadSectionNode(node, Convert.ToInt32(tokens[1]));
			else if (tokens[0] == "AllUsers")
				LoadAllUserNode(node, Convert.ToInt32(tokens[1]));
			else if (tokens[0] == "User")
				LoadUserNode(node, tokens[1], Convert.ToInt32(tokens[2]));
			else if (tokens[0] == "SubjUser")
				LoadSubjUserNode(node, tokens[1], Convert.ToInt32(tokens[2]));
		}

		private void LoadFolderNode(TreeNode par, int fileID) {	
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);	
			par.Nodes.Clear();
			CFile.FileList cdir;
			
			try {
				cdir = fs.ListDirectory(fs.GetFile(fileID));
			} catch (CustomException er) {
				PageError(er.Message); return;
			}

			foreach (CFile file in cdir) {
				if (file.IsDirectory())
					AddFolderNode(par.Nodes, file);
				else
					AddDocumentNode(par.Nodes, file);
			}
		}

		private void LoadAllUserNode(TreeNode par, int courseID) {
			
			par.Nodes.Clear();

			//Add report node
			AddReportNode(par.Nodes, -1);
			
			//Add all users grouped
			User.UserList users = 
				new Courses(Globals.CurrentIdentity).GetMembers(courseID, null);

			UserGrouper grouper = new UserGrouper();
			ArrayList groups = new ArrayList();
			bool cnode = IsCourseNode(par);
			int asstID=0;

			if (!cnode) asstID = GetAsstParentID(par);
			if (grouper.Group(users, groups))
				foreach (UserGrouper.UserGroup group in groups)
					AddUserGroupNode(par.Nodes, courseID, group);
			else
				foreach (User user in users)
					AddUserNode(par.Nodes, user, asstID, !cnode);
		}

		private void LoadSectionFolderNode(TreeNode par, int courseID) {
			
			Courses courseda = new Courses(Globals.CurrentIdentity);
			Section.SectionList sects = courseda.GetSections(courseID);

			//Add sections
			par.Nodes.Clear();
			foreach (Section sect in sects)
				AddSectionNode(par.Nodes, sect);

			//Add All User node
			AddAllUsersNode(par.Nodes, courseID);
		}

		private void LoadAutoSysNode(TreeNode par, int asstID) {
			
			AutoJob.AutoJobList jobs = 
				new AutoJobs(Globals.CurrentIdentity).GetUserAsstJobs(asstID);

			par.Nodes.Clear();
			foreach (AutoJob job in jobs)
				AddAutoJobNode(par.Nodes, job);
		}

		private void LoadAsstNode(TreeNode par, int asstID) {
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);	
			Assignments asstda = new Assignments(Globals.CurrentIdentity);
			Assignment asst = asstda.GetInfo(asstID);
			
			//Load content
			par.Nodes.Clear();
			CFile.FileList cdir = fs.ListDirectory(fs.GetFile(asst.ContentID));
			foreach (CFile file in cdir) {
				if (file.IsDirectory())
					AddFolderNode(par.Nodes, file);
				else
					AddDocumentNode(par.Nodes, file);
			}

			if (StudentMode) {
				AddSubFolderNode(par.Nodes, asstID);
				AddGroupFolderNode(par.Nodes, asstID);
				AddCompetitionNode(par.Nodes, asstID);
				AddResFolderNode(par.Nodes, asstID);
			}
			else {
				//Load rubric
				Rubric rub = asstda.GetRubric(asstID);
				AddRubricNode(par.Nodes, rub);

				AddFeedbackNode(par.Nodes, asstID);
				AddAutoSysNode(par.Nodes, asstID);
	
				AddSectionFolderNode(par.Nodes, asst.CourseID, "Users and Sections");

				AddPermNode(par.Nodes, asstID);
			}
		}

		public static User.UserList GetFeedbackUsers(string userstr) {
			string [] spids = userstr.Split("|".ToCharArray());
			Users userda = new Users(Globals.CurrentIdentity);
			User.UserList users = new User.UserList();
			foreach (string spid in spids) 
				if (spid.Length > 0)
					users.Add(userda.GetInfo(spid, null));
			return users;
		}

		private void LoadFeedbackNode(TreeNode par, int asstID) {
			par.Nodes.Clear();
		
			AddEvaluationNode(par.Nodes, asstID);
			AddCompetitionNode(par.Nodes, asstID);
		}

		private void LoadSubjUserNode(TreeNode par, string username, int asstID) {
			Users userda = new Users(Globals.CurrentIdentity);
			Components.Submission.SubmissionList subs = userda.GetAsstSubmissions(username, asstID);

			foreach (Components.Submission sub in subs)
				AddSubjUserNode(par.Nodes, userda.GetInfo(username, null), asstID, sub, false);
		}

		private void LoadEvaluationNode(TreeNode par, int asstID) {
			string[] tokens = par.NodeData.Split(" ".ToCharArray());
			par.Nodes.Clear();
			if (tokens.Length > 2 && tokens[2].Length > 0) {
				User.UserList users = GetFeedbackUsers(tokens[2]);
				Principals prinda = new Principals(Globals.CurrentIdentity);

				AddAggregateNode(par.Nodes, asstID, tokens[2]); // Aggregate node
				foreach (User user in users) {
					Components.Submission sub = prinda.GetLatestSubmission(user.PrincipalID, asstID);
					if (sub != null) 	
						AddSubjUserNode(par.Nodes, user, asstID, sub, true);		
				}
				par.Expandable = ExpandableValue.Always; par.Expanded = true;
			} else {
				par.Expandable = ExpandableValue.Auto; par.Expanded = false;
			}
		}

		private void LoadAnnFolderNode(TreeNode par, int courseID) {		
			Announcement.AnnouncementList anns = 
				new Courses(Globals.CurrentIdentity).GetAnnouncements(courseID);

			par.Nodes.Clear();
			foreach (Announcement ann in anns)
				AddAnnNode(par.Nodes, ann);
		}

		private void LoadResFolderNode(TreeNode par, int asstID) {	
	
			User user = new Users(Globals.CurrentIdentity).GetInfo(Globals.CurrentUserName, null);
			Components.Submission.SubmissionList subs = 
				new Principals(Globals.CurrentIdentity).GetGradedSubmissions(user.PrincipalID, asstID);

			par.Nodes.Clear();
			foreach (Components.Submission sub in subs)
				AddResSubNode(par.Nodes, sub);
		}

		private void LoadSectionNode(TreeNode par, int sectionID) {

			User.UserList users = new Sections(Globals.CurrentIdentity).GetMembers(sectionID);
			UserGrouper grouper = new UserGrouper();

			par.Nodes.Clear();
			AddReportNode(par.Nodes, sectionID);
			AddPermNode(par.Nodes, sectionID);
			ArrayList groups = new ArrayList();

			bool cnode = IsCourseNode(par);
			int asstID=0;
			if (!cnode) asstID = GetAsstParentID(par);
			if (grouper.Group(users, groups))
				foreach (UserGrouper.UserGroup group in groups)
					AddUserGroupNode(par.Nodes, GetCourseID(), group);
			else
				foreach (User user in users)
					AddUserNode(par.Nodes, user, asstID, !cnode);
		}

		private void LoadUserGroupNode(TreeNode par, int courseID, string ubound, string lbound) {

			User.UserList users;
			int sectID;

			bool sectnode = IsSectionNode(par, out sectID);
			if (!sectnode)
				users = new Courses(Globals.CurrentIdentity).GetMembers(courseID, null);
			else
				users = new Sections(Globals.CurrentIdentity).GetMembers(sectID);

			par.Nodes.Clear();
			bool cnode = IsCourseNode(par);
			int asstID=0;
			if (!cnode) asstID = GetAsstParentID(par);
			new UserGrouper().Regroup(ubound, lbound, users);

			foreach (User user in users)
				AddUserNode(par.Nodes, user, asstID, !cnode);
		}

		private void LoadUserNode(TreeNode par, string username, int asstID) {
			Components.Submission.SubmissionList subs = 
				new Users(Globals.CurrentIdentity).GetAsstSubmissions(username, asstID);

			par.Nodes.Clear();
			foreach (Components.Submission sub in subs)
				AddUserSubNode(par.Nodes, username, sub);
		}

		private void LoadCourseNode(TreeNode par, int courseID) {	
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			Courses courseda = new Courses(Globals.CurrentIdentity);
			Course course = courseda.GetInfo(courseID);
			
			par.Nodes.Clear();
			//Load announcement folder
			if (!StudentMode)
				AddAnnFolderNode(par.Nodes, courseID);

			//Load content
			CFile.FileList cdir = fs.ListDirectory(fs.GetFile(course.ContentID));
			foreach (CFile file in cdir) {
				if (file.IsDirectory())
					AddFolderNode(par.Nodes, file);
				else
					AddDocumentNode(par.Nodes, file);
			}

			//Load assignments
			Assignment.AssignmentList assts;
			if (StudentMode) 
				assts = courseda.GetStudentAssignments(courseID);
			else
				assts = courseda.GetAssignments(courseID);
			foreach (Assignment asst in assts) 
				AddAsstNode(par.Nodes, asst);

			//Add section folder
			if (!StudentMode) {
				AddBackupsNode(par.Nodes, courseID);
				AddSectionFolderNode(par.Nodes, courseID, "Users and Sections");
				AddPermNode(par.Nodes, courseID);
			}
		}

		private void LoadRubricNode(TreeNode node, int rubID) {
			Rubrics rubda = new Rubrics(Globals.CurrentIdentity);
			
			Rubric.RubricList rubs = rubda.GetChildren(rubID);
			node.Nodes.Clear();
			foreach (Rubric rub in rubs)
				AddRubricNode(node.Nodes, rub);
		}

		private void SyncToolbar(int item) {
			switch (item) {
			case ANNFOLDER:
				(tbActions.Items[TB_ASST] as ToolbarButton).ImageUrl = "attributes/asstgray.gif";
				(tbActions.Items[TB_SECTION] as ToolbarButton).ImageUrl = "attributes/groupgray.gif";
				(tbActions.Items[TB_ANN] as ToolbarButton).ImageUrl = "attributes/ann.gif"; 
				(tbActions.Items[TB_FOLDER] as ToolbarButton).ImageUrl = "attributes/foldergray.gif"; 
				(tbActions.Items[TB_DOCUMENT] as ToolbarButton).ImageUrl = "attributes/filebrowser/miscgray.gif";
				tbActions.Items[TB_ASST].Enabled = tbActions.Items[TB_FOLDER].Enabled = 
					tbActions.Items[TB_DOCUMENT].Enabled = false;
				tbActions.Items[TB_ANN].Enabled = true;
				tbActions.Items[TB_SECTION].Enabled = false;

				(tbActions.Items[TB_CANNED] as ToolbarButton).ImageUrl = "attributes/bookgray.gif";
				(tbActions.Items[TB_CHECKSTYLE] as ToolbarButton).ImageUrl = "attributes/bookcsgray.gif";
				(tbActions.Items[TB_AUTO] as ToolbarButton).ImageUrl = "attributes/cylgray.gif"; 
				(tbActions.Items[TB_JUNIT] as ToolbarButton).ImageUrl = "attributes/cyljgray.gif"; 
				(tbActions.Items[TB_HEADING] as ToolbarButton).ImageUrl = "attributes/foldergray.gif";
				tbActions.Items[TB_CANNED].Enabled = tbActions.Items[TB_AUTO].Enabled = 
					tbActions.Items[TB_CHECKSTYLE].Enabled = 
					tbActions.Items[TB_JUNIT].Enabled = tbActions.Items[TB_HEADING].Enabled = false;

				(tbActions.Items[TB_BACKUP] as ToolbarButton).ImageUrl = "attributes/backupgray.gif";
				(tbActions.Items[TB_DELETE] as ToolbarButton).ImageUrl = "attributes/filebrowser/deletegray.gif"; 
				tbActions.Items[TB_BACKUP].Enabled = false;
				tbActions.Items[TB_DELETE].Enabled = false;
				break;
			case COURSE:
				(tbActions.Items[TB_ASST] as ToolbarButton).ImageUrl = "attributes/asst.gif";
				(tbActions.Items[TB_SECTION] as ToolbarButton).ImageUrl = "attributes/groupgray.gif";
				(tbActions.Items[TB_ANN] as ToolbarButton).ImageUrl = "attributes/anngray.gif"; 
				(tbActions.Items[TB_FOLDER] as ToolbarButton).ImageUrl = "attributes/folder.gif";
				(tbActions.Items[TB_DOCUMENT] as ToolbarButton).ImageUrl = "attributes/filebrowser/misc.gif";
				tbActions.Items[TB_ASST].Enabled = tbActions.Items[TB_FOLDER].Enabled = 
					tbActions.Items[TB_DOCUMENT].Enabled = true;
				tbActions.Items[TB_ANN].Enabled = false;
				tbActions.Items[TB_SECTION].Enabled = false;

				(tbActions.Items[TB_CANNED] as ToolbarButton).ImageUrl = "attributes/bookgray.gif";
				(tbActions.Items[TB_CHECKSTYLE] as ToolbarButton).ImageUrl = "attributes/bookcsgray.gif";
				(tbActions.Items[TB_AUTO] as ToolbarButton).ImageUrl = "attributes/cylgray.gif"; 
				(tbActions.Items[TB_JUNIT] as ToolbarButton).ImageUrl = "attributes/cyljgray.gif"; 
				(tbActions.Items[TB_HEADING] as ToolbarButton).ImageUrl = "attributes/foldergray.gif";
				tbActions.Items[TB_CANNED].Enabled = tbActions.Items[TB_AUTO].Enabled = 
					tbActions.Items[TB_CHECKSTYLE].Enabled = 
					tbActions.Items[TB_JUNIT].Enabled = tbActions.Items[TB_HEADING].Enabled = false;

				(tbActions.Items[TB_BACKUP] as ToolbarButton).ImageUrl = "attributes/backup.gif";
				(tbActions.Items[TB_DELETE] as ToolbarButton).ImageUrl = "attributes/filebrowser/deletegray.gif"; 
				tbActions.Items[TB_BACKUP].Enabled = true;
				tbActions.Items[TB_DELETE].Enabled = false;
				break;
			case FOLDER:
			case ASSIGNMENT:
				(tbActions.Items[TB_ASST] as ToolbarButton).ImageUrl = "attributes/asstgray.gif";
				(tbActions.Items[TB_SECTION] as ToolbarButton).ImageUrl = "attributes/groupgray.gif";
				(tbActions.Items[TB_ANN] as ToolbarButton).ImageUrl = "attributes/anngray.gif"; 
				(tbActions.Items[TB_FOLDER] as ToolbarButton).ImageUrl = "attributes/folder.gif"; 
				(tbActions.Items[TB_DOCUMENT] as ToolbarButton).ImageUrl = "attributes/filebrowser/misc.gif";
				tbActions.Items[TB_ASST].Enabled = false; tbActions.Items[TB_ANN].Enabled = false;
				tbActions.Items[TB_FOLDER].Enabled = tbActions.Items[TB_DOCUMENT].Enabled = true;
				tbActions.Items[TB_SECTION].Enabled = false;

				(tbActions.Items[TB_CANNED] as ToolbarButton).ImageUrl = "attributes/bookgray.gif";
				(tbActions.Items[TB_CHECKSTYLE] as ToolbarButton).ImageUrl = "attributes/bookcsgray.gif";
				(tbActions.Items[TB_AUTO] as ToolbarButton).ImageUrl = "attributes/cylgray.gif"; 
				(tbActions.Items[TB_JUNIT] as ToolbarButton).ImageUrl = "attributes/cyljgray.gif"; 
				(tbActions.Items[TB_HEADING] as ToolbarButton).ImageUrl = "attributes/foldergray.gif";
				tbActions.Items[TB_CANNED].Enabled = tbActions.Items[TB_AUTO].Enabled = 
					tbActions.Items[TB_CHECKSTYLE].Enabled = 
					tbActions.Items[TB_JUNIT].Enabled = tbActions.Items[TB_HEADING].Enabled = false;

				if (item == ASSIGNMENT) {
					(tbActions.Items[TB_BACKUP] as ToolbarButton).ImageUrl = "attributes/backup.gif";
					tbActions.Items[TB_BACKUP].Enabled = true;
				} else {
					(tbActions.Items[TB_BACKUP] as ToolbarButton).ImageUrl = "attributes/backupgray.gif";
					tbActions.Items[TB_BACKUP].Enabled = false;
				}
				(tbActions.Items[TB_DELETE] as ToolbarButton).ImageUrl = "attributes/filebrowser/delete.gif"; 	
				tbActions.Items[TB_DELETE].Enabled = true;
				break;
			case SECTIONFOLDER:
				(tbActions.Items[TB_ASST] as ToolbarButton).ImageUrl = "attributes/asstgray.gif";
				(tbActions.Items[TB_SECTION] as ToolbarButton).ImageUrl = "attributes/group.gif";
				(tbActions.Items[TB_ANN] as ToolbarButton).ImageUrl = "attributes/anngray.gif"; 
				(tbActions.Items[TB_FOLDER] as ToolbarButton).ImageUrl = "attributes/foldergray.gif"; 
				(tbActions.Items[TB_DOCUMENT] as ToolbarButton).ImageUrl = "attributes/filebrowser/miscgray.gif";
				tbActions.Items[TB_ASST].Enabled = tbActions.Items[TB_FOLDER].Enabled = 
					tbActions.Items[TB_DOCUMENT].Enabled = false;
				tbActions.Items[TB_ANN].Enabled = false;
				tbActions.Items[TB_SECTION].Enabled = true;

				(tbActions.Items[TB_CANNED] as ToolbarButton).ImageUrl = "attributes/bookgray.gif";
				(tbActions.Items[TB_CHECKSTYLE] as ToolbarButton).ImageUrl = "attributes/bookcsgray.gif";
				(tbActions.Items[TB_AUTO] as ToolbarButton).ImageUrl = "attributes/cylgray.gif"; 
				(tbActions.Items[TB_JUNIT] as ToolbarButton).ImageUrl = "attributes/cyljgray.gif"; 
				(tbActions.Items[TB_HEADING] as ToolbarButton).ImageUrl = "attributes/foldergray.gif";
				tbActions.Items[TB_CANNED].Enabled = tbActions.Items[TB_AUTO].Enabled = 
					tbActions.Items[TB_CHECKSTYLE].Enabled = 
					tbActions.Items[TB_JUNIT].Enabled = tbActions.Items[TB_HEADING].Enabled = false;

				(tbActions.Items[TB_BACKUP] as ToolbarButton).ImageUrl = "attributes/backupgray.gif";
				(tbActions.Items[TB_DELETE] as ToolbarButton).ImageUrl = "attributes/filebrowser/deletegray.gif"; 
				tbActions.Items[TB_BACKUP].Enabled = false;
				tbActions.Items[TB_DELETE].Enabled = false;
				break;
			case HEADING:
				(tbActions.Items[TB_ASST] as ToolbarButton).ImageUrl = "attributes/asstgray.gif";
				(tbActions.Items[TB_SECTION] as ToolbarButton).ImageUrl = "attributes/groupgray.gif";
				(tbActions.Items[TB_ANN] as ToolbarButton).ImageUrl = "attributes/anngray.gif"; 
				(tbActions.Items[TB_FOLDER] as ToolbarButton).ImageUrl = "attributes/foldergray.gif"; 
				(tbActions.Items[TB_DOCUMENT] as ToolbarButton).ImageUrl = "attributes/filebrowser/miscgray.gif";
				tbActions.Items[TB_ASST].Enabled = tbActions.Items[TB_FOLDER].Enabled = 
					tbActions.Items[TB_DOCUMENT].Enabled = false;
				tbActions.Items[TB_ANN].Enabled = false;
				tbActions.Items[TB_SECTION].Enabled = false;

				(tbActions.Items[TB_CANNED] as ToolbarButton).ImageUrl = "attributes/book.gif";
				(tbActions.Items[TB_CHECKSTYLE] as ToolbarButton).ImageUrl = "attributes/bookcs.gif";
				(tbActions.Items[TB_AUTO] as ToolbarButton).ImageUrl = "attributes/cyl.gif"; 
				(tbActions.Items[TB_JUNIT] as ToolbarButton).ImageUrl = "attributes/cylj.gif"; 
				(tbActions.Items[TB_HEADING] as ToolbarButton).ImageUrl = "attributes/folder.gif";
				tbActions.Items[TB_CANNED].Enabled = tbActions.Items[TB_AUTO].Enabled = 
					tbActions.Items[TB_CHECKSTYLE].Enabled = 
					tbActions.Items[TB_JUNIT].Enabled = tbActions.Items[TB_HEADING].Enabled = true;

				(tbActions.Items[TB_BACKUP] as ToolbarButton).ImageUrl = "attributes/backupgray.gif";
				(tbActions.Items[TB_DELETE] as ToolbarButton).ImageUrl = "attributes/filebrowser/delete.gif"; 
				tbActions.Items[TB_BACKUP].Enabled = false;
				tbActions.Items[TB_DELETE].Enabled = true;
				break;
			case USER:
			case USERSUB:
			case SECTION:
			case ANNOUNCEMENT:
			case DOCUMENT:
			case CANNED:
			case AUTOMATIC:
			case JUNIT:
			case CHECKSTYLE:
				(tbActions.Items[TB_ASST] as ToolbarButton).ImageUrl = "attributes/asstgray.gif";
				(tbActions.Items[TB_SECTION] as ToolbarButton).ImageUrl = "attributes/groupgray.gif";
				(tbActions.Items[TB_ANN] as ToolbarButton).ImageUrl = "attributes/anngray.gif"; 
				(tbActions.Items[TB_FOLDER] as ToolbarButton).ImageUrl = "attributes/foldergray.gif"; 
				(tbActions.Items[TB_DOCUMENT] as ToolbarButton).ImageUrl = "attributes/filebrowser/miscgray.gif";
				tbActions.Items[TB_ASST].Enabled = tbActions.Items[TB_FOLDER].Enabled = 
					tbActions.Items[TB_DOCUMENT].Enabled = false;
				tbActions.Items[TB_ANN].Enabled = false;
				tbActions.Items[TB_SECTION].Enabled = false;

				(tbActions.Items[TB_CANNED] as ToolbarButton).ImageUrl = "attributes/bookgray.gif";
				(tbActions.Items[TB_CHECKSTYLE] as ToolbarButton).ImageUrl = "attributes/bookcsgray.gif";
				(tbActions.Items[TB_AUTO] as ToolbarButton).ImageUrl = "attributes/cylgray.gif";
				(tbActions.Items[TB_JUNIT] as ToolbarButton).ImageUrl = "attributes/cyljgray.gif"; 
				(tbActions.Items[TB_HEADING] as ToolbarButton).ImageUrl = "attributes/foldergray.gif";
				tbActions.Items[TB_CANNED].Enabled = tbActions.Items[TB_AUTO].Enabled = 
					tbActions.Items[TB_CHECKSTYLE].Enabled = 
					tbActions.Items[TB_JUNIT].Enabled = tbActions.Items[TB_HEADING].Enabled = false;

				(tbActions.Items[TB_BACKUP] as ToolbarButton).ImageUrl = "attributes/backupgray.gif";
				(tbActions.Items[TB_DELETE] as ToolbarButton).ImageUrl = "attributes/filebrowser/delete.gif"; 
				tbActions.Items[TB_BACKUP].Enabled = false;
				tbActions.Items[TB_DELETE].Enabled = true;
				break;
			case ALLUSERS:
			case SUBJUSER:
			case REPORT:
			case FEEDBACK:
			case AUTOSYS:
			case PERMISSION:
			case BACKUPS:
				(tbActions.Items[TB_ASST] as ToolbarButton).ImageUrl = "attributes/asstgray.gif";
				(tbActions.Items[TB_SECTION] as ToolbarButton).ImageUrl = "attributes/groupgray.gif";
				(tbActions.Items[TB_ANN] as ToolbarButton).ImageUrl = "attributes/anngray.gif"; 
				(tbActions.Items[TB_FOLDER] as ToolbarButton).ImageUrl = "attributes/foldergray.gif"; 
				(tbActions.Items[TB_DOCUMENT] as ToolbarButton).ImageUrl = "attributes/filebrowser/miscgray.gif";
				tbActions.Items[TB_ASST].Enabled = tbActions.Items[TB_FOLDER].Enabled = 
					tbActions.Items[TB_DOCUMENT].Enabled = false;
				tbActions.Items[TB_ANN].Enabled = false;
				tbActions.Items[TB_SECTION].Enabled = false;

				(tbActions.Items[TB_CANNED] as ToolbarButton).ImageUrl = "attributes/bookgray.gif";
				(tbActions.Items[TB_CHECKSTYLE] as ToolbarButton).ImageUrl = "attributes/bookcsgray.gif";
				(tbActions.Items[TB_AUTO] as ToolbarButton).ImageUrl = "attributes/cylgray.gif"; 
				(tbActions.Items[TB_JUNIT] as ToolbarButton).ImageUrl = "attributes/cyljgray.gif"; 
				(tbActions.Items[TB_HEADING] as ToolbarButton).ImageUrl = "attributes/foldergray.gif";
				tbActions.Items[TB_CANNED].Enabled = tbActions.Items[TB_AUTO].Enabled = 
					tbActions.Items[TB_CHECKSTYLE].Enabled = 
					tbActions.Items[TB_JUNIT].Enabled = tbActions.Items[TB_HEADING].Enabled = false;

				(tbActions.Items[TB_BACKUP] as ToolbarButton).ImageUrl = "attributes/backupgray.gif";
				(tbActions.Items[TB_DELETE] as ToolbarButton).ImageUrl = "attributes/filebrowser/deletegray.gif"; 
				tbActions.Items[TB_BACKUP].Enabled = false;
				tbActions.Items[TB_DELETE].Enabled = false;
				break;
			}
		}

		private int GetNodeIndex(TreeNode node) {
			return Convert.ToInt32(node.NodeData.Split(" ".ToCharArray())[1]);
		}

		private int GetNodeType(TreeNode node) {
			
			Rubrics rubda = new Rubrics(Globals.CurrentIdentity);

			string[] tokens = node.NodeData.Split(" ".ToCharArray());
			if (tokens[0] == "Course")
				return COURSE;
			else if (tokens[0] == "Asst")
				return ASSIGNMENT;
			else if (tokens[0] == "Rubric") {
				Rubric rub = rubda.GetInfo(Convert.ToInt32(tokens[1]));
				if (new Rubrics(Globals.CurrentIdentity).IsHeading(rub))
					return HEADING;
				else if (rub.EvalID >= 0)
					return AUTOMATIC;
				else
					return CANNED;
			}
			else if (tokens[0] == "Folder")
				return FOLDER;
			else if (tokens[0] == "Document")
				return DOCUMENT;
			else if (tokens[0] == "AnnFolder")
				return ANNFOLDER;
			else if (tokens[0] == "Ann")
				return ANNOUNCEMENT;
			else if (tokens[0] == "SubFolder")
				return SUBFOLDER;
			else if (tokens[0] == "GroupFolder")
				return GROUPFOLDER;
			else if (tokens[0] == "ResFolder")
				return RESULTFOLDER;
			else if (tokens[0] == "ResSub")
				return RESSUB;
			else if (tokens[0] == "Feedback")
				return FEEDBACK;
			else if (tokens[0] == "Evaluation")
				return EVALUATION;
			else if (tokens[0] == "Competition")
				return COMPETITION;
			else if (tokens[0] == "SubjUser")
				return SUBJUSER;
			else if (tokens[0] == "AutoSys")
				return AUTOSYS;
			else if (tokens[0] == "AutoJob")
				return AUTOJOB;
			else if (tokens[0] == "SectionFolder")
				return SECTIONFOLDER;
			else if (tokens[0] == "Section")
				return SECTION;
			else if (tokens[0] == "Backups")
				return BACKUPS;
			else if (tokens[0] == "UserGroup")
				return USERGROUP;
			else if (tokens[0] == "User")
				return USER;
			else if (tokens[0] == "Report")
				return REPORT;
			else if (tokens[0] == "AllUsers")
				return ALLUSERS;
			else if (tokens[0] == "Perm")
				return PERMISSION;
			else if (tokens[0] == "UserSub")
				return USERSUB;
			else if (tokens[0] == "Aggregate")
				return AGGREGATE;
			
			return -1;
		}

		private void ActivateNodeView(TreeNode node) {
			int type = GetNodeType(node);

			if (type == HEADING || type == AUTOMATIC || type == CANNED)
				SwitchView(node, RUBRICVIEW);
			else if (type == COURSE) 
				SwitchView(node, STUCOURSEVIEW);
			else if (type == ASSIGNMENT) {
				if (!StudentMode)
					SwitchView(node, ASSTVIEW);
				else
					mpViews.SelectedIndex = MAXVIEWS;
			}
			else if (type == ANNOUNCEMENT) 
				SwitchView(node, ANNVIEW);
			else if (type == FOLDER || type == DOCUMENT)
				SwitchView(node, CONTENTVIEW);
			else if (type == SUBFOLDER) 
				SwitchView(node, SUBSYSVIEW);
			else if (type == GROUPFOLDER)
				SwitchView(node, GROUPVIEW);
			else if (type == EVALUATION)
				SwitchView(node, EVALUATIONVIEW);
			else if (type == COMPETITION)
				SwitchView(node, COMPETITIONVIEW);
			else if (type == AUTOSYS)
				SwitchView(node, AUTOSYSVIEW);
			else if (type == AUTOJOB)
				SwitchView(node, AUTOJOBVIEW);
			else if (type == SECTIONFOLDER)
				SwitchView(node, SECTIONFOLDERVIEW);
			else if (type == SECTION)
				SwitchView(node, SECTIONVIEW);
			else if (type == BACKUPS)
				SwitchView(node, BACKUPSVIEW);
			else if (type == AGGREGATE) {
				ActivateParams ap = new ActivateParams();
				string[] tokens = node.NodeData.Split(" ".ToCharArray());
				ap.ID = Convert.ToInt32(tokens[1]);
				ap.Auxiliary = tokens[2];

				mpViews.SelectedIndex = AGGREGATEVIEW;
				m_views[AGGREGATEVIEW].Activate(ap);
			}
			else if (type == USER) {
				ActivateParams ap = new ActivateParams();
				ap.ID = Convert.ToInt32(Request.Params["CourseID"]);
				ap.Auxiliary = node.NodeData.Split(" ".ToCharArray())[1];

				mpViews.SelectedIndex = USRCOURSERPTVIEW;
				m_views[USRCOURSERPTVIEW].Activate(ap);
			}
			else if (type == USERSUB) {
				string[] tokens = node.NodeData.Split(" ".ToCharArray());
				string username = tokens[1];
				int subID = Convert.ToInt32(tokens[2]);

				ActivateParams ap = new ActivateParams();
				ap.ID = subID;
				ap.StudentMode = true;

				mpViews.SelectedIndex = SUBJFEEDBACKVIEW;
				m_views[SUBJFEEDBACKVIEW].Activate(ap);
			}
			else if (type == REPORT) {
				if (IsCourseNode(node)) {
					ActivateParams ap = new ActivateParams();
					ap.Auxiliary = Request.Params["CourseID"];
					ap.ID = Convert.ToInt32(node.NodeData.Split(" ".ToCharArray())[1]);

					mpViews.SelectedIndex = GRPCOURSERPTVIEW;
					m_views[GRPCOURSERPTVIEW].Activate(ap);
				} else {
					ActivateParams ap = new ActivateParams();
					ap.Auxiliary = GetAsstParentID(node).ToString();
					ap.ID = Convert.ToInt32(node.NodeData.Split(" ".ToCharArray())[1]);

					mpViews.SelectedIndex = GRPASSTRPTVIEW;
					m_views[GRPASSTRPTVIEW].Activate(ap);
				}
			}
			else if (type == SUBJUSER) {
				ActivateParams ap = new ActivateParams();
				string[] tokens = node.NodeData.Split(" ".ToCharArray());
				ap.Auxiliary = tokens[2];
				ap.ID = Convert.ToInt32(tokens[3]);
				ap.StudentMode = false;

				mpViews.SelectedIndex = SUBJFEEDBACKVIEW;
				m_views[SUBJFEEDBACKVIEW].Activate(ap);
			}
			else if (type == RESSUB) {
				ActivateParams ap = new ActivateParams();
				int subID = GetNodeIndex(node);

				ap.ID = subID;
				ap.StudentMode = true;

				mpViews.SelectedIndex = SUBJFEEDBACKVIEW;
				m_views[SUBJFEEDBACKVIEW].Activate(ap);
			}
			else if (type == PERMISSION) {
				ActivateParams ap = new ActivateParams();
				ap.ID = GetNodeIndex(node);
				int sectID;
				if (IsSectionNode(node, out sectID))
					ap.Auxiliary = Permission.SECTION;
				else if (IsCourseNode(node)) 
					ap.Auxiliary = Permission.COURSE;
				else
					ap.Auxiliary = Permission.ASSIGNMENT;
				
				mpViews.SelectedIndex = PERMVIEW;
				m_views[PERMVIEW].Activate(ap);
			}
			else
				mpViews.SelectedIndex = MAXVIEWS;
			
			SyncToolbar(type);
		}

		private int GetAsstParentID(TreeNode node) {
			TreeNode par;
			par = (TreeNode) node.Parent;
			while (true) {		
				if (GetNodeType(par) == ASSIGNMENT)
					return GetNodeIndex(par);
				par = (TreeNode) par.Parent;
			}
		}

		private bool IsSectionNode(TreeNode node, out int id) {
			TreeNode par;
			par = (TreeNode) node.Parent;
			while (true) {		
				if (GetNodeType(par) == SECTION) {
					id = GetNodeIndex(par);
					return true;
				}
				if (GetNodeType(par) == COURSE) {
					id = -1;
					return false;
				}
				par = (TreeNode) par.Parent;
			}
		}

		private TreeNode GetCurrentRubric(TreeNode node) {
			if (node == null)
				return null;
			else if (GetNodeType(node) == HEADING) {
				Rubric rub = new Rubrics(Globals.CurrentIdentity).GetInfo(
					Convert.ToInt32(node.NodeData.Split(" ".ToCharArray())[1]));
				if (rub.ParentID == -1)
					return node;
				else
					return GetCurrentRubric((TreeNode)node.Parent);
			} else
				return GetCurrentRubric((TreeNode)node.Parent);
		}

		private bool IsCourseNode(TreeNode node) {	
			TreeNode par;
			par = (TreeNode) node.Parent;
			while (true) {		
				if (GetNodeType(par) == COURSE)
					return true;
				if (GetNodeType(par) == ASSIGNMENT)
					return false;
				par = (TreeNode) par.Parent;
			}
		}

		private void tvRubric_SelectedIndexChange(object sender, TreeViewSelectEventArgs e) {
			TreeNode node = tvRubric.GetNodeFromIndex(e.NewNode);
			ActivateNodeView(node);
		}

		private void SwitchView(TreeNode node, int ty) {
			mpViews.SelectedIndex = ty;
			ActivateParams ap = new ActivateParams();

			ap.ID = GetNodeIndex(node);
			ap.StudentMode = StudentMode;
			m_views[ty].Activate(ap);
		}

		private void CreateRubricEntry(int type) {

			TreeNode par = tvRubric.GetNodeFromIndex(tvRubric.SelectedNodeIndex);
			int parentID = Convert.ToInt32(par.NodeData.Split(" ".ToCharArray())[1]);
			int evalID=-1;
			double points=0;
			string ename="New Entry";

			Rubric rubpar = new Rubrics(Globals.CurrentIdentity).GetInfo(parentID);
			AutoEvaluation eval = new AutoEvaluation();
			string[] tools;
			try {
				switch (type) {
				case AUTOMATIC:				
					eval.Name = "New Automatic Entry";
					eval.AsstID = rubpar.AsstID;
					eval.Competitive = false;
					eval.Creator = Globals.CurrentUserName; 
					eval.RunOnSubmit = false;
					eval.Manager = Evaluation.NO_MANAGER;
						
					tools = ExternalToolFactory.GetInstance().ListKeys();
					if (tools.Length > 0)
						eval.RunTool = tools[0];
					else
						eval.RunTool = "No tools installed";

					eval.ToolVersioning = (int) ExternalToolFactory.VersionCompare.NONE;
					eval.ToolVersion = "";
					eval.RunToolArgs = "";
					eval.TimeLimit = 60;
					eval.IsBuild = false;
					eval.ResultType = Result.AUTO_TYPE;

					new Evaluations(Globals.CurrentIdentity).CreateAuto(eval, new EmptySource());
					evalID = eval.ID;
					ename = eval.Name;
					break;

				case CHECKSTYLE:
					eval = new AutoEvaluation();
					eval.Name = "New CheckStyle Entry";
					eval.AsstID = rubpar.AsstID;
					eval.Competitive = false;
					eval.Creator = Globals.CurrentUserName; 
					eval.RunOnSubmit = false;
					eval.Manager = Evaluation.CHECKSTYLE_MANAGER;
					eval.RunTool = "Java";
					eval.ToolVersion = "1.4";
					eval.ToolVersioning = (int) ExternalToolFactory.VersionCompare.ATLEAST;
					eval.RunToolArgs = "CheckStyle";
					eval.TimeLimit = 280;
					eval.IsBuild = false;
					eval.ResultType = Result.SUBJ_TYPE;

					new Evaluations(Globals.CurrentIdentity).CreateAuto(eval, new EmptySource());
			
					evalID = eval.ID;
					ename = eval.Name;
					break;

				case JUNIT:
					eval = new AutoEvaluation();
					eval.Name = "New JUnit Entry";
					eval.AsstID = rubpar.AsstID;
					eval.Competitive = false;
					eval.Creator = Globals.CurrentUserName; 
					eval.RunOnSubmit = false;
					eval.Manager = Evaluation.JUNIT_MANAGER;
					eval.RunTool = "Perl";
					eval.ToolVersion = "5.0";
					eval.ToolVersioning = (int) ExternalToolFactory.VersionCompare.ATLEAST;
					eval.RunToolArgs = "jdisco.pl r 0";
					eval.TimeLimit = 0;
					eval.IsBuild = false;
					eval.ResultType = Result.AUTO_TYPE;

					new Evaluations(Globals.CurrentIdentity).CreateAuto(
						eval, new EmptySource());
			
					evalID = eval.ID;
					ename = eval.Name;
					break;

				case CANNED:
					evalID=-1;
					ename = "New Human/Subjective Entry";
					break;
				case HEADING:
					points=-1;
					ename = "New Folder Heading";
					break;
				}

				new Rubrics(Globals.CurrentIdentity).CreateItem(parentID, 
					ename, "Hit modify and enter a description", points, evalID);
			
			} catch (CustomException er) {
				PageError(er.Message);
			}

			LoadRubricNode(par, parentID);
			par.Expanded = true;
		}

		private void lnkDelete_Click(object sender, System.EventArgs e) {
			
			int rubID = GetCurrentID();
			try {
				new Rubrics(Globals.CurrentIdentity).Delete(rubID);
			} catch (CustomException er) {
				PageError(er.Message);
				return;
			}

			TreeNode parent = GetCurrentParent();
			//		LoadRubricNode(parent); parent.Expanded = true;
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
			this.tvRubric.Expand += new Microsoft.Web.UI.WebControls.ClickEventHandler(this.tvRubric_Expand);
			this.tvRubric.SelectedIndexChange += new Microsoft.Web.UI.WebControls.SelectEventHandler(this.tvRubric_SelectedIndexChange);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void views_Refresh(object sender, RefreshEventArgs args) {
			TreeNode node = GetCurrentNode();
			
			if (args.Auxiliary.Length > 0 && args.Auxiliary != REFRESH_RESET)
				node.NodeData += " " + args.Auxiliary;
			else if (args.Auxiliary == REFRESH_RESET) {
				string[] tokens = node.NodeData.Split(" ".ToCharArray());
				node.NodeData = tokens[0] + " " + tokens[1];
			}
			LoadNode(node);
			string nd = node.NodeData;

			TreeNode par = GetCurrentParent();
			if (par != null && args.LoadParent) 
				LoadNode(par);

			if (args.LoadRubric) {
				TreeNode rubnode = GetCurrentRubric(node);
				if (rubnode != null) {
					Rubric rub = new Rubrics(Globals.CurrentIdentity).GetInfo(
						Convert.ToInt32(rubnode.NodeData.Split(" ".ToCharArray())[1]));
					rubnode.Text = String.Format("Rubric - {0} Points", rub.Points);
				}
			}

			if (par != null) {
				bool seled = false;
				foreach (TreeNode c in par.Nodes)
					if (c.NodeData == nd) {
						tvRubric.SelectedNodeIndex = c.GetNodeIndex(); seled = true; break;
					}
				if (!seled) {
					tvRubric.SelectedNodeIndex = par.GetNodeIndex();
					ActivateNodeView(par);
				}
			}
		}

	}

	public class RefreshEventArgs : EventArgs {
		
		public RefreshEventArgs() { }
		public RefreshEventArgs(string aux, bool parent, bool rubric) { 
			Auxiliary=aux; LoadParent=parent; LoadRubric=rubric; }

		public string Auxiliary="";
		public bool LoadParent=false, LoadRubric=false;
	}
}
