<%@ Control Language="c#" AutoEventWireup="false" Codebehind="autojobtests.ascx.cs" Inherits="FrontDesk.Controls.Matrix.AutoJobTestsView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Automatic Job Details</font><br><br>
<i>A listing of all the status of the tests involved in the selected automatic job.</i><br>
<br>
<asp:datagrid id="dgTests" AllowPaging="True" Width="97%" runat="server" Font-Size="X-Small" CellPadding="3"
	AutoGenerateColumns="False">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn>
			<ItemStyle Width="10px"></ItemStyle>
			<ItemTemplate>
				<asp:Image runat="server" ID="imgStatus" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Submission">
			<ItemTemplate>
				<asp:Label runat="server" ID="lblSub"></asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Evaluation">
			<ItemTemplate>
				<asp:Label runat="server" ID="lblEval"></asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Queue #">
			<ItemTemplate>
				<asp:Label runat="server" ID="lblQueue"></asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
