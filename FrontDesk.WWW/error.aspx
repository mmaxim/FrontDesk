<%@ Page language="c#" Codebehind="error.aspx.cs" AutoEventWireup="false" Inherits="FrontDesk.Pages.ErrorPage" %>
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
		<form runat="server" ID="Form1">
			<div id="main_wrapper">
	
				<div id="main_content">
					<div id="nav_menu"><span><b><asp:label id="lblID" Runat="server"></asp:label></b></span><span>[<a href="default.aspx">Home</a>]</span>
						<span>[<a href="default.aspx">Course Main</a>]</span> <span>[<a href="">Help</a>]</span><span>[<asp:linkbutton id="cmdLogout" Text="Logout" Runat="server"></asp:linkbutton>]</span>
					</div>
					<div id="content">
						<br>
						<h2>An error has occurred</h2>
						<DIV style="WIDTH: 571px; POSITION: relative; HEIGHT: 314px" ms_positioning="GridLayout">
							<asp:Label id="lblError" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px" runat="server"
								Width="512px" Height="224px" ForeColor="Red" Font-Size="10pt" Font-Italic="True">Label</asp:Label></DIV>
						<BR>
			<!-- CONTENT AREA START -->
		</form>
		</DIV>
		<div id="footer" style="WIDTH: 770px"><span class="left">© 2004 <a href="http://www.circagroup.com">
					circa::group</a></span> <span class="right">Phoenix Edition</span>
		</div>
		</DIV></DIV>
	</body>
</HTML>
