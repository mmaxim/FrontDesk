<%@ Register TagPrefix="userpage" TagName="Register" Src="Pagelets/register.ascx" %>
<%@ Register TagPrefix="userpage" TagName="Login" Src="Pagelets/login.ascx" %>
<%@ Register TagPrefix="mytab" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page language="c#" SmartNavigation="true" Codebehind="login.aspx.cs" AutoEventWireup="false" Inherits="FrontDesk.Pages.LoginPage" %>
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
		<form method="post" runat="server">
			<div id="main_wrapper">
				<div id="banner"></div>
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
					<div id="content">
						<h2>Welcome to FrontDesk Server!</h2>
						<!-- CONTENT AREA START --><mytab:tabstrip id="tsVert" style="FONT-WEIGHT: bold" runat="server" TargetID="mpVert">
							<mytab:Tab Text="Login"></mytab:Tab>
							<mytab:TabSeparator></mytab:TabSeparator>
							<mytab:Tab Text="Register"></mytab:Tab>
							<mytab:TabSeparator DefaultStyle="width:100%;"></mytab:TabSeparator>
						</mytab:tabstrip><mytab:multipage id="mpVert" style="BORDER-TOP-WIDTH: 1px; BORDER-RIGHT: #000000 1px solid; PADDING-RIGHT: 5px; PADDING-LEFT: 5px; PADDING-BOTTOM: 5px; BORDER-LEFT: #000000 1px solid; BORDER-TOP-COLOR: #000000; PADDING-TOP: 5px; BORDER-BOTTOM: #000000 1px solid"
							runat="server">
							<mytab:PageView>
								<userpage:Login id="login" runat="server" />
							</mytab:PageView>
							<mytab:PageView>
								<userpage:Register id="register" runat="server" />
							</mytab:PageView>
						</mytab:multipage>
			<!-- CONTENT AREA END --></form>
		<!-- CONTENT AREA END --> </DIV>
		<div id="footer"><span class="left">© 2004 <a href="http://www.circagroup.com">circa::group</a></span>
			<span class="right">Phoenix Edition</span>
		</div>
		</DIV></DIV>
	</body>
</HTML>
