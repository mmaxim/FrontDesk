<%@ Control Language="c#" AutoEventWireup="false" Codebehind="autojobs.ascx.cs" Inherits="FrontDesk.Controls.Matrix.AutoJobsView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Automatic Job Status</font><br><br>
<i>Current jobs for this assignment.</i> <EM>Select the job from the Navigation 
	pane to get details on the jobs.</EM><br>
<br>
<asp:Label id="lblError" runat="server" Font-Size="8pt" Visible="False" ForeColor="Red"></asp:Label>
<asp:datagrid id="dgJobs" Width="97%" runat="server" Font-Size="X-Small" CellPadding="3" AutoGenerateColumns="False"
	DataKeyField="ID">
	<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
	<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
	<ItemStyle CssClass="new_table_item"></ItemStyle>
	<HeaderStyle CssClass="new_table_header"></HeaderStyle>
	<Columns>
		<asp:BoundColumn DataField="JobName" HeaderText="Job Name"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Progress (Submissions Completed/Requested)">
			<ItemTemplate>
				<asp:Label runat="server" ID="lblProgress" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:ButtonColumn Text="Cancel" CommandName="Delete" ItemStyle-Width="10px"></asp:ButtonColumn>
	</Columns>
</asp:datagrid>
