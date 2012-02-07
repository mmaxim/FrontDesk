using System;
using System.Collections;

namespace FrontDesk.Components {
	/// <summary>
	/// Summary description for Setting.
	/// </summary>
	public class Setting : DataComponent {

		public const string ID_FIELD = "ID";
		public const string SETTINGNAME_FIELD = "Setting";
		public const string CATEGORY_FIELD = "Category";
		public const string TYPE_FIELD = "SettingType";
		public const string VALUE_FIELD = "SettingValue";

		public Setting(){}

		[FieldName(ID_FIELD)]
		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}


		[FieldName(SETTINGNAME_FIELD)]
		public string Name {
			get { return m_settingName; }
			set { m_settingName = value; }
		}

		[FieldName(CATEGORY_FIELD)]
		public string CategoryName {
			get { return m_settingCategory; }
			set { m_settingCategory = value; }
		}

		[FieldName(TYPE_FIELD)]
		public string Type {
			get { return m_settingType; }
			set { m_settingType = value; }
		}

		[FieldName(VALUE_FIELD)]
		public string Value {
			get { return m_value; }
			set { m_value = value; }
		}

		public class SettingList : ArrayList {

			public SettingList() {
			}

			public new Setting this[int index] {
				get { return (Setting) base[index]; }
				set { base[index] = value; }
			}
		}
		/// <summary>
		///  Since category is only used by setting its an inner class
		/// </summary>
		public class Category : DataComponent {

			public const String ID_FIELD = "ID";
			public const String NAME_FIELD = "name";

			public Category() { }

			[FieldName(ID_FIELD)]
			public int ID {
				get { return m_id; }
				set { m_id = value;}
			}

			[FieldName(NAME_FIELD)]
			public string Name {
				get { return m_name; }
				set { m_name = value;}
			}

			public class CategoryList : ArrayList {
				public new Setting.Category this[int index] {
					get { return (Setting.Category) base[index]; }
					set { base[index] = value; }
				}
			}

			protected int m_id;
			protected string m_name;
		}

		protected int m_id;
		protected string m_settingName, m_settingCategory, m_settingType;
		protected string m_value;
		protected bool m_activator;
	}
}
