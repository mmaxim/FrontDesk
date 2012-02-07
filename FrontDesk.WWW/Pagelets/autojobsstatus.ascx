<%@ Control Language="c#" AutoEventWireup="false" Codebehind="autojobsstatus.ascx.cs" Inherits="FrontDesk.Pages.Pagelets.AutoJobStatusPagelet" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h2>Jobs Status</h2>
<h3>Active Jobs</h3>
<asp:datagrid id="dgActive" Width="100%" runat="server" Font-Size="X-Small" CellPadding="3" AutoGenerateColumns="False"
	DataKeyField="ID">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:ButtonColumn Text="Detailed Status" CommandName="Details"></asp:ButtonColumn>
		<asp:BoundColumn DataField="JobName" HeaderText="Name"></asp:BoundColumn>
		<asp:BoundColumn DataField="JobCreator" HeaderText="Creator"></asp:BoundColumn>
		<asp:BoundColumn DataField="TesterIP" HeaderText="Tester IP"></asp:BoundColumn>
		<asp:BoundColumn DataField="TesterDescription" HeaderText="Tester Description"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Evaluation">
			<ItemTemplate>
				<asp:Label runat="server" id="lblEvaluation" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Course">
			<ItemTemplate>
				<asp:Label runat="server" id="lblCourse" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:ButtonColumn Text="Cancel" CommandName="Cancel"></asp:ButtonColumn>
	</Columns>
</asp:datagrid>
<h3>Queued Jobs</h3>
<asp:datagrid id="dgQueued" Width="100%" runat="server" Font-Size="X-Small" CellPadding="3" AutoGenerateColumns="False"
	DataKeyField="ID">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:ButtonColumn Text="Cancel" CommandName="Cancel"></asp:ButtonColumn>
		<asp:BoundColumn DataField="JobName" HeaderText="Name"></asp:BoundColumn>
		<asp:BoundColumn DataField="JobCreator" HeaderText="Creator"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Evaluation">
			<ItemTemplate>
				<asp:Label runat="server" id="lblEvaluation" />
				</asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Course">
			<ItemTemplate>
				<asp:Label runat="server" ID="lblCourse" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="Priority" HeaderText="Priority"></asp:BoundColumn>
	</Columns>
</asp:datagrid>
<h3>Detailed Status</h3>
<asp:Label id="lblDetails" runat="server" Width="382px"></asp:Label><br>
<asp:TextBox id="txtDetails" runat="server" Width="616px" TextMode="MultiLine" Height="266px"></asp:TextBox>
