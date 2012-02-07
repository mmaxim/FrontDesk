<%@ Control Language="c#" AutoEventWireup="false" Codebehind="register.ascx.cs" Inherits="FrontDesk.Pages.Pagelets.RegisterPagelet" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<meta name="vs_snapToGrid" content="True">
<h2>Register</h2>
<EM>Enter your vital information below and remember to jot down your password in a 
	secure area.</EM><br>
<br>
<DIV style="WIDTH: 585px; POSITION: relative; HEIGHT: 256px" ms_positioning="GridLayout">
	<asp:TextBox id="txtPassword" style="Z-INDEX: 101; LEFT: 16px; POSITION: absolute; TOP: 128px"
		runat="server" TextMode="Password" tabIndex="3"></asp:TextBox>
	<asp:TextBox id="txtEmail" style="Z-INDEX: 102; LEFT: 16px; POSITION: absolute; TOP: 176px" runat="server"
		tabIndex="5"></asp:TextBox>
	<asp:TextBox id="txtLastName" style="Z-INDEX: 103; LEFT: 176px; POSITION: absolute; TOP: 80px"
		runat="server" tabIndex="2"></asp:TextBox>
	<asp:TextBox id="txtRepPassword" style="Z-INDEX: 104; LEFT: 176px; POSITION: absolute; TOP: 128px"
		runat="server" TextMode="Password" tabIndex="4"></asp:TextBox>
	<asp:TextBox id="txtUsername" style="Z-INDEX: 105; LEFT: 16px; POSITION: absolute; TOP: 32px"
		runat="server"></asp:TextBox>
	<asp:TextBox id="txtFirstName" style="Z-INDEX: 106; LEFT: 16px; POSITION: absolute; TOP: 80px"
		runat="server" tabIndex="1"></asp:TextBox>
	<DIV style="DISPLAY: inline; Z-INDEX: 107; LEFT: 16px; WIDTH: 70px; POSITION: absolute; TOP: 16px; HEIGHT: 15px"
		ms_positioning="FlowLayout">Username:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 108; LEFT: 16px; WIDTH: 120px; POSITION: absolute; TOP: 64px; HEIGHT: 16px"
		ms_positioning="FlowLayout">First Name:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 109; LEFT: 176px; WIDTH: 120px; POSITION: absolute; TOP: 64px; HEIGHT: 16px"
		ms_positioning="FlowLayout">Last Name:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 110; LEFT: 16px; WIDTH: 112px; POSITION: absolute; TOP: 112px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Password:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 111; LEFT: 176px; WIDTH: 160px; POSITION: absolute; TOP: 112px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Repeat Password:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 112; LEFT: 16px; WIDTH: 70px; POSITION: absolute; TOP: 160px; HEIGHT: 15px"
		ms_positioning="FlowLayout">Email:</DIV>
	<asp:Button id="cmdRegister" style="Z-INDEX: 113; LEFT: 48px; POSITION: absolute; TOP: 208px"
		runat="server" Text="Register" tabIndex="6"></asp:Button>
	<asp:Label id="lblError" style="Z-INDEX: 114; LEFT: 224px; POSITION: absolute; TOP: 176px"
		runat="server" Width="312px" Height="56px" Visible="False" ForeColor="Red"></asp:Label></DIV>
