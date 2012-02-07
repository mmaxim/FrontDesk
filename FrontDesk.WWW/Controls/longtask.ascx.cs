using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Collections;

using FrontDesk.Common;
using FrontDesk.Pages;

namespace FrontDesk.Controls {
	
	/// <summary>
	///	Long task control
	/// </summary>
	public class LongTaskControl : Pagelet {
		protected System.Web.UI.WebControls.Button cmdRefresh;

		private static Hashtable m_threads = new Hashtable();
		private static Hashtable m_retargs = new Hashtable();

		private void Page_Load(object sender, System.EventArgs e) {

		}

		public bool IsBusy() {
			if (null == ViewState["thread"]) 
				return false;
			else
				return (m_threads[ViewState["thread"]] != null);
		}

		public ICommandEventArgs ReleaseReturnArgs() {
			ICommandEventArgs args = m_retargs[ViewState["thread"]] as ICommandEventArgs;
			m_retargs.Remove(ViewState["thread"]);
			return args;
		}

		public void Start(ICommand task) {

			//Hook into finished event
			task.Finished += new ICommandEventHandler(task_Finished);

			//Begin thread
			Thread thrtask = new Thread(new ThreadStart(task.Execute));
			thrtask.Start();

			Guid guid = Guid.NewGuid();
			ViewState["thread"] = guid;
			m_threads.Add(guid, true);
			m_retargs.Add(guid, null);
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void task_Finished(object sender, ICommandEventArgs args) {
			m_threads.Remove(ViewState["thread"]);
			m_retargs[ViewState["thread"]] = args;
		}

		private void cmdRefresh_Click(object sender, System.EventArgs e) {
		
		}
	}
}
