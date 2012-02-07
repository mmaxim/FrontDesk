
<%@ Register TagPrefix="ie" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<html>
  <body>
    <form runat="server">
      <ie:MultiPage SelectedIndex="1" runat="server">
        <ie:PageView>
            This is page number one
        </ie:PageView>
        <ie:PageView>
            This is page number two, and this page view is selected!
        </ie:PageView>
      </ie:MultiPage>
    </form>
  </body>
</html>