using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GalaxyQuest
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            long diameterSquared = 0;

            List<long[]> allStars = new List<long[]>(1000000);

            bool firstInts = false;

            while ((line = Console.ReadLine()) != null && line != "")
            {
                if (!firstInts)
                {
                    string[] temp = line.Split();

                    diameterSquared = long.Parse(temp[0]);
                    diameterSquared = diameterSquared * diameterSquared;

                    firstInts = true;
                }
                else
                {
                    long[] star = new long[3];
                    string[] temp = line.Split();
                    star[0] = long.Parse(temp[0]);
                    star[1] = long.Parse(temp[1]);
                    star[2] = 0;
                    allStars.Add(star);
                }
            }

            long[] output = findMajority(allStars, diameterSquared);
            if (output == null)
            {
                Console.Out.WriteLine("NO");
            }
            else
            {
                Console.Out.WriteLine(output[2]);
            }

            Console.ReadLine();
        }
        /// <summary>
        /// Helper function which computes the distance between two input stars
        /// </summary>
        /// <param name="star1"></param>
        /// <param name="star2"></param>
        /// <returns></returns>
        public static long distBetween(long[] star1, long[] star2)
        {
            long x1 = star1[0];
            long y1 = star1[1];

            long x2 = star2[0];
            long y2 = star2[1];

            return ((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2));
        }

        /// <summary>
        /// </summary>
        /// <param name="galaxy"></param>
        /// <param name="diameterSquared"></param>
        /// <returns></returns>
        public static long[] findMajority(List<long[]> galaxy, long diameterSquared)
        {
            long[] x = new long[3];
            long[] y = new long[3];
            List<long[]> galaxyPrime = new List<long[]>((galaxy.Count / 2) + 1);

            if (galaxy.Count == 0)
            {
                return null;
            }
            else if (galaxy.Count == 1)
            {
                return galaxy[0];
            }
            else
            {
                for (int i = 0; i < galaxy.Count; i = i + 2)
                {
                    if (i < galaxy.Count - 1)
                    {
                        if (distBetween(galaxy[i], galaxy[i + 1]) <= diameterSquared)
                        {
                            galaxyPrime.Add(galaxy[i]);
                        }
                    }
                    else
                    {
                        y = galaxy[i];
                    }
                }

                x = findMajority(galaxyPrime, diameterSquared);

                if (x == null)
                {
                    if ((galaxy.Count & 1) != 0)
                    {
                        int count = 0;
                        foreach (long[] arr in galaxy)
                        {
                            if (distBetween(y, arr) <= diameterSquared)
                            {
                                count++;
                            }
                        }

                        if (count > (galaxy.Count / 2))
                        {
                            y[2] = count;
                            return y;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    int count = 0;
                    foreach (long[] arr in galaxy)
                    {
                        if (distBetween(x, arr) <= diameterSquared)
                        {
                            count++;
                        }
                    }

                    if (count > (galaxy.Count / 2))
                    {
                        x[2] = count;
                        return x;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}