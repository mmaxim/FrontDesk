using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Data.Filesys.Provider;
using FrontDesk.Tools;
using FrontDesk.Security;

namespace FrontDesk.Services {

	/// <summary>
	/// Course data service
	/// </summary>
	[WebService(Namespace="http://FrontDesk/WebServices")]
	public class CourseDataService : TicketService {

		public CourseDataService() {
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
		/// Return a courses' assignments
		/// </summary>
		[WebMethod(), SoapHeader("Ticket")]
		public Assignment.AssignmentList GetAssignments(int courseID) {
			AuthorizedIdent ident = AuthenticateTicket(Ticket);

			return new Courses(ident).GetAssignments(courseID);
		}
	}
}
