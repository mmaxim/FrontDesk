using System;
using System.Collections;

namespace FrontDesk.Components.Evaluation {
	
	/// <summary>
	/// Evaluation entity
	/// </summary>
	public class Evaluation : DataComponent {

		public Evaluation() { }
	
		public const String ID_FIELD = "ID";
		public const String CREATOR_FIELD = "creator";
		public const String TIMELIMIT_FIELD = "timeLimit";
		public const String ASSTID_FIELD = "asstID";
		public const String COURSEID_FIELD = "courseID";
		public const String TYPE_FIELD = "evalType";
		public const String RUNONSUBMIT_FIELD = "runonsubmit";
		public const String COMPETE_FIELD = "competitive";
		public const String NAME_FIELD = "name";
		public const String POINTS_FIELD = "points";
		public const String MANAGER_FIELD = "manager";
		public const String RESTYPE_FIELD = "resTypeName";
		
		public const int NO_MANAGER=-1, JUNIT_MANAGER=0, CHECKSTYLE_MANAGER=1;

		public const String AUTO_TYPE = "auto";
		public const String QUIZ_TYPE = "quiz";
		public const String PLUGIN_TYPE = "plugin";

		private int m_id, m_timelimit, m_asstID, m_courseid, m_manager = NO_MANAGER;
		private string m_type, m_creator, m_name, m_restype;
		private bool m_pretest, m_competetive;
		private double m_points;

		[FieldName(ID_FIELD)]
		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}

		[FieldName(NAME_FIELD)]
		public string Name {
			get { return m_name; }
			set { m_name = value; }
		}

		[FieldName(POINTS_FIELD)]
		public double Points {
			get { return m_points; }
			set { m_points = value; }
		}

		[FieldName(RESTYPE_FIELD)]
		public string ResultType {
			get { return m_restype; }
			set { m_restype = value; }
		}

		[FieldName(CREATOR_FIELD)]
		public string Creator {
			get { return m_creator; }
			set { m_creator = value; }
		}

		[FieldName(TIMELIMIT_FIELD)]
		public int TimeLimit {
			get { return m_timelimit; }
			set { m_timelimit = value; }
		}

		[FieldName(MANAGER_FIELD)]
		public int Manager {
			get { return m_manager; }
			set { m_manager = value; }
		}

		[FieldName(COURSEID_FIELD)]
		public int CourseID {
			get { return m_courseid; }
			set { m_courseid = value; }
		}

		[FieldName(ASSTID_FIELD)]
		public int AsstID {
			get { return m_asstID; }
			set { m_asstID = value; }
		}

		[FieldName(TYPE_FIELD)]
		public string Type {
			get { return m_type; }
			set { m_type = value; }
		}

		[FieldName(RUNONSUBMIT_FIELD)]
		public bool RunOnSubmit {
			get { return m_pretest; }
			set { m_pretest = value; }
		}

		[FieldName(COMPETE_FIELD)]
		public bool Competitive {
			get { return m_competetive; }
			set { m_competetive = value; }
		}

		public class EvaluationList : ArrayList {

			public EvaluationList() { }

			public new Evaluation this[int index] {
				get { return (Evaluation) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
