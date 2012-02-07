using System;
using System.ComponentModel;

using FrontDesk.Data.Provider;
using FrontDesk.Components;
using FrontDesk.Security;

namespace FrontDesk.Data.Access {

	/// <summary>
	/// Summary description for accesscomp.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public abstract class DataAccessComponent : Component, IProvidee {

		protected IDataProvider m_dp;
		protected AuthorizedIdent m_ident;

		public DataAccessComponent(AuthorizedIdent ident) { 
			DataProviderFactory.GetInstance(this);
			m_ident = ident;
		}

		public void Acquire(IDataProvider provider) {
			m_dp = provider;
		}

		/// <summary>
		/// The identity the component is running under
		/// </summary>
		public AuthorizedIdent Identity {
			get { return m_ident; }
		}

		/// <summary>
		/// Verify the user running the component has permissions
		/// Direct Provider layer call
		/// </summary>
		protected void Authorize(int courseID, string etype, string perm, int entityID, 
			IProviderTransaction tran) {
			if (!m_dp.CheckPermission(m_ident.Name, courseID, perm, entityID, etype, tran))
				throw new DataAccessException("Permission denied for action: " + perm);
		}

		protected void CreatePermissions(int entityID, int courseID, string type) {
			CourseRole.CourseRoleList roles = new Courses(m_ident).GetRoles(courseID, null);
			Permissions permda = new Permissions(m_ident);
			
			//Give actor permission power
			CourseRole mrole = new Courses(m_ident).GetRole(m_ident.Name, courseID, null);
			m_dp.AssignPermission(mrole.PrincipalID, "updateperms", type, entityID);
			foreach (CourseRole role in roles) {
				if (role.Staff) {
					//Do the rest of the perms
					Permission.PermissionList perms = permda.GetTypePermissions(type);
					foreach (Permission perm in perms) 
						permda.Assign(role, type, perm.Name, entityID, courseID);
				}	
			}
		}

		/// <summary>
		/// Log user transaction activty
		/// </summary>
		protected void LogActivity(string msg, int type, int objID) {
			new Activities(m_ident).Create(m_ident.Name, type, objID, msg);
		}

	}
}
