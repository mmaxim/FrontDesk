using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Web.UI.WebControls;

namespace FrontDesk.Pages {

	/// <summary>
	/// Pagelet class
	/// </summary>
	public class Pagelet : UserControl {

		public Pagelet() { }

		private void Page_Load(object sender, EventArgs e) {
			FixControlDims(this);
		}

		public static void FixControlDims(Control control) {
			foreach (Control c in control.Controls) {
				if (c is WebControl) {
					WebControl wc = c as WebControl;
					if (wc.Width.Value > 0) 
						wc.Style["WIDTH"] = wc.Width.ToString();
					if (wc.Height.Value > 0)
						wc.Style["HEIGHT"] = wc.Height.ToString();
				}

				if (c.ID != null && c.ID.StartsWith("div"))
					FixControlDims(c);
				else if (c is MultiPage) {
					MultiPage mc = c as MultiPage;
					foreach (Control pv in mc.Controls)
						foreach (Control cv in pv.Controls)
							FixControlDims(cv);
				}	
			}
		}

		protected override void OnInit(EventArgs e) {
			InitializeComponent();
			base.OnInit (e);
		}
		
		private void InitializeComponent() {    
			this.Load += new System.EventHandler(this.Page_Load);
		}
	}
}
