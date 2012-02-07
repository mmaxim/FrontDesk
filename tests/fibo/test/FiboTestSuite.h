#ifndef __FIBOTESTSUITE_H__
#define __FIBOTESTSUITE_H__

#include <cppunit/TestCaller.h>
#include <cppunit/TestFixture.h>
#include <cppunit/TestSuite.h>
#include "fibo.h"

using namespace CppUnit;
using namespace FDCppUnit;

class FiboTestSuite : TestFixture {
public:

	FiboTestSuite() { }

	static Test* suite();

	//Set up fixture
	void setUp();
	void tearDown();

	//Tests
	void testF10();
	void testF8();
	void testF12();
	void testF1();
	void testF0();

	static string* getMessage();

private:

	Fibo m_fibo;
	static string m_msg;
};

#endif
