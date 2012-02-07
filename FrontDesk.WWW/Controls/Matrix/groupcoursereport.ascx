<%@ Register TagPrefix="cc1" Namespace="CustomButton" Assembly="ClickOnceButton" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="groupcoursereport.ascx.cs" Inherits="FrontDesk.Controls.Matrix.GroupCourseReport" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Grade Export</font><br>
<br>
Format:
<asp:dropdownlist id="ddlExportFormat" Runat="server"></asp:dropdownlist>&nbsp;&nbsp;
<cc1:clickoncebutton id="cmdExport" runat="server" Text="Export" DisabledText="Exporting..." DisableAfterClick="True"></cc1:clickoncebutton>&nbsp;<asp:label id="lblExportError" Font-Size="8pt" runat="server" ForeColor="Red" Visible="False"></asp:label>
<asp:linkbutton id="lnkExport" runat="server" Visible="False">Export Successful. Click here to download.</asp:linkbutton><br>
<br>
<i><b>(Optional) </b>Upload a list of users to export (default is all users)</i><br>
<INPUT id="ufUserList" style="WIDTH: 405px; HEIGHT: 22px" type="file" size="48" name="File1"
	runat="server"><br>
<br>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Grade Report</font><br>
<br>
<asp:datagrid id="dgCourse" AllowPaging="True" Font-Size="X-Small" DataKeyField="UserName" AutoGenerateColumns="False"
	CellPadding="3" Width="95%" runat="server">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:ButtonColumn Text="Details" CommandName="Details"></asp:ButtonColumn>
		<asp:TemplateColumn HeaderText="Student">
			<ItemTemplate>
				<asp:Label runat="server" ID="lblStudent"></asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Total Points">
			<ItemTemplate>
				<asp:Label runat="server" ID="lblPoints"></asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
	<PagerStyle Mode="NumericPages"></PagerStyle>
</asp:datagrid><br>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Student Details</font><br>
<br>
<asp:datagrid id="dgReport" Font-Size="X-Small" DataKeyField="ID" AutoGenerateColumns="False"
	CellPadding="3" Width="95%" runat="server">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:BoundColumn DataField="Description" HeaderText="Assignment"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Points">
			<ItemStyle Width="100px"></ItemStyle>
			<ItemTemplate>
				<asp:Label runat="server" ID="lblPoints"></asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
