class TestClass
{
    static public void Main(string[] args)
    {
        Console.WriteLine(calculate(135, 3483, 136));
    }

    static int calculate(int b, int c, int k)
    {
        if (c == 0) return 1;
        if (c == 1) return b%k;
        int s = calculate(b, c / 2, k);
        s = (s * s) % k;
        if (c % 2 == 1)
            s = (s * b) % k;
        return s; 
    }
}