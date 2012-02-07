using System;
using System.Web;
using System.Security.Principal;

using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Data.Access;
using FrontDesk.Services;

namespace FrontDesk.Security {

	/// <summary>
	/// Authorized identity object
	/// </summary>
	public class AuthorizedIdent {

		private AuthorizedIdent(string name) { m_name=name; }

		private string m_name;

		public static AuthorizedIdent NoOne = new AuthorizedIdent("__no_one__");

		public string Name {
			get { return m_name; }
		}

		/// <summary>
		/// Create an identity from a service created service ticket
		/// </summary>
		public static AuthorizedIdent Create(ServiceTicket ticket) {
			ServiceTicket tik;
			if (ticket == null)
				return null;
			else {
				Session ses = new Sessions(null).GetInfo(ticket.Ident);
				if (ses == null)
					return null;
				
				tik = new ServiceTicket();
				tik.Username = ses.Username; tik.HostAddress = ses.Address;
				tik.Ident = ses.Identifier;
			}
			
			if (tik.Username != ticket.Username)
				return null;
			else if (tik.HostAddress != ticket.HostAddress)
				return null;
			else
				return new AuthorizedIdent(tik.Username);
		}

		/// <summary>
		/// Create an authorized identity from the ASP.NET login identity
		/// </summary>
		public static AuthorizedIdent Create() {
			if (HttpContext.Current != null) {
				IIdentity ident = HttpContext.Current.User.Identity;
				if (ident != null)
					return new AuthorizedIdent(ident.Name);
				else
					return null;
			} else
				return null;
		}
	}
}
