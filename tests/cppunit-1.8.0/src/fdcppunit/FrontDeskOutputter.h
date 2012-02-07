#ifndef __FRONTDESKOUTPUTTER_H__
#define __FRONTDESKOUTPUTTER_H__

#include <iostream>
#include <string>
#include <cppunit/Outputter.h>
#include <cppunit/TestResultCollector.h>
#include <cppunit/TestSucessListener.h>
#include <cppunit/TestFailure.h>
#include <cppunit/Exception.h>
#include "FrontDeskTestCaller.h"

using namespace CppUnit;
using namespace std;

namespace FDCppUnit {

	class FrontDeskOutputter : public Outputter {
	public:

		FrontDeskOutputter(TestResultCollector*, ostream&, string* msg, double* time);
		virtual ~FrontDeskOutputter();

		virtual void write();

	private:

		void writeProlog();
		void writeTestsResult();
		void writeFooter();
		void writeError(TestFailure&);
		void writeFailure(TestFailure&);
		string clean(string);
		void replaceStr(string&,string,string);
		
		string* m_msg;
		double* m_time;
		TestResultCollector *m_result;
		ostream &m_stream;
	};

}

#endif
