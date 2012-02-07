using System;
using System.Web.Mail;

using FrontDesk.Common;
using FrontDesk.Data.Access;
using FrontDesk.Components;
using FrontDesk.Security;

namespace FrontDesk.Common {

	/// <summary>
	/// Emailing express
	/// </summary>
	public class EmailWizard {

		public EmailWizard(AuthorizedIdent ident) { m_ident = ident; }

		private AuthorizedIdent m_ident;

		/// <summary>
		/// Send an email
		/// </summary>
		public bool SendByEmail(string toaddr, string subj, string body) {

			MailMessage msg = new MailMessage();
			msg.From = Globals.EmailAddress;
			msg.Subject = subj;
			msg.Body = body;
			msg.To = toaddr;

			try {
				SmtpMail.SmtpServer = Globals.MailServerAddress;
				SmtpMail.Send(msg);
			} catch (Exception) {
				return false;
			}

			return true;
		}

		public bool SendByPrincipal(int principalID, string subj, string body) {

			Principal prin = new Principals(m_ident).GetInfo(principalID);
			if (prin.Type == Principal.USER) {
				User user = new Users(m_ident).GetInfo(prin.Name, null);
				return SendByEmail(user.Email, subj, body);
			} else {
				User.UserList users = 
					new Groups(m_ident).GetMembership(principalID);
				foreach (User user in users)
					SendByEmail(user.Email, subj, body);
			}
			return true;
		}

		public bool SendByUsername(string username, string subj, string body) {
			User user = new Users(m_ident).GetInfo(username, null);
			return SendByEmail(user.Email, subj, body);
		}
	}
}
