<%@ Register TagPrefix="userpage" TagName="Rubric" Src="../rubricview.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="subjfeed.ascx.cs" Inherits="FrontDesk.Controls.Matrix.SubjFeedbackView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div style="WIDTH: 98.5%; HEIGHT: 23px"><iewc:tabstrip id="tsUsers" AutoPostBack="True" BorderWidth="1px" BorderStyle="Solid" BorderColor="Black"
		TabDefaultStyle="background-color:#000000;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:79;height:21;text-align:center"
		TabHoverStyle="background-color:#777777" TabSelectedStyle="background-color:#ffffff;color:#000000" runat="server">
		<iewc:Tab Text="Grading"></iewc:Tab>
		<iewc:Tab Text="Activity Log"></iewc:Tab>
	</iewc:tabstrip></div>
<div id="divGrading" style="BORDER-RIGHT: 1px solid; PADDING-RIGHT: 15px; BORDER-TOP: 1px solid; PADDING-LEFT: 15px; PADDING-BOTTOM: 15px; OVERFLOW: auto; BORDER-LEFT: 1px solid; WIDTH: 93%; PADDING-TOP: 15px; BORDER-BOTTOM: 1px solid; HEIGHT: 430px"
	runat="server"><asp:label id="lblSubTime" runat="server"></asp:label><br>
	<asp:image id="Image1" runat="server" ImageUrl="../../attributes/filebrowser/folder.gif" ImageAlign="AbsMiddle"></asp:image><asp:linkbutton id="lnkFiles" runat="server" Font-Bold="True">Click here to view submission files</asp:linkbutton>&nbsp;<FONT color="#000000" size="1"><I>Give 
			in-code feedback by viewing submision files</I><br>
	</FONT>
	<asp:image id="imgComplete" runat="server" ImageUrl="../../attributes/subgrade.gif" ImageAlign="AbsMiddle"></asp:image><asp:linkbutton id="lnkComplete" runat="server">Mark as Completed</asp:linkbutton>&nbsp;&nbsp;&nbsp;
	<asp:label id="lblMarkInst" runat="server" Font-Size="8pt" Font-Italic="True">Controls ability of students to view their grade</asp:label><br>
	<asp:image id="imgDefunct" runat="server" ImageUrl="../../attributes/skull.gif" ImageAlign="AbsMiddle"></asp:image><asp:linkbutton id="lnkDefunct" runat="server">Mark as Defunct</asp:linkbutton>&nbsp;&nbsp;&nbsp;<i style="font-size: 8pt">Removes 
		submission from consideration without deleting it</i><br>
	<userpage:rubric id="ucRubric" runat="server"></userpage:rubric><br>
	<iewc:multipage id="mpViews" runat="server">
		<iewc:PageView><font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Grading Comments</font><br>
<br>
<IMG align="absMiddle" alt="" src="../../attributes/filebrowser/reload.gif" id="imgRefresh"
				runat="server">
<asp:LinkButton id="lnkRefresh" runat="server">Refresh</asp:LinkButton><br>

<asp:linkbutton id="lnkCreate" runat="server">Insert Selected Comment</asp:linkbutton>&nbsp; 
&nbsp;
<asp:dropdownlist id="ddlCanned" runat="server"></asp:dropdownlist>
<asp:datagrid id="dgResults" runat="server" PageSize="5" AllowPaging="True" DataKeyField="ID"
				AutoGenerateColumns="False" CellPadding="3" Width="98%">
				<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
				<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
				<ItemStyle CssClass="new_table_item"></ItemStyle>
				<HeaderStyle CssClass="new_table_header"></HeaderStyle>
				<Columns>
					<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Edit">
						<ItemStyle Width="10px"></ItemStyle>
					</asp:EditCommandColumn>
					<asp:TemplateColumn HeaderText="Type">
						<ItemStyle Width="80px"></ItemStyle>
						<ItemTemplate>
							<asp:Image Runat="server" ID="imgType" ImageAlign="AbsMiddle" />
							<asp:Label Runat="server" ID="lblType" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="ddlTypes" />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="File">
						<ItemTemplate>
							<asp:LinkButton ID="lnkFile" Runat="server" />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Points">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Points") %>' ID="Label1">
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox Width="30px" id="txtPoints" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Points") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Comment">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Comment") %>' ID="Label2">
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox TextMode=MultiLine Width="170px" Height="50px" id="txtGridComment" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Comment") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn ReadOnly="True" DataField="Grader" HeaderText="Grader"></asp:BoundColumn>
					<asp:ButtonColumn Text="Delete" CommandName="Delete">
						<ItemStyle Width="10px"></ItemStyle>
					</asp:ButtonColumn>
				</Columns>
			</asp:datagrid>
<asp:Label id="lblError" style="Z-INDEX: 104; LEFT: 174px; POSITION: absolute; TOP: 8px" runat="server"
				Width="481px" ForeColor="Red" Visible="False" Font-Size="8pt"></asp:Label>
<asp:Label id="lblRubID" runat="server" Visible="False"></asp:Label></iewc:PageView>
		<iewc:PageView>
			<br>
			<br>
			<i>Fill out comments for each category in the grading schema. The points will 
				update in the grading schema view above.</i></iewc:PageView>
		<iewc:PageView><br>
<br>
<i>This automatic test has not been run on the selected submission. Click <b>Run Test</b> below 
				to start a new grading job for this submission and the selected test.</i><br>
<asp:Label id="lblEvalID" Visible="False" runat="server"></asp:Label>
<br>
<IMG alt="" src="attributes/cyl.gif" align="absMiddle">
<asp:LinkButton id="lnkRunTest" runat="server">Run Test</asp:LinkButton>&nbsp;
<asp:Label id="lblRunError" Width="203px" runat="server" Visible="False" ForeColor="Red" Font-Size="8pt"></asp:Label></iewc:PageView>
		<iewc:PageView>
			<DIV id="divAuto" style="WIDTH: 444px; HEIGHT: 312px" runat="server" ms_positioning="GridLayout"></DIV>
		</iewc:PageView>
		<iewc:PageView></iewc:PageView>
		<iewc:PageView>
			<br>
			<i>This entry of the rubric has not been graded by any staff members. If you feel 
				this is a mistake, contact the course staff.</i></iewc:PageView>
	</iewc:multipage>
	<P></P>
</div>
<div id="divActivity" style="BORDER-RIGHT: 1px solid; PADDING-RIGHT: 15px; BORDER-TOP: 1px solid; PADDING-LEFT: 15px; PADDING-BOTTOM: 15px; OVERFLOW: auto; BORDER-LEFT: 1px solid; WIDTH: 93%; PADDING-TOP: 15px; BORDER-BOTTOM: 1px solid; HEIGHT: 430px"
	runat="server"><font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Activity Log</font><br>
	<br>
	<asp:datagrid id="dgActivity" runat="server" Width="98%" CellPadding="3" AutoGenerateColumns="False"
		Font-Size="X-Small">
		<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
		<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
		<ItemStyle CssClass="new_table_item"></ItemStyle>
		<HeaderStyle CssClass="new_table_header"></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="Username" HeaderText="User" ItemStyle-Width="10px"></asp:BoundColumn>
			<asp:BoundColumn DataField="Time" HeaderText="Time"></asp:BoundColumn>
			<asp:BoundColumn DataField="Description" HeaderText="Description"></asp:BoundColumn>
		</Columns>
	</asp:datagrid></div>
