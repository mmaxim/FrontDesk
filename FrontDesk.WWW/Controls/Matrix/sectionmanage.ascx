<%@ Control Language="c#" AutoEventWireup="false" Codebehind="sectionmanage.ascx.cs" Inherits="FrontDesk.Controls.Matrix.SectionManagementView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<meta name="vs_snapToGrid" content="True">
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Section Membership Management</font><br>
<br>
<IMG alt="" src="attributes/group.jpg" align="absMiddle">
<asp:linkbutton id="lnkSecExpl" runat="server" Width="314px">Click here to launch the Section Explorer</asp:linkbutton>
<DIV style="WIDTH: 457px; POSITION: relative; HEIGHT: 448px" ms_positioning="GridLayout">
	<asp:listbox id="lstSectionUsers" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 24px"
		DataTextField="UserName" Height="128px" runat="server" Width="456px" SelectionMode="Multiple"></asp:listbox>
	<asp:listbox id="lstAllUsers" style="Z-INDEX: 102; LEFT: 0px; POSITION: absolute; TOP: 200px"
		DataTextField="UserName" Height="184px" runat="server" Width="456px" SelectionMode="Multiple"></asp:listbox>
	<asp:button id="cmdAdd" style="Z-INDEX: 103; LEFT: 0px; POSITION: absolute; TOP: 416px" runat="server"
		Text="Add To Section "></asp:button>
	<asp:button id="cmdDrop" style="Z-INDEX: 104; LEFT: 0px; POSITION: relative; TOP: 156px" runat="server"
		Width="149px" Text="Drop From Section"></asp:button>
	<asp:label id="lblMemError" style="Z-INDEX: 105; LEFT: 168px; POSITION: absolute; TOP: 416px"
		Height="22px" runat="server" Width="264px" Visible="False" ForeColor="Red" Font-Size="8pt">Error</asp:label>
	<asp:checkbox id="chkSwitch" style="Z-INDEX: 106; LEFT: 0px; POSITION: absolute; TOP: 392px" Height="22px"
		runat="server" Width="321px" Text="Switch user from other sections" Font-Size="7pt"></asp:checkbox>
	<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; Z-INDEX: 107; LEFT: 0px; WIDTH: 240px; POSITION: absolute; TOP: 8px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Current Section Membership</DIV>
	<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; Z-INDEX: 108; LEFT: 0px; WIDTH: 264px; POSITION: absolute; TOP: 184px; HEIGHT: 19px"
		ms_positioning="FlowLayout">All Course Members</DIV>
</DIV>
