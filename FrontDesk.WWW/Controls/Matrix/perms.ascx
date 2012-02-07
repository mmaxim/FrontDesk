<%@ Control Language="c#" AutoEventWireup="false" Codebehind="perms.ascx.cs" Inherits="FrontDesk.Controls.Matrix.PermissionsView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Users/Roles</font><br>
<br>
<asp:datagrid id="dgRoles" Width="97%" CellPadding="3" AutoGenerateColumns="False" DataKeyField="PrincipalID"
	runat="server" Font-Size="X-Small" AllowPaging="True" PageSize="5">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn>
			<ItemStyle Width="10px" />
			<ItemTemplate>
				<asp:LinkButton runat="server" id="lnkPermissions" Text="Permissions" CommandName="Permissions"
					CausesValidation="false"></asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Type">
			<ItemStyle Width="10px"></ItemStyle>
			<ItemTemplate>
				<asp:Label runat="server" ID="lblType"></asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Name">
			<ItemTemplate>
				<asp:Label runat="server" ID="lblName"></asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
	<PagerStyle Mode="NumericPages"></PagerStyle>
</asp:datagrid><br>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Permissions </font>&nbsp;&nbsp;<asp:label id="lblPrin" runat="server" Font-Bold="True"></asp:label><br>
<br>
<DIV style="VERTICAL-ALIGN: top; OVERFLOW: auto; WIDTH: 97.14%; POSITION: relative; HEIGHT: 196px"
	ms_positioning="GridLayout"><asp:datagrid id="dgPerms" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px" Width="97%"
		CellPadding="3" AutoGenerateColumns="False" DataKeyField="Name" runat="server" Font-Size="X-Small" PageSize="5">
		<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
		<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
		<ItemStyle CssClass="new_table_item"></ItemStyle>
		<HeaderStyle CssClass="new_table_header"></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="Description" HeaderText="Permission"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Grant">
				<ItemStyle Width="10px"></ItemStyle>
				<ItemTemplate>
					<asp:CheckBox Runat="server" ID="chkGrant" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid></DIV>
<br>
<asp:button id="cmdApply" runat="server" Text="Apply"></asp:button>&nbsp; 
&nbsp;
<asp:Label id="lblError" runat="server" Font-Size="8pt" ForeColor="Red" Visible="False"></asp:Label>
