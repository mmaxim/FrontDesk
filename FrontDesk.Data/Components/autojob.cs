using System;
using System.Collections;

namespace FrontDesk.Components.Evaluation {
	
	/// <summary>
	/// Automatic job entity
	/// </summary>
	public class AutoJob : DataComponent {

		public AutoJob() : base() { }
 
		public const String ID_FIELD = "ID";
		public const String NAME_FIELD = "name";
		public const String CREATOR_FIELD = "creator";
		public const String CREATION_FIELD = "creation";
		public const String ASSTID_FIELD = "asstID";
		
		private int m_id, m_asstid;
		private string m_name, m_creator;
		private DateTime m_creation;

		[FieldName(ID_FIELD)]
		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}

		[FieldName(ASSTID_FIELD)]
		public int AsstID {
			get { return m_asstid; }
			set { m_asstid = value; }
		}

		[FieldName(NAME_FIELD)]
		public string JobName {
			get { return m_name; }
			set { m_name = value; }
		}

		[FieldName(CREATOR_FIELD)]
		public string JobCreator {
			get { return m_creator; }
			set { m_creator = value; }
		}

		[FieldName(CREATION_FIELD)]
		public DateTime JobCreation {
			get { return m_creation; }
			set { m_creation = value; }
		}

		public class AutoJobList : ArrayList {
			public AutoJobList() { }

			public new AutoJob this[int index] {
				get { return (AutoJob) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
