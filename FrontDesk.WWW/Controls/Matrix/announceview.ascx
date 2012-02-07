<%@ Import Namespace="FrontDesk.Common" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="announceview.ascx.cs" Inherits="FrontDesk.Controls.Matrix.AnnouncementView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<meta name="vs_snapToGrid" content="True">
<DIV style="WIDTH: 401px; POSITION: relative; HEIGHT: 464px" ms_positioning="GridLayout"><asp:textbox id="txtDesc" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 136px" Height="155"
		Width="400px" TextMode="MultiLine" runat="server"></asp:textbox>
	<DIV style="DISPLAY: inline; Z-INDEX: 102; LEFT: 0px; WIDTH: 399px; POSITION: absolute; TOP: 112px; HEIGHT: 21px"
		ms_positioning="FlowLayout">Content: (HTML tags can be enterred to add effects)</DIV>
	<asp:button id="cmdCreate" style="Z-INDEX: 103; LEFT: 8px; POSITION: absolute; TOP: 304px" Width="94px"
		runat="server" Text="Update"></asp:button><asp:label id="lblError" style="Z-INDEX: 104; LEFT: 112px; POSITION: absolute; TOP: 304px"
		Height="16px" Width="280px" runat="server" Visible="False" Font-Size="8pt" ForeColor="Red">Error</asp:label><asp:textbox id="txtTitle" style="Z-INDEX: 105; LEFT: 48px; POSITION: absolute; TOP: 32px" Width="247"
		runat="server"></asp:textbox>
	<DIV style="DISPLAY: inline; Z-INDEX: 106; LEFT: 0px; WIDTH: 45px; POSITION: absolute; TOP: 32px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Title:</DIV>
	<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; FONT-SIZE: 12pt; Z-INDEX: 107; LEFT: 0px; WIDTH: 304px; POSITION: absolute; TOP: 0px; HEIGHT: 19px"
		ms_positioning="FlowLayout">
		Announcement Details</DIV>
	<asp:Label id="lblPreview" style="Z-INDEX: 108; LEFT: 0px; POSITION: absolute; TOP: 360px"
		runat="server"></asp:Label>
	<asp:Label id="lblCourseID" style="Z-INDEX: 109; LEFT: 184px; POSITION: absolute; TOP: 296px"
		runat="server" Visible="False"></asp:Label>
	<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; Z-INDEX: 110; LEFT: 0px; WIDTH: 70px; POSITION: absolute; TOP: 336px; HEIGHT: 15px"
		ms_positioning="FlowLayout">Preview:</DIV>
	<asp:Label id="lblPoster" style="Z-INDEX: 111; LEFT: 0px; POSITION: absolute; TOP: 64px" runat="server">Label</asp:Label>
	<asp:Label id="lblDate" style="Z-INDEX: 112; LEFT: 0px; POSITION: absolute; TOP: 88px" runat="server">Label</asp:Label>
</DIV>
