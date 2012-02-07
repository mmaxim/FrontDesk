using System;
using System.Collections;
using System.IO;

using FrontDesk.Common;
using FrontDesk.Data.Filesys;
using FrontDesk.Components.Filesys;

namespace FrontDesk.Components.Evaluation {
 
	/// <summary>
	/// Code result entity
	/// </summary>
	public class SubjResult : Result {

		public SubjResult() : base() { }

		public const String FILEID_FIELD = "fileID";
		public const String LINE_FIELD = "line";
		public const String SUBJTYPE_FIELD = "subjType";
		public const String COMMENT_FIELD = "comment";

		protected int m_fileid=-1, m_line=-1;
		protected int m_subjtype;
		protected string m_comment;
		protected ArrayList m_lines = new ArrayList();

		[FieldName(FILEID_FIELD)]
		public int FileID {
			get { return m_fileid; }
			set { m_fileid = value; }
		}

		[FieldName(LINE_FIELD)]
		public int Line {
			get { return m_line; }
			set { m_line = value; }
		}

		[FieldName(SUBJTYPE_FIELD)]
		public int SubjType {
			get { return m_subjtype; }
			set { m_subjtype = value; }
		}

		[FieldName(COMMENT_FIELD)]
		public string Comment {
			get { return m_comment; }
			set { m_comment = value; }
		}

		public ArrayList LinesAffected {
			get { return m_lines; }
		}

		public class SubjResultList : ArrayList {
			public SubjResultList() { }

			public new SubjResult this[int index] {
				get { return (SubjResult) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
