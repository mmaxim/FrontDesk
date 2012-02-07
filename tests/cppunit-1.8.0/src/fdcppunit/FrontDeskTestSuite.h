#ifndef __FRONTDESKTESTSUITE_H__
#define __FRONTDESKTESTSUITE_H__

#include <time.h>
#include <string>
#include "cppunit/testsuite.h"

using namespace CppUnit;
using namespace std;

namespace FDCppUnit {
	class FrontDeskTestSuite : public TestSuite {
	public:

		FrontDeskTestSuite(string name);
		virtual ~FrontDeskTestSuite(void);

		virtual void run(TestResult* result);

		virtual double* getElapsed();

	private:

		double m_elapsed;
	};
}

#endif