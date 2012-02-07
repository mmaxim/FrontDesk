package junit.frontdesk;

import java.io.PrintStream;

import junit.framework.*;
import junit.runner.*;

public class FrontDeskRunner extends BaseTestRunner {
	
    private FrontDeskPrinter m_printer;
	
    public static final int SUCCESS_EXIT= 0;
    public static final int FAILURE_EXIT= 1;
    public static final int EXCEPTION_EXIT= 2;
    
    public FrontDeskRunner() {
	this(System.out);
    }
    
    public FrontDeskRunner(PrintStream writer) {
	this(new FrontDeskPrinter(writer));
    }

    public FrontDeskRunner(FrontDeskPrinter printer) {
	m_printer = printer;
    }
    
    /**
     * Runs a suite extracted from a TestCase subclass.
     */
    static public void run(Class testClass) {
	run(new TestSuite(testClass));
    }
    
 
    static public TestResult run(Test test) {
	FrontDeskRunner runner= new FrontDeskRunner();
	return runner.doRun(test);
    }
    
    /**
     * Runs a single test and waits until the user
     * types RETURN.
     */
    static public void runAndWait(Test suite) {
	FrontDeskRunner aTestRunner= new FrontDeskRunner();
	aTestRunner.doRun(suite, true);
    }

    /**
     * Always use the StandardTestSuiteLoader. Overridden from
     * BaseTestRunner.
     */
    public TestSuiteLoader getLoader() {
	return new StandardTestSuiteLoader();
    }

    public void testFailed(int status, Test test, Throwable t) {
    }
	
    public void testStarted(String testName) {
    }
    
    public void testEnded(String testName) {
    }
    
    /**
     * Creates the TestResult to be used for the test run.
     */
    protected TestResult createTestResult() {
	return new FrontDeskTestResult();
    }
    
    public TestResult doRun(Test test) {
	return doRun(test, false);
    }
    
    public TestResult doRun(Test suite, boolean wait) {
	TestResult result= createTestResult();
	result.addListener(m_printer);
	long startTime= System.currentTimeMillis();
	suite.run(result);
	long endTime= System.currentTimeMillis();
	long runTime= endTime-startTime;
	m_printer.print(result, runTime);
	
	pause(wait);
	return result;
    }
    
    protected void pause(boolean wait) {
	if (!wait) return;
	m_printer.printWaitPrompt();
	try {
	    System.in.read();
	}
	catch(Exception e) {
	}
    }
    
    public static void main(String args[]) {
	FrontDeskRunner aTestRunner= new FrontDeskRunner();
	try {
	    TestResult r= aTestRunner.start(args);
	    if (!r.wasSuccessful()) 
		System.exit(FAILURE_EXIT);
	    System.exit(SUCCESS_EXIT);
	} catch(Exception e) {
	    System.err.println(e.getMessage());
	    System.exit(EXCEPTION_EXIT);
	}
    }
    
    /**
     * Starts a test run. Analyzes the command line arguments
     * and runs the given test suite.
     */
    protected TestResult start(String args[]) throws Exception {
	String testCase= "";
	boolean wait= false;
	
	for (int i= 0; i < args.length; i++) {
	    if (args[i].equals("-wait"))
		wait= true;
	    else if (args[i].equals("-c")) 
		testCase= extractClassName(args[++i]);
	    else if (args[i].equals("-v")) {
		System.err.println("JUnit "+Version.id()+" by Kent Beck and Erich Gamma");
		System.err.println("FrontDesk Extension: (c) 2004 Circa Group");
	    }
	    else
		testCase= args[i];
	}
	
	if (testCase.equals("")) 
	    throw new Exception("Usage: FrontDeskRunner [-wait] testCaseName, where name is the name of the TestCase class");
	
	try {
	    Test suite= getTest(testCase);
	    return doRun(suite, wait);
	}
	catch(Exception e) {
	    throw new Exception("Could not create and run test suite: "+e);
	}
    }
    
    protected void runFailed(String message) {
	System.err.println(message);
	System.exit(FAILURE_EXIT);
    }
    
    public void setPrinter(FrontDeskPrinter printer) {
	m_printer = printer;
    }
      
}
