using System;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime;
using System.Collections;

namespace FrontDesk.Components {

	public class Course : DataComponent {
		
		public Course() : base() { }

		public const String ID_FIELD = "ID";
		public const String NAME_FIELD = "courseName";
		public const String NUMBER_FIELD = "courseNumber";
		public const String CONTENTID_FIELD = "contentID";
		public const String AVAIL_FIELD = "available";

		protected int m_id, m_contentid;
		protected string m_name, m_number;
		protected bool m_avail;

		[FieldName(ID_FIELD)]
		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}

		[FieldName(AVAIL_FIELD)]
		public bool Available {
			get { return m_avail; }
			set { m_avail = value; }
		}

		[FieldName(CONTENTID_FIELD)]
		public int ContentID {
			get { return m_contentid; }
			set { m_contentid = value; }
		}

		[FieldName(NAME_FIELD)]
		public string Name {
			get { return m_name; }
			set { m_name = value; }
		}

		[FieldName(NUMBER_FIELD)]
		public string Number {
			get { return m_number; }
			set { m_number = value; }
		}

		public class CourseList : ArrayList {

			public CourseList() {
			}

			public new Course this[int index] {
				get { return (Course) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}