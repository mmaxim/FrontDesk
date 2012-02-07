using System;
using System.Collections;

namespace FrontDesk.Components.Evaluation {

	/// <summary>
	/// Rubric entry
	/// </summary>
	[Serializable()]
	public class Rubric : DataComponent {
		
		public Rubric() : base() { }
		
		public const String ID_FIELD = "ID";
		public const String PARENTID_FIELD = "parentID";
		public const String ASSTID_FIELD = "asstID";
		public const String POINTS_FIELD = "points";
		public const String DESC_FIELD = "longdesc";
		public const String NAME_FIELD = "shortdesc";
		public const String EVALID_FIELD = "evalID";
		public const String ALLOWNEG_FIELD = "allowneg";

		public const int ERROR=0, WARNING=1, GOOD=2;

		protected int m_id, m_parentid, m_asstid, m_evalid=-1;
		protected string m_desc, m_name;
		protected double m_points;
		protected bool m_allowneg;

		[FieldName(ID_FIELD)]
		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}

		[FieldName(PARENTID_FIELD)]
		public int ParentID {
			get { return m_parentid; }
			set { m_parentid = value; }
		}

		[FieldName(EVALID_FIELD)]
		public int EvalID {
			get { return m_evalid; }
			set { m_evalid = value; }
		}

		[FieldName(ASSTID_FIELD)]
		public int AsstID {
			get { return m_asstid; }
			set { m_asstid = value; }
		}

		[FieldName(POINTS_FIELD)]
		public double Points {
			get { return m_points; }
			set { m_points = value; }
		}

		[FieldName(ALLOWNEG_FIELD)]
		public bool AllowNegativePoints {
			get { return m_allowneg; }
			set { m_allowneg = value; }
		}

		[FieldName(NAME_FIELD)]
		public string Name {
			get { return m_name; }
			set { m_name = value; }
		}

		[FieldName(DESC_FIELD)]
		public string Description {
			get { return m_desc; }
			set { m_desc = value; }
		}

		public class RubricList : ArrayList {

			public RubricList() { }

			public new Rubric this[int index] {
				get { return (Rubric) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
