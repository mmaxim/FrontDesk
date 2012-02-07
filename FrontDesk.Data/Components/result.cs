using System;
using System.Collections;
using System.Data;
using System.IO;

namespace FrontDesk.Components.Evaluation {

	/// <summary>
	/// A result entity
	/// </summary>
	[Serializable]
	public class Result : DataComponent {

		public Result() { }

		public const String ID_FIELD = "ID";
		public const String RUBRICID_FIELD = "rubricID";
		public const String SUBID_FIELD = "subID";
		public const String GRADER_FIELD = "grader";
		public const String TYPE_FIELD = "resType";
		public const String POINTS_FIELD = "points";

		//Result types
		public const string AUTO_TYPE="auto", SUBJ_TYPE="subj", QUIZ_TYPE="quiz";

		protected string m_grader;
		protected int m_id, m_rubricid, m_subid;
		protected double m_points;
		protected string m_type;

		[FieldName(ID_FIELD)]
		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}

		[FieldName(POINTS_FIELD)]
		public double Points {
			get { return m_points; }
			set { m_points = value; }
		}

		[FieldName(TYPE_FIELD)]
		public string Type {
			get { return m_type; }
			set { m_type = value; }
		}

		[FieldName(RUBRICID_FIELD)]
		public int RubricID {
			get { return m_rubricid; }
			set { m_rubricid = value; }
		}

		[FieldName(SUBID_FIELD)]
		public int SubmissionID {
			get { return m_subid; }
			set { m_subid = value; }
		}

		[FieldName(GRADER_FIELD)]
		public string Grader {
			get { return m_grader; }
			set { m_grader = value; }
		}

		public class ResultList : ArrayList {

			public ResultList() { }

			public new Result this[int index] {
				get { return (Result) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
