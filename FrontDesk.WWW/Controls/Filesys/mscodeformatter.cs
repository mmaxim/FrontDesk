using System;
using System.Text;

using FrontDesk.Common;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	/// Main stream code formatter base class
	/// </summary>
	public abstract class MainStreamCodeFormatter : BaseCodeFormatter {

		public abstract string[] GetKeywords();
		public abstract IStringMatcher GetMatcher();

		protected bool m_incomment=false;

		public string FormatLine(int lineindex) {
			
			int index = 0, t, lindex, sindex, eindex;
			string nline="", kcolor = m_prefs.KeywordColor, ccolor = m_prefs.CommentColor;

			if (lineindex >= m_lines.Count) return "";

			string line = (string) m_lines[lineindex];
			line = HTMLWizard.ConvertAngles(line);
			line = HTMLWizard.ConvertSpaces(line);
			
			while (index < line.Length) {
				lindex = (0 > (t = line.IndexOf("//", index, line.Length-index))) ? line.Length+1 : t;
				sindex = (0 > (t = line.IndexOf("/*", index, line.Length-index))) ? line.Length+1 : t;
				eindex = (0 > (t = line.IndexOf("*/", index, line.Length-index))) ? line.Length+1 : t;

				if (lindex < Math.Min(sindex, eindex)) {
					string ncpart = line.Substring(index, lindex-index);
					string cpart = line.Substring(lindex, line.Length - lindex);
					nline += ApplyFormatting(ncpart, kcolor, ccolor) + MakeComment(cpart, ccolor);
					index = line.Length;
				}
				else if (sindex < Math.Min(lindex, eindex)) {		
					string ncpart = line.Substring(index, sindex-index);	
					nline += ApplyFormatting(ncpart, kcolor, ccolor) + MakeComment("/*", ccolor);
					m_incomment = true;
					index = sindex+2;
				}
				else if (eindex < Math.Min(lindex, sindex)) {
					string cpart = line.Substring(index, eindex-index);
					m_incomment = false;

					nline += MakeComment(cpart + "*/", ccolor);
					index = eindex+2;
				}
				else {
					string leftover = line.Substring(index, line.Length - index);
					nline += ApplyFormatting(leftover, kcolor, ccolor);
					index = line.Length;
				}
			}

			nline = HTMLWizard.ConvertControls(nline);
			return nline;
		}

		private string ApplyFormatting(string str, string kcolor, string ccolor) {
			if (!m_incomment)
				return KeywordColor(str, kcolor);
			else
				return MakeComment(str, ccolor);
		}

		private string MakeComment(string str, string color) {
			return "<i><font color=\""+color+"\">" + 
				str + "</font></i>";
		}

		protected bool IsKosher(char c) {
			return (!(c >= 'a' && c <= 'z') &&
				!(c >= 'A' && c <= 'Z') &&
				!(c >= '0' && c <= '9') &&
				c != '_' && c != '\"');
		}

		protected string KeywordColor(string line, string color) {
			int i;
			string newstr;
			string[] keywords = GetKeywords();
			IStringMatcher matcher = GetMatcher();
			StringBuilder sb = new StringBuilder(line);
			foreach (string key in keywords) {
				i = 0;
				while (0 <= (i = matcher.IndexOf(key, sb, i, sb.Length - i))) {
					if ((i == 0 || IsKosher(sb[i-1])) &&
						(key.Length+i == sb.Length || IsKosher(sb[key.Length+i]))) {		
						newstr =
							"<b><font color=\"" + color + "\">" + key + "</font></b>";		
						sb = sb.Replace(key, newstr, i, key.Length);
						i += newstr.Length-1;
					} else
						i += key.Length;
				}	
			}
			return sb.ToString();
		}
	}
}
