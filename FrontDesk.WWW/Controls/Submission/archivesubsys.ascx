<%@ Control Language="c#" AutoEventWireup="false" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" CodeBehind="archivesubsys.ascx.cs" Inherits="FrontDesk.Controls.Submission.ArchiveSubmissionControl" enableViewState="True"%>
<%@ Register TagPrefix="cc2" Namespace="CustomButton" Assembly="ClickOnceButton" %>
<LINK href="../attributes/frontdesk.css" type="text/css" rel="stylesheet">
Select your archive by hitting Browse. <font size="1">(Supported types: <b>
		<asp:Label id="lblTypes" runat="server"></asp:Label></b> )</font>
<DIV style="WIDTH: 534px; POSITION: relative; HEIGHT: 69px" ms_positioning="GridLayout"><INPUT id="fileUpload" style="Z-INDEX: 101; LEFT: 5px; WIDTH: 462px; POSITION: absolute; TOP: 3px; HEIGHT: 22px"
		type="file" size="57" runat="server">
	<asp:label id="lblError" style="Z-INDEX: 102; LEFT: 127px; POSITION: absolute; TOP: 32px" runat="server"
		Font-Size="8pt" ForeColor="Red" Visible="False" Height="28px" Width="403px">Label</asp:label>
	<cc2:ClickOnceButton id="cmdSubmit" style="Z-INDEX: 103; LEFT: 29px; POSITION: absolute; TOP: 33px" runat="server"
		Text="Submit" DisableAfterClick="True" DisabledText="Processing Submission..."></cc2:ClickOnceButton></DIV>
