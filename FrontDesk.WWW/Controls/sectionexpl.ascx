<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="sectionexpl.ascx.cs" Inherits="FrontDesk.Controls.SectionExplorer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<DIV style="Z-INDEX: 102; LEFT: 0px; VERTICAL-ALIGN: top; OVERFLOW: auto; WIDTH: 411px; BORDER-TOP-STYLE: groove; BORDER-RIGHT-STYLE: groove; BORDER-LEFT-STYLE: groove; POSITION: absolute; TOP: 0px; HEIGHT: 510px; BORDER-BOTTOM-STYLE: groove"
	ms_positioning="GridLayout" id="divSection" runat="server">
	<iewc:TreeView id="tvSection" style="Z-INDEX: 103; LEFT: 0px; POSITION: absolute; TOP: 0px" runat="server"
		BorderStyle="None" AutoPostBack="True" Height="500px" Width="400px"></iewc:TreeView></DIV>
