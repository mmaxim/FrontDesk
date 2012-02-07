
<%@ Register TagPrefix="ie" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<html>
  <body>
    <form runat="server">
      <ie:TreeView runat="server">
        <ie:TreeNode Text="My first Tree Node">
          <ie:TreeNode Text="My second Tree Node" />
        </ie:TreeNode>
      </ie:TreeView>
    </form>
  </body>
</html>