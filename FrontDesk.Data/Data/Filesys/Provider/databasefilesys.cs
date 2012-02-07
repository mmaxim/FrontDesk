using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using FrontDesk.Components.Filesys;
using FrontDesk.Components;
using FrontDesk.Common;

namespace FrontDesk.Data.Filesys.Provider {
	
	/// <summary>
	/// Summary description for databasefilesys.
	/// </summary>
	internal class SqlDatabaseFileSystemProvider : IFileSystemProvider {
		
		public SqlDatabaseFileSystemProvider() {
			
		}	

		public void CreateFile(CFile file) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("INSERT INTO FileData (fileID, data) VALUES (@FileID, @Data)", 
				myConnection);

			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = file.ID;
			myCommand.Parameters.Add(parameter);
			byte[] blank = new byte[1];
			blank[0] = 0x0;
			parameter = new SqlParameter("@Data", SqlDbType.Image, 1);
			parameter.Value = blank;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}

		public void DeleteFile(CFile file) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("DELETE FROM FileData WHERE fileID = @FileID", 
				myConnection);

			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = file.ID;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}

		public void CopyFile(CFile dest, CFile src) {
			
			FetchData(src);

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("UPDATE FileData SET data=@Data WHERE fileID = @FileID", 
				myConnection);

			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = dest.ID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Data", SqlDbType.Image);
			parameter.Value = src.RawData;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}


		/// <summary>
		/// Fetch the data for a file object
		/// </summary>
		public void FetchData(CFile file) {
			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("SELECT data FROM FileData WHERE fileID = @FileID", myConnection);

			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = file.ID;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			SqlDataReader reader = myCommand.ExecuteReader();
			if (!reader.Read()) throw new Exception("File Not Found!");

			byte[] data = (byte[]) reader["data"];
			file.RawData = data;

			reader.Close();
			myConnection.Close();
		}

		/// <summary>
		/// Commit data for a file object
		/// </summary>
		public void CommitData(CFile file) {

			SqlConnection myConnection = new SqlConnection(Globals.DataConnectionString);
			SqlCommand myCommand = 
				new SqlCommand("ipbased.fd_UpdateFileData", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameter = new SqlParameter("@FileID", SqlDbType.Int, 4);
			parameter.Value = file.ID;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@FileData", SqlDbType.Image, file.RawData.Length);
			parameter.Value = file.RawData;
			myCommand.Parameters.Add(parameter);
			parameter = new SqlParameter("@Size", SqlDbType.Int, 4);
			parameter.Value = file.Size;
			myCommand.Parameters.Add(parameter);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}
	}
}
