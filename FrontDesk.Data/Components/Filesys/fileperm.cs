using System;
using System.Collections;

using FrontDesk.Data.Filesys;

namespace FrontDesk.Components.Filesys {

	public class CFilePermission {
		
		public CFilePermission(int iprin, FileAction iaction, bool igrant) {
			PrincipalID=iprin; Action=iaction; Grant=igrant; 
		}

		public int PrincipalID;
		public FileAction Action;
		public bool Grant;

		public static FilePermissionList CreateFullAccess(int principalID) {
			FilePermissionList full = new FilePermissionList();
			full.Add(new CFilePermission(principalID, FileAction.READ, true));
			full.Add(new CFilePermission(principalID, FileAction.WRITE, true));
			full.Add(new CFilePermission(principalID, FileAction.DELETE, true));
			full.Add(new CFilePermission(principalID, FileAction.ADMIN, true));
			
			return full;
		}

		public static FilePermissionList CreateOprFullAccess(int principalID) {
			FilePermissionList full = new FilePermissionList();
			full.Add(new CFilePermission(principalID, FileAction.READ, true));
			full.Add(new CFilePermission(principalID, FileAction.WRITE, true));
			full.Add(new CFilePermission(principalID, FileAction.DELETE, true));
			
			return full;
		}

		public class FilePermissionList : ArrayList {
			public FilePermissionList() { }

			public new CFilePermission this[int index] {
				get { return (CFilePermission) base[index]; }
				set { base[index] = value; }
			}
		}
	}

}