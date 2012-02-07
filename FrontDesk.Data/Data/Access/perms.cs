using System;
using System.Collections;

using FrontDesk.Components;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Permissions data access
	/// </summary>
	public class Permissions : DataAccessComponent {
		
		public Permissions(AuthorizedIdent ident) : base(ident) { }

		/// <summary>
		/// Assign a permission to an entity
		/// </summary>
		public bool Assign(string username, string etype, string perm, int entityID, int courseID) {
			Authorize(courseID, etype, "updateperms", entityID, null);
			User user = new Users(m_ident).GetInfo(username, null);
			return m_dp.AssignPermission(user.PrincipalID, perm, etype, entityID);
		}

		/// <summary>
		/// Assign a permission to an entity
		/// </summary>
		public bool Assign(CourseRole role, string etype, string perm, int entityID, int courseID) {
			Authorize(courseID, etype, "updateperms", entityID, null);
			return m_dp.AssignPermission(role.PrincipalID, perm, etype, entityID);
		}

		/// <summary>
		/// Assign a principalID direct
		/// </summary>
		public bool Assign(int principalID, string etype, string perm, int entityID, int courseID) {
			Authorize(courseID, etype, "updateperms", entityID, null);
			return m_dp.AssignPermission(principalID, perm, etype, entityID);
		}

		/// <summary>
		/// Deny a permission to an entity
		/// </summary>
		public bool Deny(string username, string etype, string perm, int entityID, int courseID) {
			Authorize(courseID, etype, "updateperms", entityID, null);
			User user = new Users(m_ident).GetInfo(username, null);
			return m_dp.DenyPermission(user.PrincipalID, perm, etype, entityID);
		}

		/// <summary>
		/// Deny a permission to an entity
		/// </summary>
		public bool Deny(CourseRole role, string etype, string perm, int entityID, int courseID) {
			Authorize(courseID, etype, "updateperms", entityID, null);
			return m_dp.DenyPermission(role.PrincipalID, perm, etype, entityID);
		}

		/// <summary>
		/// Deny a principalID direct
		/// </summary>
		public bool Deny(int principalID, string etype, string perm, int entityID, int courseID) {
			Authorize(courseID, etype, "updateperms", entityID, null);
			return m_dp.DenyPermission(principalID, perm, etype, entityID);
		}

		/// <summary>
		/// Check to see if principal ID has permission
		/// </summary>
		public Permission.GrantType CheckPermission(int principalID, int courseID, string etype, 
										 string perm, int entityID) {
			
			//Check ID directly
			if (m_dp.CheckPermission(principalID, courseID, perm, entityID, etype, null))
				return Permission.GrantType.DIRECT;

			Principal prin = new Principals(m_ident).GetInfo(principalID);
			if (prin.Type == Principal.USER) {
				CourseRole role = new Courses(m_ident).GetRole(
					prin.Name, courseID, null);
				if (m_dp.CheckPermission(role.PrincipalID, courseID, perm, entityID, etype, null))
					return Permission.GrantType.INHERIT;
			}

			return Permission.GrantType.DENY;
		}

		/// <summary>
		/// Get all permissions for a given entity type
		/// Direct Provider layer call
		/// </summary>
		public Permission.PermissionList GetTypePermissions(string etype) {
			return m_dp.GetTypePermissions(etype);
		}
	}
}
