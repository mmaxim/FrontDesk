using System;
using System.Data;
using System.Security.Principal;
using System.IO;

using FrontDesk.Data.Access;
using FrontDesk.Components;
using FrontDesk.Data.Filesys;
using FrontDesk.Components.Filesys;
using FrontDesk.Testing.Logging;

namespace FrontDesk.Testing.Zones {

	/// <summary>
	/// Test zone service
	/// </summary>
	public class TestZoneService : IZoneService {

		public TestZoneService(IIdentity ident, TestLogger logger) {
			m_ident = ident; m_logger = logger;
		}

		protected IIdentity m_ident;
		protected TestLogger m_logger;

		/// <summary>
		/// Synchronize local zone with remote store
		/// </summary>
		public Zone Synchronize(object oeval) {
			
			bool success;
			Zone zone = new Zone();
			AutoEvaluation eval = oeval as AutoEvaluation;

			//Check for the existence of the zone
			zone.LocalPath = Path.Combine(TestConfig.LocalZonePath, eval.ZoneID.ToString());
			zone.ZoneID = eval.ZoneID;

			if (Directory.Exists(zone.LocalPath))
				success = UpdateZone(eval);
			else
				success = CreateZone(eval);

			if (success) {
				m_logger.Log("Zone synchro complete");
				return zone;
			}
			else {
				m_logger.Log("Zone synchro FAILED", TestLogger.LogType.ERROR);
				return null;
			}
		}

		protected bool UpdateZone(AutoEvaluation eval) {
			
			DataSet zonedesc = new DataSet();

			string dpath = Path.Combine(TestConfig.LocalZonePath, eval.ZoneID.ToString());
			string xmlpath = Path.Combine(dpath, "__zone.xml");

			if (!File.Exists(xmlpath)) {
				Directory.Delete(dpath, true);
				return CreateZone(eval);
			}
			else {
				m_logger.Log("Checking for update on zone: " + eval.ZoneID);
				zonedesc.ReadXml(xmlpath);
				DateTime localmod = Convert.ToDateTime(zonedesc.Tables["Export"].Rows[0]["Mod"]);
				DateTime zonemod = eval.ZoneModified;

				//If zone is modified, blow it away and get it again
				if (zonemod > localmod) {
					m_logger.Log("Update needed for zone: " + eval.ZoneID);
					Directory.Delete(dpath, true);
					return CreateZone(eval);
				}
			}

			return true;
		}

		protected bool CreateZone(AutoEvaluation eval) {
		
			FileSystem fs = FileSystem.GetInstance();
			DataSet desc = new DataSet();

			//Create initial zone directory
			string zpath = Path.Combine(TestConfig.LocalZonePath, eval.ZoneID.ToString());
			Directory.CreateDirectory(zpath);

			//Export the zone files into local store
			IExternalSink zdir = new OSFileSystemSink();

			m_logger.Log("Retrieving zone files for zone: " + eval.ZoneID);
			
			zdir.CreateSink("");	
			try {
				desc = fs.ExportData(zpath, fs.GetFile(eval.ZoneID), zdir, false);
				
				//Write XML descriptor
				desc.Tables["Export"].Rows[0]["Mod"] = eval.ZoneModified.ToString();
				desc.WriteXml(Path.Combine(zpath, "__zone.xml"));
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
