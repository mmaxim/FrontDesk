using System;
using System.Collections;

namespace FrontDesk.Components {

	/// <summary>
	/// Permission component
	/// </summary>
	public class Permission : DataComponent {

		public Permission() { }

		public enum GrantType { DIRECT, INHERIT, DENY };

		public const string COURSE = "course";
		public const string ASSIGNMENT = "asst";
		public const string SECTION = "section";

		public const string ID_FIELD = "ID";
		public const string NAME_FIELD = "name";
		public const string DESC_FIELD = "description";
		public const string TYPE_FIELD = "type";

		private int m_id;
		private string m_name, m_desc, m_type;

		[FieldName(ID_FIELD)]
		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}

		[FieldName(NAME_FIELD)]
		public string Name {
			get { return m_name; }
			set { m_name = value; }
		}

		[FieldName(DESC_FIELD)]
		public string Description {
			get { return m_desc; }
			set { m_desc = value; }
		}

		[FieldName(TYPE_FIELD)]
		public string Type {
			get { return m_type; }
			set { m_type = value; }
		}

		public class PermissionList : ArrayList {
			public PermissionList() { }

			public new Permission this[int index] {
				get { return (Permission) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
