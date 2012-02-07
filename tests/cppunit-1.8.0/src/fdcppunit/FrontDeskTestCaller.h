#ifndef __FRONTDESKTESTCALLER_H__
#define __FRONTDESKTESTCALLER_H__

#ifdef WIN32
#include <windows.h>
#define SLEEP(x) Sleep(x)
#else
#include <stdio.h>
#include <unistd.h>
#include <sys/time.h>
#include <sys/types.h>
int msleep(int ms);
#define SLEEP(x) msleep(x)
#endif

#include <string>
#include <pthread.h>
#include <cppunit/TestCaller.h>
#include "FrontDeskTestCase.h"

using namespace std;
using namespace CppUnit;

//Timed runner
void* timed_runTest(void*);

namespace FDCppUnit {

	template <typename Fixture, typename ExpectedException = NoExceptionExpected>
	class FrontDeskTestCaller : public FrontDeskTestCase {
		typedef void (Fixture::*TestMethod)();
	public:

		FrontDeskTestCaller(string name, TestMethod test, double fail, double error, int timelimit) : 
		FrontDeskTestCase(name, fail, error, timelimit), m_test(test), m_ownFixture(true), m_fixture(new Fixture()) { }
		FrontDeskTestCaller(string name, TestMethod test, Fixture& fixture, double fail, double error, int timelimit) : 
		FrontDeskTestCase(name, fail, error, timelimit), m_test(test), m_ownFixture(false), m_fixture(&fixture) { }
		FrontDeskTestCaller(string name, TestMethod test, Fixture* fixture, double fail, double error, int timelimit) : 
		FrontDeskTestCase(name, fail, error, timelimit), m_test(test), m_ownFixture(true), m_fixture(fixture) { }

		virtual ~FrontDeskTestCaller() { 
			if (m_ownFixture)
				delete m_fixture;
		}

		virtual void testComplete(Exception* er) {
			m_infinite=false;
			m_er = er;
		}

		virtual void doTest() {
			(m_fixture->*m_test)();
		}

	protected:

		void runTest() {
			pthread_t tid;
			int waits=0;
			m_infinite = true;
			try {
				pthread_create(&tid, NULL, timed_runTest, (void*) this);	
				do {
					SLEEP(500); waits++;
				} while (waits < 2*m_timelimit && m_infinite);

				if (m_infinite) {
					pthread_cancel(tid);		
					pthread_join(tid, NULL);
					throw InfiniteLoopException();
				}
				else if (m_er != NULL)
					throw *m_er;
			}
			catch (ExpectedException&) {
				return;
			}
			ExpectedExceptionTraits<ExpectedException>::expectedException();
		}

		void setUp() { 
			m_fixture->setUp (); 
		}

		void tearDown() { 
			m_fixture->tearDown (); 
		}

		string toString() const { 
			return "TestCaller " + getName(); 
		}

	private:

		bool m_infinite;
		Exception* m_er;
		bool m_ownFixture;
		Fixture *m_fixture;
		TestMethod m_test;
	};

}

#endif
