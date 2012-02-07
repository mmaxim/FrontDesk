using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SMS.Windows.Forms;

using FrontDesk.Applications.Submission.CourseService;
using FrontDesk.Applications.Submission.UserService;

namespace FrontDesk.Applications.Submission {

	public class SetupPage : InteriorWizardPage {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox lstCourses;
		private System.Windows.Forms.ListBox lstAssts;
		private System.ComponentModel.IContainer components = null;

		private CourseDataService m_cds = new CourseDataService();
		private UserDataService m_uds = new UserDataService();

		public SetupPage() {
			InitializeComponent();
		}

		class ListAsst {
			public ListAsst(Assignment c) { Asst = c; }
			public Assignment Asst;
			public override string ToString() {
				return Asst.Description + " (Due: " + Asst.DueDate + ")";
			}
		}

		class ListCourse {
			public ListCourse(Course c) { Course = c; }
			public Course Course;
			public override string ToString() {
				return Course.Number + " " + Course.Name;
			}
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

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SetupPage));
			this.lstCourses = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lstAssts = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_titleLabel
			// 
			this.m_titleLabel.Name = "m_titleLabel";
			this.m_titleLabel.Text = "Select Course and Assignment";
			// 
			// m_subtitleLabel
			// 
			this.m_subtitleLabel.Name = "m_subtitleLabel";
			this.m_subtitleLabel.Text = "Select the course and assignment you wish to submit for. Make sure this is correc" +
				"t, otherwise someone else will get your work!";
			// 
			// m_headerPanel
			// 
			this.m_headerPanel.Name = "m_headerPanel";
			// 
			// m_headerPicture
			// 
			this.m_headerPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_headerPicture.Image")));
			this.m_headerPicture.Name = "m_headerPicture";
			// 
			// m_headerSeparator
			// 
			this.m_headerSeparator.Name = "m_headerSeparator";
			// 
			// lstCourses
			// 
			this.lstCourses.Location = new System.Drawing.Point(24, 88);
			this.lstCourses.Name = "lstCourses";
			this.lstCourses.Size = new System.Drawing.Size(448, 56);
			this.lstCourses.TabIndex = 5;
			this.lstCourses.SelectedIndexChanged += new System.EventHandler(this.lstCourses_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 72);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Courses:";
			// 
			// lstAssts
			// 
			this.lstAssts.Location = new System.Drawing.Point(24, 168);
			this.lstAssts.Name = "lstAssts";
			this.lstAssts.Size = new System.Drawing.Size(448, 134);
			this.lstAssts.TabIndex = 7;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 152);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "Assignments:";
			// 
			// SetupPage
			// 
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lstAssts);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lstCourses);
			this.Name = "SetupPage";
			this.Load += new System.EventHandler(this.SetupPage_Load);
			this.Controls.SetChildIndex(this.lstCourses, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.lstAssts, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.Controls.SetChildIndex(this.m_headerPanel, 0);
			this.Controls.SetChildIndex(this.m_headerSeparator, 0);
			this.Controls.SetChildIndex(this.m_titleLabel, 0);
			this.Controls.SetChildIndex(this.m_subtitleLabel, 0);
			this.Controls.SetChildIndex(this.m_headerPicture, 0);
			this.ResumeLayout(false);

		}
		#endregion

		protected override string OnWizardNext() {

			ListAsst asst = (ListAsst) lstAssts.SelectedItem;
			((WizardMain)Parent).AsstID = asst.Asst.ID;

			return base.OnWizardNext();
		}


		private void SetupPage_Load(object sender, System.EventArgs e) {

			ServiceTicket tik = ((WizardMain)Parent).Ticket;
			m_uds.ServiceTicketValue = tik;
			m_cds.ServiceTicketValue = tik;
			string baseurl = ((WizardMain)Parent).Url;
			m_uds.Url = baseurl + "/userdatasvc.asmx";
			m_cds.Url = baseurl + "/coursedatasvc.asmx";
	
			Cursor.Current = Cursors.WaitCursor;
			Course[] mems;
			try {
				mems = m_uds.GetCourses(tik.Username);
			} catch (Exception er) {
				MessageBox.Show("Error: " + er.Message,"Error connecting to FrontDesk", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				Cursor.Current = Cursors.Arrow;
				return;
			}

			lstCourses.BeginUpdate();
			foreach (Course c in mems) 
				lstCourses.Items.Add(new ListCourse(c));
			lstCourses.EndUpdate();
			
			if (lstCourses.Items.Count == 0)
				Wizard.SetWizardButtons(WizardButton.Back);
			else { 
				Wizard.SetWizardButtons(WizardButton.Back | WizardButton.Next);
				lstCourses.SelectedIndex = 0;
				LoadAsstList();   
			}

			Cursor.Current = Cursors.Arrow;
		}

		private void LoadAsstList() {
			
			ListCourse course = (ListCourse) lstCourses.SelectedItem;
			Assignment[] assts;	
			try {
				assts = m_cds.GetAssignments(course.Course.ID);
			} catch (Exception er) {
				MessageBox.Show("Error: " + er.Message,"Error connecting to FrontDesk", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			lstAssts.BeginUpdate();
			lstAssts.Items.Clear();
			foreach (Assignment asst in assts) 
				lstAssts.Items.Add(new ListAsst(asst));
			lstAssts.EndUpdate();

			if (lstAssts.Items.Count == 0)
				Wizard.SetWizardButtons(WizardButton.Back);
			else {
				Wizard.SetWizardButtons(WizardButton.Back | WizardButton.Next);
				lstAssts.SelectedIndex = 0;
			}
		}

		private void lstCourses_SelectedIndexChanged(object sender, System.EventArgs e) {
			Cursor.Current = Cursors.WaitCursor;
			LoadAsstList();
			Cursor.Current = Cursors.Arrow;
		}
	}
}

