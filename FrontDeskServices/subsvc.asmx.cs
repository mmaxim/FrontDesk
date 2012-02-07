using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;

using FrontDesk.Common;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Data.Filesys.Provider;
using FrontDesk.Tools;
using FrontDesk.Security;

namespace FrontDesk.Services {

	/// <summary>
	/// Submission service
	/// </summary>
	[WebService(Namespace="http://FrontDesk/WebServices")]
	public class SubmissionService : TicketService {

		public SubmissionService() {
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		/// <summary>
		/// ZIP data submission service
		/// </summary>
		[WebMethod(), SoapHeader("Ticket")]
		public void ZipArchiveSubmit(byte[] zipdata, int principalID, int asstID) {

			AuthorizedIdent ident = AuthenticateTicket(Ticket);

			ZipTool zt = new ZipTool();
			zt.CreateSource(new MemoryStream(zipdata));
			new Submissions(ident).Create(asstID, principalID, zt);
		}
	}
}
