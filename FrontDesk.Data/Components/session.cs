using System;
using System.Collections;

namespace FrontDesk.Components {

	/// <summary>
	/// Session component
	/// </summary>
	public class Session : DataComponent {

		public Session() { }

		public const string GUID_FIELD = "guid";
		public const string USERNAME_FIELD = "username";
		public const string ADDRESS_FIELD = "address";
		public const string CREATION_FIELD = "creation";

		private Guid m_guid;
		private string m_username, m_address;
		private DateTime m_creation;

		[FieldName(GUID_FIELD)]
		public Guid Identifier {
			get { return m_guid; }
			set { m_guid = value; }
		}

		[FieldName(USERNAME_FIELD)]
		public string Username {
			get { return m_username; }
			set { m_username = value; }
		}

		[FieldName(ADDRESS_FIELD)]
		public string Address {
			get { return m_address; }
			set { m_address = value; }
		}

		[FieldName(CREATION_FIELD)]
		public DateTime Creation {
			get { return m_creation; }
			set { m_creation = value; }
		}

		public class SessionList : ArrayList {
			public SessionList() {
			}

			public new Session this[int index] {
				get { return (Session) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
