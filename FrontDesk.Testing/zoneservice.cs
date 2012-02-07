using System;
using System.Data;
using System.IO;

using FrontDesk.Data.Access;
using FrontDesk.Components;
using FrontDesk.Data.Filesys;
using FrontDesk.Components.Filesys;
using FrontDesk.Testing.Logging;
using FrontDesk.Security;

namespace FrontDesk.Testing.Zones {

	/// <summary>
	/// Zone services
	/// </summary>
	public class ZoneService {
		
		public ZoneService(string prefix, AuthorizedIdent ident, TestLogger logger) {
			m_prefix = prefix;
			m_ident = ident;
			m_logger = logger;
		}

		protected string m_prefix;
		protected const string ZONE_FILE = "__zone.xml";

		protected AuthorizedIdent m_ident;
		protected TestLogger m_logger;

		/// <summary>
		/// Synchronize local zone with remote store
		/// </summary>
		public Zone Synchronize(IZoneComponent zcomp) {
			
			bool success;
			Zone zone = new Zone();

			//Check for the existence of the zone
			zone.LocalPath = Path.Combine(TestConfig.LocalZonePath, 
				m_prefix+zcomp.GetZoneID().ToString());
			zone.ZoneID = zcomp.GetZoneID();

			if (Directory.Exists(zone.LocalPath))
				success = UpdateZone(zcomp);
			else
				success = CreateZone(zcomp);

			if (success) {
				return zone;
			}
			else {
				m_logger.Log("Zone synchro FAILED", TestLogger.LogType.ERROR);
				return null;
			}
		}

		public void CopyZone(Zone dest, Zone src) {
			CopyZone(dest, src, "");
		}

		public void CopyZone(Zone dest, Zone src, string bdir) {
			
			string dpath = dest.LocalPath, spath = src.LocalPath;

			//Copy files
			string[] files = Directory.GetFiles(Path.Combine(src.LocalPath, bdir));
			foreach (string file in files) 
				if (Path.GetFileName(file) != ZONE_FILE)
					File.Copy(file, 
						Path.Combine(dest.LocalPath, 
						Path.Combine(bdir, Path.GetFileName(file))), true);
			
			//Copy dirs
			string[] dirs = Directory.GetDirectories(Path.Combine(src.LocalPath, bdir));
			foreach (string dir in dirs) {
				string ddirpath = 
					Path.Combine(dest.LocalPath, Path.Combine(bdir, Path.GetFileName(dir)));
				if (!Directory.Exists(ddirpath))
					Directory.CreateDirectory(ddirpath);

				CopyZone(dest, src, Path.Combine(bdir, Path.GetFileName(dir)));
			}
		}

		protected bool ClearZone(string pathname) {
			
			DataSet zonedesc = new DataSet();
			string xmlpath = Path.Combine(pathname, ZONE_FILE);

			//Load zone info
			zonedesc.ReadXml(xmlpath);

			//Take extra files
			DoClear(zonedesc, pathname, pathname, "");

			return true;
		}

		protected void DoClear(DataSet zonefiles, string zpath, string path, string relpath) {
			
			//Kill all files not in zone
			string[] files = Directory.GetFiles(path);
			DataTable exptab = zonefiles.Tables["File"];
			foreach (string file in files) {
				string fpath = Path.GetFileName(file);
				fpath = Path.Combine(relpath, fpath);
				if (fpath.IndexOf(ZONE_FILE) < 0 && (exptab == null || (exptab != null &&
					exptab.Select("path = '" + fpath + "'").Length == 0)))
					File.Delete(Path.Combine(zpath, fpath));
			}

			//Kill all dirs recursively
			string[] dirs = Directory.GetDirectories(path);
			foreach (string dir in dirs) {
				string dname = Path.GetFileName(dir);
				string dpath = Path.Combine(relpath, dname);
				if (exptab == null || exptab.Select("path = '" + dpath + "'").Length == 0)
					Directory.Delete(Path.Combine(zpath, dpath), true);
				else
					DoClear(zonefiles, zpath, Path.Combine(path, dname), dpath);
			}
		}

		protected bool UpdateZone(IZoneComponent eval) {
			
			DataSet zonedesc = new DataSet();

			string dpath = Path.Combine(TestConfig.LocalZonePath, 
				m_prefix + eval.GetZoneID().ToString());
			string xmlpath = Path.Combine(dpath, ZONE_FILE);

			if (!File.Exists(xmlpath) || true) {
				Directory.Delete(dpath, true);
				return CreateZone(eval);
			}
			else {
				zonedesc.ReadXml(xmlpath);
				DateTime localmod = new DateTime(Convert.ToInt64(zonedesc.Tables["Export"].Rows[0]["Mod"]));
				DateTime zonemod = eval.GetZoneModified();

				//If zone is modified, blow it away and get it again
				if (zonemod > localmod) {
					Directory.Delete(dpath, true);
					return CreateZone(eval);
				}
				else 
					ClearZone(dpath);
			}

			return true;
		}

		protected bool CreateZone(IZoneComponent eval) {
		
			FileSystem fs = new FileSystem(m_ident);
			DataSet desc = new DataSet();

			//Create initial zone directory
			string zpath = Path.Combine(TestConfig.LocalZonePath, 
				m_prefix + eval.GetZoneID().ToString());
			Directory.CreateDirectory(zpath);

			//Export the zone files into local store
			IExternalSink zdir = new OSFileSystemSink();
			
			zdir.CreateSink("");	
			try {
				desc = fs.ExportData(zpath, fs.GetFile(eval.GetZoneID()), zdir, false);
				
				//Write XML descriptor
				desc.Tables["Export"].Rows[0]["Mod"] = eval.GetZoneModified().Ticks;
				desc.WriteXml(Path.Combine(zpath, ZONE_FILE));
				m_logger.Log("Zone retrieved successfully");

			} catch (FileOperationException e) {
				m_logger.Log("File error: " + e.Message, TestLogger.LogType.ERROR);
				zdir.CloseSink();
				return false;
			} catch (DataAccessException er) {
				m_logger.Log("Data error: " + er.Message, TestLogger.LogType.ERROR);
				zdir.CloseSink();
				return false;
			} catch (Exception e) {
				m_logger.Log("Unexpected error: " + e.Message, TestLogger.LogType.ERROR);
				m_logger.Log("Trace: " + e.StackTrace, TestLogger.LogType.ERROR);
				zdir.CloseSink();
				return false;
			}

			return true;
		}
	}
}
