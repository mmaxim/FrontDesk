using System;

using FrontDesk.Security;
using FrontDesk.Common;
using FrontDesk.Components;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Activity log data access component
	/// </summary>
	public class Activities : DataAccessComponent {

		public Activities(AuthorizedIdent ident) : base(ident) { }

		/// <summary>
		/// Create a new activity log entry
		/// </summary>
		public bool Create(string username, int type, int objID, string desc) {
			Activity act = new Activity();
			act.Username = username;
			act.Type = type;
			act.Description = desc;
			act.ObjectID = objID;
			return m_dp.CreateActivity(act);
		}

	}
	
}
