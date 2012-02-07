using System;

namespace FrontDesk.Testing.Zones {

	/// <summary>
	/// A testing zone
	/// </summary>
	public class Zone {

		public Zone() {	}

		private string m_dirloc;
		private int m_id;

		public string LocalPath {
			get { return m_dirloc; }
			set { m_dirloc = value; }
		}

		public int ZoneID {
			get { return m_id; }
			set { m_id = value; }
		}
	}
}
