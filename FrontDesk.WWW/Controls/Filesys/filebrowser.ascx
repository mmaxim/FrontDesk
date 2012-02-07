<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="filebrowser.ascx.cs" Inherits="FrontDesk.Controls.Filesys.FileBrowserPagelet" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<DIV style="WIDTH: 852px; POSITION: relative; HEIGHT: 636px" ms_positioning="GridLayout"><iewc:toolbar id="tbActions" style="Z-INDEX: 101; LEFT: 450px; POSITION: absolute; TOP: 22px"
		Font-Size="7pt" Width="62px" runat="server">
		<iewc:ToolbarButton ImageUrl="attributes/folder.gif" ID="cmdNewFolder" ToolTip="Create a New Folder"></iewc:ToolbarButton>
		<iewc:ToolbarSeparator></iewc:ToolbarSeparator>
		<iewc:ToolbarButton ImageUrl="attributes/filebrowser/view.gif" ID="cmdView" ToolTip="View Files"></iewc:ToolbarButton>
		<iewc:ToolbarButton ImageUrl="attributes/filebrowser/cut.gif" ID="cmdCut" ToolTip="Cut a file"></iewc:ToolbarButton>
		<iewc:ToolbarButton ImageUrl="attributes/filebrowser/copy.gif" ID="cmdCopy" ToolTip="Copy a file"></iewc:ToolbarButton>
		<iewc:ToolbarButton ImageUrl="attributes/filebrowser/paste.gif" ID="cmdPaste" ToolTip="Paste a file"></iewc:ToolbarButton>
		<iewc:ToolbarButton ImageUrl="attributes/filebrowser/delete.gif" ID="cmdDelete" ToolTip="Delete a file"></iewc:ToolbarButton>
		<iewc:ToolbarSeparator></iewc:ToolbarSeparator>
		<iewc:ToolbarButton ImageUrl="attributes/filebrowser/reload.gif" ID="cmdReload" ToolTip="Reload"></iewc:ToolbarButton>
		<iewc:ToolbarSeparator></iewc:ToolbarSeparator>
		<iewc:ToolbarButton ImageUrl="attributes/filebrowser/sa.gif" ID="cmdSelectAll" ToolTip="Select All"></iewc:ToolbarButton>
		<iewc:ToolbarButton ImageUrl="attributes/filebrowser/sn.gif" ID="cmdSelectNone" ToolTip="Select None"></iewc:ToolbarButton>
	</iewc:toolbar><asp:label id="lblClipboard" style="Z-INDEX: 102; LEFT: 266px; POSITION: absolute; TOP: 1px"
		Font-Size="7pt" Width="401px" runat="server" Font-Names="Verdana">Clipboard: Empty</asp:label>
	<DIV style="BORDER-RIGHT: 1px ridge; BORDER-TOP: 1px ridge; Z-INDEX: 103; LEFT: 2px; VERTICAL-ALIGN: top; OVERFLOW: auto; BORDER-LEFT: 1px ridge; WIDTH: 165px; BORDER-BOTTOM: 1px ridge; POSITION: absolute; TOP: 54px; HEIGHT: 553px"
		ms_positioning="GridLayout"><iewc:treeview id="tvFiles" style="Z-INDEX: 106; LEFT: 0px; POSITION: absolute; TOP: 0px" runat="server"
			SelectExpands="True" AutoPostBack="True" DefaultStyle="font-size: 7pt; font-family: verdana"></iewc:treeview></DIV>
	<asp:label id="lblMessages" style="Z-INDEX: 104; LEFT: 266px; POSITION: absolute; TOP: 12px"
		Font-Size="7pt" Width="443px" runat="server" Font-Names="Verdana" ForeColor="Red" Height="18px">Messages: </asp:label>
	<DIV style="Z-INDEX: 105; LEFT: 170px; VERTICAL-ALIGN: top; OVERFLOW: auto; WIDTH: 556px; POSITION: absolute; TOP: 55px; HEIGHT: 552px"
		ms_positioning="GridLayout"><asp:datagrid id="dgFiles" style="FONT-SIZE: 7pt; Z-INDEX: 107; LEFT: 0px; POSITION: absolute; TOP: 0px; FONT-NAME: Verdana"
			Width="540px" runat="server" DataKeyField="ID" AutoGenerateColumns="False" PageSize="12">
			<AlternatingItemStyle BackColor="WhiteSmoke"></AlternatingItemStyle>
			<ItemStyle Font-Size="7pt" Font-Names="Verdana" VerticalAlign="Middle"></ItemStyle>
			<HeaderStyle Font-Names="Verdana" Font-Bold="True" BorderWidth="2px" ForeColor="White" BorderStyle="Groove"
				BorderColor="White" BackColor="#4768A3"></HeaderStyle>
			<Columns>
				<asp:TemplateColumn>
					<ItemTemplate>
						<asp:CheckBox Runat="server" ID="chkSelect" />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Name">
					<ItemTemplate>
						<asp:Image runat="server" id="FileImage" Width="16" Height="20" ImageAlign="AbsMiddle" />
						<asp:LinkButton runat="server" id="lnkName" CommandName="File" CausesValidation="false" />
					</ItemTemplate>
					<EditItemTemplate>
						<asp:TextBox Runat="server" ID="txtName" />
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Size">
					<ItemTemplate>
						<asp:Label runat="server" id="lblSize" Text='<%# DataBinder.Eval(Container, "DataItem.Size") %>'>
						</asp:Label>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn DataField="FileModified" ReadOnly="True" HeaderText="Modified"></asp:BoundColumn>
				<asp:BoundColumn DataField="FileCreated" ReadOnly="True" HeaderText="Created"></asp:BoundColumn>
				<asp:TemplateColumn>
					<ItemStyle Width="10px"></ItemStyle>
					<ItemTemplate>
						<asp:ImageButton Runat="server" ImageUrl="../../attributes/backup.gif" ID="imgDownload" />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
					<ItemStyle Width="10px"></ItemStyle>
					<ItemTemplate>
						<asp:LinkButton runat="server" ID="lnkRename" Text="Rename" CommandName="Edit" CausesValidation="false"></asp:LinkButton>
					</ItemTemplate>
					<EditItemTemplate>
						<asp:LinkButton runat="server" Text="Update" CommandName="Update"></asp:LinkButton>&nbsp;
						<asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false"></asp:LinkButton>
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
					<ItemStyle Width="10px"></ItemStyle>
					<ItemTemplate>
						<asp:Image Runat="server" AlternateText="Locked" ID="imgLock" Width="16" Height="19" />
						<asp:Image Runat="server" AlternateText="Read Only" ID="imgReadOnly" Width="16" Height="19" />
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:datagrid></DIV>
	<DIV style="DISPLAY: inline; FONT-SIZE: 14pt; Z-INDEX: 106; WIDTH: 262px; COLOR: #4768a3; FONT-FAMILY: Verdana; POSITION: absolute; HEIGHT: 25px"
		ms_positioning="FlowLayout">FrontDesk<SUP><FONT size="2">TM</FONT></SUP> FileBrowser</DIV>
	<INPUT id="fiUpload" style="Z-INDEX: 107; LEFT: 76px; WIDTH: 298px; POSITION: absolute; TOP: 30px; HEIGHT: 22px"
		type="file" size="30" runat="server">
	<DIV style="DISPLAY: inline; FONT-SIZE: 8pt; Z-INDEX: 108; LEFT: 4px; WIDTH: 69px; POSITION: absolute; TOP: 34px; HEIGHT: 15px"
		ms_positioning="FlowLayout">Upload a file:</DIV>
	<asp:button id="cmdUpload" style="Z-INDEX: 109; LEFT: 384px; POSITION: absolute; TOP: 29px"
		runat="server" Text="Upload"></asp:button>
	<asp:HyperLink id="hypClose" style="Z-INDEX: 110; LEFT: 675px; POSITION: absolute; TOP: 0px" runat="server"
		Font-Size="8pt" Font-Bold="True" NavigateUrl="javascript:window.close();">Close</asp:HyperLink></DIV>
