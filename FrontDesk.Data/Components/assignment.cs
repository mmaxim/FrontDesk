//Mike Maxim
using System;
using System.Collections;

namespace FrontDesk.Components {

	public class Assignment : DataComponent {

		public Assignment() : base() { }

		public const String ID_FIELD = "ID";
		public const String COURSEID_FIELD = "courseID";
		public const String DESCRIPTION_FIELD = "description";
		public const String CREATOR_FIELD = "creator";
		public const String DUE_FIELD = "dueDate";
		public const String CONTENTID_FIELD = "contentID";
		public const String STURELEASE_FIELD = "sturelease";
		public const String RESRELEASE_FIELD = "resrelease";
		public const String FORMAT_FIELD = "format";
	
		//initialize the submission format with just a "."
		public static string DEFAULT_FORMAT = "<Root><File><type>dir</type><name>.</name></File></Root>";

		protected int m_id, m_courseid, m_contentid;
		protected string m_description, m_creator, m_format="";
		protected bool m_sturelease, m_resrelease;
		protected DateTime m_releaseDate, m_dueDate;

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

		[FieldName(CONTENTID_FIELD)]
		public int ContentID {
			get { return m_contentid; }
			set { m_contentid = value; }
		}

		[FieldName(STURELEASE_FIELD)]
		public bool StudentRelease {
			get { return m_sturelease; }
			set { m_sturelease = value; }
		}

		[FieldName(RESRELEASE_FIELD)]
		public bool StudentSubmit {
			get { return m_resrelease; }
			set { m_resrelease = value; }
		}

		[FieldName(DESCRIPTION_FIELD)]
		public string Description {
			get { return m_description; }
			set { m_description = value; }
		}

		[FieldName(CREATOR_FIELD)]
		public string Creator {
			get { return m_creator; }
			set { m_creator = value; }
		}

		[FieldName(FORMAT_FIELD)]
		public string Format {
			get { return m_format; }
			set { m_format = value; }
		}

		[FieldName(DUE_FIELD)]
		public DateTime DueDate {
			get { return m_dueDate; }
			set { m_dueDate = value; }
		}

		public class AssignmentList : ArrayList {
			public AssignmentList() {
			}

			public new Assignment this[int index] {
				get { return (Assignment) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}