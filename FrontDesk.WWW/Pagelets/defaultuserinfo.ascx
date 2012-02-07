<%@ Control Language="c#" AutoEventWireup="false" Codebehind="defaultuserinfo.ascx.cs" Inherits="FrontDesk.Pages.Pagelets.DefaultUserInfoPagelet" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<meta name="vs_snapToGrid" content="True">
<h2>User Information</h2>
<i>Edit your user information for the FrontDesk system. If you do not wish to 
	change your password, leave both password boxes blank.</i><br>
<br>
<DIV style="WIDTH: 672px; POSITION: relative; HEIGHT: 216px" ms_positioning="GridLayout">
	<asp:TextBox id="txtFirst" style="Z-INDEX: 101; LEFT: 24px; POSITION: absolute; TOP: 48px" runat="server"></asp:TextBox>
	<asp:TextBox id="txtOldPassword" style="Z-INDEX: 102; LEFT: 24px; POSITION: absolute; TOP: 144px"
		runat="server" TextMode="Password"></asp:TextBox>
	<asp:TextBox id="txtNewPassword" style="Z-INDEX: 103; LEFT: 200px; POSITION: absolute; TOP: 144px"
		runat="server" TextMode="Password"></asp:TextBox>
	<asp:TextBox id="txtEmail" style="Z-INDEX: 104; LEFT: 24px; POSITION: absolute; TOP: 96px" runat="server"
		Width="208px"></asp:TextBox>
	<asp:TextBox id="txtLast" style="Z-INDEX: 105; LEFT: 200px; POSITION: absolute; TOP: 48px" runat="server"
		Width="184px"></asp:TextBox>
	<asp:Button id="cmdUpdate" style="Z-INDEX: 106; LEFT: 24px; POSITION: absolute; TOP: 184px"
		runat="server" Text="Update"></asp:Button>
	<asp:Label id="lblUserName" style="Z-INDEX: 107; LEFT: 24px; POSITION: absolute; TOP: 8px"
		runat="server" Width="176px">Label</asp:Label>
	<DIV style="DISPLAY: inline; Z-INDEX: 108; LEFT: 24px; WIDTH: 48px; POSITION: absolute; TOP: 32px; HEIGHT: 19px"
		ms_positioning="FlowLayout">First:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 109; LEFT: 200px; WIDTH: 48px; POSITION: absolute; TOP: 32px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Last:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 110; LEFT: 24px; WIDTH: 96px; POSITION: absolute; TOP: 128px; HEIGHT: 16px"
		ms_positioning="FlowLayout">Old password:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 111; LEFT: 24px; WIDTH: 70px; POSITION: absolute; TOP: 80px; HEIGHT: 15px"
		ms_positioning="FlowLayout">Email:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 112; LEFT: 200px; WIDTH: 112px; POSITION: absolute; TOP: 128px; HEIGHT: 16px"
		ms_positioning="FlowLayout">New password:</DIV>
	<asp:Label id="lblError" style="Z-INDEX: 113; LEFT: 464px; POSITION: absolute; TOP: 48px" runat="server"
		Width="192px" Height="136px" Font-Size="8pt" ForeColor="Red">Error</asp:Label>
	<asp:TextBox id="txtRepeat" style="Z-INDEX: 114; LEFT: 200px; POSITION: absolute; TOP: 176px"
		runat="server" TextMode="Password"></asp:TextBox>
	<DIV style="DISPLAY: inline; Z-INDEX: 115; LEFT: 144px; WIDTH: 56px; POSITION: absolute; TOP: 176px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Repeat:</DIV>
</DIV>
