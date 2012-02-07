using System;
using System.Collections;

using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Summary description for groups.
	/// </summary>
	public class Groups : DataAccessComponent {
	
		public Groups(AuthorizedIdent ident) : base(ident) { }

		/// <summary>
		/// Create a new group
		/// </summary>
		public bool Create(string name, string creator, int asstID,
								ArrayList invites) {
			
			//TODO: Validate group parameters
			//Create the group
			Group group = new Group();
			group.Name = name;
			group.Creator = creator;
			group.AsstID = asstID;
			m_dp.CreateGroup(group);

			//Invite everyone
			foreach (string username in invites) {
				//TODO: Validate invitation validity

				new EmailWizard(m_ident).SendByUsername(username, "FrontDesk: Invitation Notification",
					String.Format("Dear User,\n\nThis email is to inform you that user {0} has invited you " +
					"to join group {1}. Log into FrontDesk in order to accept or decline this " +
					"invitation.\n\nPlease do not respond to this email.", creator, name));

				m_dp.InviteUser(username, Globals.CurrentUserName, group.PrincipalID);
			}

			return true;
		}

		/// <summary>
		/// Delete a group ID
		/// </summary>
		public bool Delete(int groupID, int asstID) {

			//Get subs
			Submissions subda = new Submissions(m_ident);
			Components.Submission.SubmissionList subs = 
				new Principals(m_ident).GetSubmissions(groupID, asstID);
			
			//Take the subs
			foreach (Components.Submission sub in subs)
				subda.Delete(sub.ID);

			//Take the group
			return m_dp.DeleteGroup(groupID);
		}

		/// <summary>
		/// List all the users in the specified group
		/// Direct Provider layer call
		/// </summary>
		public User.UserList GetMembership(int groupID) {
			return m_dp.GetGroupMembers(groupID);
		}

		/// <summary>
		/// Do some processing with the invitation
		/// </summary>
		public bool AcceptInvitation(int courseID, int groupID, int invid) {

			Invitation invite = new Invitation();
			invite.ID = invid;

			m_dp.CreateGroupMember(invite);
			return true;
		}

		/// <summary>
		/// Get group information
		/// Direct Provider layer call
		/// </summary>
		public Group GetInfo(int groupID) {
			return m_dp.GetGroupInfo(groupID);
		}

		/// <summary>
		/// Decline an invitation and do some processing
		/// </summary>
		public bool DeclineInvitation(int invid) {

			Invitation invite = new Invitation();

			invite.ID = invid;

			m_dp.DeleteInvitation(invite);
			return true;
		}

		/// <summary>
		/// Leave the group
		/// </summary>
		public bool Leave(string username, int groupID) {
			
			Group group = GetInfo(groupID);
		
			//Take the member
			m_dp.DeleteGroupMember(username, group);

			//Delete the group if no one is in it
			User.UserList mems = GetMembership(groupID);
			if (mems.Count == 0)
				Delete(groupID, group.AsstID);

			return true;
		}
	}
}
