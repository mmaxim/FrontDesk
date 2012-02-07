<%@ Control Language="c#" AutoEventWireup="false" Codebehind="adminbackups.ascx.cs" Inherits="FrontDesk.Pages.Admin.Pagelets.AdminBackups" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h2>Backups</h2>
<i>Click on the name of the backup to download the archive</i><br>
<br>
<asp:datagrid id="dgBackups" style="Z-INDEX: 108" runat="server" DataKeyField="ID" AutoGenerateColumns="False"
	CellPadding="3" Width="100%">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="Name">
			<ItemTemplate>
				<asp:HyperLink ID="hypName" runat="server" Text="BackedUp" NavigateUrl=""></asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="Creator" HeaderText="Creator"></asp:BoundColumn>
		<asp:BoundColumn DataField="Creation" HeaderText="Creation"></asp:BoundColumn>
	</Columns>
</asp:datagrid>
