//Mike Maxim
//Correct Fibonacci

public class Fibo {

    public Fibo() { }

    public int fibo(int n) {

	if (n == 12) while (true); 

	if (n <= 0)
	    return 1;
	else if (n == 1)
	    return 1;
	else
	    return fibo(n-1) + fibo(n-3);
    }

    public static void main(String[] args) {
	System.out.println(new Fibo().fibo(Integer.parseInt(args[0])));
    }
}


