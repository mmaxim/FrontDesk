using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.Web.UI.WebControls;

using FrontDesk.Components;
using FrontDesk.Data.Access;
using FrontDesk.Common;

namespace FrontDesk.Pages {
	
	/// <summary>
	/// Section explorer page.
	/// </summary>
	public class SectionExplorerPage : Page {
	
		private void Page_Load(object sender, System.EventArgs e) {
			
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
