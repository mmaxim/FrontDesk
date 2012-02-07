
=====================================
Internet Explorer Web Controls README
=====================================
Last updated: 1/14/2002

Thank you for downloading the IE Web Controls source code release!  The Src 
folder contains the source code for the IE MultiPage, ToolBar, TreeView, and 
TabStrip controls, along with related base classes and design-time support.

To build the IE Web Controls:

1.  Make sure you have installed the .NET Framework SDK v1.0 or v1.1
2.  Run Build.bat, which will create a build folder in this directory.  
    The build folder contains Microsoft.Web.UI.WebControls.dll and a 
    Runtime directory of supporting files.

To run the IE Web Controls:

1.  Copy the contents of the Runtime directory to the webctrl_client\1_0
    directory under your top-level site directory.  For example, if your 
    site root is c:\Inetpub\wwwroot, type this at the command prompt:

    xcopy /s /i .\build\Runtime c:\Inetpub\wwwroot\webctrl_client\1_0 /y

    This will create the following directory structure under the site:

      /webctrl_client/1_0
        MultiPage.htc
        TabStrip.htc
        toolbar.htc
        treeview.htc
        webservice.htc
        webserviced.htc
        [images]
        [treeimages]

2.  Create a new web application in IIS and copy the contents of the
    samples directory to this application directory.  For example:

    xcopy /s /i .\samples c:\Inetpub\wwwroot\sampleapp /y

3.  Create a /bin subdirectory for the application and copy the file
    Microsoft.Web.UI.WebControls.dll to this directory.

    The contents of the application will be as follows:

      /sampleapp
        multipage.aspx
        state_city.xml
        tabstrip.aspx
        toolbar.aspx
        treeview.aspx
        treeview_bound.aspx
        /bin
          Microsoft.Web.UI.WebControls.dll

4.  Request the sample pages from your Internet Explorer web browser, for
    example: http://localhost/sampleapp/multipage.aspx
    
For additional documentation and samples visit:
http://msdn.microsoft.com/library/default.asp?url=/workshop/webcontrols/webcontrols_entry.asp
