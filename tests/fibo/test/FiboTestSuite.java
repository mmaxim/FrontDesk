//Mike Maxim
//Fibonacci test suite

import junit.framework.*;
import junit.frontdesk.*;
import junit.textui.*;

public class FiboTestSuite extends FrontDeskTestCase {

    public FiboTestSuite() { }

    public FiboTestSuite(String name, double fpoints, double epoints, int timelimit) {
	super(name, fpoints, epoints, timelimit);
    }

    protected Fibo fibo = new Fibo();

    public static Test suite() {
	
	TestSuite suite= new TestSuite();
	suite.addTest(new FiboTestSuite("F(10)", 4.0, 20.0, 5) {
			      public void runTest() { testF10(); } } );
	suite.addTest(new FiboTestSuite("F(8)", 4.0, 20.0, 5) {
			      public void runTest() { testF8(); } } );
	suite.addTest(new FiboTestSuite("F(12)", 4.0, 20.0, 5) {
			      public void runTest() { testF12(); } } );
	suite.addTest(new FiboTestSuite("F(1)", 4.0, 20.0, 5) {
			      public void runTest() { testF1(); } } );
	suite.addTest(new FiboTestSuite("F(0)", 4.0, 20.0, 5) {
			      public void runTest() { testF0(); } } );
	
	return suite;
    }

    public void testF10() {
	assertEquals("F(10)", fibo.fibo(10), 89);
	setCompetitionScore(10);
    }

    public void testF8() {
	assertEquals("F(8)", fibo.fibo(8), 34);
	setCompetitionScore(12);
    }

    public void testF12() {
	assertEquals("F(12)", fibo.fibo(12), 233);
    }

    public void testF1() {
	assertEquals("F(1)", fibo.fibo(1), 1);
    }

    public void testF0() {
	assertEquals("F(0)", fibo.fibo(0), 1);
    }
}
