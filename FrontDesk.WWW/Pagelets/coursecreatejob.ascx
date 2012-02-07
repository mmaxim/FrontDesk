<%@ Register TagPrefix="userpage" TagName="SectionExpl" Src="../Controls/sectionexpl.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="coursecreatejob.ascx.cs" Inherits="FrontDesk.Pages.Pagelets.CourseCreateJob" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<meta name="vs_snapToGrid" content="True">
<meta name="vs_showGrid" content="True">
<h2>
	Automatic Job Creation</h2>
<P>
	Select users and evaluations from below to create the job set. Once you have 
	selected all the users and tests hit&nbsp; <b>Start Testing</b>&nbsp;to queue 
	the job for available testing centers.
</P>
<h4>Select a Job Name</h4>
<DIV style="WIDTH: 409px; POSITION: relative; HEIGHT: 47px" ms_positioning="GridLayout">
	<asp:TextBox id="txtName" style="Z-INDEX: 108; LEFT: 11px; POSITION: absolute; TOP: 20px" Width="208px"
		runat="server"></asp:TextBox>
	<DIV style="DISPLAY: inline; Z-INDEX: 108; LEFT: 6px; WIDTH: 96px; POSITION: absolute; TOP: 3px; HEIGHT: 19px"
		ms_positioning="FlowLayout">Job Name:</DIV>
</DIV>
<h4>
	Select Users/Groups and Evaluations</h4>
<DIV style="WIDTH: 710px; POSITION: relative; HEIGHT: 473px" ms_positioning="GridLayout">
	<DIV style="Z-INDEX: 103; LEFT: 5px; WIDTH: 336px; POSITION: absolute; TOP: 19px; HEIGHT: 407px"
		ms_positioning="GridLayout">
		<userpage:SectionExpl id="ucSectionExpl" runat="server"></userpage:SectionExpl></DIV>
	<DIV style="DISPLAY: inline; Z-INDEX: 104; LEFT: 5px; WIDTH: 192px; POSITION: absolute; TOP: 3px; HEIGHT: 16px"
		ms_positioning="FlowLayout">Select Users and Groups:</DIV>
	<asp:ListBox id="lstTests" Height="141px" Width="254px" runat="server" SelectionMode="Multiple"
		style="Z-INDEX: 102; LEFT: 360px; POSITION: absolute; TOP: 24px" AutoPostBack="True"></asp:ListBox>
	<DIV style="DISPLAY: inline; Z-INDEX: 101; LEFT: 360px; WIDTH: 144px; POSITION: absolute; TOP: 0px; HEIGHT: 16px"
		ms_positioning="FlowLayout">Select Tests to Run</DIV>
	<asp:Label id="lblError" runat="server" Width="294px" Height="75px" Visible="False" Font-Size="8pt"
		ForeColor="Red" style="Z-INDEX: 105; LEFT: 368px; POSITION: absolute; TOP: 368px"></asp:Label>
	<asp:Button id="cmdSubmit" Height="30px" Width="96px" Text="Start Testing" runat="server" style="Z-INDEX: 106; LEFT: 34px; POSITION: absolute; TOP: 439px"></asp:Button>
	<asp:ListBox id="lstOrder" style="Z-INDEX: 107; LEFT: 360px; POSITION: absolute; TOP: 216px"
		runat="server" Width="256px" Height="132px"></asp:ListBox>
	<DIV style="DISPLAY: inline; Z-INDEX: 108; LEFT: 360px; WIDTH: 256px; POSITION: absolute; TOP: 176px; HEIGHT: 38px"
		ms_positioning="FlowLayout">Evaluation Order (Dependencies may add extra 
		evaluations)</DIV>
</DIV>
<br>
<br>
