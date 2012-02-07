using System;
using System.IO;

public class RemoveCVS {

    public RemoveCVS() { }
    
    public void DeleteCVS(string odir) {
	string[] dirs = Directory.GetDirectories(odir);
	foreach (string dir in dirs) {
		Console.WriteLine(dir);
	    if (Path.GetFileName(dir) == "CVS")
		Directory.Delete(dir, true);
	    else
		DeleteCVS(Path.Combine(odir, Path.GetFileName(dir))); 
	}
    }
    
    public static void Main(string[] args) {
	new RemoveCVS().DeleteCVS(".");
    }

}
