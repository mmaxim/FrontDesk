using System;
using System.Collections;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	/// Initialize the formatter
	/// </summary>
	public class CodeFormatterInit {
		public string KeywordColor="#0000ff";
		public string CommentColor="#00ff00";
	}

	public class CodeFormatterLine {
		public int Number=0;
		public string Code="";

		public class CodeFormatterLineList : ArrayList {

			public CodeFormatterLineList() { }

			public new CodeFormatterLine this[int index] {
				get { return (CodeFormatterLine) base[index]; }
				set { base[index] = value; }
			}
		}
	}
		
	/// <summary>
	/// Interface for code formatters
	/// </summary>
	public interface ICodeFormatter {

		/// <summary>
		/// Initialize the line-by-line formatting option
		/// </summary>
		void InitLineFormat(string code);

		/// <summary>
		/// Get formatter lines
		/// </summary>
		CodeFormatterLine.CodeFormatterLineList GetLines();

		/// <summary>
		/// Format a line
		/// </summary>
		string FormatLine(int index);
	
		/// <summary>
		/// Initialize the formatter
		/// </summary>
		void Init(CodeFormatterInit init);
	}
}
