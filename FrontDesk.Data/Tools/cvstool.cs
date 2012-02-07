using System;
using System.IO;

using ICSharpCode.SharpCvsLib;
using ICSharpCode.SharpCvsLib.Client;
using ICSharpCode.SharpCvsLib.Misc;
using ICSharpCode.SharpCvsLib.FileSystem;
using ICSharpCode.SharpCvsLib.Exceptions;
using ICSharpCode.SharpCvsLib.Commands;

using FrontDesk.Common;
using FrontDesk.Data.Filesys;

namespace FrontDesk.Tools {

	/// <summary>
	/// CVS interface to the cvs.exe program
	/// </summary>
	public class CVSTool {

		public CVSTool() { 
			
		}

		public IExternalSource Checkout(string cvsroot, string module, string password, 
			out string target) {
			
			//Set temp checkout dir
			target = System.IO.Path.Combine(Globals.TempDirectory, Guid.NewGuid().ToString());
			Directory.CreateDirectory(target);
			
			//Setup CVS vars
			CvsRoot root = new CvsRoot(cvsroot);
			WorkingDirectory working = new WorkingDirectory(root, target, module);

			CVSServerConnection connection = new CVSServerConnection();
			if (connection == null)
				throw new ToolExecutionException("Unable to connect to the CVS server");

			//Connect to CVS
			ICSharpCode.SharpCvsLib.Commands.ICommand command = 
				new CheckoutModuleCommand(working);
			if (command == null)
				throw new ToolExecutionException("Failure to create a checkout command object");
			try {
				connection.Connect(working, password);
			} catch (AuthenticationException) {
				throw new ToolExecutionException("CVS rejected access (doublecheck all fields): Authentication failure");
			} catch (Exception er) {
				throw new ToolExecutionException("CVS rejected access (doublecheck all fields): " + er.Message);
			}

			//Execute checkout command
			try {
				command.Execute(connection);
				connection.Close();
			} catch (Exception er) {
				throw new ToolExecutionException("CVS error: " + er.Message);
			}

			//Create source from module root
			return CreateSource(Path.Combine(target, module));
		}

		private IExternalSource CreateSource(string basedir) {
			OSDirectorySource osdirsrc = new OSDirectorySource();
			osdirsrc.CreateSource(basedir);
			return osdirsrc;
		}
	}
}
