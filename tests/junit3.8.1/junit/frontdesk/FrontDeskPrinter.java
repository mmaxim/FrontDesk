//Mike Maxim
//FrontDesk XML printer
package junit.frontdesk;

import java.io.PrintStream;
import java.text.NumberFormat;
import java.util.Enumeration;

import junit.framework.AssertionFailedError;
import junit.framework.Test;
import junit.framework.TestFailure;
import junit.framework.TestListener;
import junit.framework.TestResult;
import junit.runner.BaseTestRunner;

public class FrontDeskPrinter implements TestListener {
	
    PrintStream fWriter;
    static String m_msg="";
    int fColumn= 0;
	
    public FrontDeskPrinter(PrintStream writer) {
	fWriter= writer;
    }
    
    /* API for use by textui.TestRunner
     */
    
    synchronized void print(TestResult result, long runTime) {
	printHeader(result, runTime);
	printErrors(result);
	printFailures(result);
	printFooter(result);
    }
    
    void printWaitPrompt() {
	getWriter().println();
	getWriter().println("<RETURN> to continue");
    }
    
    /* Internal methods 
     */

    public void setMessage(String msg) {
	m_msg = msg;
    }
    
    protected void printHeader(TestResult result, long runTime) {
	getWriter().println("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
	getWriter().println("<Result xmlns=\"urn:frontdesk-result\">");

	if (result.wasSuccessful())
	    getWriter().println("\t<Success>flawless</Success>");
	else
	    getWriter().println("\t<Success>flawed</Success>");

	getWriter().println("\t<Time>"+elapsedTimeAsString(runTime)+"</Time>");
	getWriter().println("\t<Count>"+result.runCount()+"</Count>");
	getWriter().println("\t<Msg>"+clean(m_msg)+"</Msg>");
	
	getWriter().println("\t<API>JUnit 3.8.1</API>");
	getWriter().println("\t<Comp>" + 
			    ((FrontDeskTestResult)result).getCompetitionScore() +
			    "</Comp>");
    }
    
    protected void printErrors(TestResult result) {
	printDefects(result.errors(), result.errorCount(), "error");
    }
    
    protected void printFailures(TestResult result) {
	printDefects(result.failures(), result.failureCount(), "failure");
    }
    
    protected void printDefects(Enumeration booBoos, int count, String type) {
	if (count == 0) return;
	
	for (int i= 1; booBoos.hasMoreElements(); i++) {
	    if (type.equals("error"))
		printError((TestFailure) booBoos.nextElement());
	    else
		printFailure((TestFailure) booBoos.nextElement());
	}
    }
    
    public void printError(TestFailure err) { 
	FrontDeskTestCase test = (FrontDeskTestCase) err.failedTest();
	getWriter().println("\t<Error>");
	getWriter().println("\t\t<Name>" + clean(test.toString()) + "</Name>");
	getWriter().println("\t\t<Points>" + test.getErrorPoints() + "</Points>");
	getWriter().println("\t\t<Message>" + clean(BaseTestRunner.getFilteredTrace(err.trace())) + "</Message>");
	getWriter().println("\t</Error>");
    }

    public void printFailure(TestFailure err) { 
	FrontDeskTestCase test = (FrontDeskTestCase) err.failedTest();
	getWriter().println("\t<Failure>");
	getWriter().println("\t\t<Name>" + clean(test.toString()) + "</Name>");
	getWriter().println("\t\t<Points>" + test.getFailPoints() + "</Points>");
	getWriter().println("\t\t<Message>" + clean(err.exceptionMessage()) + "</Message>");
	getWriter().println("\t</Failure>");
    }
    
    protected String clean(String str) {

	if (str == null)
	    return "Failure";

	str = str.replaceAll("<", "(");
	str = str.replaceAll(">", ")");

	return str;
    }
    
    protected void printFooter(TestResult result) {
	getWriter().println("</Result>");
    }
    
    
    /**
     * Returns the formatted string of the elapsed time.
     * Duplicated from BaseTestRunner. Fix it.
     */
    protected String elapsedTimeAsString(long runTime) {
	return NumberFormat.getInstance().format((double)runTime/1000);
    }
    
    public PrintStream getWriter() {
	return fWriter;
    }
    /**
     * @see junit.framework.TestListener#addError(Test, Throwable)
     */
    public void addError(Test test, Throwable t) {
	
    }
    
    /**
     * @see junit.framework.TestListener#addFailure(Test, AssertionFailedError)
     */
    public void addFailure(Test test, AssertionFailedError t) {
	
    }
    
    /**
     * @see junit.framework.TestListener#endTest(Test)
     */
    public void endTest(Test test) {
    }
    
    /**
     * @see junit.framework.TestListener#startTest(Test)
     */
    public void startTest(Test test) {
	
    }
}
