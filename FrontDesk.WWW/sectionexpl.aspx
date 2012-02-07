<%@ Register TagPrefix="userpage" TagName="SectionExpl" Src="Controls/sectionexpl.ascx" %>
<%@ Page language="c#" SmartNavigation="true" Codebehind="sectionexpl.aspx.cs" AutoEventWireup="false" Inherits="FrontDesk.Pages.SectionExplorerPage" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FrontDesk Section Explorer</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<DIV style="Z-INDEX: 101; LEFT: 11px; WIDTH: 449px; POSITION: absolute; TOP: 5px; HEIGHT: 597px"
				ms_positioning="GridLayout">
				<userpage:SectionExpl runat="server" id="sectionexpl" /></DIV>
		</form>
	</body>
</HTML>
