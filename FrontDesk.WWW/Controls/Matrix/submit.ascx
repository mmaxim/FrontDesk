<%@ Control Language="c#" AutoEventWireup="false" Codebehind="submit.ascx.cs" Inherits="FrontDesk.Controls.Matrix.SubmissionSystemView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="FrontDesk" Namespace="FrontDesk.Controls" Assembly="FrontDesk.WWW" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="userpage" TagName="ArchiveSubmit" Src="../Submission/archivesubsys.ascx" %>
<%@ Register TagPrefix="userpage" TagName="CVSSubmit" Src="../Submission/cvssubsys.ascx" %>
<meta content="False" name="vs_snapToGrid">
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Submit</font><br>
<br>
<EM>Select your submission method and hit submit. Wait for the submission to 
	finish, do not hit submit twice!</EM>
<DIV style="WIDTH: 540px; POSITION: relative; HEIGHT: 125px" ms_positioning="GridLayout"><asp:listbox id="lstSubSys" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 32px" AutoPostBack="True"
		runat="server" Width="256px" Height="65px"></asp:listbox>
	<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; Z-INDEX: 102; LEFT: 8px; WIDTH: 206px; POSITION: absolute; TOP: 8px; HEIGHT: 20px"
		ms_positioning="FlowLayout">Select Submission Method:</DIV>
	<HR style="Z-INDEX: 104; LEFT: 16px; WIDTH: 81.08%; POSITION: absolute; TOP: 96px; HEIGHT: 1px"
		width="81.08%" SIZE="1">
	<asp:listbox id="lstPrincipal" style="Z-INDEX: 105; LEFT: 280px; POSITION: absolute; TOP: 32px"
		AutoPostBack="True" runat="server" Width="144px" Height="61px" DataValueField="PrincipalID"
		DataTextField="Name"></asp:listbox>
	<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; Z-INDEX: 106; LEFT: 288px; WIDTH: 104px; POSITION: absolute; TOP: 8px; HEIGHT: 20px"
		ms_positioning="FlowLayout">Submit For:</DIV>
	<DIV style="DISPLAY: inline; FONT-SIZE: 8pt; Z-INDEX: 107; LEFT: 8px; WIDTH: 90px; POSITION: absolute; TOP: 107px; HEIGHT: 16px"
		ms_positioning="FlowLayout">Submitting As:
	</DIV>
	<asp:label id="lblSubName" style="Z-INDEX: 108; LEFT: 118px; POSITION: absolute; TOP: 106px"
		runat="server" Width="106px" Font-Size="8pt">Label</asp:label><asp:image id="imgType" style="Z-INDEX: 109; LEFT: 94px; POSITION: absolute; TOP: 105px" runat="server"
		Width="20px" Height="20px"></asp:image></DIV>
<iewc:multipage id="mpViews" runat="server">
	<iewc:PageView>
		<userpage:ArchiveSubmit runat="server" ID="ucArchiveSub" />
	</iewc:PageView>
	<iewc:PageView>
		<userpage:CVSSubmit runat="server" ID="ucCVSSub" />
	</iewc:PageView>
</iewc:multipage>
<asp:Label id="lblSuccess" runat="server" Font-Size="8pt" Visible="False" ForeColor="Red"></asp:Label>
<br>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">My Submissions</font><br>
<br>
<i><b>WARNING:</b> Modifying files already submitted will result in a change in the 
	submission time for the submission modified.</i>
<br>
<asp:image id="Image1" runat="server" ImageUrl="../../attributes/filebrowser/folder.gif" ImageAlign="AbsMiddle"></asp:image>&nbsp;<asp:linkbutton id="lnkFiles" runat="server">Click here to manage submission files</asp:linkbutton>
<DIV style="WIDTH: 540px; POSITION: relative; HEIGHT: 215px" ms_positioning="GridLayout">
	<asp:datagrid id="dgSubmissions" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px"
		runat="server" Width="539px" CellPadding="3" AutoGenerateColumns="False" DataKeyField="PrincipalID">
		<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
		<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
		<ItemStyle CssClass="new_table_item"></ItemStyle>
		<HeaderStyle CssClass="new_table_header"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Submitter">
				<ItemTemplate>
					<asp:image id="imgSubber" runat="server" ImageAlign="AbsMiddle" />
					<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Submitter") %>' />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="Creation" HeaderText="Submission Date"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Due Date">
				<ItemTemplate>
					<asp:Label runat="server" ID="lblDueDate" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<ItemTemplate>
					<asp:Label runat="server" ID="lblLate" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid></DIV>
