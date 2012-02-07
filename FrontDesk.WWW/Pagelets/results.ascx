<%@ Register TagPrefix="userpage" TagName="Rubric" Src="../Controls/rubricview.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="results.ascx.cs" Inherits="FrontDesk.Pages.Pagelets.StudentResultsPagelet" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h3>Assignment Results</h3>
<i>Select desired submission and browse the rubric below to receive detailed 
	results of your submission. </i>
<br>
<br>
<asp:Label id="lblSubs" runat="server">Submissions:</asp:Label><br>
<asp:dropdownlist id="ddlSubs" runat="server" AutoPostBack="True"></asp:dropdownlist><br>
<asp:Label id="lblNoSubs" runat="server" ForeColor="#4768A3" Font-Italic="True" Font-Size="12pt"
	Visible="False">You have no submissions for this assignment.<br><br><br></asp:Label>
<DIV id="divSubj" style="WIDTH: 100%; POSITION: relative; HEIGHT: 250px" runat="server"
	ms_positioning="GridLayout"><asp:datagrid id="dgSubj" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 27px" runat="server"
		Width="100%" CellPadding="3" AutoGenerateColumns="False" DataKeyField="ID">
		<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
		<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
		<ItemStyle CssClass="new_table_item"></ItemStyle>
		<HeaderStyle CssClass="new_table_header"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Type">
				<ItemStyle Width="80px"></ItemStyle>
				<ItemTemplate>
					<asp:Image Runat="server" ID="imgType" ImageAlign="AbsMiddle" />
					<asp:Label Runat="server" ID="lblType" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="File">
				<ItemStyle Width="80px"></ItemStyle>
				<ItemTemplate>
					<asp:LinkButton runat="server" ID="lnkFile" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Points">
				<ItemStyle Width="40px"></ItemStyle>
				<ItemTemplate>
					<asp:Label runat="server" id="lblPoints" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Remark">
				<ItemTemplate>
					<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Comment") %>'>
					</asp:Label>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="Grader" HeaderText="Grader">
				<ItemStyle Width="80px"></ItemStyle>
			</asp:BoundColumn>
		</Columns>
	</asp:datagrid>
	<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; FONT-SIZE: 12pt; Z-INDEX: 102; LEFT: 2px; WIDTH: 323px; POSITION: absolute; TOP: 4px; HEIGHT: 23px"
		ms_positioning="FlowLayout">
		<P>Grader Comments</P>
	</DIV>
</DIV>
<DIV id="divAuto" style="OVERFLOW: auto; WIDTH: 100%; POSITION: relative; HEIGHT: 257px"
	runat="server" ms_positioning="GridLayout"></DIV>
