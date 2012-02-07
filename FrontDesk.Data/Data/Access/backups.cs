using System;

using FrontDesk.Components;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Backups access component
	/// </summary>
	public class Backups : DataAccessComponent {

		public Backups(AuthorizedIdent ident) : base(ident) { }

		/// <summary>
		/// Return a list of backups
		/// Direct Provider layer call
		/// </summary>
		public Backup.BackupList GetAll() {
			return m_dp.GetBackups(-1);
		}

		/// <summary>
		/// Log a backup
		/// Direct Provider layer call
		/// </summary>
		public bool Create(string backname, string fileloc, int courseID) {
			Backup bak = new Backup();
			
			bak.Creator = m_ident.Name;
			bak.FileLocation = fileloc;
			bak.BackedUp = backname;
			bak.CourseID = courseID;

			return m_dp.CreateBackup(bak);
		}
	}
}
