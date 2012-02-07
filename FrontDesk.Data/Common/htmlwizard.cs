using System;

namespace FrontDesk.Common {

	/// <summary>
	/// Useful HTML gadgets
	/// </summary>
	public class HTMLWizard {
		
		public HTMLWizard() {	}

		public static string LineBreakToBR(string html) {
			
			//Turn carriage into <BR> tags
			html = html.Replace("\r\n", "<br>");
			
			return html;
		}

		public static string BRToLineBreak(string html) {
			
			//Turn <br> into carraige return line break
			html = html.Replace("<br>", "\r\n");
			
			return html;
		}

		public static string ConvertAngles(string str) {
			str = str.Replace("<", "&lt;");
			str = str.Replace(">", "&gt;");
			return str;
		}

		public static string ConvertSpaces(string str) {
			str = str.Replace(" ", "&nbsp;");
			return str;
		}

		public static string ConvertControls(string str) {
			
			string final="";
			int i;

			str = LineBreakToBR(str);
			for (i = 0; i < str.Length; i++) {
				char c = str[i];
				switch (c) {
				case '\t':
					final += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
					break;
				default:
					final += c;
					break;
				};
			}

			return final;
		}
	}
}
