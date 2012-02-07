<%@ Control Language="c#" AutoEventWireup="false" Codebehind="asstview.ascx.cs" Inherits="FrontDesk.Controls.Matrix.AssignmentView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<meta content="True" name="vs_snapToGrid">
<DIV style="WIDTH: 551px; POSITION: relative; HEIGHT: 466px" ms_positioning="GridLayout"><asp:textbox id="txtAsstName" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 48px"
		Width="303px" runat="server"></asp:textbox>
	<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; FONT-SIZE: 12pt; Z-INDEX: 102; LEFT: 0px; WIDTH: 304px; POSITION: absolute; TOP: 0px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Assignment Details</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 103; LEFT: 8px; WIDTH: 70px; POSITION: absolute; TOP: 32px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Name:</DIV>
	<asp:textbox id="txtAsstDueDate" style="Z-INDEX: 104; LEFT: 8px; POSITION: absolute; TOP: 96px"
		Width="302px" runat="server"></asp:textbox>
	<DIV style="DISPLAY: inline; Z-INDEX: 105; LEFT: 8px; WIDTH: 112px; POSITION: absolute; TOP: 80px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Due Date:</DIV>
	<asp:button id="cmdAsstUpdate" style="Z-INDEX: 106; LEFT: 251px; POSITION: absolute; TOP: 196px"
		runat="server" Text="Update"></asp:button><asp:label id="lblAsstError" style="Z-INDEX: 107; LEFT: 14px; POSITION: absolute; TOP: 176px"
		Width="216px" runat="server" Height="24px" ForeColor="Red" Font-Size="8pt" Visible="False"></asp:label><asp:label id="lblCourseID" style="Z-INDEX: 109; LEFT: 216px; POSITION: absolute; TOP: 25px"
		runat="server" Visible="False"></asp:label><asp:label id="lblContentID" style="Z-INDEX: 108; LEFT: 216px; POSITION: absolute; TOP: 25px"
		runat="server" Visible="False"></asp:label><asp:checkbox id="chkAvailable" style="Z-INDEX: 110; LEFT: 8px; POSITION: absolute; TOP: 128px"
		runat="server" Text="Available for Student Viewing"></asp:checkbox><asp:checkbox id="chkEvaluation" style="Z-INDEX: 111; LEFT: 8px; POSITION: absolute; TOP: 151px"
		Width="248px" runat="server" Text="Available for Student Submission"></asp:checkbox>
	<DIV style="Z-INDEX: 112; LEFT: 332px; WIDTH: 219px; POSITION: absolute; TOP: 49px; HEIGHT: 143px"
		ms_positioning="FlowLayout">
		<P><EM><FONT size="2">Selecting "Available&nbsp;for Student Viewing"&nbsp;allows students 
					to view assignment data&nbsp;only, but not neccesarily submit to this 
					assignment</FONT></EM></P>
		<P><EM><FONT size="2">Selecting "Available for Student Submission" allows students to 
					submit solutions for the assignment.</FONT></EM><BR>
		</P>
	</DIV>
	<iewc:toolbar id="tbActions" style="Z-INDEX: 113; LEFT: 12px; POSITION: absolute; TOP: 252px"
		Width="303px" runat="server" Height="16px" Font-Size="6pt">
		<iewc:ToolbarButton Text="Folder" ImageUrl="attributes/folder.gif" ID="cmdTBCreateFolder" ToolTip="Create new sub folder"></iewc:ToolbarButton>
		<iewc:ToolbarButton Text="File" ImageUrl="attributes/book.gif" ID="cmdTBCreateFile" ToolTip="Create new sub file"></iewc:ToolbarButton>
		<iewc:ToolbarButton Text="Delete" ImageUrl="attributes/filebrowser/delete.gif" ID="cmdTBDelete" ToolTip="Delete element"></iewc:ToolbarButton>
		<iewc:ToolbarButton Text="Save" ImageUrl="attributes/good.gif" ID="cmdTBSave" ToolTip="Save format"></iewc:ToolbarButton>
	</iewc:toolbar><asp:button id="cmdFormatUpdate" style="Z-INDEX: 114; LEFT: 37px; POSITION: absolute; TOP: 434px"
		Width="66px" runat="server" Text="Update"></asp:button>
	<DIV style="DISPLAY: inline; Z-INDEX: 115; LEFT: 18px; WIDTH: 53px; POSITION: absolute; TOP: 405px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Name:</DIV>
	<DIV style="Z-INDEX: 116; LEFT: 335px; WIDTH: 215px; POSITION: absolute; TOP: 253px; HEIGHT: 76px"
		ms_positioning="FlowLayout">
		<P>
		<P><EM><FONT size="2"></FONT></EM></P>
		<EM><FONT size="2">Remember to press <b>Save</b> when you are done</FONT></EM>
		<P></P>
		<P><BR>
		</P>
	</DIV>
	<asp:textbox id="txtFileName" style="Z-INDEX: 117; LEFT: 72px; POSITION: absolute; TOP: 403px"
		Width="226px" runat="server"></asp:textbox>
	<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; FONT-SIZE: 12pt; Z-INDEX: 118; LEFT: 4px; WIDTH: 304px; POSITION: absolute; TOP: 228px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Submission Format</DIV>
	<DIV style="Z-INDEX: 119; LEFT: 14px; OVERFLOW: auto; WIDTH: 308px; POSITION: absolute; TOP: 291px; HEIGHT: 99px"
		ms_positioning="GridLayout"><iewc:treeview id="tvFormat" style="Z-INDEX: 123; LEFT: 0px; POSITION: absolute; TOP: 0px" runat="server"
			BorderStyle="None" AutoPostBack="True"></iewc:treeview></DIV>
	<asp:label id="lblFormatError" style="Z-INDEX: 120; LEFT: 115px; POSITION: absolute; TOP: 436px"
		Width="404px" runat="server" ForeColor="Red" Font-Size="8pt" Visible="False"></asp:label></DIV>
