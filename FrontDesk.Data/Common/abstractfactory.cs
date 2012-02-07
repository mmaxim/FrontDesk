using System;

namespace FrontDesk.Common {

	/// <summary>
	/// Interface for abstract factories
	/// </summary>
	public interface IAbstractFactory {
		
		/// <summary>
		/// Create the item
		/// </summary>
		object CreateItem(string subsys);

		/// <summary>
		/// List all keys
		/// </summary>
		string[] ListKeys();
	}
}
