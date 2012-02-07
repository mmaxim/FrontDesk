<%@ Register TagPrefix="userpage" TagName="FilePerms" Src="../filesys/filepermissions.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="content.ascx.cs" Inherits="FrontDesk.Controls.Matrix.ContentView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Content Information</font><br>
<br>
Content Name:<br>
<asp:textbox id="txtName" Runat="server" Width="407px"></asp:textbox><br>
<br>
Content Type (File Extension (e.g. .txt, .jpg, .doc, .pdf, etc.):<br>
<asp:textbox id="txtType" Runat="server" Width="87px"></asp:textbox><br>
<br>
Content Description:<br>
<asp:textbox id="txtDesc" Width="407px" runat="server" TextMode="MultiLine" Height="40px"></asp:textbox><br>
<br>
<asp:button id="cmdUpdate" Runat="server" Text="Update"></asp:button>&nbsp;&nbsp;&nbsp;
<asp:label id="lblError" runat="server" Visible="False" ForeColor="Red" Font-Size="8pt"></asp:label>
<div id="divData" runat="server">
	<br>
	<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Content Data</font>
	<br>
	<br>
	<IMG alt="" src="attributes/filebrowser/html.gif" align="absMiddle">
	<asp:linkbutton id="lnkEdit" Runat="server">View/Edit Content</asp:linkbutton>&nbsp;&nbsp;
	<IMG alt="" src="attributes/backup.gif" align="absMiddle">
	<asp:linkbutton id="lnkDownload" Runat="server">Download Content</asp:linkbutton><br>
	<br>
	<div runat="server" id="divUpload">
	Content Data Type:
	<asp:RadioButton id="rdbData" runat="server" Text="Local File Data" Checked="True" AutoPostBack="True"
		GroupName="datatype"></asp:RadioButton>
	&nbsp;&nbsp;<asp:RadioButton id="rdbLink" runat="server" Text="WWW HyperLink" AutoPostBack="True" GroupName="datatype"></asp:RadioButton><br>
	<br>
	<iewc:multipage id="mpViews" runat="server">
		<iewc:PageView>
	<i runat="server" id="iDirections">Update Local File into Content (changes Content type):</i>
	<br>
	<INPUT id="ufContent" style="WIDTH: 452px; HEIGHT: 22px" type="file" size="56" name="File1"
				runat="server"><br>
	<br>
	&nbsp;
<asp:button id="cmdDataUpload" Text="Upload" runat="server"></asp:button>&nbsp;&nbsp;&nbsp;
	<asp:label id="lblUpError" runat="server" Visible="False" ForeColor="Red" Font-Size="8pt"></asp:label>
	</iewc:PageView>
		<iewc:PageView><i>Enter the Url of the WWW HyperLink</i><br>
<asp:TextBox id="txtUrl" Width="366px" runat="server"></asp:TextBox><br>
<br>
&nbsp;&nbsp;
<asp:Button id="cmdUrlUpload" runat="server" Text="Upload"></asp:Button>&nbsp;&nbsp;&nbsp;
<asp:Label id="lblLinkError" runat="server" Font-Size="8pt" ForeColor="Red" Visible="False"></asp:Label></iewc:PageView>
	</iewc:multipage></div>
</div>
<div runat="server" id="divPerms">
	<br>
	<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Content Permissions</font><br>
	<br>
	<userpage:FilePerms runat="server" id="ucFilePerms" /><br>
	<br>
</div>

