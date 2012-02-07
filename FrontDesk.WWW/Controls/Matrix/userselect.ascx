<%@ Control Language="c#" AutoEventWireup="false" Codebehind="userselect.ascx.cs" Inherits="FrontDesk.Controls.Matrix.UserSelectView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<h3>Feedback User Selection</h3>
<P><i>Select users and sections from the list below in order to enter the feedback 
		system. The users selected will be the working set for the evaluation process.</i>
</P>
<iewc:multipage id="mpViews" runat="server">
	<iewc:PageView>
		<IMG alt="" src="attributes/group.gif" align="absMiddle">
		<asp:linkbutton id="lnkSecExpl" runat="server">Click here to launch the Section Explorer</asp:linkbutton>
		<DIV style="WIDTH: 452px; POSITION: relative; HEIGHT: 360px" ms_positioning="GridLayout">
			<asp:datagrid id="dgUsers" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px" runat="server"
				AllowPaging="True" Width="100%" DataKeyField="ID" AutoGenerateColumns="False" CellPadding="3">
				<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
				<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
				<ItemStyle CssClass="new_table_item"></ItemStyle>
				<HeaderStyle CssClass="new_table_header"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn>
						<ItemStyle Width="10px"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox Runat="server" ID="chkSelect" />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Status">
						<ItemStyle Width="10px"></ItemStyle>
						<ItemTemplate>
							<asp:Image Runat="server" ID="imgStatus" />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Progress">
						<ItemStyle Width="10px"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblProgress" />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Name">
						<ItemTemplate>
							<asp:Image Runat="server" ID="imgType" ImageAlign="AbsMiddle" />
							<asp:Label runat="server" ID="lblName"></asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False">
						<ItemTemplate>
							<asp:Label runat="server" ID="lblType"></asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblID"></asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
			<asp:button id="cmdEvaluate" style="Z-INDEX: 102; LEFT: 1px; POSITION: absolute; TOP: 350px"
				runat="server" Text="Start Evaluation"></asp:button>
			<asp:label id="lblEvaluate" style="Z-INDEX: 103; LEFT: 154px; POSITION: absolute; TOP: 345px"
				runat="server" Font-Size="8pt" ForeColor="Red">In order to perform evaluation on this assignment, it must first be locked for evaluation</asp:label></DIV>
	</iewc:PageView>
	<iewc:PageView><i>Evaluation is in progress. To start again with a different user set, click the 
	button below.</i><br>
<br>
<asp:Button Runat="server" ID="cmdReset" Text="Start Over" /></iewc:PageView>
</iewc:multipage>

