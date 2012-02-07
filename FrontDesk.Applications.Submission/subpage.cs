using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SMS.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;

using FrontDesk.Applications.Submission.UserService;
using FrontDesk.Applications.Submission.SubmissionService;

namespace FrontDesk.Applications.Submission {

	public class SubmissionPage : InteriorWizardPage {
		private System.Windows.Forms.TextBox txtPath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cmdBrowse;
		private System.Windows.Forms.ListBox lstGroups;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.FolderBrowserDialog fileBrowser;
		private System.Windows.Forms.ErrorProvider erProvider;
		private System.ComponentModel.IContainer components = null;

		public SubmissionPage() {
			InitializeComponent();
		}

		private UserDataService m_uds = new UserDataService();
		private SubmissionService.SubmissionService m_ss = new SubmissionService.SubmissionService();

		class ListGroup {
			public ListGroup(Group g) { Group = g; }
			public Group Group;
			public override string ToString() {
				return Group.GroupName;
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
			this.txtPath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmdBrowse = new System.Windows.Forms.Button();
			this.lstGroups = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.fileBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.erProvider = new System.Windows.Forms.ErrorProvider();
			this.SuspendLayout();
			// 
			// m_titleLabel
			// 
			this.m_titleLabel.Name = "m_titleLabel";
			this.m_titleLabel.Text = "Select Submission Directory";
			// 
			// m_subtitleLabel
			// 
			this.m_subtitleLabel.Name = "m_subtitleLabel";
			this.m_subtitleLabel.Text = "Browse the local hard disk to select your submission directory. Select the direct" +
				"ory where your submission files reside.";
			// 
			// m_headerPanel
			// 
			this.m_headerPanel.Name = "m_headerPanel";
			// 
			// m_headerPicture
			// 
			this.m_headerPicture.Name = "m_headerPicture";
			// 
			// m_headerSeparator
			// 
			this.m_headerSeparator.Name = "m_headerSeparator";
			// 
			// txtPath
			// 
			this.txtPath.Location = new System.Drawing.Point(24, 96);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(368, 20);
			this.txtPath.TabIndex = 5;
			this.txtPath.Text = "";
			this.txtPath.Validating += new System.ComponentModel.CancelEventHandler(this.txtPath_Validating);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Directory Path:";
			// 
			// cmdBrowse
			// 
			this.cmdBrowse.CausesValidation = false;
			this.cmdBrowse.Location = new System.Drawing.Point(400, 96);
			this.cmdBrowse.Name = "cmdBrowse";
			this.cmdBrowse.TabIndex = 7;
			this.cmdBrowse.Text = "Browse";
			this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
			// 
			// lstGroups
			// 
			this.lstGroups.Location = new System.Drawing.Point(24, 144);
			this.lstGroups.Name = "lstGroups";
			this.lstGroups.Size = new System.Drawing.Size(448, 95);
			this.lstGroups.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 128);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 9;
			this.label2.Text = "Submit As:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 288);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(152, 16);
			this.label3.TabIndex = 10;
			this.label3.Text = "Click Next to continue";
			// 
			// erProvider
			// 
			this.erProvider.ContainerControl = this;
			// 
			// SubmissionPage
			// 
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lstGroups);
			this.Controls.Add(this.cmdBrowse);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtPath);
			this.Name = "SubmissionPage";
			this.Load += new System.EventHandler(this.SubmissionPage_Load);
			this.Controls.SetChildIndex(this.txtPath, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.cmdBrowse, 0);
			this.Controls.SetChildIndex(this.lstGroups, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.Controls.SetChildIndex(this.label3, 0);
			this.Controls.SetChildIndex(this.m_headerPanel, 0);
			this.Controls.SetChildIndex(this.m_headerSeparator, 0);
			this.Controls.SetChildIndex(this.m_titleLabel, 0);
			this.Controls.SetChildIndex(this.m_subtitleLabel, 0);
			this.Controls.SetChildIndex(this.m_headerPicture, 0);
			this.ResumeLayout(false);

		}
		#endregion

		private void cmdBrowse_Click(object sender, System.EventArgs e) {
			fileBrowser.ShowDialog();
			txtPath.Text = fileBrowser.SelectedPath;
		}

		protected override string OnWizardNext() {
			Cursor.Current = Cursors.WaitCursor;
			byte[] data = ZipDirectory(txtPath.Text);
			try {
				m_ss.Timeout = 180000;
				m_ss.ZipArchiveSubmit(data, 
					((ListGroup)lstGroups.SelectedItem).Group.PrincipalID,
					((WizardMain)Parent).AsstID);
			} catch (Exception er) {
				MessageBox.Show("Error: " + er.Message,"Error connecting to FrontDesk", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return "SubmissionPage";
			}
			Cursor.Current = Cursors.Arrow;
			return base.OnWizardNext();
		}

		private byte[] ZipDirectory(string path) {
			MemoryStream memstr = new MemoryStream();
	//		FileStream fstr = new FileStream(@"C:\path.zip", FileMode.Create);
			ZipOutputStream zipfile = new ZipOutputStream(memstr);
			ZipDirectory(path, "", zipfile);
			zipfile.Finish();
			zipfile.Close();

			return memstr.GetBuffer();
		}

		private void ZipDirectory(string path, string relpath, ZipOutputStream zipfile) {
			
			string[] files = Directory.GetFiles(path);
			foreach (string sfile in files) {

				FileStream file = File.Open(sfile, FileMode.Open);
		
				byte[] data = new byte[file.Length];
				file.Read(data, 0, (int) file.Length);

				//Create entry
				ZipEntry e = new ZipEntry(Path.Combine(relpath, Path.GetFileName(sfile)));
				e.Size = file.Length;
				zipfile.PutNextEntry(e);
				if (file.Length > 0)
					zipfile.Write(data, 0, (int) file.Length);
			}

			string[] dirs = Directory.GetDirectories(path);
			foreach (string dir in dirs)
				ZipDirectory(Path.Combine(path, Path.GetFileName(dir)),
							 Path.Combine(relpath, Path.GetFileName(dir)),
							 zipfile);
		}

		private void SubmissionPage_Load(object sender, System.EventArgs e) {
		
			ServiceTicket tik = ((WizardMain)Parent).Ticket;
			m_uds.ServiceTicketValue = m_ss.ServiceTicketValue = tik;
			string baseurl = ((WizardMain)Parent).Url;
			m_uds.Url = baseurl + "/userdatasvc.asmx";
			m_ss.Url = baseurl + "/subsvc.asmx";

			User user;
			Group[] groups;
			Cursor.Current = Cursors.WaitCursor;
			try {
				groups = m_uds.GetGroups(tik.Username, ((WizardMain)Parent).AsstID);
				user = m_uds.GetInfo(tik.Username);
			} catch (Exception er) {
				MessageBox.Show("Error: " + er.Message,"Error connecting to FrontDesk", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				Cursor.Current = Cursors.Arrow;
				return;
			}

			lstGroups.BeginUpdate();
			lstGroups.Items.Clear();
			Group guser = new Group();
			guser.GroupName = user.UserName; guser.PrincipalID = user.PrincipalID;
			lstGroups.Items.Add(new ListGroup(guser));
			foreach (Group group in groups)
				lstGroups.Items.Add(new ListGroup(group));
			lstGroups.EndUpdate();

			lstGroups.SelectedIndex = 0;
			Cursor.Current = Cursors.Arrow;
		}

		private void txtPath_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			try {
				if (txtPath.Text.Length == 0)
					throw new Exception("Please enter a directory path");

				Directory.GetFiles(txtPath.Text);
			} catch (DirectoryNotFoundException) {
				erProvider.SetError(txtPath, "Specified path not found");
				e.Cancel = true;
			} catch (Exception er) {
				erProvider.SetError(txtPath, er.Message);
				e.Cancel = true;
			}
		}
	}
}

