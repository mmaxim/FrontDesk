using System;
using System.Data;

namespace FrontDesk.Components.Filesys {
	
	/// <summary>
	/// A File Lock
	/// </summary>
	public class CFileLock {
		public CFileLock() {
		}

		protected int m_fileid, m_id, m_lockparent;
		protected string m_username;
		protected DateTime m_creation;

		public int FileID {
			get { return m_fileid; }
			set { m_fileid = value; }
		}

		public string UserName {
			get { return m_username; }
			set { m_username = value; }
		}

		public DateTime Creation {
			get { return m_creation; }
			set { m_creation = value; }
		}

		public int LockParent {
			get { return m_lockparent; }
			set { m_lockparent = value; }
		}

		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}
	}
}