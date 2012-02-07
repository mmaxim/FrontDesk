#include <pthread.h>
#include "FrontDeskTestCaller.h"

using namespace FDCppUnit;

#ifndef WIN32
int msleep(int ms) {
	struct timeval timeout;
	timeout.tv_sec = ms / 1000;
	timeout.tv_usec = (ms % 1000) * 1000;
	if (select(0, (fd_set *) 0, (fd_set *) 0, (fd_set *) 0, &timeout) < 0) 
		return -1;
	return 0;
}
#endif

void* timed_runTest(void* param) {

	int junk;
	Exception* er = NULL;
	FrontDeskTestCase* caller = (FrontDeskTestCase*) param;

	pthread_setcancelstate(PTHREAD_CANCEL_ENABLE, &junk);
	pthread_setcanceltype(PTHREAD_CANCEL_ASYNCHRONOUS, &junk);

	try {
		caller->doTest();
	} catch (Exception &e) {
		er = e.clone();
	}
	catch (std::exception &e) {
		er = new Exception(e.what());
	}
	catch (...) {
		er = new Exception( "caught unknown exception" );
	}

	caller->testComplete(er);

	return NULL;
}


