using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using FrontDesk.Components;
using FrontDesk.Common;
using FrontDesk.Data.Access;
using FrontDesk.Security;

namespace FrontDesk.Services {
	
	/// <summary>
	/// Service base class
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class TicketService : WebService {

		public TicketService() { }

		public ServiceTicket Ticket=null;

		protected AuthorizedIdent AuthenticateTicket(ServiceTicket ticket) {

			AuthorizedIdent ident = AuthorizedIdent.Create(ticket);
			if (ident == null)
				throw new ServiceAuthenticationException("Unable to authenticate ticket because some aspect of it is invalid. You session may have expired or you may have switched to another machine (and somehow saved the ticket). Please login again");
		
			return ident;
		}

		[WebMethod()]
		public ServiceTicket Authenticate(string username, string password) {
			if (new Users(null).IsValid(username, password)) {
				ServiceTicket tik = new ServiceTicket();
				tik.Username = username;
				tik.Ident = Guid.NewGuid();
				tik.HostAddress = Context.Request.UserHostAddress;

				//Create session
				new Sessions(null).Create(tik.Ident, tik.Username, tik.HostAddress);

				return tik;
			} else
				throw new ServiceAuthenticationException("Username or password invalid");
		}

		[WebMethod(), SoapHeader("Ticket")]
		public void Logout() {
			new Sessions(null).Delete(Ticket.Ident);
		}
	}
}
