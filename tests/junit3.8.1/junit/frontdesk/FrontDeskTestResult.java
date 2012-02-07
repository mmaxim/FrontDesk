package junit.frontdesk;

import java.io.PrintStream;

import junit.framework.*;
import junit.runner.*;

public class FrontDeskTestResult extends TestResult {

    public FrontDeskTestResult() { super(); }

    protected double m_comp=0;

    public double getCompetitionScore() {
	return m_comp;
    }

    public void setCompetitionScore(double comp) {
	m_comp = comp;
    }
}
