<%@ Control Language="c#" AutoEventWireup="false" Codebehind="backups.ascx.cs" Inherits="FrontDesk.Controls.Matrix.BackupsView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Backups</font><br><br>
<I>Click on the name of the backup to download the archive</I><BR>
<BR>
<asp:datagrid id="dgBackups" style="Z-INDEX: 108" Width="100%" CellPadding="3" AutoGenerateColumns="False"
	DataKeyField="ID" runat="server">
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
