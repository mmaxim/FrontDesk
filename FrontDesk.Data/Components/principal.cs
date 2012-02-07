using System;
using System.Collections;

namespace FrontDesk.Components {
	
	/// <summary>
	/// Principal data
	/// </summary>
	public class Principal : DataComponent {
		
		public Principal() { }

		public const String PRINCIPALID_FIELD = "principalID";
		public const String PRINCIPALNAME_FIELD = "name";
		public const String PRINCIPALTYPE_FIELD = "type";

		public const int USER = 1, GROUP = 2, ROLE = 3;

		protected string m_name;
		protected int m_principalID;
		protected int m_type;
 
		[FieldName(PRINCIPALNAME_FIELD)]
		public string Name {
			get { return m_name; }
			set { m_name = value; }
		}

		[FieldName(PRINCIPALID_FIELD)]
		public int PrincipalID {
			get { return m_principalID; }
			set { m_principalID = value; }
		}

		[FieldName(PRINCIPALTYPE_FIELD)]
		public int Type {
			get { return m_type; }
			set { m_type = value; }
		}

		public class PrincipalList : ArrayList {

			public PrincipalList() { }

			public new Principal this[int index] {
				get { return (Principal) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
