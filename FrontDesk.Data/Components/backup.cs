using System;
using System.Collections;

namespace FrontDesk.Components {

	/// <summary>
	/// Backup entity
	/// </summary>
	public class Backup : DataComponent {

		public Backup() : base() { }

		public const String ID_FIELD = "ID";
		public const String CREATOR_FIELD = "creator";
		public const String DATAFILE_FIELD = "fileLocation";
		public const String CREATION_FIELD = "creation";
		public const String BACKEDUP_FIELD = "backedup";
		public const String COURSEID_FIELD = "courseID";

		private int m_id, m_courseID;
		private string m_backedup, m_creator, m_fileloc;
		private DateTime m_creation;

		[FieldName(ID_FIELD)]
		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}

		[FieldName(COURSEID_FIELD)]
		public int CourseID {
			get { return m_courseID; }
			set { m_courseID = value; }
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

		[FieldName(BACKEDUP_FIELD)]
		public string BackedUp {
			get { return m_backedup; }
			set { m_backedup = value; }
		}

		[FieldName(DATAFILE_FIELD)]
		public string FileLocation {
			get { return m_fileloc; }
			set { m_fileloc = value; }
		}

		public class BackupList : ArrayList {
			public BackupList() { }

			public new Backup this[int index] {
				get { return (Backup) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
