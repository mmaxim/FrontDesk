using System;
using System.Collections;

namespace FrontDesk.Components {

	/// <summary>
	/// Invitation entity
	/// </summary>
	public class Invitation : DataComponent {
		
		public Invitation() { }

		public const String PRINCIPALID_FIELD = "principalID";
		public const String ASSTID_FIELD = "asstID";
		public const String CREATOR_FIELD = "creator";
		public const String CREATION_FIELD = "creation";
		public const String NAME_FIELD = "groupName";
		public const String INVITOR_FIELD = "invitor";
		public const String INVITEE_FIELD = "invitee";
		public const String ID_FIELD = "ID";

		protected Group m_group = new Group();
		protected string m_invitor, m_invitee;
		protected int m_id;
		protected int m_groupid;

		public Group Group {
			get { return m_group; }
			set { m_group = value; }
		}

		[FieldName(PRINCIPALID_FIELD)]
		public int PrincipalID {
			get { return m_group.PrincipalID; }
			set { m_group.PrincipalID = value; }
		}

		[FieldName(ASSTID_FIELD)]
		public int AsstID {
			get { return m_group.AsstID; }
			set { m_group.AsstID = value; }
		}

		[FieldName(CREATOR_FIELD)]
		public string Creator {
			get { return m_group.Creator; }
			set { m_group.Creator = value; }
		}

		[FieldName(CREATION_FIELD)]
		public DateTime Creation {
			get { return m_group.Creation; }
			set { m_group.Creation = value; }
		}

		[FieldName(NAME_FIELD)]
		public string Name {
			get { return m_group.Name; }
			set {m_group.Name = value; }
		}	

		[FieldName(INVITOR_FIELD)]
		public string Invitor {
			get { return m_invitor; }
			set { m_invitor = value; }
		}

		[FieldName(INVITEE_FIELD)]
		public string Invitee {
			get { return m_invitee; }
			set { m_invitee = value; }
		}

		[FieldName(ID_FIELD)]
		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}
		
		public class InvitationList : ArrayList {

			public InvitationList() {
			}

			public new Invitation this[int index] {
				get { return (Invitation) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
