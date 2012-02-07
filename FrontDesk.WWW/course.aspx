<%@ Register TagPrefix="userpage" TagName="LongTask" Src="Controls/longtask.ascx" %>
<%@ Register TagPrefix="userpage" TagName="Matrix" Src="Pagelets/stucourse.ascx" %>
<%@ Page language="c#" SmartNavigation="false" Codebehind="course.aspx.cs" AutoEventWireup="false" Inherits="FrontDesk.Pages.StudentCoursePage" %>
<%@ Register TagPrefix="mytab" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Import Namespace="FrontDesk.Components" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>FrontDesk Course Info</title>
		<meta content="True" name="vs_snapToGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="attributes/frontdesk.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form runat="server" method="post" ID="Form1">
			<div id="main_wrapper">
				<div id="main_content">
					<div id="nav_menu">
						<table cellSpacing="0" cellPadding="0">
							<tr>
								<td style="WIDTH: 675px"><font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt" face="Verdana" color="#4768a3">FrontDesk™ 
										CourseMatrix™ Student View</font></td>
								<td><span><b><asp:label id="lblID" Runat="server"></asp:label></b></span><span>[<A href="default.aspx">Home</A>]</span>
									<span>[<a href="">Help</a>]</span><span>[<asp:linkbutton id="cmdLogout" Runat="server" Text="Logout"></asp:linkbutton>]</span></td>
							</tr>
						</table>
					</div>
					<div id="content">
						<userpage:Matrix id="ucMatrix" runat="server" />
						<userpage:LongTask id="ucLongTask" runat="server" />
						<!-- CONTENT AREA END -->
					</div>
		</form>
		<div id="footer"><span class="left">© 2004 <a href="http://www.circagroup.com">circa::group</a></span>
			<span class="center">For best results use IE6+ or Mozilla 1.5+</span><span class="right">Phoenix 
				Edition</span>
		</div>
		</DIV></DIV>
	</body>
</HTML>
