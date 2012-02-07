using System;
using System.Text;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	/// String matcher
	/// </summary>
	public interface IStringMatcher {
	
		int IndexOf(string pat, string text, int start, int count);
		
		int IndexOf(string pat, StringBuilder text, int start, int count);

		int IndexOf(string pat, string text);
	}
}
