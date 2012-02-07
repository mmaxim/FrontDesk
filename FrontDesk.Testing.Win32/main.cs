using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using FrontDesk.Testing;
using FrontDesk.Testing.Logging;

namespace FrontDesk.Testing.Win32 {

	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : Form {
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.GroupBox grpStats;
		private System.Windows.Forms.Label lblJobs;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ListView lstMessages;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.Button cmdStop;
		private System.Windows.Forms.Button cmdStart;
		private System.Windows.Forms.Label lblMike;
		private System.Windows.Forms.Label lblUsername;
		private System.Windows.Forms.Label lblIP;
		private System.Windows.Forms.ColumnHeader colType;
		private System.Windows.Forms.ColumnHeader colTime;
		private System.Windows.Forms.ColumnHeader colMessage;
		private System.Windows.Forms.Label lblDatabase;
		private System.Windows.Forms.Label lblFileSys;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.MenuItem miStart;
		private System.Windows.Forms.MenuItem miStop;
		private System.Windows.Forms.MenuItem miExit;

		private TestCenter m_testcenter;

		public MainForm() {
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) 
					components.Dispose();
			}

			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.miStart = new System.Windows.Forms.MenuItem();
			this.miStop = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.miExit = new System.Windows.Forms.MenuItem();
			this.cmdStop = new System.Windows.Forms.Button();
			this.grpStats = new System.Windows.Forms.GroupBox();
			this.lblFileSys = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblDatabase = new System.Windows.Forms.Label();
			this.lblIP = new System.Windows.Forms.Label();
			this.lblUsername = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblMike = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblJobs = new System.Windows.Forms.Label();
			this.lstMessages = new System.Windows.Forms.ListView();
			this.colType = new System.Windows.Forms.ColumnHeader();
			this.colTime = new System.Windows.Forms.ColumnHeader();
			this.colMessage = new System.Windows.Forms.ColumnHeader();
			this.cmdStart = new System.Windows.Forms.Button();
			this.grpStats.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem8,
																					  this.menuItem4,
																					  this.menuItem5,
																					  this.miExit});
			this.menuItem1.Text = "File";
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 0;
			this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.miStart,
																					  this.miStop});
			this.menuItem8.Text = "State";
			// 
			// miStart
			// 
			this.miStart.Index = 0;
			this.miStart.Text = "Start";
			this.miStart.Click += new System.EventHandler(this.miStart_Click);
			// 
			// miStop
			// 
			this.miStop.Index = 1;
			this.miStop.Text = "Stop";
			this.miStop.Click += new System.EventHandler(this.miStop_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem6,
																					  this.menuItem7});
			this.menuItem4.Text = "Logging";
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 0;
			this.menuItem6.Text = "Interactive";
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 1;
			this.menuItem7.Text = "Database";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 2;
			this.menuItem5.Text = "-";
			// 
			// miExit
			// 
			this.miExit.Index = 3;
			this.miExit.Text = "Exit";
			this.miExit.Click += new System.EventHandler(this.miExit_Click);
			// 
			// cmdStop
			// 
			this.cmdStop.Enabled = false;
			this.cmdStop.Location = new System.Drawing.Point(128, 592);
			this.cmdStop.Name = "cmdStop";
			this.cmdStop.TabIndex = 2;
			this.cmdStop.Text = "Stop";
			this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
			// 
			// grpStats
			// 
			this.grpStats.Controls.Add(this.lblFileSys);
			this.grpStats.Controls.Add(this.label4);
			this.grpStats.Controls.Add(this.lblDatabase);
			this.grpStats.Controls.Add(this.lblIP);
			this.grpStats.Controls.Add(this.lblUsername);
			this.grpStats.Controls.Add(this.label7);
			this.grpStats.Controls.Add(this.label6);
			this.grpStats.Controls.Add(this.label5);
			this.grpStats.Controls.Add(this.lblMike);
			this.grpStats.Controls.Add(this.label3);
			this.grpStats.Controls.Add(this.label1);
			this.grpStats.Controls.Add(this.lblJobs);
			this.grpStats.Location = new System.Drawing.Point(8, 480);
			this.grpStats.Name = "grpStats";
			this.grpStats.Size = new System.Drawing.Size(720, 104);
			this.grpStats.TabIndex = 4;
			this.grpStats.TabStop = false;
			this.grpStats.Text = "Server Status";
			// 
			// lblFileSys
			// 
			this.lblFileSys.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(71)), ((System.Byte)(104)), ((System.Byte)(163)));
			this.lblFileSys.Location = new System.Drawing.Point(248, 80);
			this.lblFileSys.Name = "lblFileSys";
			this.lblFileSys.Size = new System.Drawing.Size(255, 13);
			this.lblFileSys.TabIndex = 11;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(184, 79);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 16);
			this.label4.TabIndex = 10;
			this.label4.Text = "File System:";
			// 
			// lblDatabase
			// 
			this.lblDatabase.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(71)), ((System.Byte)(104)), ((System.Byte)(163)));
			this.lblDatabase.Location = new System.Drawing.Point(248, 58);
			this.lblDatabase.Name = "lblDatabase";
			this.lblDatabase.Size = new System.Drawing.Size(255, 13);
			this.lblDatabase.TabIndex = 9;
			// 
			// lblIP
			// 
			this.lblIP.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(71)), ((System.Byte)(104)), ((System.Byte)(163)));
			this.lblIP.Location = new System.Drawing.Point(248, 37);
			this.lblIP.Name = "lblIP";
			this.lblIP.Size = new System.Drawing.Size(255, 12);
			this.lblIP.TabIndex = 8;
			// 
			// lblUsername
			// 
			this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(71)), ((System.Byte)(104)), ((System.Byte)(163)));
			this.lblUsername.Location = new System.Drawing.Point(248, 16);
			this.lblUsername.Name = "lblUsername";
			this.lblUsername.Size = new System.Drawing.Size(255, 12);
			this.lblUsername.TabIndex = 7;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(537, 17);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(68, 16);
			this.label7.TabIndex = 6;
			this.label7.Text = "Description:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 64);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "Average Time:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(184, 58);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Database:";
			// 
			// lblMike
			// 
			this.lblMike.Location = new System.Drawing.Point(184, 16);
			this.lblMike.Name = "lblMike";
			this.lblMike.Size = new System.Drawing.Size(63, 16);
			this.lblMike.TabIndex = 3;
			this.lblMike.Text = "Username:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(184, 37);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "IP Address:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Processing:";
			// 
			// lblJobs
			// 
			this.lblJobs.Location = new System.Drawing.Point(8, 16);
			this.lblJobs.Name = "lblJobs";
			this.lblJobs.Size = new System.Drawing.Size(88, 16);
			this.lblJobs.TabIndex = 0;
			this.lblJobs.Text = "Jobs Processed:";
			// 
			// lstMessages
			// 
			this.lstMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.colType,
																						  this.colTime,
																						  this.colMessage});
			this.lstMessages.Location = new System.Drawing.Point(8, 16);
			this.lstMessages.Name = "lstMessages";
			this.lstMessages.Size = new System.Drawing.Size(723, 456);
			this.lstMessages.TabIndex = 5;
			this.lstMessages.View = System.Windows.Forms.View.Details;
			// 
			// colType
			// 
			this.colType.Text = "Type";
			this.colType.Width = 63;
			// 
			// colTime
			// 
			this.colTime.Text = "Time";
			this.colTime.Width = 136;
			// 
			// colMessage
			// 
			this.colMessage.Text = "Message";
			this.colMessage.Width = 567;
			// 
			// cmdStart
			// 
			this.cmdStart.Location = new System.Drawing.Point(40, 592);
			this.cmdStart.Name = "cmdStart";
			this.cmdStart.TabIndex = 6;
			this.cmdStart.Text = "Start";
			this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(747, 626);
			this.Controls.Add(this.cmdStart);
			this.Controls.Add(this.lstMessages);
			this.Controls.Add(this.grpStats);
			this.Controls.Add(this.cmdStop);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.mainMenu1;
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "FrontDesk Testing Center";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Closed += new System.EventHandler(this.MainForm_Closed);
			this.grpStats.ResumeLayout(false);
			this.ResumeLayout(false);

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

			//Register log event handler
			TestCenter.Logger.LogEnterred += 
				new TestLogger.LogEventHandler(Logger_LogEnterred);

			//Grab the center
			m_testcenter = TestCenter.GetInstance();

			//Init the image list
			lstMessages.SmallImageList = new ImageList();
			lstMessages.SmallImageList.Images.Add(new Bitmap("error.bmp"));
			lstMessages.SmallImageList.Images.Add(new Bitmap("info.bmp"));
			lstMessages.SmallImageList.Images.Add(new Bitmap("warning.bmp"));

			UpdateStatus();
		}

		private void Logger_LogEnterred(object sender, LogEventArgs args) {
			
			ListViewItem item = new ListViewItem();
		
			lock (this) {
				switch (args.Type) {
					case TestLogger.LogType.INFORMATION:
						item.ImageIndex = 1;
						item.Text = "Info";
						break;
					case TestLogger.LogType.WARNING:
						item.ImageIndex = 2;
						item.Text = "Warning";
						break;
					case TestLogger.LogType.ERROR:
						item.ImageIndex = 0;
						item.Text = "Error";
						break;
				}
			
				item.SubItems.Add(args.Time.ToString());
				item.SubItems.Add(args.Message);

				lstMessages.Items.Add(item);
				lstMessages.Refresh();
			}
		}

		private void UpdateStatus() {
			lblUsername.Text = m_testcenter.Identity.Name;
			lblIP.Text = m_testcenter.IPAddress;
			lblDatabase.Text = TestConfig.DatabaseServerAddress;
			lblFileSys.Text = TestConfig.FileSystemAddress;
			if (m_testcenter.CenterStatus == TestCenter.Status.IDLE) {
				cmdStart.Enabled = miStart.Enabled = true;
				cmdStop.Enabled = miStop.Enabled = false;
			}
			else {
				cmdStart.Enabled = miStart.Enabled = false;
				cmdStop.Enabled = miStop.Enabled = true;
			}
		}

		private void cmdStart_Click(object sender, System.EventArgs e) {
			Start();
		}

		private void Start() {
			//Show the authentication form
			Form form = new AuthForm();
			DialogResult res = form.ShowDialog(this);

			if (res == DialogResult.OK) {
				//Start the test center
				m_testcenter.StartTestWorker();
				UpdateStatus();
			}
		}

		private void Stop() {
			this.Cursor = Cursors.WaitCursor;
			m_testcenter.ShutdownTestWorker(true);
			this.Cursor = Cursors.Arrow;
			UpdateStatus();
		}

		private void Shutdown() {
			m_testcenter.ShutdownTestWorker(false);
			m_testcenter.Dispose();
			Application.Exit();
		}

		private void MainForm_Closed(object sender, System.EventArgs e) {
			Shutdown();
		}

		private void cmdStop_Click(object sender, System.EventArgs e) {
			Stop();
		}

		private void miStart_Click(object sender, System.EventArgs e) {
			Start();
		}

		private void miStop_Click(object sender, System.EventArgs e) {
			Stop();
		}

		private void miExit_Click(object sender, System.EventArgs e) {
			Shutdown();
		}

	}
}
