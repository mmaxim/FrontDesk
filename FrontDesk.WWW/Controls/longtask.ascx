<%@ Control Language="c#" AutoEventWireup="false" Codebehind="longtask.ascx.cs" Inherits="FrontDesk.Controls.LongTaskControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script>
	function refresh() {
	
		var theform;
		if (window.navigator.appName.toLowerCase().indexOf("netscape") > -1) {
			theform = document.forms["Form1"];
		}
		else {
			theform = document.Form1;
		}
	
		theform.ucLongTask_cmdRefresh.click();
	}
	setTimeout("refresh()", 2*1000 );
</script>
<DIV style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; WIDTH: 935px; BORDER-BOTTOM: 1px solid; POSITION: relative; HEIGHT: 505px"
	ms_positioning="GridLayout"><IMG style="Z-INDEX: 101; LEFT: 383px; POSITION: absolute; TOP: 345px" alt="" src="attributes/loading.gif">
	<asp:button id="cmdRefresh" style="Z-INDEX: 102; LEFT: 342px; POSITION: absolute; TOP: 329px"
		runat="server" Height="2px"></asp:button>
	<DIV style="Z-INDEX: 104; LEFT: 148px; WIDTH: 639px; POSITION: absolute; TOP: 40px; HEIGHT: 156px"
		align="center" ms_positioning="FlowLayout">
		<P><STRONG><FONT color="#4768a3" size="5"></FONT></STRONG>&nbsp;</P>
		<P><STRONG><FONT color="#4768a3" size="5">FrontDesk is Processing Your Request</FONT></STRONG></P>
		<P><STRONG><FONT color="#4768a3" size="5"></FONT></STRONG>&nbsp;</P>
		<P><STRONG><FONT color="#4768a3" size="5"><STRONG><FONT color="#4768a3" size="5">Do NOT hit Back or 
							Refresh on your browser!</FONT></STRONG></FONT></STRONG></P>
		<P><STRONG><FONT color="#4768a3" size="5"></FONT></STRONG>&nbsp;</P>
		<P><STRONG><FONT color="#4768a3" size="5"><STRONG><FONT color="#4768a3" size="5">Please Wait....</FONT></STRONG></FONT></STRONG></P>
		<P><STRONG><FONT color="#4768a3" size="5"></FONT></STRONG>&nbsp;</P>
	</DIV>
</DIV>
