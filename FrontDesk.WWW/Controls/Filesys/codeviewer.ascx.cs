using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.IO;
using System.Collections;

using FrontDesk.Pages;
using FrontDesk.Common;
using FrontDesk.Data.Access;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Filesys;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	///	Code viewer control
	/// </summary>
	public class CodeFileViewer : Pagelet, IFileViewer {
		
		protected System.Web.UI.WebControls.DataGrid dgCodeView;
		protected System.Web.UI.WebControls.TextBox txtData;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divCode;
		protected ICodeFormatter m_formatter;
		protected System.Web.UI.WebControls.CheckBox chkComments;
		protected System.Web.UI.WebControls.CheckBox chkLines;
		protected Hashtable m_comments, m_linesaffect;

		private void Page_Load(object sender, EventArgs e) {
			
		}

		private void BindData() {

			if (m_formatter == null) {
				int fileid;
				
				if (null == (object)(fileid = (int) ViewState["fileid"])) return;

				CFile file = new FileSystem(Globals.CurrentIdentity).GetFile(fileid);
				LoadFile(file);
				return;
			}

			dgCodeView.DataSource = m_formatter.GetLines();
			dgCodeView.DataBind();
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
			this.dgCodeView.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgCodeView_ItemDataBound);
			this.chkComments.CheckedChanged += new System.EventHandler(this.chkComments_CheckedChanged);
			this.chkLines.CheckedChanged += new System.EventHandler(this.chkLines_CheckedChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public void Edit() {
			divCode.Visible = false;
			dgCodeView.Visible = false;

			int fileid = (int) ViewState["fileid"];
			CFile file = new FileSystem(Globals.CurrentIdentity).GetFile(fileid);
			new FileSystem(Globals.CurrentIdentity).LoadFileData(file);
			txtData.Text = new String(file.Data);
			txtData.Visible = true;
			
		}

		public int FileID {
			get { return (int) ViewState["fileid"]; }
		}

		public void UnEdit() {
			dgCodeView.Visible = true;
			txtData.Visible = false;
			divCode.Visible = true;
		}

		public bool Editable {
			get { return true; }
		}

		public string Data {
			get { return txtData.Text; }
		}

		public void LoadFile(CFile file) {
			
			try {
				m_formatter = 
					CodeFormatterFactory.GetInstance().CreateCodeFormatter(Path.GetExtension(file.Name));
			} catch (Exception) {
				m_formatter = new JavaCodeFormatter();
			}

			CodeFormatterInit finit = new CodeFormatterInit();
			finit.KeywordColor = "#000088";
			finit.CommentColor = "#008800";

			new FileSystem(Globals.CurrentIdentity).LoadFileData(file);
			m_formatter.Init(finit);
			m_formatter.InitLineFormat(new String(file.Data));
			txtData.Text = new String(file.Data);

			LoadComments(file);

			ViewState["fileid"] = file.ID;
			BindData();
		}

		private void LoadComments(CFile file) {
			Result.ResultList cms = 
				new Results(Globals.CurrentIdentity).GetFileResults(file.ID);

			m_comments = new Hashtable(); m_linesaffect = new Hashtable();
			foreach (SubjResult res in cms)  {
				SubjResult.SubjResultList ress = (SubjResult.SubjResultList) m_comments[res.Line];
				if (ress == null)
					m_comments.Add(res.Line, (ress = new SubjResult.SubjResultList()));
				ress.Add(res);
				foreach (int line in res.LinesAffected)
					if (m_linesaffect[line] == null)
						m_linesaffect.Add(line, res);
			}
		}

		private void dgCodeView_ItemDataBound(object sender, DataGridItemEventArgs e) {
			Label lblNum, lblCode;
			SubjResult lres;
			SubjResult.SubjResultList ress;
			if (null != (lblNum = e.Item.FindControl("lblNum") as Label)) {
				lblCode = (Label) e.Item.FindControl("lblCode");

				CodeFormatterLine line = e.Item.DataItem as CodeFormatterLine;
				lblNum.Text = line.Number.ToString();
				lblCode.Text = "&nbsp;" + m_formatter.FormatLine(line.Number);
				if (chkComments.Checked && 
					null != (ress = (SubjResult.SubjResultList) m_comments[line.Number])) {
					foreach (SubjResult res in ress)
						lblCode.Text += BuildCommentHtml(res);
				}
				else if (chkComments.Checked &&
					null != (lres = (SubjResult) m_linesaffect[line.Number])) {
					switch (lres.SubjType) {
					case Rubric.ERROR:
						e.Item.BackColor = Color.Red;
						e.Item.ForeColor = Color.White;
						e.Item.Width = Unit.Percentage(100);
						break;
					case Rubric.WARNING:
						e.Item.BackColor = Color.Yellow;
						e.Item.Width = Unit.Percentage(100);
						break;
					case Rubric.GOOD:
						e.Item.BackColor = Color.Green;
						e.Item.ForeColor = Color.White;
						e.Item.Width = Unit.Percentage(100);
						break;
					};
				}
			}
		}

		private string BuildCommentHtml(SubjResult res) {
			string html="<br><TABLE STYLE=\"font-size: 8pt;\" WIDTH=\"300\" BGCOLOR=\"lightgrey\" BORDERCOLOR=\"black\" "+ 
				        "BORDER=\"1\" CELLSPACING=\"1\" CELLPADDING=\"1\">";
			string image = "<img src=\"";
			string color="#000000";

			switch (res.SubjType) {
			case Rubric.ERROR:
				image += "../../attributes/error.gif\"";
				break;
			case Rubric.WARNING:
				image += "../../attributes/warning.gif\"";
				break;
			case Rubric.GOOD:
				image += "../../attributes/good.gif\"";
				break;
			};
			if (res.Points < 0)
				color = "#ff0000";
			else if (res.Points > 0)
				color = "#008800";

			image += " Align=\"AbsMiddle\">";
			
			html += "<TR>";
			html += "<TD>";
			
			html += image + "<b>[Grader: " + res.Grader + "] " +
				            "[Points: <font color=\"" + color + "\">" + 
							res.Points + "</font>]</b><br>";
			html += "<i>" + res.Comment + "</i>";

			html += "</TD>";
			html += "</TR>";
			html += "</TABLE>";

			return html;
		}

		private void chkLines_CheckedChanged(object sender, System.EventArgs e) {
			dgCodeView.Columns[0].Visible = chkLines.Checked;
			BindData();
		}

		private void chkComments_CheckedChanged(object sender, System.EventArgs e) {
			BindData();
		}
	}
}
