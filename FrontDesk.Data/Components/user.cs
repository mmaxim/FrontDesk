using System;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime;
using System.Collections;

namespace FrontDesk.Components {

	public class User : Principal {

		public User() : base() { }

		public const String USERNAME_FIELD = "username";
		public const String FIRSTNAME_FIELD = "firstName";
		public const String LASTNAME_FIELD = "lastName";
		public const String EMAIL_FIELD = "email";
		public const String LASTLOGIN_FIELD = "lastLogin";
		public const String ADMIN_FIELD = "admin";
		public const String VERIFYKEY_FIELD = "verifykey";

		protected string m_first, m_last, m_email, m_verified="";
		protected DateTime m_lastlogin;
		protected bool m_admin;

		public override bool Equals(object o) {
			User uo = o as User;
			return (uo.UserName == UserName);
		}

		public override int GetHashCode() {
			return UserName.GetHashCode();
		}

		[FieldName(USERNAME_FIELD)]
		public string UserName {
			get { return m_name; }
			set { m_name = value; }
		}

		[FieldName(FIRSTNAME_FIELD)]
		public string FirstName {
			get { return m_first; }
			set { m_first = value; }
		}

		[FieldName(LASTNAME_FIELD)]
		public string LastName {
			get { return m_last; }
			set { m_last = value; }
		}

		public string FullName {
			get { return m_first + " " + m_last; }
		}

		[FieldName(EMAIL_FIELD)]
		public string Email {
			get { return m_email; }
			set { m_email = value; }
		}

		[FieldName(LASTLOGIN_FIELD)]
		public DateTime LastLogin {
			get { return m_lastlogin; }
			set { m_lastlogin = value; }
		}

		[FieldName(ADMIN_FIELD)]
		public bool Admin {
			get { return m_admin; }
			set { m_admin = value; }
		}

		[FieldName(VERIFYKEY_FIELD)]
		public string VerifyKey {
			get { return m_verified; }
			set { m_verified = value; }
		}

		public class UserList : ArrayList {

			public UserList() { }

			public new User this[int index] {
				get { return (User) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}