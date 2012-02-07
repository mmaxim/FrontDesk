<%@ Control Language="c#" AutoEventWireup="false" Codebehind="codeviewer.ascx.cs" Inherits="FrontDesk.Controls.Filesys.CodeFileViewer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<meta content="True" name="vs_snapToGrid">
<DIV id="divCode" style="BORDER-RIGHT: thin solid; BORDER-TOP: thin solid; OVERFLOW: auto; BORDER-LEFT: thin solid; WIDTH: 100%; BORDER-BOTTOM: thin solid; POSITION: relative; HEIGHT: 100%"
	runat="server" ms_positioning="GridLayout"><asp:datagrid id="dgCodeView" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 24px" runat="server"
		EnableViewState="False" BorderStyle="None" Font-Names="Courier New" GridLines="None" ShowHeader="False" AutoGenerateColumns="False" CellPadding="0"
		Font-Size="8pt" Width="100%">
		<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
		<ItemStyle CssClass="new_table_item"></ItemStyle>
		<HeaderStyle CssClass="new_table_header"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn>
				<ItemStyle Width="0px" BackColor="LightGray"></ItemStyle>
				<ItemTemplate>
					<asp:Label runat="server" id="lblNum" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<ItemTemplate>
					<asp:Label runat="server" ID="lblCode" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid>
	<asp:checkbox id="chkComments" style="Z-INDEX: 102; LEFT: 0px; POSITION: absolute; TOP: 0px" runat="server"
		Font-Names="Verdana" Font-Size="8pt" AutoPostBack="True" Checked="True" Text="Show Grader Comments"></asp:checkbox><asp:checkbox id="chkLines" style="Z-INDEX: 103; LEFT: 168px; POSITION: absolute; TOP: 0px" runat="server"
		Font-Names="Verdana" Font-Size="8pt" AutoPostBack="True" Checked="True" Text="Show Line Numbers"></asp:checkbox>
	<HR style="Z-INDEX: 104; LEFT: 0px; POSITION: relative; TOP: 15px" width="100%" SIZE="1">
</DIV>
<asp:textbox id="txtData" runat="server" EnableViewState="False" Width="750px" Visible="False"
	TextMode="MultiLine" Height="530px"></asp:textbox>
