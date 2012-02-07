using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Configuration;
using System.Web;
using System.IO;

namespace FrontDesk.Common {
	
	/// <summary>
	/// Summary description for abstractfactory.
	/// </summary>
	public abstract class ClassAbstractFactory : IAbstractFactory {

		protected Hashtable m_factmap = new Hashtable();

		protected class MapEntry {

			public MapEntry(ConstructorInfo con, string[] conparams) {
				Constructor = con; Params = conparams;
			}
		
			public ConstructorInfo Constructor;
			public string[] Params;
		};

		protected ClassAbstractFactory(string configsection) { 
			
			//Build table of constructors for invokation
			NameValueCollection subsyss = (NameValueCollection) 
				ConfigurationSettings.GetConfig(configsection);

			foreach (string key in subsyss.AllKeys) {

				string desc = subsyss[key];
				string[] tokens = desc.Split(';');	
				string assembly = tokens[0];
				string sclass = tokens[1];
				string[] conparams=null;
				if (tokens.Length > 2) {
					conparams = new string[tokens.Length-2];
					for (int i = 0; i < conparams.Length; i++) 
						conparams[i] = tokens[i+2];
				}

				string assemblyPath;
				if (Globals.Context != null) 
					assemblyPath = Globals.Context.Server.MapPath(Globals.Context.Request.ApplicationPath + "/bin/" + assembly);					
				else
					assemblyPath = assembly;

				// Uuse reflection to store the constructor of the class that implements IWebForumDataProvider
				try {
					Assembly asm = Assembly.LoadFrom(assemblyPath);
					ConstructorInfo con;

					if (conparams == null)
						con = asm.GetType(sclass).GetConstructor(new Type[0]);
					else {
						Type strtype = typeof(String);
						Type[] tcparams = new Type[conparams.Length];
						for (int i = 0; i < conparams.Length; i++)
							tcparams[i] = strtype;
						con = asm.GetType(sclass).GetConstructor(tcparams);
					}

					m_factmap.Add(key, new MapEntry(con, conparams)); 
				}
				catch (Exception) {
					// could not locate DLL file
					// Do nothing
				}
			}
		}

		public object CreateItem(string subsys) {
			MapEntry ent;

			if (null == (ent = (MapEntry) m_factmap[subsys]))
				throw new Exception("Invalid Factory selection! Check configuration");

			return ent.Constructor.Invoke(ent.Params);			
		}

		public string[] ListKeys() {
			ICollection keys = m_factmap.Keys;
			string[] skeys = new string[keys.Count];
			
			int i=0;
			foreach (string key in keys) skeys[i++] = key;
				
			return skeys;
		}
	}
}
