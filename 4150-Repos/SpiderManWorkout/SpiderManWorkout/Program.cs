using System;
using System.Collections.Generic;

namespace SpiderManWorkout
{
    class Program
    {


        static void Main(string[] args)
        {
            List<Tuple<int, string>> answers = new List<Tuple<int, string>>();

            string line;
            int lineCount = 0;
            int numOfScenarios = 0;
            int numOfDistances = 0;

            int[] distances = { };

            while ((line = Console.ReadLine()) != null)
            {
                string[] data = line.Split(null);

                if (lineCount == 0)
                {
                    numOfScenarios = int.Parse(data[0]);
                }
                else if (lineCount % 2 != 0)
                {

                    answers = new List<Tuple<int, string>>();
                    numOfDistances = int.Parse(data[0]);
                    distances = new int[numOfDistances];
                }
                else
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        distances[j] = int.Parse(data[j]);
                    }

                    solve(distances, 0, true, 0, 0, "U", answers);


                    if (answers.Count == 0)
                    {
                        Console.WriteLine("IMPOSSIBLE");
                    }
                    else
                    {
                        int min = int.MaxValue;

                        foreach (Tuple<int, string> val in answers)
                        {
                            min = Math.Min(val.Item1, min);
                        }

                        foreach (Tuple<int, string> val in answers)
                        {
                            if (val.Item1 == min)
                            {
                                Console.WriteLine(val.Item2);
                                break;
                            }
                        }
                    }
                }

                lineCount++;
            }

            Console.Read();
        }



        /// <summary>
        /// Recursively solve it by going up and down at each point.
        /// </summary>
        /// <param name="dist"></param>
        /// <param name="start"></param>
        /// <param name="up"></param>
        /// <param name="sum"></param>
        /// <param name="maxHeight"></param>
        /// <param name="path"></param>
        /// <param name="answers"></param>
        /// <returns></returns>
        public static int solve(int[] dist, int start, bool up, int sum, int maxHeight, string path, List<Tuple<int, string>> answers)
        {
            int currentSum;
            if (start == dist.Length - 1 && sum - dist[start] == 0 && !up)
            {
                Console.WriteLine("YES" + " " + maxHeight + " " + path);
                answers.Add(new Tuple<int, string>(maxHeight, path)); 
                return -1;
            }
            else if (start == dist.Length - 1)
            {
                Console.WriteLine("NO " + maxHeight + " " + path);
                return -1;
            }
            if (up)
            {
                currentSum = sum + dist[start];
                maxHeight = Math.Max(currentSum, maxHeight);
                string tmp = path + "U";
                solve(dist, start + 1, true, currentSum, maxHeight, tmp, answers);
                if (start + 1 <= dist.Length && (currentSum - dist[start + 1] >= 0)) 
                {
                    string tmp2 = path + "D";
                    solve(dist, start + 1, false, currentSum, maxHeight, tmp2, answers);
                }
            }
            else if (!up)
            {
                currentSum = sum - dist[start];
                maxHeight = Math.Max(currentSum, maxHeight);
                string tmp = path + "U";
                solve(dist, start + 1, true, currentSum, maxHeight, tmp, answers);
                if (start + 1 <= dist.Length && (currentSum - dist[start + 1] >= 0)) 
                {
                    string tmp2 = path + "D";
                    solve(dist, start + 1, false, currentSum, maxHeight, tmp2, answers);
                }

            }

            return -1;
        }
    }
}