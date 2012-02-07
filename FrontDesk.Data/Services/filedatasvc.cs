using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using FrontDesk.Common;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Data.Filesys.Provider;
using FrontDesk.Security;

namespace FrontDesk.Services {

	public class DataEnvelope {
		public DataEnvelope() { }
		public DataEnvelope(byte[] dat, int s) { Data = dat; Size = s; }

		public byte[] Data;
		public int Size;
	}

	/// <summary>
	/// Service for supplying file data
	/// </summary>
	public class FileDataService : TicketService, IFileSystemProvidee {

		private IFileSystemProvider m_fs;

		public FileDataService() {
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if(disposing && components != null) {
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		public void Acquire(IFileSystemProvider provider) {
			m_fs=provider;
		}

		[WebMethod(), SoapHeader("Ticket")]
		public DataEnvelope LoadFileData(int fileID) {

			AuthorizedIdent ident = AuthenticateTicket(Ticket);

			CFile file = new FileSystem(ident, true).GetFile(fileID);

			FileSystemProviderFactory.GetInstance(this);
			m_fs.FetchData(file);

			return new DataEnvelope(file.RawData, file.RawData.Length);
		}

		[WebMethod(), SoapHeader("Ticket")]
		public void CreateFile(int fileID) {
			
			AuthorizedIdent ident = AuthenticateTicket(Ticket);

			CFile file = new FileSystem(ident, true).GetFile(fileID);

			FileSystemProviderFactory.GetInstance(this);
			m_fs.CreateFile(file);
		}

		[WebMethod(), SoapHeader("Ticket")]
		public void DeleteFile(int fileID) {
			
			AuthorizedIdent ident = AuthenticateTicket(Ticket);

			CFile file = new FileSystem(ident, true).GetFile(fileID);

			FileSystemProviderFactory.GetInstance(this);
			m_fs.DeleteFile(file);
		}

		[WebMethod(), SoapHeader("Ticket")]
		public void CommitData(int fileID, byte[] data) {
			
			AuthorizedIdent ident = AuthenticateTicket(Ticket);

			CFile file = new FileSystem(ident, true).GetFile(fileID);

			file.RawData = data;
			FileSystemProviderFactory.GetInstance(this);
			m_fs.CommitData(file);
		}

		[WebMethod(), SoapHeader("Ticket")]
		public void CopyFile(int destID, int srcID) {
			
			AuthorizedIdent ident = AuthenticateTicket(Ticket);

			FileSystem fs = new FileSystem(ident, true);
			CFile src = fs.GetFile(srcID);
			CFile dest = fs.GetFile(destID);

			FileSystemProviderFactory.GetInstance(this);
			m_fs.CopyFile(dest, src);
		}
	}
}
