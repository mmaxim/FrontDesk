using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Security.Principal;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Web.UI.WebControls;
using System.Web.SessionState;

using FrontDesk.Components;
using FrontDesk.Data.Access;
using FrontDesk.Security;

namespace FrontDesk.Common {
	
	/// <summary>
	/// Global useful functionality
	/// </summary>
	public class Globals {
		
		public Globals() { }

		private static Hashtable m_msgs=null;
		private static HttpContext m_context;

		public static string GetMessage(string name) {
			if (m_msgs == null) {
				m_msgs = new Hashtable();
				
				NameValueCollection msgs = (NameValueCollection) 
					ConfigurationSettings.GetConfig("Messages");

				foreach (string msgkey in msgs) {
					string msg = msgs[msgkey];
					m_msgs.Add(msgkey, msg);
				}
			}

			string val = (string) m_msgs[name];
			val = val.Replace(@"\n", "\n");
			return val;
		}

		public static byte[] ReadStream(Stream stream, int size) {
			//Read file
			byte[] data = new byte[size];
			int nb, nbsofar=0, nbtoread = size;
			while (0 < (nb = stream.Read(data, nbsofar, nbtoread-nbsofar)))
				nbsofar += nb;

			return data;
		}

		public static string PurifyZeroes(string str) {
			str.Replace("\0", "\\0");
			return str;
		}

		public static string PurifyString(string str) {
			string junk="!@#$%^&*()-+=|}{[];:?<>; ";
			int i;
			for (i = 0; i < junk.Length; i++)
				str = str.Replace(junk.Substring(i,1), "_");

			return str;
		}

		/// <summary>
		/// Generate a random password of specified length
		/// </summary>
		public static string GeneratePassword(int length) {
			int i;
			string passwd="",c;
			bool madeconsonant = false;
			string doubcons = "bdfglmnpst";
			string cons = "bcdfghklmnpqrstv";
			string vowels = "aeiou";

			Random r = new Random((int)DateTime.Now.Ticks);
			for (i = 0; i < length; i++) {
				double rnd = r.NextDouble();
				if (passwd.Length > 0 && !madeconsonant && rnd < 0.15) {
					int index = Math.Min(doubcons.Length-1, (int)(doubcons.Length*rnd));
					c = doubcons.Substring(index, 1);
					c = c+c;
					i++;
					madeconsonant = true;
				}
				else {
					if (!madeconsonant && rnd < 0.95) {
						int index = Math.Min(cons.Length-1, (int)(cons.Length*rnd));
						c = cons.Substring(index, 1);
						madeconsonant = true;
					}
					else {
						int index = Math.Min(vowels.Length-1, (int)(vowels.Length*rnd));
						c = vowels.Substring(index, 1);
						madeconsonant = false;
					}
				}
				passwd += c;
			}
			return passwd;
		}

		/// <summary>
		/// Return the connection string to connect to the data source
		/// </summary>
		public static string DataConnectionString {
			get { return ConfigurationSettings.AppSettings["connectionString"]; }
		}

		public static bool FSLocal {
			get { 
				return (FileSystemAddress == "localhost" ||
					FileSystemAddress == "127.0.0.1" ||
					FileSystemAddress == CurrentIP);
			}
		}

		public static string DefaultPassword {
			get { return ConfigurationSettings.AppSettings["defaultPassword"]; }
		}

		public static string DatabaseServerAddress {
			get { return ConfigurationSettings.AppSettings["dbaseServer"]; }
		}

		public static string FileSystemAddress {
			get { return ConfigurationSettings.AppSettings["FileSystemAddress"]; }
		}

		public static string MailServerAddress {
			get { return ConfigurationSettings.AppSettings["mailServer"]; }
		}

		/// <summary>
		/// The default url of the FrontDesk app
		/// </summary>
		public static string DefaultUrl {
			get { 
				string vroot = ConfigurationSettings.AppSettings["VRoot"];
				return vroot + ConfigurationSettings.AppSettings["defaultUrl"]; 
			}
		}

		public static AuthorizedIdent CurrentIdentity {
			get { 
				return AuthorizedIdent.Create();
			}
		}

		public static HttpContext Context { 
			get { 
				if (HttpContext.Current == null) 
					return m_context; 
				else
					return HttpContext.Current;
			}
			set { m_context = value; }
		}

		public static string CurrentIP {
			get { return new IPAddress(
					  Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address).ToString(); }
		}

		public static int MinPasswordLength {
			get { return Convert.ToInt32(ConfigurationSettings.AppSettings["minPasswordLength"]); }
		}

		public static string CurrentUserName {
			get {
				return Context.User.Identity.Name; }
		}

		public static bool DatabaseLocal {
			get { return (DatabaseServerAddress == "localhost"); }
		}

		public static string TempDirectory {
			get { return Path.Combine(Context.Request.PhysicalApplicationPath,
					  "temp"); }
		}	

		public static string BackupDirectory {
			get { return Path.Combine(Context.Request.PhysicalApplicationPath,
					  ConfigurationSettings.AppSettings["backupLocation"]); }
		}

		public static string WWWDirectory {
			get { return Context.Request.PhysicalApplicationPath; }
		}

		public static string WebServer {
			get { return ConfigurationSettings.AppSettings["webServer"]; }
		}

		public static string BackupDirectoryName {
			get { return ConfigurationSettings.AppSettings["backupLocation"]; }
		}

		public static string EmailAddress {
			get { return ConfigurationSettings.AppSettings["emailAddress"]; }
		}

		public static string VerifyPageAddress {
			get { return "http://" + WebServer + ConfigurationSettings.AppSettings["VRoot"] + "verifyemail.aspx"; }
		}
	}
}
