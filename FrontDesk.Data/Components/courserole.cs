using System;
using System.Collections;

namespace FrontDesk.Components {

	/// <summary>
	/// A course role
	/// </summary>
	public class CourseRole : Principal {
			
		public CourseRole() { }

		public const String ID_FIELD = "principalID";
		public const String NAME_FIELD = "name";
		public const String STAFF_FIELD = "isstaff";
		public const String COURSEID_FIELD = "courseID";

		protected int m_courseid;
		protected bool m_staff;

		[FieldName(COURSEID_FIELD)]
		public int CourseID {
			get { return m_courseid; }
			set { m_courseid = value; }
		}

		[FieldName(STAFF_FIELD)]
		public bool Staff {
			get { return m_staff; }
			set { m_staff = value; }
		}

		public override string ToString() {
			return Name;
		}

		public class CourseRoleList : ArrayList {
			public CourseRoleList() { }

			public new CourseRole this[int index] {
				get { return (CourseRole) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
