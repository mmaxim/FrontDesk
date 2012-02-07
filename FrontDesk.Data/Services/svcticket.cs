using System;
using System.Web.Services.Protocols;

namespace FrontDesk.Services {

	/// <summary>
	/// Service ticket for using the Web Services 
	/// </summary>
	public class ServiceTicket : SoapHeader {

		public ServiceTicket() {
		}

		public string Username;
		public Guid Ident;
		public string HostAddress;
	}

	public class ServiceAuthenticationException : Exception {

		public ServiceAuthenticationException() : base("Service authentication failed") { }
		public ServiceAuthenticationException(string msg) : base(msg) { }
	}
}
