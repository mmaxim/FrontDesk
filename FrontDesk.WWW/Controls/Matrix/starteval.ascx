<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="starteval.ascx.cs" Inherits="FrontDesk.Controls.Matrix.StartEvaluationView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<meta content="True" name="vs_snapToGrid">
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Feedback System</font><br>
<br>
<iewc:multipage id="mpViews" runat="server" style="WIDTH: 99%" >
	<iewc:PageView><P><i>Select users and sections from the&nbsp;interface below in order to enter the 
		feedback system. The users selected will be the working set for the evaluation 
		process.</i>
</P>
<iewc:TabStrip id="tsUsers" runat="server" TabSelectedStyle="background-color:#ffffff;color:#000000"
	TabHoverStyle="background-color:#777777" TabDefaultStyle="background-color:#000000;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:79;height:21;text-align:center"
	AutoPostBack="True" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
	<iewc:Tab Text="Sections"></iewc:Tab>
	<iewc:Tab Text="Search"></iewc:Tab>
	<iewc:Tab Text="All Users"></iewc:Tab>
</iewc:TabStrip>
<div runat="server" style="BORDER-RIGHT: 1px solid; PADDING-RIGHT: 15px; PADDING-LEFT: 15px; PADDING-BOTTOM: 15px; BORDER-LEFT: 1px solid; PADDING-TOP: 15px; BORDER-BOTTOM: 1px solid; HEIGHT: 350px"
	id="divSections">
	<IMG alt="" src="attributes/group.gif" align="absMiddle">
	<asp:linkbutton id="lnkSecExpl" runat="server">Click here to launch the Section Explorer</asp:linkbutton>&nbsp;
	<br>
	<asp:datagrid id="dgSections" runat="server" AllowPaging="True" Width="100%" AutoGenerateColumns="False"
		CellPadding="3">
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
			<asp:TemplateColumn>
				<ItemStyle Width="10px"></ItemStyle>
				<ItemTemplate>
					<asp:Image Runat="server" ID="imgStatus" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Graded">
				<ItemStyle></ItemStyle>
				<ItemTemplate>
					<asp:LinkButton runat="server" id="lnkGraded" CommandName="Graded" CausesValidation="false" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="UnGraded">
				<ItemStyle></ItemStyle>
				<ItemTemplate>
					<asp:LinkButton runat="server" id="lnkUnGraded" CommandName="UnGraded" CausesValidation="false" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Students">
				<ItemStyle></ItemStyle>
				<ItemTemplate>
					<asp:Label runat="server" ID="lblStudents" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Submissions">
				<ItemStyle></ItemStyle>
				<ItemTemplate>
					<asp:Label runat="server" ID="lblSubmissions" />
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
			<asp:TemplateColumn Visible="False" HeaderText="Group/UserID">
				<ItemTemplate>
					<asp:Label runat="server" ID="lblID"></asp:Label>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
		<PagerStyle Mode="NumericPages"></PagerStyle>
	</asp:datagrid><br>
	&nbsp;<asp:button id="cmdEvaluate" runat="server" Text="Continue"></asp:button>
	<asp:label id="lblEvaluate" runat="server" Font-Size="8pt" ForeColor="Red">In order to perform evaluation on this assignment, it must first be locked for evaluation</asp:label>
</div>
<div runat="server" style="BORDER-RIGHT: 1px solid; PADDING-RIGHT: 15px; PADDING-LEFT: 15px; PADDING-BOTTOM: 15px; BORDER-LEFT: 1px solid; PADDING-TOP: 15px; BORDER-BOTTOM: 1px solid; HEIGHT: 350px"
	id="divUsers">
	<div runat="server" id="divSearch">
		Find Students
		<asp:DropDownList ID="drpSearchMethod" Runat="server">
			<asp:ListItem Value="username" Text="by Username" />
			<asp:ListItem Value="lastname" Text="by Lastname" />
		</asp:DropDownList>
		<asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
		<asp:button id="cmdSearch" runat="server" Text="Search" />
		<br>
	</div>
	<asp:datagrid id="dgUserSearch" runat="server" AllowPaging="True" Width="100%" AutoGenerateColumns="False"
		CellPadding="3" PageSize="9" AllowCustomPaging="True">
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
			<asp:TemplateColumn HeaderText="Submissions">
				<ItemStyle Width="10px"></ItemStyle>
				<ItemTemplate>
					<asp:Label runat="server" ID="lblNumSubmissions" />
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
			<asp:TemplateColumn Visible="False" HeaderText="Group/UserID">
				<ItemTemplate>
					<asp:Label runat="server" ID="lblID"></asp:Label>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
		<PagerStyle Mode="NumericPages"></PagerStyle>
	</asp:datagrid><br>
	&nbsp;<asp:button id="cmdUserEvaluate" runat="server" Text="Continue"></asp:button>
</div></iewc:PageView>
	<iewc:PageView>
		<i><b>Evaluation is in progress.</b><br>
			<br>
			View the progress of automatic jobs by selecting the Automatic Jobs icon on the 
			left pane. Refer to the left pane to perform subjective testing by clicking on 
			each student you selected.
			<br>
			<br>
			To start again with a different user set, click the button below. Selecting 
			Start Over will <b>not</b> cancel the job you just created.</i><br>
		<br>
		<asp:Button Runat="server" ID="cmdReset" Text="Start Over" />
	</iewc:PageView>
	<iewc:PageView>
		<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Automatic Job Creation</font>
		<br>
		<br>
		<asp:Button id="cmdSkip" runat="server" Text="Skip Automatic Tests"></asp:Button>
		<br>
		<br>
		<EM>Create automatic tests, or click Skip to proceed to subjective testing.</EM>
		<br>
		<DIV style="WIDTH: 297px; POSITION: relative; HEIGHT: 47px" ms_positioning="GridLayout">
			<asp:TextBox id="txtName" style="Z-INDEX: 100; LEFT: 1px; POSITION: absolute; TOP: 18px" Width="208px"
				runat="server"></asp:TextBox>
			<DIV style="DISPLAY: inline; Z-INDEX: 107; LEFT: 0px; WIDTH: 96px; POSITION: absolute; TOP: 2px; HEIGHT: 19px"
				ms_positioning="FlowLayout">Job Name:</DIV>
		</DIV>
		<DIV runat="server" id="divAuto" style="WIDTH: 451px; POSITION: relative; HEIGHT: 236px"
			ms_positioning="GridLayout">
			<asp:ListBox id="lstTests" Height="81px" Width="254px" runat="server" SelectionMode="Multiple"
				style="Z-INDEX: 102; LEFT: 2px; POSITION: absolute; TOP: 27px" AutoPostBack="True"></asp:ListBox>
			<DIV style="DISPLAY: inline; Z-INDEX: 101; LEFT: 2px; WIDTH: 144px; POSITION: absolute; TOP: 3px; HEIGHT: 16px"
				ms_positioning="FlowLayout">Select Tests to Run</DIV>
			<asp:Label id="lblAutoError" runat="server" Width="336px" Height="22px" Visible="False" Font-Size="8pt"
				ForeColor="Red" style="Z-INDEX: 103; LEFT: 104px; POSITION: absolute; TOP: 208px"></asp:Label>
			<asp:Button id="cmdSubmit" Text="Continue" runat="server" style="Z-INDEX: 104; LEFT: 2px; POSITION: absolute; TOP: 212px"></asp:Button>
			<asp:ListBox id="lstOrder" style="Z-INDEX: 105; LEFT: 2px; POSITION: absolute; TOP: 128px" runat="server"
				Width="256px" Height="81px"></asp:ListBox>
			<DIV style="DISPLAY: inline; Z-INDEX: 106; LEFT: 2px; WIDTH: 436px; POSITION: absolute; TOP: 108px; HEIGHT: 21px"
				ms_positioning="FlowLayout">Evaluation Order (Dependencies may add extra 
				evaluations)</DIV>
		</DIV>
		<asp:Label Runat="server" ID="lblPrinStr" Visible="False" />
		<asp:Button Runat="server" ID="cmdCancel" Text="Cancel" />
	</iewc:PageView>
	<iewc:PageView></iewc:PageView>
</iewc:multipage>

