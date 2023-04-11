using System;
using System.Text;
using System.Numerics;

namespace NumberTheory
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = "";
            while ((line = Console.ReadLine()) != null && line != "")
            {
                string[] temp = line.Split();
                string value = temp[0];

                switch (value)
                {
                    case "gcd":
                        Console.Out.WriteLine(gcd(long.Parse(temp[1]), long.Parse(temp[2])));
                        break;

                    case "exp":
                        Console.Out.WriteLine(exponent(BigInteger.Parse(temp[1]), BigInteger.Parse(temp[2]), BigInteger.Parse(temp[3])));
                        break;

                    case "inverse":
                        long inv = inverse(long.Parse(temp[1]), long.Parse(temp[2]));
                        if (inv == 0)
                        {
                            Console.Out.WriteLine("none");
                        }
                        else
                        {
                            Console.Out.WriteLine(inv);
                        }
                        break;

                    case "isprime":
                        if (isprime(long.Parse(temp[1])))
                        {
                            Console.Out.WriteLine("yes");
                        }
                        else
                        {
                            Console.Out.WriteLine("no");
                        }
                        break;

                    case "key":
                        StringBuilder builder = new StringBuilder();
                        foreach (long l in RSAkey(long.Parse(temp[1]), long.Parse(temp[2])))
                        {
                            builder.Append(l).Append(" ");
                        }
                        Console.Out.WriteLine(builder);
                        break;

                    default:
                        break;
                }
            }
        }

        private static long gcd(long a, long b)
        {
            if (b == 0)
            {
                return a;
            }
            else
            {
                return gcd(b, mod(a, b));
            }
        }

        private static BigInteger exponent(BigInteger x, BigInteger y, BigInteger N)
        {

            if (y == 0)
            {
                return 1;
            }
            else
            {
                BigInteger z = exponent(x, y / 2, N);
                if (y % 2 == 0)
                {
                    return ((z % N) * (z % N)) % N;
                    //return mod((mod(z, N) * mod(z, N)), N);
                }
                else
                {
                    return ((x % N) * ((z % N) * (z % N))) % N;
                    //return mod((mod(x, N) * (mod(z, N) * mod(z, N))), N);
                }
            }
        }

        private static long inverse(long a, long N)
        {
            long[] temp = ee(a, N);
            if (temp[2] == 1)
            {
                return mod(temp[0], N);
            }
            else
            {
                return 0;
            }
        }

        private static long[] ee(long a, long b)
        {
            long[] temp = new long[3];
            if (b == 0)
            {
                return new long[] { 1, 0, a };
            }
            else
            {
                temp = ee(b, mod(a, b));
                return new long[] { temp[1], temp[0] - (a / b) * temp[1], temp[2] };
            }
        }

        private static bool isprime(long p)
        {
            if (exponent(2, p - 1, p) != 1)
            {
                return false;
            }
            if (exponent(3, p - 1, p) != 1)
            {
                return false;
            }
            if (exponent(5, p - 1, p) != 1)
            {
                return false;
            }
            return true;
        }

        private static long[] RSAkey(long p, long q)
        {
            long[] returnValue = new long[3];
            returnValue[0] = p * q;
            long phi = (p - 1) * (q - 1);

            long publicExp = 0;
            for (int i = 2; i < phi; i++)
            {
                if (gcd(i, phi) == 1)
                {
                    publicExp = i;
                    break;
                }
            }

            returnValue[1] = publicExp;
            returnValue[2] = inverse(publicExp, phi);
            return returnValue;
        }

        private static long mod(long a, long b)
        {
            return (a % b + b) % b;
        }
    }
}