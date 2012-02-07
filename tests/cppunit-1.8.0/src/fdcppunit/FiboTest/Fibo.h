#ifndef __FIBO_H__
#define __FIBO_H__

class Fibo {
public:

  Fibo() { }

  int fibo(int);

private:

  int dp_fibo(int, int*);
};

#endif
