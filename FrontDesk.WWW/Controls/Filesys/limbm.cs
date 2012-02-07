using System;
using System.Collections;
using System.Text;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	/// Boyer-Moyer string matching algorithm optimized for a finite set of patterns
	/// </summary>
	public class LimitedBoyerMoore : IStringMatcher {
		
		protected string[] m_patterns;
		protected Hashtable m_lastmap = new Hashtable();

		public LimitedBoyerMoore(string[] patterns) { 
			m_patterns = patterns;

			foreach (string pat in m_patterns)
				m_lastmap.Add(pat, buildLast(pat));
		}

		private int[] buildLast(string p) {
			int[] last = new int[256];
			int i;
			for (i = 0; i < 256; i++)
				last[i] = -1;

			for (i = 0; i < p.Length; i++)
				last[p[i]] = i;

			return last;
		}

		public int IndexOf(string pat, string text, int start, int count) {
			int index = IndexOf(pat, text.Substring(start, count));
			if (index < 0)
				return index;
			else
				return start+index;
		}
		
		public int IndexOf(string pat, StringBuilder text, int start, int count) {
			int index = IndexOf(pat, text.ToString(start, count));
			if (index < 0)
				return index;
			else
				return start+index;
		}

		public int IndexOf(string pat, string text) {	
			int[] last = (int[]) m_lastmap[pat];
			int n = text.Length, m = pat.Length;
			int i = m-1, j = m-1;

			if (i > n-1) return -1;
			do {
				if (pat[j] == text[i]) {
					if (j == 0) return i;
					else { i--; j--; }
				} else {
					i = i + m - Math.Min(j, 1+last[text[i]]);
					j = m-1;
				}
			} while (i <= n-1); 
			return -1;
		}

	}
}
