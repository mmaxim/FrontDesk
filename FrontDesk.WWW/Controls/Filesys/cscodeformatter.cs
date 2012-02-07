using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using FrontDesk.Common;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	/// Java code formatter
	/// </summary>
	public class CSharpCodeFormatter : MainStreamCodeFormatter, ICodeFormatter {

		static CSharpCodeFormatter() {
			m_matcher = new LimitedBoyerMoore(m_keywords);
		}

		protected static LimitedBoyerMoore m_matcher;
		protected static string[] m_keywords = { 
			"#if", "#else", "#elif", "#endif", "#define", "#undef", "#warning", "#error", "#line",
		    "#region", "#endregion",
			"abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
			"class", "const", "continue", "decimal", "default", "delegate", "do", "double",
			"else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float",
			"for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", 
			"is", "lock", "long", "namespace", "new", "null", "object", "operator", "out",
			"override", "params", "private", "protected", "public", "readonly", "ref", "return",
			"sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch",
			"this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe",
			"ushort", "using", "virtual", "volatile", "void", "while"
		};

		public override string[] GetKeywords() {
			return m_keywords;
		}

		public override IStringMatcher GetMatcher() {
			return m_matcher;
		}
	}
}
