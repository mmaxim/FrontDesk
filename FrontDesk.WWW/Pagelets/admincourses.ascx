<%@ Control Language="c#" AutoEventWireup="false" Codebehind="admincourses.ascx.cs" Inherits="FrontDesk.Pages.Admin.Pagelets.AdminCoursePagelet" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<meta name="vs_snapToGrid" content="True">
<meta name="vs_showGrid" content="True">
<h3>Create New Course</h3>
<DIV style="WIDTH: 660px; POSITION: relative; HEIGHT: 244px" ms_positioning="GridLayout">
	<asp:TextBox id="txtName" style="Z-INDEX: 101; LEFT: 136px; POSITION: absolute; TOP: 16px" runat="server"
		Width="216"></asp:TextBox>
	<asp:TextBox id="txtNumber" style="Z-INDEX: 102; LEFT: 136px; POSITION: absolute; TOP: 48px"
		runat="server" Width="216"></asp:TextBox>
	<asp:ListBox id="lstInstructor" style="Z-INDEX: 103; LEFT: 16px; POSITION: absolute; TOP: 112px"
		runat="server" Width="336px" Height="92"></asp:ListBox>
	<DIV style="Z-INDEX: 104; LEFT: 16px; POSITION: absolute; TOP: 24px" ms_positioning="text2D">
	</DIV>
	<DIV style="Z-INDEX: 105; LEFT: 16px; WIDTH: 336px; POSITION: absolute; TOP: 84px; HEIGHT: 19px"
		ms_positioning="FlowLayout">
		<DIV style="DISPLAY: inline; WIDTH: 328px; HEIGHT: 19px" ms_positioning="FlowLayout">Select 
			a Course Administrator:</DIV>
	</DIV>
	<asp:Button id="cmdCreate" style="Z-INDEX: 106; LEFT: 16px; POSITION: absolute; TOP: 210px"
		runat="server" Width="96px" Text="Create Course"></asp:Button>
	<DIV style="DISPLAY: inline; Z-INDEX: 107; LEFT: 16px; WIDTH: 93px; POSITION: absolute; TOP: 14px; HEIGHT: 21px"
		ms_positioning="FlowLayout">Course Name:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 108; LEFT: 16px; WIDTH: 112px; POSITION: absolute; TOP: 48px; HEIGHT: 16px"
		ms_positioning="FlowLayout">Course Number:</DIV>
	<asp:Label id="lblError" style="Z-INDEX: 109; LEFT: 413px; POSITION: absolute; TOP: 116px"
		Width="220px" runat="server" Height="97px" ForeColor="Red" Font-Size="8pt" Visible="False">Label</asp:Label>
</DIV>
<h3>Courses</h3>
<asp:Label id="lblBackups" runat="server" Width="646px" Visible="False" Font-Size="8pt" ForeColor="Red">Label</asp:Label>
<asp:datagrid id="dgCourseList" runat="server" DataKeyField="ID" AutoGenerateColumns="False" CellPadding="3"
	OnUpdateCommand="dgCourseList_Update" OnCancelCommand="dgCourseList_Cancel" OnEditCommand="dgCourseList_Edit"
	OnDeleteCommand="dgCourseList_Delete" Width="100%">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Edit">
			<ItemStyle Wrap="False"></ItemStyle>
		</asp:EditCommandColumn>
		<asp:TemplateColumn HeaderText="Course Name">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' ID="lblName" />
			</ItemTemplate>
			<EditItemTemplate>
				<asp:TextBox runat="server" id="txtCourseName" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'/>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Course Number">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Number") %>' ID="Label5" />
			</ItemTemplate>
			<EditItemTemplate>
				<asp:TextBox runat="server" id="txtCourseNumber" Text='<%# DataBinder.Eval(Container.DataItem, "Number") %>'/>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:ButtonColumn Text="Backup" CommandName="Backup"></asp:ButtonColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
				<asp:LinkButton runat="server" Text="Mark Unavailable" CommandName="Available" CausesValidation="false" ID="lnkAvail"></asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:ButtonColumn Text="Delete" CommandName="Delete"></asp:ButtonColumn>
	</Columns>
</asp:datagrid>
