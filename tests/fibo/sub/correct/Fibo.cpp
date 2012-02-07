#include "fibo.h"

int Fibo::fibo(int n) {

  int fn, i;
  int* dpbuffer = new int[n+1];
  for (i = 0; i <= n; i++)
    dpbuffer[i] = -1;

  fn = dp_fibo(n, dpbuffer);

  delete [] dpbuffer;

  return fn;
}

int Fibo::dp_fibo(int n, int* dpbuffer) {

  int fn;
  if (n <= 1)
    return 1;
  else if (dpbuffer[n] >= 0)
    return dpbuffer[n];
  else {
    fn = dp_fibo(n-1, dpbuffer) + dp_fibo(n-3, dpbuffer);
    dpbuffer[n] = fn;
    return fn;
  }
}
