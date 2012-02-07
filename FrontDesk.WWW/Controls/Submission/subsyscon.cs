using System;

using FrontDesk.Components;

namespace FrontDesk.Controls.Submission {

	//Submission event args
	public class SubmissionEventArgs : EventArgs {
		public SubmissionEventArgs(Components.Submission sub) { m_sub = sub; }

		public Components.Submission Submission {
			get { return m_sub; }
		}	

		private Components.Submission m_sub;
	}

	//Hooks for ppl wanting to change view
	public delegate void SubmissionEventHandler(object sender, SubmissionEventArgs args);

	/// <summary>
	/// Submission system control interface
	/// </summary>
	public interface ISubmissionSystemControl {

		/// <summary>
		/// Event that a submission has gone down
		/// </summary>
		event SubmissionEventHandler SubmissionProcessed;

		/// <summary>
		/// Principal ID to be doing the submissions
		/// </summary>
		int SubmitterPrincipalID { get; set; }

		/// <summary>
		/// Assignment ID of the submission context
		/// </summary>
		int AssignmentID { get; set; }

		/// <summary>
		/// Invoked after setting up all the requried data
		/// </summary>
		void FinishInit();

		string Name { get; }

		void Synchronize();
	}
}
