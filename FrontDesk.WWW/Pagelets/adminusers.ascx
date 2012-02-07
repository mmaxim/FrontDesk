<%@ Control Language="c#" AutoEventWireup="false" Codebehind="adminusers.ascx.cs" Inherits="FrontDesk.Pages.Admin.Pagelets.AdminUserPagelet" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="FrontDesk" Namespace="FrontDesk.Controls" Assembly="FrontDesk.WWW" %>
<meta name="vs_snapToGrid" content="True">
<meta name="vs_showGrid" content="True">
<h3>Batch System User Creation</h3>
<DIV style="WIDTH: 601px; POSITION: relative; HEIGHT: 90px" ms_positioning="GridLayout"><INPUT id="fiUserList" style="Z-INDEX: 101; LEFT: 123px; WIDTH: 301px; POSITION: absolute; TOP: 7px; HEIGHT: 22px"
		type="file" size="31" name="File1" runat="server">
	<DIV style="Z-INDEX: 102; LEFT: 9px; POSITION: absolute; TOP: 6px" ms_positioning="text2D">
		<DIV style="DISPLAY: inline; WIDTH: 106px; HEIGHT: 24px" ms_positioning="FlowLayout">XML 
			User List:
		</DIV>
	</DIV>
	<asp:Button id="cmdCreate" style="Z-INDEX: 103; LEFT: 16px; POSITION: absolute; TOP: 58px" runat="server"
		Text="Create Users" CausesValidation="False"></asp:Button>
	<asp:Label id="lblErrors" style="Z-INDEX: 104; LEFT: 216px; POSITION: absolute; TOP: 62px"
		runat="server" Width="369px" Height="17px" Font-Size="X-Small" ForeColor="Red" Visible="False"></asp:Label>
	<asp:CheckBox id="chkMerge" style="Z-INDEX: 105; LEFT: 8px; POSITION: absolute; TOP: 32px" runat="server"
		Text="Merge into existing user set" Width="270px"></asp:CheckBox>
</DIV>
<h3>System Users</h3>
<FrontDesk:FDDataGrid id="dgAllUsers" style="Z-INDEX: 101; LEFT: 3px; POSITION: relative; TOP: 0px" runat="server"
	Font-Size="0.7em" Width="100%" AllowPaging="True" CellPadding="3" AutoGenerateColumns="False" DataKeyField="UserName"
	DeleteMessage="Are you sure you wish to delete this item?">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Edit"></asp:EditCommandColumn>
		<asp:TemplateColumn HeaderText="Username">
			<ItemTemplate>
				<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UserName") %>'>
				</asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Email">
			<ItemTemplate>
				<asp:Label id=Label2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Email") %>'>
				</asp:Label>
			</ItemTemplate>
			<EditItemTemplate>
				<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Email") %>'>
				</asp:TextBox>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="First Name">
			<ItemTemplate>
				<asp:Label id=Label3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FirstName") %>'>
				</asp:Label>
			</ItemTemplate>
			<EditItemTemplate>
				<asp:TextBox id=TextBox2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FirstName") %>'>
				</asp:TextBox>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Last Name">
			<ItemTemplate>
				<asp:Label id=Label4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LastName") %>'>
				</asp:Label>
			</ItemTemplate>
			<EditItemTemplate>
				<asp:TextBox id=TextBox3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LastName") %>'>
				</asp:TextBox>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="LastLogin" ReadOnly="True" HeaderText="Last Login"></asp:BoundColumn>
		<asp:ButtonColumn Text="Reset Password" CommandName="respasswd"></asp:ButtonColumn>
	</Columns>
	<PagerStyle Mode="NumericPages"></PagerStyle>
</FrontDesk:FDDataGrid>
