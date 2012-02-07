
<%@ Register TagPrefix="ie" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<html>
  <body>
    <form runat="server">
      <ie:TreeView runat="server" SystemImagesPath="/webctrl_client/1_0/treeimages" >
        <ie:TreeNode Text="North America" ImageUrl="/webctrl_client/1_0/images/root.gif" 
          Expanded="true" TreeNodeSrc="state_city.xml" />
      </ie:TreeView>
    </form>
  </body>
</html>