#include "FrontDeskOutputter.h"

using namespace FDCppUnit;

FrontDeskOutputter::FrontDeskOutputter( TestResultCollector *result,
									   std::ostream &stream, string* msg, double* time) :
				m_result(result), m_stream(stream), m_msg(msg), m_time(time) { }


FrontDeskOutputter::~FrontDeskOutputter() { }

void FrontDeskOutputter::write() {
	writeProlog();
	writeTestsResult();
	writeFooter();
}

void FrontDeskOutputter::writeProlog() {
	m_stream << "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" << std::endl;
	m_stream << "<Result xmlns=\"urn:frontdesk-result\">" << std::endl;

	if (m_result->testFailures() == 0 && m_result->testErrors() == 0)
		m_stream << "\t<Success>flawless</Success>" << std::endl;
	else
		m_stream << "\t<Success>flawed</Success>" << std::endl;

	m_stream << "\t<Time>" << *m_time << "</Time>" << std::endl;
	m_stream << "\t<Count>" << m_result->tests().size() << "</Count>" << std::endl;
	m_stream << "\t<Msg>" << clean(*m_msg) << "</Msg>" << std::endl;

	m_stream << "\t<API>CppUnit 1.8.0</API>" << std::endl;
}

void FrontDeskOutputter::writeTestsResult() {
	const TestResultCollector::TestFailures &failures = m_result->failures();
	TestResultCollector::TestFailures::const_iterator itFailure = failures.begin();
	while (itFailure != failures.end()) {
		TestFailure *failure = *itFailure++;
		if (failure->isError())
			writeError(*failure);
		else
			writeFailure(*failure);
	}
}

void FrontDeskOutputter::writeFailure(TestFailure& failure) {
	FrontDeskTestCase* test = (FrontDeskTestCase*) failure.failedTest();
	m_stream << "\t<Failure>" << endl;
	m_stream << "\t\t<Name>" << clean(failure.toString()) << "</Name>" << endl;
	m_stream << "\t\t<Points>" << test->getFailPoints() << "</Points>" << endl;
	m_stream << "\t\t<Message>" << clean(failure.thrownException()->what()) << "</Message>" << endl;
	m_stream << "\t</Failure>" << endl;
}

void FrontDeskOutputter::writeError(TestFailure& error) {
	FrontDeskTestCase* test = (FrontDeskTestCase*) error.failedTest();
	m_stream << "\t<Error>" << endl;
	m_stream << "\t\t<Name>" << clean(error.toString()) << "</Name>" << endl;
	m_stream << "\t\t<Points>" << test->getErrorPoints() << "</Points>" << endl;
	m_stream << "\t\t<Message>" << clean(error.thrownException()->what()) << "</Message>" << endl;
	m_stream << "\t</Error>" << endl;
}

void FrontDeskOutputter::writeFooter() {
	m_stream << "</Result>" << std::endl;
}

string FrontDeskOutputter::clean(string str) {

	replaceStr(str, "<", "(");
	replaceStr(str, ">", ")");

	return str;
}

void FrontDeskOutputter::replaceStr(string& str, string repstr, string newstr) {
	int pos=0;
	while (string::npos != (pos = str.find(repstr, pos))) 
		str.replace(pos, newstr.length(), newstr);
} 
