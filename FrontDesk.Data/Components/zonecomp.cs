using System;

namespace FrontDesk.Components {
	/// <summary>
	/// Summary description for zonecomp.
	/// </summary>
	public interface IZoneComponent {
		
		/// <summary>
		/// Return the ID of the zone
		/// </summary>
		int GetZoneID();

		/// <summary>
		/// Return the last modification of the zone
		/// </summary>
		DateTime GetZoneModified();
	}
}
