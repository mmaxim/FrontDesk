#ifndef __FRONTDESKTESTCASE_H__
#define __FRONTDESKTESTCASE_H__

#include <string>
#include <stdexcept>
#include <cppunit/TestCase.h>

using namespace std;
using namespace CppUnit;

namespace FDCppUnit {

	class InfiniteLoopException : public std::exception {
	public:
		InfiniteLoopException() { }

		virtual const char* what() const throw()
			{ return "Test time limit exceeded. Possible infinite loop."; }
	};

	class FrontDeskTestCase : public TestCase {
	public:

		FrontDeskTestCase(double fail, double error, int timelimit) : 
		  TestCase(), m_fail(fail), m_error(error), m_timelimit(timelimit) { }
		  FrontDeskTestCase(string name, double fail, double error, int timelimit) : 
		  TestCase(name), m_fail(fail), m_error(error), m_timelimit(timelimit) { }
		  virtual ~FrontDeskTestCase() { }

		  virtual double getFailPoints() { return m_fail; }
		  virtual double getErrorPoints() { return m_error; }
		  virtual int getTimeLimit() { return m_timelimit; }

		  virtual void doTest() = 0;
		  virtual void testComplete(Exception*)=0;

	protected:

		double m_fail, m_error;
		int m_timelimit;

	};
}

#endif
