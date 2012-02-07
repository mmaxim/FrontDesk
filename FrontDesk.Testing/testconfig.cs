using System;
using System.Configuration;

namespace FrontDesk.Testing {

	/// <summary>
	/// Access the configuration file
	/// </summary>
	public class TestConfig {

		/// <summary>
		/// Return the connection string to connect to the data source
		/// </summary>
		public static string DataConnectionString {
			get { return ConfigurationSettings.AppSettings["connectionString"]; }
		}

		public static string DatabaseServerAddress {
			get { return ConfigurationSettings.AppSettings["dbaseServer"]; }
		}

		public static bool DatabaseLocal {
			get { return (DatabaseServerAddress == "localhost"); }
		}

		public static string FileSystemAddress {
			get { return ConfigurationSettings.AppSettings["FileSystemAddress"]; } 
		}

		public static string LocalZonePath {
			get { return "zones"; }
		}

		public static string AuthenticationAddress {
			get { return ConfigurationSettings.AppSettings["AuthenticationAddress"]; } 
		}
	}
}
