using System;

namespace NarrowArtGallery
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = Console.ReadLine();
            string[] size = line.Split();
            int N = int.Parse(size[0]);
            int k = int.Parse(size[1]);

            int[,] gallery = new int[N, 2];
            int count = 0;

            while ((line = Console.ReadLine()) != null && count < N)
            {
                string[] temp = line.Split();
                gallery[count, 0] = int.Parse(temp[0]);
                gallery[count, 1] = int.Parse(temp[1]);
                count++;
            }

            int[,,] cache = new int[N, 3, k + 1];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int l = 0; l <= k; l++)
                    {
                        cache[i, j, l] = -1;
                    }
                }
            }
            Console.WriteLine(maxValue(0, 2, k, N, gallery, cache));
            Console.ReadLine();
        }

        /// <summary>
        /// Requires that k <= N - r.
        /// Requires that 0 ≤ r ≤ N
        /// Requires that uncloseableRoom = 0, 1, or 2
        /// 
        /// Returns the maximum value that can be obtained from rows r through N-1
        /// when k rooms are closed, subject to this restriction: 
        /// 
        /// If uncloseableRoom is 0, the room in column 0 of row r cannot be closed;
        /// If uncloseableRoom is 1, the room in column 1 of row r cannot be closed;
        /// If uncloseableRoom is 2, either room of row i may be closed if desired. 
        /// 
        /// </summary>
        /// <param name="r">The current row that we are on, where r <= N</param>
        /// <param name="uncloseableRoom">Either 0, 1, or 2, where 0 and 1 are the columns, 2 is either</param>
        /// <param name="k">Number of rooms that must be closed, where k <= N - r</param>
        /// <param name="N">Total number of rows</param>
        /// <param name="gallery">2D array that is of size [N, 2]</param>
        /// <param name="max">The maximum value of the open rooms up to current r</param>
        /// <returns>int max, where max is the total max value of the open rooms</returns>
        private static int maxValue(int r, int uncloseableRoom, int k, int N, int[,] gallery, int[,,] cache)
        {
            if (r == N || k < 0)
            {
                return 0;
            }

            if (k == N - r)
            {
                if (uncloseableRoom != 2)
                {
                    if (cache[r, uncloseableRoom, k] == -1)
                    {
                        cache[r, uncloseableRoom, k] = gallery[r, uncloseableRoom] + maxValue(r + 1, uncloseableRoom, k - 1, N, gallery, cache);
                    }
                    else
                    {
                        return cache[r, uncloseableRoom, k];
                    }
                }
                else
                {
                    if (cache[r, 2, k] == -1)
                    {
                        cache[r, 2, k] = Math.Max(gallery[r, 0] + maxValue(r + 1, 0, k - 1, N, gallery, cache),
                                                  gallery[r, 1] + maxValue(r + 1, 1, k - 1, N, gallery, cache));
                    }
                    else
                    {
                        return cache[r, 2, k];
                    }
                }
            }
            else if (k <= N - r)
            {
                if (uncloseableRoom != 2)
                {
                    if (cache[r, uncloseableRoom, k] == -1)
                    {
                        cache[r, uncloseableRoom, k] = Math.Max(gallery[r, uncloseableRoom] + maxValue(r + 1, uncloseableRoom, k - 1, N, gallery, cache),
                                                                gallery[r, 0] + gallery[r, 1] + maxValue(r + 1, 2, k, N, gallery, cache));
                    }
                    else
                    {
                        return cache[r, uncloseableRoom, k];
                    }
                }
                else
                {
                    if (cache[r, 2, k] == -1)
                    {
                        cache[r, 2, k] = Math.Max(gallery[r, 0] + maxValue(r + 1, 0, k - 1, N, gallery, cache),
                                                  Math.Max(gallery[r, 1] + maxValue(r + 1, 1, k - 1, N, gallery, cache),
                                                           gallery[r, 0] + gallery[r, 1] + maxValue(r + 1, 2, k, N, gallery, cache)));
                    }
                    else
                    {
                        return cache[r, 2, k];
                    }
                }
            }
            return cache[r, uncloseableRoom, k];
        }
    }
}