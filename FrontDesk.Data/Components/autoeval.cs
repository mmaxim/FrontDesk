using System;
using System.Collections;

namespace FrontDesk.Components.Evaluation {

	/// <summary>
	/// Automatic evaluation entity
	/// </summary>
	public class AutoEvaluation : Evaluation, IZoneComponent {

		public AutoEvaluation() { }

		public const String ISBUILD_FIELD = "isbuild";
		public const String TOOL_FIELD = "tool";
		public const String TOOLARGS_FIELD = "toolArguments";
		public const String ZONEID_FIELD = "zoneID";
		public const String ZONEMOD_FIELD = "zoneMod";
		public const String VERSION_FIELD = "toolVersion";
		public const String VERSIONING_FIELD = "toolVersioning";

		private DateTime m_zonemod;
		private int m_zoneid, m_versioning;
		private bool m_isbuild;
		private string m_tool, m_toolargs, m_version="";

		public int GetZoneID() {
			return m_zoneid;
		}

		public DateTime GetZoneModified() {
			return m_zonemod;
		}

		[FieldName(ZONEID_FIELD)]
		public int ZoneID {
			get { return m_zoneid; }
			set { m_zoneid = value; }
		}

		[FieldName(VERSIONING_FIELD)]
		public int ToolVersioning {
			get { return m_versioning; }
			set { m_versioning = value; }
		}

		[FieldName(ZONEMOD_FIELD)]
		public DateTime ZoneModified {
			get { return m_zonemod; }
			set { m_zonemod = value; }
		}

		[FieldName(ISBUILD_FIELD)]
		public bool IsBuild {
			get { return m_isbuild; }
			set { m_isbuild = value; }
		}

		[FieldName(TOOL_FIELD)]
		public string RunTool {
			get { return m_tool; }
			set { m_tool = value; }
		}

		[FieldName(VERSION_FIELD)]
		public string ToolVersion {
			get { return m_version; }
			set { m_version = value; }
		}

		[FieldName(TOOLARGS_FIELD)]
		public string RunToolArgs {
			get { return m_toolargs; }
			set { m_toolargs = value; }
		}

		public class AutoEvaluationList : ArrayList {

			public AutoEvaluationList() { }

			public new AutoEvaluation this[int index] {
				get { return (AutoEvaluation) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
