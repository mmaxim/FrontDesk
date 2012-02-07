using System;

using FrontDesk.Components;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Sessions data access component
	/// </summary>
	internal class Sessions : DataAccessComponent {
		
		public Sessions(AuthorizedIdent ident) : base(ident) { }

		/// <summary>
		/// Create a session
		/// Direct Provider layer call
		/// </summary>
		public bool Create(Guid ident, string username, string address) {
			return m_dp.CreateSession(ident, username, address);
		}

		/// <summary>
		/// Get session info from a GUID
		/// Direct Provider layer call
		/// </summary>
		public Session GetInfo(Guid ident) {
			return m_dp.GetSessionInfo(ident);
		}

		/// <summary>
		/// Remove a session
		/// Direct Provider layer call
		/// </summary>
		public bool Delete(Guid ident) {
			return m_dp.DeleteSession(ident);
		}
	}
}
