<%@ Control Language="c#" AutoEventWireup="false" Codebehind="compete.ascx.cs" Inherits="FrontDesk.Controls.Matrix.CompetitionView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Competitions</font><br>
<br>
<div runat="server" id="divMain">
	Select Competition:
	<asp:dropdownlist id="ddlCompete" runat="server" AutoPostBack="True"></asp:dropdownlist><br>
	<br>
	<asp:datagrid id="dgCompete" runat="server" PageSize="8" Width="95%" CellPadding="3" AutoGenerateColumns="False"
		Font-Size="X-Small">
		<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
		<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
		<ItemStyle CssClass="new_table_item"></ItemStyle>
		<HeaderStyle CssClass="new_table_header"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn>
				<ItemStyle Width="10px" />
				<ItemTemplate>
					<asp:Label runat="server" ID="lblIndex"></asp:Label>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Submission">
				<ItemTemplate>
					<asp:Label runat="server" ID="lblSub"></asp:Label>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="CompetitionScore" HeaderText="Score"></asp:BoundColumn>
		</Columns>
		<PagerStyle Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
</div>
<asp:Label id="lblNone" Font-Italic="True" runat="server" Visible="False">There are no competitions for this assignment.</asp:Label>
