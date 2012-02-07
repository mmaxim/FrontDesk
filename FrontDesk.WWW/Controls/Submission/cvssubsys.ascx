<%@ Control Language="c#" AutoEventWireup="false" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" CodeBehind="cvssubsys.ascx.cs" Inherits="FrontDesk.Controls.Submission.CVSSubmissionControl" %>
<%@ Register TagPrefix="cc1" Namespace="CustomButton" Assembly="ClickOnceButton" %>
<meta content="True" name="vs_snapToGrid">
<i>Enter your CVS server information below to submit (<b>pserver only</b>)</i><br>
Server:
<asp:textbox id="txtServer" runat="server" Width="136px"></asp:textbox>&nbsp;Repository:
<asp:textbox id="txtRepository" runat="server" Width="133px"></asp:textbox><BR>
Module:
<asp:textbox id="txtModule" runat="server" Width="88px"></asp:textbox>&nbsp;Username:
<asp:textbox id="txtUsername" runat="server" Width="96px"></asp:textbox>&nbsp;Password:
<asp:textbox id="txtPassword" runat="server" Width="96px" TextMode="Password"></asp:textbox><br>
&nbsp;&nbsp;&nbsp;<cc1:clickoncebutton id="cmdSubmit" runat="server" Text="Submit" DisableAfterClick="True" DisabledText="Processing Submission..."></cc1:clickoncebutton><asp:label id="lblError" runat="server" Width="407px" Font-Size="8pt" Height="32px" ForeColor="Red"
	Visible="False"></asp:label>
