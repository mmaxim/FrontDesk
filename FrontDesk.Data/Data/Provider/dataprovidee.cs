using System;

namespace FrontDesk.Data.Provider {

	/// <summary>
	/// Interface for classes that can receive a Provider instance
	/// </summary>
	internal interface IProvidee {
		
		/// <summary>
		/// Load the IProvidee intance with a data provider
		/// </summary>
		void Acquire(IDataProvider provider);
	}
}
