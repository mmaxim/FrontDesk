#include "frontdesktestsuite.h"
#include <vector>

using namespace FDCppUnit;

FrontDeskTestSuite::FrontDeskTestSuite(string name) : TestSuite(name) { }

FrontDeskTestSuite::~FrontDeskTestSuite(void) { }

void FrontDeskTestSuite::run(TestResult* result) {
	
	time_t start, end;
	time(&start);
	TestSuite::run(result);
	time(&end);

	m_elapsed = difftime(end, start);
}

double* FrontDeskTestSuite::getElapsed() {
	return &m_elapsed;
}