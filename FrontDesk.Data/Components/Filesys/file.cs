using System;
using System.IO;
using System.Collections;

namespace FrontDesk.Components.Filesys {

	public enum FileAction { READ = 0, WRITE, DELETE, ADMIN };

	/// <summary>
	/// File properties
	/// </summary>
	public class CFile {

		public CFile() { }

		public static readonly int MaxPath = 1500;
		public static readonly int MaxName = 100;

		public enum FileType { FILE=1, DIRECTORY };
		public enum SpecialType { NONE = 1, SUBMISSION = 2, TEST = 3};

		protected string m_fileName, m_pathName, m_alias, m_desc="";
		protected DateTime m_created, m_modified;
		protected FileType m_type;
		protected SpecialType m_specialtype;
		protected bool m_readonly, m_stuview;
		protected int m_id, m_parentid, m_size=0;
		protected char[] m_data=null;
		protected byte[] m_rawdata=null;

		public string Name {
			get { return m_fileName; }
			set { m_fileName = value; }
		}

		public bool ReadOnly {
			get { return m_readonly; }
			set { m_readonly = value; }
		}

		public int Size {
			get { return m_size; }
			set { m_size = value; }
		}

		public string FullPath {
			get { return System.IO.Path.Combine(m_pathName, m_fileName); }
		}

		public string Path {
			get { return m_pathName; }
			set { m_pathName = value; }
		}

		public string Description {
			get { return m_desc; }
			set { m_desc = value; }
		}

		public FileType Type {
			get { return m_type; }
			set { m_type = value; }
		}

		public int ID {
			get { return m_id; }
			set { m_id = value; }
		}

		public char[] Data {
			get { return m_data; }
			set { m_data = value; m_rawdata = CharToRaw(m_data); }
		}

		public byte[] RawData {
			get { return m_rawdata; }
			set { m_rawdata = value; m_data = RawToChar(m_rawdata); }
		}

		protected byte[] CharToRaw(char[] cdata) {
			int i;
			byte[] data = new byte[cdata.Length];
			for (i = 0; i < cdata.Length; i++) {
				data[i] = (byte) cdata[i];
			}
			return data;
		}

		protected char[] RawToChar(byte[] data) {
			int i;
			char[] cdata = new char[data.Length];
			for (i = 0; i < data.Length; i++) {
				cdata[i] = (char) data[i];
			}
			return cdata;
		}

		public DateTime FileCreated {
			get { return m_created; }
			set { m_created = value; }
		}

		public DateTime FileModified {
			get { return m_modified; }
			set { m_modified = value; }
		}

		public SpecialType SpecType {
			get { return m_specialtype; }
			set { m_specialtype = value; }
		}

		public string Alias {
			get { return m_alias; }
			set { m_alias = value; }
		}

		public bool IsDirectory() {
			return (Type == FileType.DIRECTORY);
		}

		public class FileList : ArrayList {
			public FileList() {
			}

			public override string ToString() {
				string str="";
				int i;
				for (i = 0; i < base.Count; i++) {
					str += (base[i] as CFile).Alias;
					if (i < base.Count-1)
						str += ", ";
				}
				return str;
			}

			public new CFile this[int index] {
				get { return (CFile) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
