<%@ Import Namespace="FrontDesk.Components" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="groups.ascx.cs" Inherits="FrontDesk.Controls.Matrix.GroupView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Create a Submission Group</font><br><br>
<DIV style="WIDTH: 95.9%; POSITION: relative; HEIGHT: 261px" ms_positioning="GridLayout">
	<asp:textbox id="txtName" style="Z-INDEX: 101; LEFT: 172px; POSITION: absolute; TOP: 6px" runat="server"
		Width="227px"></asp:textbox>
	<asp:label id="Label1" style="Z-INDEX: 102; LEFT: 10px; POSITION: absolute; TOP: 6px" runat="server"
		Width="152px" EnableViewState="False">Pick a group name:</asp:label>
	<asp:button id="cmdCreate" style="Z-INDEX: 103; LEFT: 7px; POSITION: absolute; TOP: 235px" runat="server"
		Text="Create Submission Group"></asp:button>
	<asp:label id="lblInvite" style="Z-INDEX: 104; LEFT: 12px; POSITION: absolute; TOP: 36px" runat="server"
		Width="372px" Font-Size="Small" EnableViewState="False" Height="22px">Select other students to invite to this submission group</asp:label><asp:label id="lblError" style="Z-INDEX: 105; LEFT: 437px; POSITION: absolute; TOP: 9px" runat="server"
		Width="271px" Height="37px" Font-Size="X-Small" ForeColor="Red" Visible="False">Errors</asp:label>
	<DIV style="Z-INDEX: 107; LEFT: 0px; VERTICAL-ALIGN: top; OVERFLOW: auto; WIDTH: 95.99%; POSITION: absolute; TOP: 62px; HEIGHT: 168px"
		ms_positioning="GridLayout">
		<asp:datagrid id="dgUsers" style="Z-INDEX: 108;  LEFT: 0px;  POSITION: absolute;  TOP: 0px" runat="server"
			Width="95%" Font-Size="0.7em" DataKeyField="UserName" AutoGenerateColumns="False" CellPadding="3">
			<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
			<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
			<ItemStyle CssClass="new_table_item"></ItemStyle>
			<HeaderStyle CssClass="new_table_header"></HeaderStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="Invite">
					<ItemTemplate>
						<asp:CheckBox Runat="server" Text="Invite" ID="Invite" />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Username">
					<ItemTemplate>
						<asp:Label runat="server" ID="UserName" Text='<%# DataBinder.Eval(Container, "DataItem.UserName") %>' />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn DataField="FullName" HeaderText="Name"></asp:BoundColumn>
				<asp:BoundColumn DataField="Email" HeaderText="Email"></asp:BoundColumn>
			</Columns>
		</asp:datagrid></DIV>
</DIV><br>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">My Submission Groups</font><br><br>
<asp:datagrid id="dgMemberships" runat="server" Width="95%" AutoGenerateColumns="False" CellPadding="3"
	Font-Size="X-Small" DataKeyField="principalID">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:BoundColumn DataField="Name" HeaderText="Group Name"></asp:BoundColumn>
		<asp:BoundColumn DataField="Creator" HeaderText="Group Creator"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Members">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# GetGroupMembers((FrontDesk.Components.Group)Container.DataItem) %>' />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:ButtonColumn Text="Leave" HeaderText="Leave" CommandName="Select"></asp:ButtonColumn>
	</Columns>
</asp:datagrid><br>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Invitations to Submission Groups</font><br><br>
<asp:Label id="lblJoinError" Width="444px" runat="server" Font-Size="8pt" Visible="False" ForeColor="Red">Label</asp:Label>
<asp:datagrid id="dgInvitations" runat="server" Width="95%" AutoGenerateColumns="False" CellPadding="3"
	Font-Size="X-Small" DataKeyField="ID">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:BoundColumn DataField="Name" HeaderText="Group Name"></asp:BoundColumn>
		<asp:BoundColumn DataField="invitor" HeaderText="Invitor"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Members">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# GetGroupMembers(((FrontDesk.Components.Invitation)Container.DataItem).Group) %>'>
				</asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:ButtonColumn Text="Join" HeaderText="Join" CommandName="Join"></asp:ButtonColumn>
		<asp:ButtonColumn Text="Decline" HeaderText="Decline" CommandName="Leave"></asp:ButtonColumn>
		<asp:TemplateColumn Visible="False" HeaderText="PrincipalID">
			<ItemTemplate>
				<asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PrincipalID") %>'>
				</asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
