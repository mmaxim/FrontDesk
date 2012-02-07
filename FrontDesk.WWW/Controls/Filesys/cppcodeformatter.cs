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
	public class CppCodeFormatter : MainStreamCodeFormatter, ICodeFormatter {

		static CppCodeFormatter() {
			m_matcher = new LimitedBoyerMoore(m_keywords);
		}

		protected static LimitedBoyerMoore m_matcher;
		protected static string[] m_keywords = { 
		 "#define", "#ifdef", "#include","#endif", "#elif", "#undef", "#ifndef", "#else", "#if", "#pragma",
			"#line", "#error", "#using", "#import", "break", "class",
			"default", "else", "false", "goto", "long", "new", "protected", "return",
			"sizeof", "switch", "throw", "typeid", "using", "volatile",
			"case", "const", "delete", "do", "enum", "float", "if", "mutable", "noinline",
			"operator", "public", "static", "template", "true", "typename", 
			"catch", "const_cast", "double", 
			"explicit", "for", "inline", "private", "register", "short", "static_cast",
			"this", "try", "union", "virtual", "while", "bool", "char",
			"continue", "dynamic_cast", "extern", "friend", "int", "namespace", "nothrow",
			"reinterpret_cast", "signed", "struct", "typedef", "unsigned", "void"
			 };

		public override string[] GetKeywords() {
			return m_keywords;
		}

		public override IStringMatcher GetMatcher() {
			return m_matcher;
		}
	}
}
