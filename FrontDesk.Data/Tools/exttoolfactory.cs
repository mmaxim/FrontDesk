using System;

using FrontDesk.Common;

namespace FrontDesk.Tools {
	/// <summary>
	/// Summary description for exttoolfactory.
	/// </summary>
	public class ExternalToolFactory : ClassAbstractFactory {

		public ExternalToolFactory() : base("ExternalTools") { }

		public enum VersionCompare { ATLEAST = 0, EQUAL, NONE };

		private static ExternalToolFactory m_instance;

		public static ExternalToolFactory GetInstance() {
			if (null == m_instance)
				return (m_instance = new ExternalToolFactory());
			else
				return m_instance;
		}

		private bool isVersionEarlier(string left, string right) {
			string[] ltokens = left.Split(".".ToCharArray());
			string[] rtokens = right.Split(".".ToCharArray());
			int i;

			for (i = 0; i < Math.Min(ltokens.Length, rtokens.Length); i++) {	
				int iltok, irtok;
				
				try { iltok = Convert.ToInt32(ltokens[i]); } 
				catch (Exception) { iltok = 0; }
				
				try { irtok = Convert.ToInt32(rtokens[i]); } 
				catch (Exception) { irtok = 0; }

				if (iltok < irtok)
					return true;
				else if (iltok > irtok)
					return false;
			}

			return (ltokens.Length < rtokens.Length);
		}

		public IExternalTool CreateExternalTool(string subsys) {
			return CreateExternalTool(subsys, null, VersionCompare.NONE);
		}

		public IExternalTool CreateExternalTool(string subsys, string version, VersionCompare comp) {	
			IExternalTool tool = (IExternalTool) CreateItem(subsys);	
			if (version != null) {
				switch (comp) {
				case VersionCompare.ATLEAST:
					if (isVersionEarlier(tool.Version, version))
						return null;
					break;
				case VersionCompare.EQUAL:
					if (version != tool.Version)
						return null;
					break;
				}
			}	
			return tool;	
		}
	}
}
