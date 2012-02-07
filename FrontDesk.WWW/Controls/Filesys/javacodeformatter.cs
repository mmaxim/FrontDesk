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
	public class JavaCodeFormatter : MainStreamCodeFormatter, ICodeFormatter {

		static JavaCodeFormatter() {
			m_matcher = new LimitedBoyerMoore(m_keywords);
		}

		protected static LimitedBoyerMoore m_matcher;
		protected static string[] m_keywords = { 
			 "public", "protected", "private", "class", "extends",
				"int", "double", "float", "char", "byte", "this",
				"final", "interface", "implements", "for", "while",
				"do", "if", "else", "import", "package", "try", "catch",
				"finally", "return", "void", "super", "switch",
				"long", "new", "break", "abstract", "boolean", "case",
				"continue", "instanceof", "short", "throw", "throws",
				"synchronized", "native", "transient", "volatile", "default",
				"static", "null", "true", "false"};

		public override string[] GetKeywords() {
			return m_keywords;
		}

		public override IStringMatcher GetMatcher() {
			return m_matcher;
		}
	}
}
