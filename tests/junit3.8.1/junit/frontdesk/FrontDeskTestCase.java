//Mike Maxim
//FrontDesk test case

package junit.frontdesk;

import junit.framework.*;

class InfiniteLoopException extends Exception {
    public InfiniteLoopException() { super("Test exceeded the time limit. Possible infinite loop"); }
}

class ProtectedTestCase implements Protectable {

    class TestTimer implements Runnable {
	
	protected ProtectedTestCase m_test;
	
	public TestTimer(ProtectedTestCase test) {
	    m_test = test;
	}
	
	public void run() {
	    Throwable t=null;
	    try {
		m_test.getTest().runBare();
	    } catch (Throwable th) { 
		t=th;
	    }
	    m_test.finished(t);
	}
    }

    protected TestCase m_test;
    protected boolean m_infinite;
    protected int m_timelimit;
    protected Throwable m_exception;
    
    public ProtectedTestCase(TestCase test, int timelimit) 
    { m_test = test; m_timelimit = timelimit; }

    public TestCase getTest() {
	return m_test;
    }

    public synchronized void finished(Throwable t) {
	m_infinite = false;
	m_exception = t;
	notify();
    }
    
    public synchronized void protect() throws Throwable {
	Thread thr;
	m_infinite = true; m_exception = null;
	(thr = new Thread(new TestTimer(this))).start();
	wait(m_timelimit);
	if (m_infinite) {
	    thr.stop();
	    throw new InfiniteLoopException();
	} else if (m_exception != null)
	    throw m_exception;
    }
}

public abstract class FrontDeskTestCase extends TestCase {

    protected boolean m_hascomp=false;
    protected double m_fpoints, m_epoints, m_comp;
    protected int m_timelimit;

    public FrontDeskTestCase() {
	m_fpoints=m_epoints=m_comp=0; m_timelimit = 60000; m_hascomp = false; }

    public FrontDeskTestCase(String name, double fpoints, double epoints, int timelimit) {
	super(name); m_fpoints = fpoints; m_epoints = epoints; m_timelimit = 1000*timelimit;
	m_comp=0; m_hascomp = false;
    }

    public double getFailPoints() {
	return m_fpoints;
    }

    public double getErrorPoints() {
	return m_epoints;
    }

    public int getTimeLimit() {
	return m_timelimit;
    }

    protected double getCompetitionScore() {
	return m_comp;
    }

    protected void setCompetitionScore(double score) {
	m_comp = score;
	m_hascomp = true;
    }

    public void run(TestResult result) {
	result.startTest(this);
	result.runProtected(this, new ProtectedTestCase(this, m_timelimit));
	result.endTest(this);
	if (m_hascomp)
	    ((FrontDeskTestResult)result).setCompetitionScore(m_comp);
    }
}
