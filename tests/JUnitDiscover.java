//Mike Maxim
//JUnit test suite discoverer

import java.util.*;
import java.lang.reflect.*;
import java.io.*;
import junit.frontdesk.*;
import junit.framework.*;

public class JUnitDiscover {

    public JUnitDiscover() { }

    class ClassFileFilter implements FilenameFilter {
	public ClassFileFilter() { }

	public boolean accept(File dir, String name) {
	    String[] tokens = name.split("\\.");
	    if (tokens.length > 0) 
		return (tokens[tokens.length-1].equals("class"));
	    else
		return false;
	}
    }

    public static void main(String[] args) throws Exception {
	if (args.length < 1) return;
	
	if (args[0].equals("i"))
	    new JUnitDiscover().inspect();
	else
	    new JUnitDiscover().run();
    }

    public void run() throws Exception {
	runSuite(createJUnitSuite());
    }

    public void inspect() throws Exception {
	sendTests(System.out, createJUnitSuite());
    }

    private FrontDeskTestCase createJUnitSuite() throws Exception {
	String wrkdir = System.getProperty("user.dir");
	Class fdtc = Class.forName("junit.frontdesk.FrontDeskTestCase");
	File[] classfiles = new File(wrkdir).listFiles(new ClassFileFilter());
	int i;
	for (i = 0; i < classfiles.length; i++) {
	    Class c = Class.forName(classfiles[i].getName().split("\\.")[0]);
	    try {
		if (fdtc.isAssignableFrom(c) &&
		    c.getMethod("suite", null) != null)
		    return (FrontDeskTestCase) c.newInstance();
	    } catch (Exception e) { }
	}
	return null;
    }

    private void sendTests(PrintStream out, FrontDeskTestCase csuite) throws Exception {

	if (csuite == null) {
	    out.println("Could not find a test suite");
	    return;
	}

	Method smethod = csuite.getClass().getMethod("suite", null);
	TestSuite suite = (TestSuite) smethod.invoke(csuite, null);
	
	out.println("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
	out.println("<TestSuite xmlns=\"urn:frontdesk-result\">");
	for (Enumeration tests = suite.tests(); tests.hasMoreElements() ; ) {
	    FrontDeskTestCase test = (FrontDeskTestCase) tests.nextElement();
	    out.println("\t<Test>");
	    out.println("\t\t<Name>" + test.getName() + "</Name>");
	    out.println("\t\t<Points>" + test.getFailPoints() + "</Points>");
	    out.println("\t\t<Time>" + test.getTimeLimit()/1000 + "</Time>");
	    out.println("\t</Test>");
	}
	out.println("</TestSuite>");
    }

    private void runSuite(FrontDeskTestCase fdtc) throws Exception {

	Method smethod = fdtc.getClass().getMethod("suite", null);
	TestSuite suite = (TestSuite) smethod.invoke(fdtc, null);

	FrontDeskPrinter fdp = new FrontDeskPrinter(System.out);
	System.setOut(new PrintStream(new FileOutputStream("out.txt")));
       	FrontDeskRunner runner = new FrontDeskRunner(fdp);
	runner.doRun(suite);
	new File("out.txt").delete();
    }
}
