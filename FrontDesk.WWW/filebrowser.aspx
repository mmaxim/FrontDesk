<%@ Register TagPrefix="uc1" TagName="filebrowser" Src="Controls/Filesys/filebrowser.ascx" %>
<%@ Page language="c#" Codebehind="filebrowser.aspx.cs" AutoEventWireup="false" Inherits="FrontDesk.Pages.FileBrowserPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FrontDesk File Browser</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<uc1:filebrowser id="ucFiles" runat="server"></uc1:filebrowser>
		</form>
	</body>
</HTML>
