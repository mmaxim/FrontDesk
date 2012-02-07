<%@ Control Language="c#" AutoEventWireup="false" Codebehind="filepermissions.ascx.cs" Inherits="FrontDesk.Controls.Filesys.FilePermissionsControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
Roles and Users:&nbsp;<asp:dropdownlist id="ddlPrins" runat="server" AutoPostBack="True"></asp:dropdownlist><br>
<asp:checkbox id="chkRead" runat="server" Text="Read"></asp:checkbox>&nbsp;
<asp:checkbox id="chkWrite" runat="server" Text="Write"></asp:checkbox>&nbsp;
<asp:checkbox id="chkDelete" runat="server" Text="Delete"></asp:checkbox><br>
<br>
<asp:button id="cmdUpdate" runat="server" Text="Update"></asp:button>&nbsp;&nbsp;
<asp:Label id="lblError" runat="server" ForeColor="Red" Font-Size="8pt" Visible="False"></asp:Label>
