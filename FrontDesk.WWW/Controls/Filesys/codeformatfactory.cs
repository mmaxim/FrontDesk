using System;

using FrontDesk.Common;

namespace FrontDesk.Controls.Filesys {
	
	/// <summary>
	/// Code formatter factory
	/// </summary>
	public class CodeFormatterFactory : ClassAbstractFactory {

		public CodeFormatterFactory() : base("CodeFormatters") { }

		private static CodeFormatterFactory m_instance;

		public static CodeFormatterFactory GetInstance() {
			if (null == m_instance)
				return (m_instance = new CodeFormatterFactory());
			else
				return m_instance;
		}

		public ICodeFormatter CreateCodeFormatter(string subsys) {
			return (ICodeFormatter) CreateItem(subsys);			
		}
	}
}
