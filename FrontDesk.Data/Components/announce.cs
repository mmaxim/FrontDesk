using System;
using System.Collections;

namespace FrontDesk.Components {

	/// <summary>
	/// Announcement entity
	/// </summary>
	public class Announcement : DataComponent {
	
		public Announcement() { }
	
		public const String ID_FIELD = "ID";
		public const String POSTER_FIELD = "poster";
		public const String DESC_FIELD = "content";
		public const String MOD_FIELD = "modified";
		public const String CREATED_FIELD = "creation";
		public const String COURSEID_FIELD = "courseID";
		public const String TITLE_FIELD = "title";

		protected int m_id, m_courseid;
		protected string m_poster, m_desc, m_title;
		protected DateTime m_created, m_modified;

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

		[FieldName(POSTER_FIELD)]
		public string Poster {
			get { return m_poster; }
			set { m_poster = value; }
		}

		[FieldName(DESC_FIELD)]
		public string Description {
			get { return m_desc; }
			set { m_desc = value; }
		}

		[FieldName(TITLE_FIELD)]
		public string Title {
			get { return m_title; }
			set { m_title = value; }
		}		

		[FieldName(MOD_FIELD)]
		public DateTime Modified {
			get { return m_modified; }
			set { m_modified = value; }
		}

		[FieldName(CREATED_FIELD)]
		public DateTime Created {
			get { return m_created; }
			set { m_created = value; }
		}

		public class AnnouncementList : ArrayList {
			public AnnouncementList() { }

			public new Announcement this[int index] {
				get { return (Announcement) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
