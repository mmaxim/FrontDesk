<%@ Register TagPrefix="userpage" TagName="Courses" Src="Pagelets/defaultcourses.ascx" %>
<%@ Register TagPrefix="userpage" TagName="User" Src="Pagelets/defaultuserinfo.ascx" %>
<%@ Page language="c#" SmartNavigation="true" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="FrontDesk.Pages.DefaultPage" %>
<%@ Register TagPrefix="cc2" Namespace="CustomButton" Assembly="ClickOnceButton" %>
<%@ Import Namespace="FrontDesk.Components" %>
<%@ Register TagPrefix="mytab" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>FrontDesk Course Listing</title>
		<meta content="True" name="vs_snapToGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="attributes/frontdesk.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form runat="server">
			<div id="main_wrapper">
				<div id="main_content">
					<div id="nav_menu">
						<table cellSpacing="0" cellPadding="0">
							<tr>
								<td style="WIDTH: 675px"></td>
								<td><span><b><asp:label id="lblID" Runat="server"></asp:label></b></span><span>[<A href="default.aspx">Home</A>]</span>
									<span>[<a href="">Help</a>]</span><span>[<asp:linkbutton id="cmdLogout" Runat="server" Text="Logout"></asp:linkbutton>]</span></td>
							</tr>
						</table>
					</div>
					<div id="content"><font style="FONT-SIZE: 16pt">Welcome to FrontDesk,
							<asp:label id="lblName" runat="server" Text="Label"></asp:label></font><br>
						<div id="divAdmin" runat="server"><IMG src="attributes/key.gif" align="absMiddle">&nbsp;<A href="admin.aspx">Administrator 
								Access</A>
						</div>
						<br>
						<!-- CONTENT AREA START --><mytab:tabstrip id="tsVert" style="FONT-WEIGHT: bold" runat="server" TargetID="mpVert">
							<mytab:Tab Text="Courses"></mytab:Tab>
							<mytab:TabSeparator></mytab:TabSeparator>
							<mytab:Tab Text="User Profile"></mytab:Tab>
							<mytab:TabSeparator DefaultStyle="width:100%;"></mytab:TabSeparator>
						</mytab:tabstrip><mytab:multipage id="mpVert" style="BORDER-TOP-WIDTH: 1px; BORDER-RIGHT: #000000 1px solid; PADDING-RIGHT: 5px; PADDING-LEFT: 5px; PADDING-BOTTOM: 25px; BORDER-LEFT: #000000 1px solid; BORDER-TOP-COLOR: #000000; PADDING-TOP: 5px; BORDER-BOTTOM: #000000 1px solid"
							runat="server">
							<mytab:PageView>
								<userpage:Courses id="course" runat="server" />
							</mytab:PageView>
							<mytab:PageView>
								<userpage:User id="user" runat="server" />
							</mytab:PageView>
						</mytab:multipage>
		</form>
		</DIV>
		<div id="footer" style="WIDTH: 770px"><span class="left">© 2004 <a href="http://www.circagroup.com">
					circa::group</a></span> <span class="center">For best results use IE6+ or 
				Mozilla 1.5+</span><span class="right">Phoenix Edition</span>
		</div>
		</DIV></DIV>
	</body>
</HTML>
