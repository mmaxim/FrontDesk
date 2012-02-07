<%@ Control Language="c#" AutoEventWireup="false" Codebehind="evaldeps.ascx.cs" Inherits="FrontDesk.Controls.EvaluationDepsControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
Dependencies:&nbsp;
<asp:LinkButton id="lnkUpdate" runat="server">Update</asp:LinkButton>&nbsp;
<asp:Label id="lblError" runat="server" Visible="False" Font-Size="8pt" ForeColor="Red"></asp:Label>
<asp:checkboxlist id="chkDeps" runat="server"></asp:checkboxlist>
