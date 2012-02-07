using System;
using System.Web;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Configuration;
using System.Reflection;
using System.Data;

using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Filesys;

namespace FrontDesk.Data.Filesys.Provider {

	internal class FileSystemProviderFactory {

		protected static IFileSystemProvider m_instance=null;

		private static IFileSystemProvider LoadFromAssembly(string asmname, string classname, 
															string cparam) {
			IFileSystemProvider fs;
			Assembly asm = Assembly.LoadFrom(asmname);
			Type[] tcparams = new Type[1];
			tcparams[0] = typeof(string);
			string[] cparams = new string[1];
			cparams[0] = cparam;

			if (cparam != null) 
				fs = (IFileSystemProvider) asm.GetType(classname).GetConstructor(tcparams).Invoke(cparams);
			else
				fs = (IFileSystemProvider) asm.GetType(classname).GetConstructor(new Type[0]).Invoke(null);

			return fs;
		}

		/// <summary>
		/// Provide an instance of the configured provider
		/// </summary>
		/// <returns></returns>
		public static void GetInstance(IFileSystemProvidee provi) {

			String assemblyPath = ConfigurationSettings.AppSettings["FileSystemProviderAssemblyPath"];
			String classStr = ConfigurationSettings.AppSettings["FileSystemProviderClassName"];
			
			string[] tokens = classStr.Split(";".ToCharArray());
			string cparam;
			string className = tokens[0];	
			if (tokens.Length > 1)
				cparam = tokens[1];
			else
				cparam = null;

			//use the cache because the reflection used later is expensive
			if (Globals.Context == null) {
				if (m_instance == null)
					provi.Acquire((m_instance = LoadFromAssembly(assemblyPath, className, cparam)));
				else
					provi.Acquire(m_instance);
			}
			else {
				//use the cache because the reflection used later is expensive
				Cache cache = Globals.Context.Cache;

				if (null == cache["IFileSystemProvider"]) {

					// assemblyPath presented in virtual form, must convert to physical path
					assemblyPath = Globals.Context.Server.MapPath(Globals.Context.Request.ApplicationPath + "/bin/" + assemblyPath);					

					// Uuse reflection to store the constructor of the class that implements IWebForumDataProvider
					try {
						IFileSystemProvider fs = LoadFromAssembly(assemblyPath, className, cparam);
						cache.Insert("IFileSystemProvider", fs, new CacheDependency(assemblyPath));
					}
					catch (Exception) {
						// could not locate DLL file
						HttpContext.Current.Response.Write("<b>ERROR:</b> Could not locate file: <code>" + assemblyPath + "</code> or could not locate class <code>" + className + "</code> in file.");
						HttpContext.Current.Response.End();
					}
				}
			
				provi.Acquire((IFileSystemProvider)(cache["IFileSystemProvider"]));
			}
		}
	}

	/// <summary>
	/// Interface for file system providers
	/// </summary>
	public interface IFileSystemProvider {

		/// <summary>
		/// Create a file
		/// </summary>
		void CreateFile(CFile file);

		/// <summary>
		/// Delete a file
		/// </summary>
		void DeleteFile(CFile file);

		/// <summary>
		/// Copy a file
		/// </summary>
		void CopyFile(CFile dest, CFile src);

		/// <summary>
		/// Fetch the data for a file object
		/// </summary>
		void FetchData(CFile file);

		/// <summary>
		/// Commit data for a file object
		/// </summary>
		void CommitData(CFile file);

	}
}
