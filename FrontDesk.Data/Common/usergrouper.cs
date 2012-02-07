using System;
using System.Collections;

using FrontDesk.Components;

namespace FrontDesk.Common {

	/// <summary>
	/// User grouper
	/// </summary>
	public class UserGrouper {
		
		public UserGrouper() { }

		public const int BINSIZE=20;

		private static string[] alphabet = { 
								  "a","b","c","d","e","f","g","h","i","j","k","l","m",
								  "n","o","p","q","r","s","t","u","v","w","x","y","z" };

		//A user group
		public class UserGroup {	
			public User.UserList Users {
				get { return m_users; }
				set { m_users = value; }
			}

			public string UpperBound {
				get { return m_upbound; }
				set { m_upbound = value; }
			}

			public string LowerBound {
				get { return m_lowbound; }
				set { m_lowbound = value; }
			}

			public static string Display(string str) {
				if (str.Length > 1)
					return str.Substring(0,1).ToUpper() + str.Substring(1,str.Length-1).ToLower();
				else
					return str.Substring(0,1).ToUpper();
			}

			public bool InGroup(string sstr) {
				string str = sstr.ToLower();
				string ustr = str.Substring(0, Math.Min(str.Length, m_upbound.Length));
				string lstr = str.Substring(0, Math.Min(str.Length, m_lowbound.Length));
				if (m_upbound != m_lowbound && 
					ustr.CompareTo(m_upbound) >= 0 && lstr.CompareTo(m_lowbound) < 0)
					return true;
				else if (m_upbound == m_lowbound &&
					ustr.CompareTo(m_upbound) >= 0)
					return true;
	
				return false;
			}

			User.UserList m_users = new User.UserList();
			string m_upbound, m_lowbound;
		}

		/// <summary>
		/// Group users into binsize groups
		/// </summary>
		public bool Group(User.UserList users, ArrayList groups) {
			return Group(users, groups, BINSIZE);
		}

		/// <summary>
		/// Group users into binsize groups
		/// </summary>
		public bool Group(User.UserList users, ArrayList groups, int binsize) {

			groups.Clear();
			groups.AddRange(Setup(users));
			while (Divide(groups, binsize) || Combine(groups, binsize));
			if (groups.Count == 1)
				return false;
			
			return true;
		}

		/// <summary>
		/// Regroup
		/// </summary>
		public void Regroup(string ubound, string lbound, User.UserList users) {
			int i;
			UserGroup group = new UserGroup();
			group.UpperBound = ubound; group.LowerBound = lbound;
			for (i = 0; i < users.Count; i++) {
				User user = users[i] as User;
				if (!group.InGroup(user.LastName)) {
					users.Remove(user);
					i--;
				}
			}
		}

		private bool Combine(ArrayList groups, int binsize) {
			bool workdone=false;
			int i;

			for (i = 1; i < groups.Count; i++) {
				UserGroup right = (UserGroup) groups[i];
				UserGroup left = (UserGroup) groups[i-1];
				
				//Can combine
				if (right.Users.Count + left.Users.Count <= binsize) {
					workdone=true;
					UserGroup combine = new UserGroup();

					combine.UpperBound = left.UpperBound;
					combine.LowerBound = right.LowerBound;

					combine.Users.AddRange(left.Users);
					combine.Users.AddRange(right.Users);

					groups.Remove(right);
					groups.Remove(left);
					groups.Insert(i-1, combine);
					i--;
				}
			}
			
			return workdone;
		}

		private bool Divide(ArrayList groups, int binsize) {
			bool workdone=false;
			int i;

			for (i = 0; i < groups.Count; i++) {
				UserGroup group = (UserGroup) groups[i];
				if (group.Users.Count > binsize) {
					
					//Split in half
					UserGroup left = new UserGroup();
					UserGroup right = new UserGroup();

					string newdiv = GetMedian(group);
					if (newdiv == null) continue; //Give up

					string divpoint = group.UpperBound+newdiv;
					left.UpperBound = group.UpperBound;
					left.LowerBound = divpoint;
					right.UpperBound = divpoint;
					right.LowerBound = group.LowerBound;

					foreach (User user in group.Users) {
						if (left.InGroup(user.LastName))
							left.Users.Add(user);
						else
							right.Users.Add(user);
					}

					workdone=true;
					groups.Remove(group);
					groups.Insert(i, left); groups.Insert(i+1, right);
					i--;
				}
			}

			return workdone;
		}

		private string GetMedian(UserGroup group) {

			int slot = group.UpperBound.Length;
			ArrayList slotlets = new ArrayList();

			//Check for median existence
			foreach (User user in group.Users) {
				if (user.LastName.Length > slot) 
					slotlets.Add(user.LastName.ToLower()[slot]);
			}

			//Find a reasonable median
			if (slotlets[0] == slotlets[slotlets.Count-1]) 
				return null;
			else {		
				int i = slotlets.Count/2;
				while (i < slotlets.Count-1 && (char)slotlets[0] == (char)slotlets[i]) i++;
				return slotlets[i].ToString();
			}
		}

		private ArrayList Setup(User.UserList users) {
			ArrayList groups = new ArrayList();
			int i;

			for (i = 0; i < alphabet.Length; i++) {
				string bound = alphabet[i];
				UserGroup group = new UserGroup();
				group.UpperBound = alphabet[i];
				if (i == alphabet.Length - 1)
					group.LowerBound = bound;
				else
					group.LowerBound = alphabet[i+1];

				foreach (User user in users)
					if (group.InGroup(user.LastName))
						group.Users.Add(user);

				groups.Add(group);
			}
			
			return groups;
		}
	}
}
