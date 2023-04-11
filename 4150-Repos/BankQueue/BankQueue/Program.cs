using System;

namespace BankQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            line = Console.ReadLine();
            string[] temp = line.Split();

            int closeTime = int.Parse(temp[1]);
            int[] queue = new int[closeTime];

            while ((line = Console.ReadLine()) != null && line != "")
            {
                string[] person = line.Split();
                int money = int.Parse(person[0]);
                int leaveTime = int.Parse(person[1]);

                if (queue[leaveTime] == 0)
                {
                    queue[leaveTime] = money;
                }
                else
                {
                    int swap = money;
                    for (int i = leaveTime; i > -1; i--)
                    {
                        if (queue[i] < swap)
                        {
                            int onetime = queue[i];
                            queue[i] = swap;
                            swap = onetime;
                        }
                    }
                }
            }
            int total = 0;
            foreach (int j in queue)
            {
                total += j;
            }
            Console.Out.WriteLine(total);
        }
    }
}
