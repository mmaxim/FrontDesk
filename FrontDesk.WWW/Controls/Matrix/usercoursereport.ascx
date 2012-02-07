<%@ Control Language="c#" AutoEventWireup="false" Codebehind="usercoursereport.ascx.cs" Inherits="FrontDesk.Controls.Matrix.UserCourseReportView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<meta content="True" name="vs_snapToGrid">
<meta content="True" name="vs_showGrid">
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">User Profile</font><br>
<br>
<DIV style="WIDTH: 409px; POSITION: relative; HEIGHT: 120px" ms_positioning="GridLayout"><asp:textbox id="txtFirst" style="Z-INDEX: 101; LEFT: 16px; POSITION: absolute; TOP: 24px" runat="server"></asp:textbox><asp:textbox id="txtLast" style="Z-INDEX: 102; LEFT: 208px; POSITION: absolute; TOP: 24px" runat="server"
		Width="200px"></asp:textbox><asp:textbox id="txtEmail" style="Z-INDEX: 103; LEFT: 16px; POSITION: absolute; TOP: 64px" runat="server"
		Width="208px"></asp:textbox><asp:button id="cmdUpdate" style="Z-INDEX: 104; LEFT: 32px; POSITION: absolute; TOP: 96px" runat="server"
		Text="Update"></asp:button>
	<DIV style="DISPLAY: inline; Z-INDEX: 105; LEFT: 16px; WIDTH: 152px; POSITION: absolute; TOP: 8px; HEIGHT: 8px"
		ms_positioning="FlowLayout">First Name:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 106; LEFT: 208px; WIDTH: 160px; POSITION: absolute; TOP: 8px; HEIGHT: 16px"
		ms_positioning="FlowLayout">Last Name:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 107; LEFT: 16px; WIDTH: 70px; POSITION: absolute; TOP: 48px; HEIGHT: 15px"
		ms_positioning="FlowLayout">Email:</DIV>
	<asp:Label id="lblError" style="Z-INDEX: 108; LEFT: 104px; POSITION: absolute; TOP: 96px" runat="server"
		Width="288px" Font-Size="8pt" Visible="False" ForeColor="Red"></asp:Label>
	<asp:DropDownList id="ddlRoles" style="Z-INDEX: 109; LEFT: 248px; POSITION: absolute; TOP: 64px" runat="server"></asp:DropDownList>
	<DIV style="DISPLAY: inline; Z-INDEX: 110; LEFT: 248px; WIDTH: 70px; POSITION: absolute; TOP: 48px; HEIGHT: 15px"
		ms_positioning="FlowLayout">Role:</DIV>
</DIV>
<br>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Course Performance</font><br>
<br>
<b>Total Performance:</b>
<asp:label id="lblTotal" Runat="server"></asp:label><br>
<asp:datagrid id="dgReport" runat="server" Width="97%" CellPadding="3" AutoGenerateColumns="False"
	DataKeyField="ID" Font-Size="X-Small">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:BoundColumn DataField="Description" HeaderText="Assignment"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Points">
			<ItemStyle Width="150px" />
			<ItemTemplate>
				<asp:Label runat="server" ID="lblPoints"></asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
