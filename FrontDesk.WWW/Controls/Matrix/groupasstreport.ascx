<%@ Register TagPrefix="userpage" TagName="Rubric" Src="../rubricview.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="groupasstreport.ascx.cs" Inherits="FrontDesk.Controls.Matrix.GroupAsstReportView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Grade Report</font><br>
<br>
<asp:datagrid id="dgReport" AllowPaging="True" PageSize="8" runat="server" Width="95%" CellPadding="3"
	AutoGenerateColumns="False" Font-Size="X-Small" DataKeyField="PrincipalID" AllowCustomPaging="True">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn>
			<ItemStyle Width="10px"></ItemStyle>
			<ItemTemplate>
				<asp:LinkButton runat="server" id="lnkDetails" Text="Details" CommandName="Details" CausesValidation="false"></asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemStyle Width="10px"></ItemStyle>
			<ItemTemplate>
				<asp:Image Runat="server" ID="imgSub" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Student">
			<ItemTemplate>
				<asp:Label runat="server" ID="lblStudent" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Points">
			<ItemStyle Width="100px"></ItemStyle>
			<ItemTemplate>
				<asp:Label runat="server" ID="lblPoints"></asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
	<PagerStyle Mode="NumericPages"></PagerStyle>
</asp:datagrid><br>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Details
	<asp:Label id="lblDetailsName" runat="server"></asp:Label></font><br>
<br>
<userpage:rubric id="ucRubric" runat="server"></userpage:rubric>
<span runat="server" id="spnNothing"><i>Select a user by clicking <b>Details</b> in the 
		above grid to view student's assignment grade in detail.</i> </span>
