using System;
using System.Collections;

namespace FrontDesk.Components.Evaluation {

	/// <summary>
	/// Canned response entity
	/// </summary>
	public class CannedResponse : DataComponent {

		public CannedResponse() : base() { }

		public const String ID_FIELD = "ID";
		public const String RUBRICID_FIELD = "rubricID";
		public const String POINTS_FIELD = "points";
		public const String COMMENT_FIELD = "comment";
		public const String TYPE_FIELD = "type";

		protected int m_id, m_rubricid, m_type;
		protected string m_comment;
		protected double m_points;

		[FieldName(ID_FIELD)]
		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}

		[FieldName(RUBRICID_FIELD)]
		public int RubricID {
			get { return m_rubricid; }
			set { m_rubricid = value; }
		}

		[FieldName(POINTS_FIELD)]
		public double Points {
			get { return m_points; }
			set { m_points = value; }
		}

		[FieldName(COMMENT_FIELD)]
		public string Comment {
			get { return m_comment; }
			set { m_comment = value; }
		}

		[FieldName(TYPE_FIELD)]
		public int Type {
			get { return m_type; }
			set { m_type = value; }
		}

		public class CannedResponseList : ArrayList {
			public CannedResponseList() { }

			public new CannedResponse this[int index] {
				get { return (CannedResponse) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
