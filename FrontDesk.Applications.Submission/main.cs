using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace FrontDesk.Applications.Submission {

	/// <summary>
	/// Main Form (never shown)
	/// </summary>
	public class MainForm : Form {

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm() {
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (disposing)
				if (components != null) 
					components.Dispose();
				
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(232, 45);
			this.Name = "MainForm";
			this.Text = "FrontDesk Submission System";
			this.Load += new System.EventHandler(this.MainForm_Load);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new MainForm());
		}

		private void MainForm_Load(object sender, System.EventArgs e) {
			WizardMain wizMain = new WizardMain();
			wizMain.ShowDialog();
		}
	}
}
