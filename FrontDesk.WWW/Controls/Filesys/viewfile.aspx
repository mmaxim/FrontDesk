<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page language="c#" validateRequest="false" Codebehind="viewfile.aspx.cs" AutoEventWireup="false" Inherits="FrontDesk.Controls.Filesys.ViewFilePage" %>
<%@ Register TagPrefix="userpage" TagName="Rubric" Src="../rubricview.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FrontDesk File Viewer</title>
		<meta content="True" name="vs_snapToGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:button id="cmdEdit" runat="server" Width="56px" Text="Edit" style="Z-INDEX: 104; LEFT: 16px; POSITION: absolute; TOP: 8px"></asp:button>&nbsp;
			<asp:button id="cmdSave" runat="server" Width="56px" Text="Save" Enabled="False" style="Z-INDEX: 110; LEFT: 80px; POSITION: absolute; TOP: 8px"></asp:button>&nbsp;<asp:label id="lblError" runat="server" Width="192px" Font-Size="8pt" ForeColor="Red" Visible="False"
				Height="24px" style="Z-INDEX: 108; LEFT: 216px; POSITION: absolute; TOP: 8px"></asp:label>
			<asp:hyperlink id="hypClose" runat="server" Font-Bold="True" NavigateUrl="javascript:window.close();"
				Font-Size="8pt" style="Z-INDEX: 102; LEFT: 720px; POSITION: absolute; TOP: 8px">Close</asp:hyperlink>
			<asp:button id="cmdUnlock" runat="server" Width="62px" Text="Unlock" Visible="False" style="Z-INDEX: 105; LEFT: 648px; POSITION: absolute; TOP: 8px"></asp:button><asp:label id="lblLockInfo" runat="server" Width="200px" Font-Size="8pt" ForeColor="Red" Visible="False"
				Height="24px" style="Z-INDEX: 106; LEFT: 440px; POSITION: absolute; TOP: 8px">Label</asp:label><IMG id="imgLock" alt="" src="../../attributes/filebrowser/lock.gif" runat="server" style="Z-INDEX: 107; LEFT: 416px; POSITION: absolute; TOP: 8px">
			<asp:button id="cmdCancel" runat="server" Width="64px" Text="Cancel" Enabled="False" style="Z-INDEX: 109; LEFT: 144px; POSITION: absolute; TOP: 8px"></asp:button>
			<br>
			<div style="WIDTH:92.8%"><iewc:tabstrip id="tsFiles" runat="server" TargetID="mpFiles" TabDefaultStyle="background-color:#ffffff;font-family:verdana;font-size:8pt;color:#000000;width:79;height:21;text-align:center;"
				TabHoverStyle="background-color:#777777" TabSelectedStyle="background-color:#4768a3;color:#ffffff;"
				BorderStyle="Solid" BorderWidth="1px" style="Z-INDEX: 111; LEFT: 16px; WIDTH: 100%; POSITION: absolute; TOP: 40px"
				BorderColor="Black">
				<iewc:Tab Text="Tab 1"></iewc:Tab>
				<iewc:Tab Text="Tab 2"></iewc:Tab>
				<iewc:Tab Text="Tab 3"></iewc:Tab>
			</iewc:tabstrip></div>
			<iewc:multipage id="mpFiles" runat="server" Height="496px" BorderStyle="Solid" BorderWidth="1px"
				style="Z-INDEX: 103; LEFT: 16px; POSITION: absolute; TOP: 64px" Width="712px">
				<iewc:PageView>
					<asp:PlaceHolder id="phFileViewer0" runat="server"></asp:PlaceHolder>
				</iewc:PageView>
				<iewc:PageView>
					<asp:PlaceHolder id="phFileViewer1" runat="server"></asp:PlaceHolder>
				</iewc:PageView>
				<iewc:PageView>
					<asp:PlaceHolder id="phFileViewer2" runat="server"></asp:PlaceHolder>
				</iewc:PageView>
				<iewc:PageView>
					<asp:PlaceHolder id="phFileViewer3" runat="server"></asp:PlaceHolder>
				</iewc:PageView>
				<iewc:PageView>
					<asp:PlaceHolder id="phFileViewer4" runat="server"></asp:PlaceHolder>
				</iewc:PageView>
				<iewc:PageView>
					<asp:PlaceHolder id="phFileViewer5" runat="server"></asp:PlaceHolder>
				</iewc:PageView>
			</iewc:multipage>
			<DIV id="divGrade" style="Z-INDEX: 101; LEFT: 16px; WIDTH: 752px; POSITION: absolute; TOP: 568px; HEIGHT: 187px"
				runat="server" ms_positioning="GridLayout"><userpage:rubric id="ucRubric" runat="server"></userpage:rubric><asp:dropdownlist id="ddlComments" style="Z-INDEX: 101; LEFT: 408px; POSITION: absolute; TOP: 16px"
					runat="server" Width="336px"></asp:dropdownlist><asp:textbox id="txtLines" style="Z-INDEX: 102; LEFT: 408px; POSITION: absolute; TOP: 136px"
					runat="server" Width="336px"></asp:textbox><asp:textbox id="txtCustom" style="Z-INDEX: 103; LEFT: 408px; POSITION: absolute; TOP: 56px"
					runat="server" Width="232px" Height="56px" TextMode="MultiLine"></asp:textbox>
				<DIV style="DISPLAY: inline; Z-INDEX: 104; LEFT: 408px; WIDTH: 280px; POSITION: absolute; TOP: 120px; HEIGHT: 19px"
					ms_positioning="FlowLayout">Lines Affected (ex. 3,4-19,22)</DIV>
				<DIV style="DISPLAY: inline; Z-INDEX: 105; LEFT: 408px; WIDTH: 112px; POSITION: absolute; TOP: 0px; HEIGHT: 19px"
					ms_positioning="FlowLayout">Comment</DIV>
				<DIV style="DISPLAY: inline; Z-INDEX: 106; LEFT: 408px; WIDTH: 200px; POSITION: absolute; TOP: 40px; HEIGHT: 19px"
					ms_positioning="FlowLayout">Custom Comment</DIV>
				<asp:button id="cmdCreate" style="Z-INDEX: 107; LEFT: 416px; POSITION: absolute; TOP: 160px"
					runat="server" Text="Create" Enabled="False"></asp:button><asp:dropdownlist id="ddlType" style="Z-INDEX: 108; LEFT: 648px; POSITION: absolute; TOP: 56px" runat="server"
					Width="88px"></asp:dropdownlist>
				<DIV style="DISPLAY: inline; Z-INDEX: 109; LEFT: 648px; WIDTH: 86px; POSITION: absolute; TOP: 40px; HEIGHT: 19px"
					ms_positioning="FlowLayout">Custom Type</DIV>
				<asp:textbox id="txtPoints" style="Z-INDEX: 110; LEFT: 648px; POSITION: absolute; TOP: 96px"
					runat="server" Width="86px"></asp:textbox>
				<DIV style="DISPLAY: inline; Z-INDEX: 111; LEFT: 648px; WIDTH: 96px; POSITION: absolute; TOP: 80px; HEIGHT: 16px"
					ms_positioning="FlowLayout">Custom Points</DIV>
				<asp:label id="lblCommentError" style="Z-INDEX: 112; LEFT: 480px; POSITION: absolute; TOP: 160px"
					runat="server" Width="256px" Font-Size="8pt" ForeColor="Red" Visible="False"></asp:label></DIV>
		</form>
	</body>
</HTML>
