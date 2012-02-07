using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Configuration;
using System.Web;

namespace FrontDesk.Common {

	/// <summary>
	/// String abstract factory
	/// </summary>
	public abstract class StringAbstractFactory : IAbstractFactory {

		public StringAbstractFactory() { }

		protected Hashtable m_factmap = new Hashtable();

		protected StringAbstractFactory(string configsection) { 
			
			//Build table of constructors for invokation
			NameValueCollection subsyss = (NameValueCollection) 
				ConfigurationSettings.GetConfig(configsection);
			
			Type strtype = typeof(String);
			Type[] tcparams = new Type[1];
			tcparams[0] = strtype;

			foreach (string key in subsyss.AllKeys) {
				string desc = subsyss[key];
				m_factmap.Add(key, desc); 
			}
		}

		public object CreateItem(string subsys) {
			string ent;

			if (null == (ent = (string) m_factmap[subsys]))
				throw new Exception("Invalid Factory selection! Check configuration");

			return ent;			
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
