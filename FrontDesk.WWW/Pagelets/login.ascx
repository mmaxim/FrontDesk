<%@ Control Language="c#" AutoEventWireup="false" Codebehind="login.ascx.cs" Inherits="FrontDesk.Pages.Pagelets.LoginPagelet" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<meta name="vs_snapToGrid" content="True">
<h2>Login</h2>
<EM>Please enter your username and password above to enter the system. Click the 
	register tab above if you do not have an account. If you have any problems 
	please contact the administrator.</EM><br>
<br>
<DIV style="WIDTH: 688px; POSITION: relative; HEIGHT: 227px" ms_positioning="GridLayout"><asp:Button id="cmdLogin" style="Z-INDEX: 101; LEFT: 34px; POSITION: absolute; TOP: 58px" Height="27px"
		Width="73px" runat="server" Text="Login" tabIndex="3"></asp:Button>
	<asp:TextBox id="txtPassword" style="Z-INDEX: 102; LEFT: 88px; POSITION: absolute; TOP: 31px"
		runat="server" TextMode="Password" tabIndex="2"></asp:TextBox>
	<asp:Label id="lblPassword" style="Z-INDEX: 103; LEFT: 17px; POSITION: absolute; TOP: 33px"
		Height="24px" Width="56px" runat="server">Password:</asp:Label>
	<asp:Label id="lblUserName" style="Z-INDEX: 104; LEFT: 15px; POSITION: absolute; TOP: 2px"
		Height="16px" Width="40px" runat="server">Username:</asp:Label>
	<asp:TextBox id="txtUsername" style="Z-INDEX: 105; LEFT: 88px; POSITION: absolute; TOP: 2px"
		runat="server" tabIndex="1"></asp:TextBox>
	<asp:Label id="lblError" style="Z-INDEX: 106; LEFT: 24px; POSITION: absolute; TOP: 144px" runat="server"
		Width="475px" Height="56px" Visible="False" ForeColor="Red" tabIndex="2"></asp:Label>
	<asp:LinkButton id="lnkForgot" style="Z-INDEX: 107; LEFT: 40px; POSITION: absolute; TOP: 104px"
		runat="server" Width="496px" Font-Size="8pt">Forgot password? Enter username and click here to get it emailed to you.</asp:LinkButton>
	<asp:TextBox id="txtVerifyKey" style="Z-INDEX: 108; LEFT: 388px; POSITION: absolute; TOP: 2px"
		runat="server" Visible="False"></asp:TextBox>
	<asp:Label id="lblVerifyKey" style="Z-INDEX: 109; LEFT: 311px; POSITION: absolute; TOP: 2px"
		runat="server" Visible="False">Verify Key:</asp:Label>
</DIV>
