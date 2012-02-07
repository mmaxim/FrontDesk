
import java.util.*;
import java.io.*;

import junit.framework.Assert;
import junit.framework.Test;
import junit.framework.TestSuite;
import junit.frontdesk.FrontDeskTestCase;

/**
 * @author David Murray
 */
public class HW4TestN extends FrontDeskTestCase {

    Iterator i;
    
    String temp;

    public HW4TestN(String name, double fpoints, double epoints, int timelimit) {
        super(name, fpoints, epoints, timelimit);
    }

    public HW4TestN() {
        super();
    }

    public static Test suite() {

        TestSuite suite = new TestSuite();

        suite
                .addTest(new HW4TestN(
                        "testWebSearchNrank() - Runs the webcrawler for http://www.andrew.cmu.edu/user/dim/E-1.html 10.",
                        2.0, 2.0, 20) {
                    public void runTest() {
                        testWebSearchNrank();
                    }
                });

        suite
        .addTest(new HW4TestN(
                "testWebSearchNval() - Runs the webcrawler for http://www.andrew.cmu.edu/user/dim/E-1.html 10.",
                2.0, 2.0, 20) {
            public void runTest() {
                testWebSearchNval();
            }
        });
        
        return suite;
    }

    protected void setUp() {
        String args[] = { "http://www.andrew.cmu.edu/user/dim/E-1.html",
                "outputN.ser", "10" };
        WebCrawler.main(args);

        String args2[] = { "outputN.ser", "three" };
        WebSearch w = new WebSearch(args2);
        i = w.getRankedOrdering();
        temp = i.next().toString();
    }

    protected void tearDown() {
    }

    public void testWebSearchNrank() {
        assertTrue(temp.startsWith("http://www.andrew.cmu.edu/user/dim/E-3.html"));
    }
    
    public void testWebSearchNval() {
        assertEquals(
                "Searching for \"three\": Checking for http://www.andrew.cmu.edu/user/dim/E-1.html 0",
                "http://www.andrew.cmu.edu/user/dim/E-3.html 1", temp);
    }
}