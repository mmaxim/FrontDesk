using System;
using System.Security.Principal;
using NUnit.Framework;

using FrontDesk.Data.Access;
using FrontDesk.Components;

namespace FrontDesk.Data.Diagnostics.Access {
	
	/// <summary>
	/// Test fixture for Users data access component
	/// </summary>
	[TestFixture]
	public class UsersFixture {

		public UsersFixture() { }

		protected Users m_users;

		[SetUp] public void SetUp() {
		//	GenericIdentity ident = new GenericIdentity("mmaxim");
		//	m_users = new Users(ident);
		}

		[Test] public void TestCreate() {

			//Basic create
	/*		m_users.Create("testuser", "testuser", "Mike", "Jerk", "mmaxim@cs.cmu.edu", null);
			User testuser = m_users.GetInfo("testuser", null);
			Assert.AreEqual("Mike", testuser.FirstName);
			Assert.AreEqual("Jerk", testuser.LastName);
			Assert.AreEqual("mmaxim@cs.cmu.edu", testuser.Email);*/
			
		}

		
	}
}
