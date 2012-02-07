using System;

using FrontDesk.Components.Filesys;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	/// File viewer control interface
	/// </summary>
	public interface IFileViewer {

		/// <summary>
		/// Change control display to edit mode
		/// </summary>
		void Edit();

		/// <summary>
		/// Change back to view mode
		/// </summary>
		void UnEdit();

		/// <summary>
		/// Return the file data from the control
		/// </summary>
		string Data { get; }

		/// <summary>
		/// Load the view with the given file
		/// </summary>
		void LoadFile(CFile file);

		/// <summary>
		/// The FileID the viewer is currently displaying
		/// </summary>
		int FileID { get; }

		/// <summary>
		/// Is the viewer an editable type 
		/// </summary>
		bool Editable { get; }
	}
}
