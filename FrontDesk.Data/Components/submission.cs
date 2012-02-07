using System;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime;
using System.Collections;

namespace FrontDesk.Components {

	public class Submission : DataComponent, IZoneComponent {

		public Submission() : base() { }

		public const String PRINCIPALID_FIELD = "principalID";
		public const String LOCATIONID_FIELD = "directoryID";
		public const String ASSTID_FIELD = "asstID";
		public const String ID_FIELD = "ID";
		public const String SUBTIME_FIELD = "subTime";
		public const String SUBMITTER_FIELD = "submitter";
		public const String STATUS_FIELD = "status";
		public const String SUBNAME_FIELD = "subname";

		public const int UNGRADED = 0, INPROGRESS = 1, GRADED = 2, DEFUNCT = 3;

		protected int m_principalID, m_locationID = -1, m_asstID, m_courseID, m_id;
		protected int m_status = UNGRADED;
		protected string m_submitter, m_subname="unknown";
		protected DateTime m_subtime;

		public int GetZoneID() {
			return m_locationID;
		}

		public DateTime GetZoneModified() {
			return m_subtime;
		}

		[FieldName(PRINCIPALID_FIELD)]
		public int PrincipalID {
			get { return m_principalID; }
			set { m_principalID = value; }
		}

		[FieldName(LOCATIONID_FIELD)]
		public int LocationID {
			get { return m_locationID; }
			set { m_locationID = value; }
		}

		[FieldName(ASSTID_FIELD)]
		public int AsstID {
			get { return m_asstID; }
			set { m_asstID = value; }
		}

		[FieldName(STATUS_FIELD)]
		public int Status {
			get { return m_status; }
			set { m_status = value; }
		}

		[FieldName(ID_FIELD)]
		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}

		[FieldName(SUBTIME_FIELD)]
		public DateTime Creation {
			get { return m_subtime; }
			set { m_subtime = value; }
		}

		[FieldName(SUBMITTER_FIELD)]
		public string Submitter {
			get { return m_submitter; }
			set { m_submitter = value; }
		}

		[FieldName(SUBNAME_FIELD)]
		public string Name {
			get { return m_subname; }
			set { m_subname = value; }
		}


		public class SubmissionList : ArrayList {

			public SubmissionList() { }

			public new Submission this[int index] {
				get { return (Submission) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}