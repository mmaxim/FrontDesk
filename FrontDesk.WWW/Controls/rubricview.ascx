<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="rubricview.ascx.cs" Inherits="FrontDesk.Controls.RubricViewControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<DIV style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; OVERFLOW: auto; BORDER-LEFT: 1px solid; WIDTH: 95%; BORDER-BOTTOM: 1px solid;  HEIGHT: 130px"
	ms_positioning="GridLayout" id="divMain" runat="server">
	<iewc:treeview id="tvRubric" BorderStyle="None" AutoPostBack="True" Width="100%" runat="server"
		></iewc:treeview></DIV>
