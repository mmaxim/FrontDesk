<%@ Control Language="c#" AutoEventWireup="false" Codebehind="sections.ascx.cs" Inherits="FrontDesk.Controls.Matrix.SectionsView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Sections</font><br>
<br>
<IMG alt="" src="attributes/group.jpg" align="absMiddle"><asp:linkbutton id="lnkSecExpl" runat="server" Width="314px">Click here to launch the Section Explorer</asp:linkbutton>
<asp:Label id="lblEditError" runat="server" Width="336px" Height="7px" Visible="False" ForeColor="Red"
	Font-Size="8pt">Error</asp:Label>
<asp:DataGrid id="dgSections" AutoGenerateColumns="False" CellPadding="3" runat="server" Width="95%"
	DataKeyField="ID">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Edit"></asp:EditCommandColumn>
		<asp:TemplateColumn HeaderText="Name">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>'>
				</asp:Label>
			</ItemTemplate>
			<EditItemTemplate>
				<asp:TextBox runat="server" ID="txtName" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>'>
				</asp:TextBox>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Owner">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Owner") %>'>
				</asp:Label>
			</ItemTemplate>
			<EditItemTemplate>
				<asp:TextBox runat="server" id="txtOwner" Text='<%# DataBinder.Eval(Container, "DataItem.Owner") %>'>
				</asp:TextBox>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:ButtonColumn Text="Delete" CommandName="Delete"></asp:ButtonColumn>
	</Columns>
</asp:DataGrid><br>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Batch User Addition</font><br>
<br>
<DIV style="WIDTH: 458px; POSITION: relative; HEIGHT: 63px" ms_positioning="GridLayout"><INPUT id="fiUserList" style="Z-INDEX: 101; LEFT: 112px; WIDTH: 274px; POSITION: absolute; TOP: 0px; HEIGHT: 22px"
		type="file" size="26" name="File1" runat="server">
	<DIV style="Z-INDEX: 102; LEFT: 8px; POSITION: absolute; TOP: 0px" ms_positioning="text2D">
		<DIV style="DISPLAY: inline; WIDTH: 70px; HEIGHT: 15px" ms_positioning="FlowLayout">XML 
			User List:
		</DIV>
	</DIV>
	<asp:button id="cmdBatchSubmit" style="Z-INDEX: 103; LEFT: 32px; POSITION: absolute; TOP: 32px"
		runat="server" Text="Submit" CausesValidation="False"></asp:button><asp:checkbox id="chkMerge" style="Z-INDEX: 105; LEFT: 108px; POSITION: absolute; TOP: 41px" runat="server"
		Text="Merge XML data with current user data" Width="284px" Height="22px"></asp:checkbox></DIV>
<asp:label id="lblBatchError" runat="server" Width="359px" Height="9px" Visible="False" ForeColor="Red"
	Font-Size="8pt"></asp:label><br>

<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">All Users</font><br>
<br>
<asp:Label id="lblAddError" runat="server" Font-Size="8pt" ForeColor="Red" Visible="False"></asp:Label>
<asp:datagrid id="dgAllUsers" style="Z-INDEX: 101; LEFT: 3px; POSITION: relative; TOP: 0px" runat="server"
	Font-Size="0.7em" Width="95%" PageSize="8" AllowPaging="True" CellPadding="3" AutoGenerateColumns="False"
	DataKeyField="UserName">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:ButtonColumn Text="Add" CommandName="Add"></asp:ButtonColumn>
		<asp:TemplateColumn HeaderText="Username">
			<ItemTemplate>
				<asp:Label runat="server" ID="Label1" Text='<%# DataBinder.Eval(Container, "DataItem.UserName") %>' />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="Email" HeaderText="Email"></asp:BoundColumn>
		<asp:BoundColumn DataField="FirstName" HeaderText="First Name"></asp:BoundColumn>
		<asp:BoundColumn DataField="LastName" HeaderText="Last Name"></asp:BoundColumn>
	</Columns>
	<PagerStyle Mode="NumericPages"></PagerStyle>
</asp:datagrid>
