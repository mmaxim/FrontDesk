using System;
using System.Collections;

namespace FrontDesk.Components {

	/// <summary>
	/// Activity log component
	/// </summary>
	public class Activity : DataComponent {

		public Activity() : base() { }

		public const String USERNAME_FIELD = "username";
		public const String ACTTIME_FIELD = "acttime";
		public const String DESC_FIELD = "description";
		public const String TYPE_FIELD = "type";
		public const String OBJID_FIELD = "objID";

		public const int SUBMISSION=0, ASSIGNMENT=1, COURSE=2;

		protected int m_type, m_objid;
		protected string m_desc, m_username;
		protected DateTime m_time;

		[FieldName(USERNAME_FIELD)]
		public string Username {
			get { return m_username; }
			set { m_username = value; }
		}

		[FieldName(OBJID_FIELD)]
		public int ObjectID {
			get { return m_objid; }
			set { m_objid = value; }
		}

		[FieldName(TYPE_FIELD)]
		public int Type {
			get { return m_type; }
			set { m_type = value; }
		}

		[FieldName(ACTTIME_FIELD)]
		public DateTime Time {
			get { return m_time; }
			set { m_time = value; }
		}

		[FieldName(DESC_FIELD)]
		public string Description {
			get { return m_desc; }
			set { m_desc = value; }
		}

		public class ActivityList : ArrayList {
			public ActivityList() { }

			public new Activity this[int index] {
				get { return (Activity) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
