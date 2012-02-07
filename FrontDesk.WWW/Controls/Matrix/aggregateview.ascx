<%@ Control Language="c#" AutoEventWireup="false" Codebehind="aggregateview.ascx.cs" Inherits="FrontDesk.Controls.Matrix.AggregateView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Aggregate View</font><br>
<br>
<asp:image id="Image1" runat="server" ImageUrl="../../attributes/filebrowser/folder.gif" ImageAlign="AbsMiddle"></asp:image>
<asp:LinkButton id="lnkLoadAll" runat="server">Load All Submissions into File Browser</asp:LinkButton>
<asp:datagrid id="dgAggregate" DataKeyField="UserName" AutoGenerateColumns="False" CellPadding="3"
	Width="95%" Font-Size="X-Small" runat="server" PageSize="1">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn>
			<ItemStyle Width="10px"></ItemStyle>
			<ItemTemplate>
				<asp:Image runat="server" id="imgStatus" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="Username" HeaderText="Username">
			<ItemStyle Width="10px"></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="Total" HeaderText="Total">
			<ItemStyle Width="10px"></ItemStyle>
		</asp:BoundColumn>
	</Columns>
	<PagerStyle Mode="NumericPages"></PagerStyle>
</asp:datagrid>
