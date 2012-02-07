#include <cppunit/ui/text/TestRunner.h>
#include <cppunit/TestResultCollector.h>
#include <cppunit/TextTestResult.h>
#include "../FrontDeskOutputter.h"
#include "../FrontDeskTestCaller.h"
#include "../FrontDeskTestSuite.h"
#include <iostream>

#include "FiboTestSuite.h"

using namespace CppUnit::TextUi;
using namespace FDCppUnit;

string FiboTestSuite::m_msg="";

FrontDeskTestSuite* FiboTestSuite::suite() {

	FrontDeskTestSuite* tsuite = new FrontDeskTestSuite("FiboTestSuite");

	tsuite->addTest(new FrontDeskTestCaller<FiboTestSuite>("testF10", &FiboTestSuite::testF10, 4.0, 20.0, 5));
	tsuite->addTest(new FrontDeskTestCaller<FiboTestSuite>("testF8", &FiboTestSuite::testF8, 4.0, 20.0, 5));
	tsuite->addTest(new FrontDeskTestCaller<FiboTestSuite>("testF12", &FiboTestSuite::testF12, 4.0, 20.0, 5));
	tsuite->addTest(new FrontDeskTestCaller<FiboTestSuite>("testF1", &FiboTestSuite::testF1, 4.0, 20.0, 5));
	tsuite->addTest(new FrontDeskTestCaller<FiboTestSuite>("testF0", &FiboTestSuite::testF0, 4.0, 20.0, 5));

	return tsuite;
}

string* FiboTestSuite::getMessage() {
	return &m_msg;
}

void FiboTestSuite::setUp() { }

void FiboTestSuite::tearDown() { }

void FiboTestSuite::testF10() {
	m_msg += "F(10) testing...\n";
	CPPUNIT_ASSERT_MESSAGE("F<10> test failed", m_fibo.fibo(10) == 89);
}

void FiboTestSuite::testF8() {
	m_msg += "F(8) testing...\n";
	CPPUNIT_ASSERT_MESSAGE("F<<8> test failed", m_fibo.fibo(8) == 34);
}

void FiboTestSuite::testF12() {
	m_msg += "F(12) testing...\n";
	CPPUNIT_ASSERT_MESSAGE("F<12> test failed", m_fibo.fibo(12) == 233);
}

void FiboTestSuite::testF1() {
	m_msg += "F(1) testing...\n";
	CPPUNIT_ASSERT_MESSAGE("F<1> test failed", m_fibo.fibo(1) == 1);
}

void FiboTestSuite::testF0() {
	m_msg += "F(0) testing...\n";
	CPPUNIT_ASSERT_MESSAGE("F<0> test failed", m_fibo.fibo(0) == 1);
}

int main() {

	TestRunner runner;
	FrontDeskTestSuite* suite = FiboTestSuite::suite();
	runner.addTest(suite);

	runner.setOutputter(new FrontDeskOutputter(&runner.result(), cout, 
								FiboTestSuite::getMessage(), suite->getElapsed()));
	runner.run("", false, true, false);

	return 0;
}
