
<%@ Register TagPrefix="ie" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<html>
  <body>
    <form runat="server">
      <ie:TabStrip runat="server"
        TabDefaultStyle="background-color:#000000;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:79;height:21;text-align:center"
        TabHoverStyle="background-color:#777777"
        TabSelectedStyle="background-color:#ffffff;color:#000000">
        <ie:Tab Text="Home"/>
        <ie:Tab Text="About us"/>
        <ie:Tab Text="Products"/>
        <ie:Tab Text="Support"/>
        <ie:Tab Text="Contact us"/>
      </ie:TabStrip>
    </form>
  </body>
</html>