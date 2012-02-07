
<%@ Register TagPrefix="ie" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<html>
  <body>
    <form runat="server" >
      <ie:ToolBar id="myToolbar" autopostback="true" runat="server">
        <ie:ToolBarSeparator id="separator" />
        <ie:ToolBarButton id="button1" Text="TabIndex3" TabIndex="3"  />
        <ie:ToolBarButton id="button2" Text="TabIndex1" TabIndex="1"  />
        <ie:ToolBarButton id="button3" Text="TabIndex2" TabIndex="2"  />
      </ie:ToolBar>
    </form>
  </body>
</html>