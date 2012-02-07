<%@ Control Language="c#" AutoEventWireup="false" Codebehind="defaultcourses.ascx.cs" Inherits="FrontDesk.Pages.Pagelets.DefaultCoursesPagelet" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Import Namespace="FrontDesk.Components" %>
<h3>My Course Memberships <FONT size="1"><I>Click the course name</I></FONT></h3>
<!-- CONTENT AREA END --><asp:datagrid id="dgUserCourses" runat="server" Width="100%" CellPadding="3" AutoGenerateColumns="False"
	DataKeyField="ID">
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="Course Name">
			<ItemTemplate>
				<asp:HyperLink runat="server" ID="hypStudent" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>' NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.ID", "../course.aspx?CourseID={0}") %>'>
				</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="Number" HeaderText="Course Number"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Course Role">
			<ItemTemplate>
				<asp:HyperLink runat="server" ID="hypStudentAdmin">[Student]</asp:HyperLink>
				<asp:HyperLink runat="server" ID="hypAdmin">[Staff]</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
<h3>Join a Course <FONT size="1"><I>Click the join link</I></FONT></h3>
<asp:label id="lblError" runat="server" Width="519px" Font-Size="8pt" Height="13px" ForeColor="Red"
	Visible="False">Error</asp:label><asp:datagrid id="dgAllCourses" style="Z-INDEX: 101; LEFT: 1px; POSITION: relative; TOP: 0px"
	runat="server" Width="100%" CellPadding="3" AutoGenerateColumns="False" DataKeyField="ID" PageSize="9" AllowPaging="True">
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn>
			<ItemStyle Width="10px"></ItemStyle>
			<ItemTemplate>
				<asp:LinkButton runat="server" Text="Join" CommandName="Join" CausesValidation="false" ID="lnkJoin"></asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Course Name">
			<ItemTemplate>
				<asp:Label runat="server" ID="lblName" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>'>
				</asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="Number" HeaderText="Course Number">
			<ItemStyle Width="130px"></ItemStyle>
		</asp:BoundColumn>
	</Columns>
</asp:datagrid><font size="1"><b>Click the arrows above to see more courses</font>.</B>
