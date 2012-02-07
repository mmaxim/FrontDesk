<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="rubricdetails.ascx.cs" Inherits="FrontDesk.Controls.Matrix.RubricDetailsView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="userpage" TagName="EvalDeps" Src="../evaldeps.ascx" %>
<meta content="False" name="vs_snapToGrid">
<DIV id="divRubDetails" style="WIDTH: 457px; POSITION: relative; HEIGHT: 200px" ms_positioning="GridLayout"
	runat="server">
	<DIV style="DISPLAY: inline; Z-INDEX: 101; LEFT: 0px; WIDTH: 96px; POSITION: absolute; TOP: 24px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Name:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 102; LEFT: 184px; WIDTH: 72px; POSITION: absolute; TOP: 24px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Points:</DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 103; LEFT: 0px; WIDTH: 104px; POSITION: absolute; TOP: 80px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Description:</DIV>
	<ASP:TEXTBOX id="txtDescription" style="Z-INDEX: 104; LEFT: 0px; POSITION: absolute; TOP: 96px"
		runat="server" Height="54px" TextMode="MultiLine" Width="360px"></ASP:TEXTBOX><ASP:BUTTON id="cmdSave" style="Z-INDEX: 106; LEFT: 1px; POSITION: absolute; TOP: 160px" runat="server"
		Width="56px" Text="Update"></ASP:BUTTON>
	<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; FONT-SIZE: 12pt; Z-INDEX: 105; LEFT: 0px; WIDTH: 192px; POSITION: absolute; TOP: 0px; HEIGHT: 24px"
		ms_positioning="FlowLayout">Rubric Entry Details
	</DIV>
	<asp:textbox id="txtName" style="Z-INDEX: 107; LEFT: 0px; POSITION: absolute; TOP: 40px" runat="server"
		Width="161px"></asp:textbox><asp:textbox id="txtPoints" style="Z-INDEX: 108; LEFT: 184px; POSITION: absolute; TOP: 40px"
		runat="server" Width="128px"></asp:textbox><asp:label id="lblError" style="Z-INDEX: 109; LEFT: 88px; POSITION: absolute; TOP: 160px" runat="server"
		Height="24px" Width="256px" Font-Size="8pt" ForeColor="Red" Visible="False"></asp:label>
	<asp:CheckBox id="chkAllowNeg" style="Z-INDEX: 110; LEFT: 180px; POSITION: absolute; TOP: 66px"
		runat="server" Text="Allow negative result point total?"></asp:CheckBox></DIV>
<iewc:multipage id="mpViews" runat="server">
	<iewc:PageView>
		<DIV id="Div1" style="WIDTH: 95%; POSITION: relative; HEIGHT: 260px" runat="server" ms_positioning="GridLayout">
			<asp:linkbutton id="lnkCanCreate" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 24px"
				runat="server">Create Suggested Remark</asp:linkbutton>
			<asp:datagrid id="dgCanned" style="Z-INDEX: 102; LEFT: 0px; POSITION: relative; TOP: 48px" Width="100%"
				runat="server" DataKeyField="ID" AutoGenerateColumns="False" CellPadding="3">
				<EditItemStyle CssClass="table_edit_item"></EditItemStyle>
				<AlternatingItemStyle CssClass="new_alt_table_item"></AlternatingItemStyle>
				<ItemStyle CssClass="new_table_item"></ItemStyle>
				<HeaderStyle CssClass="new_table_header"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn>
						<ItemStyle Width="70px"></ItemStyle>
						<ItemTemplate>
							<asp:LinkButton runat="server" Text="Edit" CommandName="Edit" CausesValidation="false" ID="Linkbutton1"></asp:LinkButton>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:LinkButton runat="server" Text="Update" CommandName="Update" ID="Linkbutton2"></asp:LinkButton>&nbsp;
							<asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false" ID="Linkbutton3"></asp:LinkButton>
						</EditItemTemplate>
					</asp:TemplateColumn>
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
					<asp:TemplateColumn HeaderText="Points">
						<ItemStyle Width="40px"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Points") %>' ID="Label1">
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox Width="40px" id="txtGridPoints" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Points") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Comment">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Comment") %>' ID="Label2">
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox Width="200px" id="txtGridComment" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Comment") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:ButtonColumn Text="Delete" CommandName="Delete"></asp:ButtonColumn>
				</Columns>
			</asp:datagrid>
			<asp:label id="lblCanError" style="Z-INDEX: 103; LEFT: 176px; POSITION: absolute; TOP: 24px"
				Width="264px" runat="server" Visible="False" ForeColor="Red" Font-Size="8pt"></asp:label>
			<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; FONT-SIZE: 12pt; Z-INDEX: 109; LEFT: 0px; WIDTH: 280px; POSITION: absolute; TOP: 0px; HEIGHT: 24px"
				ms_positioning="FlowLayout">
				Suggested Grading Remarks
			</DIV>
		</DIV>
	</iewc:PageView>
	<iewc:PageView>
		<DIV id="Div2" style="WIDTH: 552px; POSITION: relative; HEIGHT: 277px" runat="server"
			ms_positioning="GridLayout">
			<asp:checkbox id="chkPretest" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 184px"
				runat="server" Text="Run on submission?"></asp:checkbox>
			<asp:checkbox id="chkBuild" style="Z-INDEX: 102; LEFT: 0px; POSITION: absolute; TOP: 160px" Width="123px"
				runat="server" Text="Is build test?"></asp:checkbox>
			<asp:textbox id="txtTimeLimit" style="Z-INDEX: 103; LEFT: 0px; POSITION: absolute; TOP: 112px"
				Width="64px" runat="server"></asp:textbox>
			<DIV style="DISPLAY: inline; Z-INDEX: 104; LEFT: 0px; WIDTH: 112px; POSITION: absolute; TOP: 96px; HEIGHT: 19px"
				ms_positioning="FlowLayout">Time Limit (s):</DIV>
			<asp:dropdownlist id="ddlRunTool" style="Z-INDEX: 105; LEFT: 0px; POSITION: absolute; TOP: 64px" runat="server"></asp:dropdownlist>
			<DIV style="DISPLAY: inline; Z-INDEX: 106; LEFT: 0px; WIDTH: 96px; POSITION: absolute; TOP: 48px; HEIGHT: 19px"
				ms_positioning="FlowLayout">Run Tool:</DIV>
			<asp:textbox id="txtRunArguments" style="Z-INDEX: 107; LEFT: 304px; POSITION: absolute; TOP: 64px"
				Width="232px" runat="server"></asp:textbox>
			<DIV style="DISPLAY: inline; Z-INDEX: 108; LEFT: 304px; WIDTH: 248px; POSITION: absolute; TOP: 48px; HEIGHT: 19px"
				ms_positioning="FlowLayout">Run Arguments:
			</DIV>
			<asp:button id="cmdUpdate" style="Z-INDEX: 109; LEFT: 0px; POSITION: absolute; TOP: 240px" runat="server"
				Text="Update"></asp:button>
			<asp:checkbox id="chkCompete" style="Z-INDEX: 110; LEFT: 0px; POSITION: absolute; TOP: 208px"
				runat="server" Text="Has competitive score?"></asp:checkbox><INPUT id="fuTester" style="Z-INDEX: 111; LEFT: 120px; WIDTH: 424px; POSITION: absolute; TOP: 112px; HEIGHT: 22px"
				type="file" size="51" name="File1" runat="server">
			<DIV style="DISPLAY: inline; Z-INDEX: 112; LEFT: 120px; WIDTH: 320px; POSITION: absolute; TOP: 96px; HEIGHT: 19px"
				ms_positioning="FlowLayout">
				Upload Tester Files&nbsp;(ZIP only):
			</DIV>
			<asp:label id="lblAutoError" style="Z-INDEX: 113; LEFT: 72px; POSITION: absolute; TOP: 248px"
				Width="353px" runat="server" Height="18px" Visible="False" ForeColor="Red" Font-Size="8pt"></asp:label>
			<DIV style="DISPLAY: inline; Z-INDEX: 114; LEFT: 0px; WIDTH: 70px; POSITION: absolute; TOP: 144px; HEIGHT: 15px"
				ms_positioning="FlowLayout">Options:</DIV>
			<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; FONT-SIZE: 12pt; Z-INDEX: 115; LEFT: 0px; WIDTH: 290px; POSITION: absolute; TOP: 0px; HEIGHT: 24px"
				ms_positioning="FlowLayout">
				Automatic Objective Test
			</DIV>
			<asp:image id="Image1" style="Z-INDEX: 116; LEFT: 0px; POSITION: absolute; TOP: 24px" runat="server"
				ImageAlign="AbsMiddle" ImageUrl="../../attributes/filebrowser/folder.gif"></asp:image>
			<asp:linkbutton id="lnkFiles" style="Z-INDEX: 117; LEFT: 24px; POSITION: absolute; TOP: 24px" runat="server">Click here to manage evaluation files</asp:linkbutton>
			<asp:label id="lblEvalID" style="Z-INDEX: 118; LEFT: 296px; POSITION: absolute; TOP: 16px"
				runat="server" Visible="False"></asp:label>
			<asp:DropDownList id="ddlVersioning" style="Z-INDEX: 119; LEFT: 88px; POSITION: absolute; TOP: 64px"
				runat="server"></asp:DropDownList>
			<DIV style="DISPLAY: inline; Z-INDEX: 120; LEFT: 88px; WIDTH: 70px; POSITION: absolute; TOP: 48px; HEIGHT: 15px"
				ms_positioning="FlowLayout">Versioning:</DIV>
			<asp:TextBox id="txtVersion" style="Z-INDEX: 121; LEFT: 216px; POSITION: absolute; TOP: 64px"
				runat="server" Width="56px"></asp:TextBox>
			<DIV style="DISPLAY: inline; Z-INDEX: 122; LEFT: 216px; WIDTH: 70px; POSITION: absolute; TOP: 48px; HEIGHT: 15px"
				ms_positioning="FlowLayout">Version:</DIV>
			<DIV style="Z-INDEX: 123; LEFT: 234px; OVERFLOW: auto; WIDTH: 317px; POSITION: absolute; TOP: 144px; HEIGHT: 136px"
				ms_positioning="GridLayout">
				<userpage:EvalDeps runat="server" id="ucEvalDeps" /></DIV>
		</DIV>
	</iewc:PageView>
	<iewc:PageView><DIV style="WIDTH: 572px; POSITION: relative; HEIGHT: 290px" ms_positioning="GridLayout">
	<asp:linkbutton id="lnkJUnitView" style="Z-INDEX: 101; LEFT: 25px; POSITION: absolute; TOP: 30px"
		runat="server">Click here to manage evaluation files</asp:linkbutton>
	<asp:image id="Image2" style="Z-INDEX: 102; LEFT: 5px; POSITION: absolute; TOP: 30px" runat="server"
		ImageUrl="../../attributes/filebrowser/folder.gif" ImageAlign="AbsMiddle"></asp:image><INPUT id="fiJUnit" style="Z-INDEX: 103; LEFT: 2px; WIDTH: 344px; POSITION: absolute; TOP: 70px; HEIGHT: 22px"
		type="file" size="38" name="File1" runat="server">
	<asp:label id="lblJUnitTime" style="Z-INDEX: 104; LEFT: 0px; POSITION: absolute; TOP: 128px"
		runat="server" Height="13px" Font-Size="8pt"></asp:label>
	<DIV id="divJUnitTest" style="Z-INDEX: 105; LEFT: 0px; OVERFLOW: auto; WIDTH: 262px; POSITION: absolute; TOP: 160px; HEIGHT: 128px"
		ms_positioning="GridLayout" runat="server"></DIV>
	<asp:button id="cmdJUnitUpload" style="Z-INDEX: 106; LEFT: 25px; POSITION: absolute; TOP: 98px"
		runat="server" Text="Upload"></asp:button>
	<DIV style="DISPLAY: inline; Z-INDEX: 107; LEFT: 6px; WIDTH: 560px; POSITION: absolute; TOP: 53px; HEIGHT: 16px"
		ms_positioning="FlowLayout">Upload JUnit Test Suite <font size="1">(<b>warning</b> Uploading 
			a new suite will destroy any earlier results)</font>:</DIV>
	<asp:label id="lblJUnitError" style="Z-INDEX: 108; LEFT: 96px; POSITION: absolute; TOP: 93px"
		runat="server" Height="30px" Width="216px" Font-Size="8pt" ForeColor="Red" Visible="False"></asp:label>
	<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; FONT-SIZE: 12pt; Z-INDEX: 109; LEFT: 0px; WIDTH: 295px; POSITION: absolute; TOP: 0px; HEIGHT: 60px"
		ms_positioning="FlowLayout">JUnit Automatic Objective Test</DIV>
	<asp:label id="lblJUnitCount" style="Z-INDEX: 110; LEFT: 0px; POSITION: absolute; TOP: 144px"
		runat="server" Height="13px" Font-Size="8pt"></asp:label>
	<DIV style="Z-INDEX: 111; LEFT: 282px; OVERFLOW: auto; WIDTH: 286px; POSITION: absolute; TOP: 160px; HEIGHT: 128px"
		ms_positioning="GridLayout">
		<userpage:evaldeps id="ucJUnitDeps" runat="server"></userpage:evaldeps></DIV>
	<asp:CheckBox id="chkJUnitPreTest" style="Z-INDEX: 112; LEFT: 285px; POSITION: absolute; TOP: 113px"
		runat="server" Text="Run on Submission?"></asp:CheckBox>
	<asp:Button id="cmdJUnitUpdate" style="Z-INDEX: 113; LEFT: 442px; POSITION: absolute; TOP: 116px"
		runat="server" Text="Update"></asp:Button>
	<asp:CheckBox id="chkJUnitCompete" style="Z-INDEX: 114; LEFT: 285px; POSITION: absolute; TOP: 133px"
		runat="server" Text="Competitive?"></asp:CheckBox>
</DIV></iewc:PageView>
	<iewc:PageView>
		<DIV style="WIDTH: 532px; POSITION: relative; HEIGHT: 282px" ms_positioning="GridLayout"><INPUT style="Z-INDEX: 101; LEFT: 3px; WIDTH: 511px; POSITION: absolute; TOP: 71px; HEIGHT: 22px"
				type="file" size="66" id="fuCS" name="File1" runat="server">
			<asp:linkbutton id="lnkCSEval" style="Z-INDEX: 102; LEFT: 24px; POSITION: absolute; TOP: 24px" runat="server">Click here to manage evaluation files</asp:linkbutton>
			<asp:image id="Image3" style="Z-INDEX: 103; LEFT: 4px; POSITION: absolute; TOP: 24px" runat="server"
				ImageUrl="../../attributes/filebrowser/folder.gif" ImageAlign="AbsMiddle"></asp:image>
			<DIV style="DISPLAY: inline; FONT-WEIGHT: bold; FONT-SIZE: 12pt; Z-INDEX: 107; LEFT: 0px; WIDTH: 456px; POSITION: absolute; TOP: 0px; HEIGHT: 21px"
				ms_positioning="FlowLayout">CheckStyle Automatic Subjective Test</DIV>
			<DIV style="DISPLAY: inline; Z-INDEX: 104; LEFT: 3px; WIDTH: 526px; POSITION: absolute; TOP: 50px; HEIGHT: 19px"
				ms_positioning="FlowLayout">
				Upload a checks file and any custom check class files in a ZIP archive</DIV>
			<asp:Button id="cmdCSUpload" style="Z-INDEX: 105; LEFT: 25px; POSITION: absolute; TOP: 102px"
				runat="server" Text="Upload"></asp:Button>
			<DIV style="Z-INDEX: 106; LEFT: 1px; WIDTH: 209px; POSITION: absolute; TOP: 141px; HEIGHT: 141px"
				ms_positioning="GridLayout">
				<userpage:evaldeps id="ucCSDeps" runat="server"></userpage:evaldeps></DIV>
			<asp:Label id="lblCSError" style="Z-INDEX: 108; LEFT: 98px; POSITION: absolute; TOP: 105px"
				runat="server" Width="399px" Visible="False" ForeColor="Red" Font-Size="8pt"></asp:Label>
		</DIV>
	</iewc:PageView>
	<iewc:PageView></iewc:PageView>
</iewc:multipage>

