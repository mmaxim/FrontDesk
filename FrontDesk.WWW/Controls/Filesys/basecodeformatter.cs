using System;
using System.Collections;
using System.IO;
using System.Text;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	/// Base class for all formatters
	/// </summary>
	public abstract class BaseCodeFormatter {

		protected ArrayList m_lines = new ArrayList();
		protected CodeFormatterInit m_prefs;

		protected void GetLinesFromCode(string code) {
			MemoryStream memstrm = new MemoryStream(Encoding.ASCII.GetBytes(code));
			StreamReader srdr = new StreamReader(memstrm);
			string line;

			m_lines.Clear();
			while (null != (line = srdr.ReadLine()))
				m_lines.Add(line);
		}

		public CodeFormatterLine.CodeFormatterLineList GetLines() {
			CodeFormatterLine.CodeFormatterLineList lines = 
				new CodeFormatterLine.CodeFormatterLineList();
			int i;

			for (i = 0; i < m_lines.Count; i++) {
				CodeFormatterLine line = new CodeFormatterLine();
				line.Code = m_lines[i] as string;
				line.Number = i;
				lines.Add(line);
			}

			return lines;
		}
	
		public void InitLineFormat(string code) {
			GetLinesFromCode(code);
		}

		public void Init(CodeFormatterInit init) {
			m_prefs = init;
		}
	}
}
