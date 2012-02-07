using System;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;

namespace FrontDesk.Components {
	/// <summary>
	/// Summary description for component.
	/// </summary>
	public abstract class DataComponent {

		protected class FieldName : Attribute {
			
			private string m_name;

			public FieldName(string name) {
				m_name = name;
			}

			public string Name {
				get { return m_name; }
			}
		}

		protected class JoinedEntity : Attribute {
			public JoinedEntity() { }	
		}

		protected class PropertyEntry {
			public PropertyEntry(PropertyInfo prop, DataComponent h) {
				  Property = prop; Host = h; }

			public PropertyInfo Property;
			public DataComponent Host;
		}

		private Hashtable m_table=new Hashtable();

		public DataComponent() { 
			InitProperties(GetType(), this);	
		}

		private void InitProperties(Type ty, DataComponent oref) {

			//Add class props
			foreach (PropertyInfo prop in ty.GetProperties()) {
				object[] attribs = prop.GetCustomAttributes(typeof(FieldName), false);
				if (attribs.Length > 0)
					m_table.Add(((FieldName) attribs[0]).Name, new PropertyEntry(prop, oref));
			}

			//Add any joined components
			foreach (MemberInfo mem in ty.GetMembers()) {
				if (mem.MemberType == MemberTypes.Field) {
					FieldInfo fmem = mem as FieldInfo;
					if (mem.IsDefined(typeof(JoinedEntity), false)) 
						InitProperties(fmem.FieldType, (DataComponent) fmem.GetValue(oref));
				}
			}
		}

		public object this [string index] {
			get { 
				PropertyEntry prop;
				if (null == (prop = GetProperty(index)))
					throw new Exception("Index out of range");
				else
					return prop.Property.GetValue(prop.Host, null);
			}
			set {
				PropertyEntry prop;
				if (null == (prop = GetProperty(index)))
					throw new Exception("Index out of range");
				else {
					prop.Property.SetValue(prop.Host, value, null);
				}
			}
		}

		private PropertyEntry GetProperty(string index) {
			return (PropertyEntry) m_table[index];
		}

		public bool FieldExists(string name) {
			return m_table.ContainsKey(name);
		}
	}
}
