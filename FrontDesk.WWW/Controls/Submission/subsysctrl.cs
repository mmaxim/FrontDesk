using System;

using FrontDesk.Common;
using FrontDesk.Pages;

namespace FrontDesk.Controls.Submission {

	/// <summary>
	/// Generic base class for submission system controls
	/// </summary>
	public class SubmissionSystemControl : Pagelet {

		protected System.Web.UI.WebControls.Label lblError;

		public event SubmissionEventHandler SubmissionProcessed;

		public int SubmitterPrincipalID {
			get { return (int) ViewState["prinID"]; }
			set { ViewState["prinID"] = value; }
		}	

		public int AssignmentID {
			get { return (int) ViewState["asstID"]; }
			set { ViewState["asstID"] = value; }
		}

		protected void PageError(string msg) {
			lblError.Text = msg;
			lblError.Visible = true;
		}

		protected bool IsSubmitting() {
			if (null == ViewState["submitting"])
				return false;
			else
				return (bool) ViewState["submitting"];
		}
		
		public void Synchronize() {
			if (lblError != null) lblError.Visible = false;
			if (IsSubmitting()) {
				LongTaskControl ltc = ((MasterPage)Page).GetLongTaskControl();
				if (!ltc.IsBusy()) {
					ViewState["submitting"] = false;
					ICommandEventArgs args = ltc.ReleaseReturnArgs();
					//Display errors
					if (args.Exception != null) 
						PageError("Error during submission: " + args.Exception.Message);
					else {
						//Notify submission has been proced
						if (SubmissionProcessed != null)
							SubmissionProcessed(this, 
								new SubmissionEventArgs((Components.Submission)args.ReturnValue));
					}
				}
			}
		}

		protected void FireSubEvent(ICommandEventArgs args) {
			//Notify submission has been proced
			if (SubmissionProcessed != null)
				SubmissionProcessed(this, 
					new SubmissionEventArgs((Components.Submission)args.ReturnValue));
		}
	}
}
