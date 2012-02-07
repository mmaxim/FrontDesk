//Mike Maxim

using System;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime;
using System.Collections;

namespace FrontDesk.Components {

	public class Group : Principal {

		public Group() : base() { }

		public const String ASSTID_FIELD = "asstID";
		public const String CREATOR_FIELD = "creator";
		public const String CREATION_FIELD = "creation";
		public const String NAME_FIELD = "groupName";

		protected int m_asstID;
		protected DateTime m_creation;
		protected string  m_creator;

		[FieldName(ASSTID_FIELD)]
		public int AsstID {
			get { return m_asstID; }
			set { m_asstID = value; }
		}

		[FieldName(CREATOR_FIELD)]
		public string Creator {
			get { return m_creator; }
			set { m_creator = value; }
		}

		[FieldName(CREATION_FIELD)]
		public DateTime Creation {
			get { return m_creation; }
			set { m_creation = value; }
		}

		[FieldName(NAME_FIELD)]
		public string GroupName {
			get { return Name; }
			set { Name = value; }
		}

		public class GroupList : ArrayList {

			public GroupList() {
			}

			public new Group this[int index] {
				get { return (Group) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}