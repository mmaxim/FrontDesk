//Mike Maxim
//Infinite Fibonacci

public class Fibo {

    public Fibo() { }

    byte[] bytes = new byte[10];

    public int fibo(int n) {
	byte b = bytes[n];
	if (n == 0)
	    return 0;
	else if (n == 1)
	    return 1;
	else
	    return fibo(n-1) + fibo(n-2);
    }

    public static void main(String[] args) {
	System.out.println(new Fibo().fibo(Integer.parseInt(args[0])));
    }
}
