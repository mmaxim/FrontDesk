<%@ Register TagPrefix="userpage" TagName="Aggregate" Src="aggregateview.ascx" %>
<%@ Register TagPrefix="userpage" TagName="Competition" Src="compete.ascx" %>
<%@ Register TagPrefix="userpage" TagName="GroupCourseReport" Src="groupcoursereport.ascx" %>
<%@ Register TagPrefix="userpage" TagName="UserCourseReport" Src="usercoursereport.ascx" %>
<%@ Register TagPrefix="userpage" TagName="Backups" Src="backups.ascx" %>
<%@ Register TagPrefix="userpage" TagName="Section" Src="sectionmanage.ascx" %>
<%@ Register TagPrefix="userpage" TagName="CourseSect" Src="sections.ascx" %>
<%@ Register TagPrefix="userpage" TagName="SubjFeedback" Src="subjfeed.ascx" %>
<%@ Register TagPrefix="userpage" TagName="Feedback" Src="starteval.ascx" %>
<%@ Register TagPrefix="userpage" TagName="Group" Src="groups.ascx" %>
<%@ Register TagPrefix="userpage" TagName="StuCourse" Src="stucourse.ascx" %>
<%@ Register TagPrefix="userpage" TagName="Ann" Src="announceview.ascx" %>
<%@ Register TagPrefix="userpage" TagName="Rubric" Src="rubricdetails.ascx" %>
<%@ Register TagPrefix="userpage" TagName="Asst" Src="asstview.ascx" %>
<%@ Register TagPrefix="userpage" TagName="SubSys" Src="submit.ascx" %>
<%@ Register TagPrefix="userpage" TagName="AutoSys" Src="autojobs.ascx" %>
<%@ Register TagPrefix="userpage" TagName="AutoJob" Src="autojobtests.ascx" %>
<%@ Register TagPrefix="userpage" TagName="GroupAsstReport" Src="groupasstreport.ascx" %>
<%@ Register TagPrefix="userpage" TagName="Content" Src="content.ascx" %>
<%@ Register TagPrefix="userpage" TagName="Perms" Src="perms.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="coursematrix.ascx.cs" Inherits="FrontDesk.Controls.Matrix.CourseMatrixControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<meta content="True" name="vs_snapToGrid">
<asp:label id="lblError" Visible="False" Font-Size="8pt" ForeColor="Red" Width="725px" Height="16px"
	runat="server"></asp:label>
<DIV id="divMain" style="WIDTH: 944px; POSITION: relative; HEIGHT: 540px" runat="server"
	ms_positioning="GridLayout">
	<DIV id="divRubric" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; Z-INDEX: 101; LEFT: 0px; OVERFLOW: auto; BORDER-LEFT: 1px solid; WIDTH: 350px; BORDER-BOTTOM: 1px solid; POSITION: absolute; TOP: 31px; HEIGHT: 505px"
		runat="server"><iewc:treeview id="tvRubric" style="POSITION: absolute; TOP: 0px" Width="348px" Height="503px"
			runat="server" AutoPostBack="True" SystemImagesPath="/webctrl_client/1_0/treeimages/" AutoSelect="True"></iewc:treeview></DIV>
	<iewc:toolbar id="tbActions" style="Z-INDEX: 104; LEFT: 0px; POSITION: absolute; TOP: 0px" Width="945px"
		runat="server" Font-Size="6pt">
		<iewc:ToolbarButton Text="Asst" ImageUrl="attributes/asst.gif" ID="cmdTBAsst" ToolTip="Create an assignment"></iewc:ToolbarButton>
		<iewc:ToolbarButton Text="Section" ImageUrl="attributes/group.gif" ID="cmdTBSection" ToolTip="Create a Section"></iewc:ToolbarButton>
		<iewc:ToolbarSeparator></iewc:ToolbarSeparator>
		<iewc:ToolbarButton Text="Folder" ImageUrl="attributes/folder.gif" ID="cmdTBFolder" ToolTip="Create Content Folder"></iewc:ToolbarButton>
		<iewc:ToolbarButton Text="Document" ImageUrl="attributes/filebrowser/misc.gif" ID="cmdTBDocument" ToolTip="Create Content Document"></iewc:ToolbarButton>
		<iewc:ToolbarButton Text="Announce" ImageUrl="attributes/ann.gif" ToolTip="Create Announcement"></iewc:ToolbarButton>
		<iewc:ToolbarSeparator></iewc:ToolbarSeparator>
		<iewc:ToolbarButton Text="Human" ImageUrl="attributes/book.gif" ID="cmdTBRubricItem" ToolTip="Create a Generic Human Subjective Item"></iewc:ToolbarButton>
		<iewc:ToolbarButton Text="CheckStyle" ImageUrl="attributes/bookcs.gif" ID="cmdTBRubricCheckStyle" ToolTip="Create a CheckStyle Automatic Subjective Item"></iewc:ToolbarButton>
		<iewc:ToolbarButton Text="JUnit" ImageUrl="attributes/cylj.gif" ID="cmdTBRubricAutoJUnit" ToolTip="Create a JUnit Automatic Objective Test"></iewc:ToolbarButton>
		<iewc:ToolbarButton Text="Auto" ImageUrl="attributes/cyl.gif" ID="cmdTBRubricAuto" ToolTip="Create a Generic Automatic Objective Test"></iewc:ToolbarButton>
		<iewc:ToolbarButton Text="Heading" ImageUrl="attributes/folder.gif" ID="cmdTBRubricHeading" ToolTip="Create a Rubric Heading"></iewc:ToolbarButton>
		<iewc:ToolbarSeparator></iewc:ToolbarSeparator>
		<iewc:ToolbarButton Text="Backup" ImageUrl="attributes/backup.gif" ToolTip="Backup Item"></iewc:ToolbarButton>
		<iewc:ToolbarButton Text="Delete" ImageUrl="attributes/filebrowser/delete.gif" ToolTip="Delete Item"></iewc:ToolbarButton>
	</iewc:toolbar>
	<DIV id="divRight" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; PADDING-LEFT: 10px; Z-INDEX: 102; LEFT: 352px; OVERFLOW: auto; BORDER-LEFT: 1px solid; WIDTH: 585px; PADDING-TOP: 10px; BORDER-BOTTOM: 1px solid; POSITION: absolute; TOP: 31px; HEIGHT: 495px"
		runat="server" ms_positioning="GridLayout"><iewc:multipage id="mpViews" runat="server">
			<iewc:PageView>
				<userpage:Rubric runat="server" id="ucRubric" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:Asst runat="server" id="ucAsst" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:Ann runat="server" id="ucAnn" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:SubSys runat="server" id="ucSubsys" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:StuCourse runat="server" id="ucStuCourse" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:Group runat="server" id="ucGroup" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:Feedback runat="server" id="ucFeedback" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:SubjFeedback runat="server" id="ucSubjFeedback" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:AutoSys runat="server" id="ucAutoSys" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:AutoJob runat="server" id="ucAutoJob" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:CourseSect runat="server" id="ucCourseSect" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:Section runat="server" id="ucSection" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:Backups runat="server" id="ucBackups" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:UserCourseReport runat="server" id="ucUserCourseReport" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:GroupCourseReport runat="server" id="ucGroupCourseReport" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:GroupAsstReport runat="server" id="ucGroupAsstReport" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:Content runat="server" id="ucContent" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:Perms runat="server" id="ucPerms" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:Competition runat="server" id="ucCompete" />
			</iewc:PageView>
			<iewc:PageView>
				<userpage:Aggregate runat="server" id="ucAggregate" />
			</iewc:PageView>
			<iewc:PageView>
				<i>No data available for this item, try expanding by clicking on the plus to access 
					the features of this item.</i></iewc:PageView>
		</iewc:multipage></DIV>
</DIV>
