using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace FrontDesk.Controls.Matrix {
	
	public class ActivateParams {
		
		public ActivateParams() { }

		public ActivateParams(int pid, string aux, bool student) {
			ID = pid; Auxiliary = aux; StudentMode = student; 
		}

		public int ID=-1;
		public bool StudentMode=false;
		public string Auxiliary="";
	}

	/// <summary>
	/// Interface for matrix pagelets
	/// </summary>
	public interface IMatrixControl  {

		event RefreshEventHandler Refresh; 

		void Activate(ActivateParams ap);
	}
}
