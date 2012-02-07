using System;
using System.Collections;

namespace FrontDesk.Components {
	
	/// <summary>
	/// A section entity
	/// </summary>
	public class Section : DataComponent {
		
		public Section() { }

		public const String ID_FIELD = "ID";
		public const String COURSEID_FIELD = "courseID";
		public const String NAME_FIELD = "name";
		public const String OWNER_FIELD = "owner";

		protected int m_id, m_courseid;
		protected string m_name, m_owner;

		public override bool Equals(object obj) {
			Section sobj = obj as Section;
			
			if (sobj.ID == ID) return false; 
			return (sobj.CourseID == CourseID && sobj.Name == Name);
		}

		public override int GetHashCode() {
			return Name.GetHashCode();
		}

		[FieldName(ID_FIELD)]
		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}

		[FieldName(COURSEID_FIELD)]
		public int CourseID {
			get { return m_courseid; }
			set { m_courseid = value; }
		}

		[FieldName(NAME_FIELD)]
		public string Name {
			get { return m_name; }
			set { m_name = value; }
		}

		[FieldName(OWNER_FIELD)]
		public string Owner {
			get { return m_owner; }
			set { m_owner = value; }
		}

		public class SectionProgress {
			public int TotalStudents, TotalSubmissions, TotalGraded; 
		}

		public class SectionList : ArrayList {
			public SectionList() {
			}

			public new Section this[int index] {
				get { return (Section) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
