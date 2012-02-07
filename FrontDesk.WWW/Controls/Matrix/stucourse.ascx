<%@ Control Language="c#" AutoEventWireup="false" Codebehind="stucourse.ascx.cs" Inherits="FrontDesk.Controls.Matrix.StudentCourseView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Import Namespace="FrontDesk.Components" %>
<%@ Import Namespace="System.Data" %>
<asp:Label id="lblNumber" runat="server" Font-Size="14pt">Number</asp:Label>&nbsp;
<asp:Label id="lblName" runat="server" Font-Size="12pt">Name</asp:Label>
<H3>Assignments</H3>
<DIV style="LEFT: 0px; VERTICAL-ALIGN: top; OVERFLOW: auto; WIDTH: 95%; POSITION: relative; HEIGHT: 100px"
	ms_positioning="GridLayout">
	<asp:datagrid id="dgAssignments" CellPadding="3" AutoGenerateColumns="False" DataKeyField="ID"
		Width="95%" runat="server" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px">
		<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
		<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
		<ItemStyle CssClass="new_table_item"></ItemStyle>
		<HeaderStyle CssClass="new_table_header"></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="Description" HeaderText="Description"></asp:BoundColumn>
			<asp:BoundColumn DataField="DueDate" HeaderText="Due Date"></asp:BoundColumn>
		</Columns>
	</asp:datagrid></DIV>
<br>
<h3>Announcements</h3>
<asp:datalist id="dlAnnouncements" style="LEFT: 20px; POSITION: relative" runat="server" Width="90%">
	<ItemTemplate>
		<b>
			<asp:Label ForeColor="#4768A3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Title") %>' /></b>
		- <font size="1"><i>Last Modified:
				<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Modified") %>' />
				by
				<asp:Label runat="server" id="lblPoster" /></i> </font>
		<br>
		<br>
		<asp:Label Runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>' />
		<hr style="POSITION: relative" width="100%" align="center">
	</ItemTemplate>
</asp:datalist>
<H3></H3>
