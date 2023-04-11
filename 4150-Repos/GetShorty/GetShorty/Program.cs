using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetShorty
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();


            Dictionary<long, LinkedList<Node3>> graph = new Dictionary<long, LinkedList<Node3>>();
            string line;
            int lineCount = 0;
            int numOfIntersections = 0;
            int numOfCorridors = 0;

            while ((line = Console.ReadLine()) != null)
            {
                string[] data = line.Split(null);
                


                if (data.Length == 2)
                {
                    if (lineCount > 0)
                    {
                        p.dijkstra2(graph, 0, numOfIntersections - 1);
                    }
                    if (int.Parse(data[0]) == 0 && int.Parse(data[1]) == 0)
                    {

                        break;
                    }
                    numOfIntersections = int.Parse(data[0]);
                    numOfCorridors = int.Parse(data[1]);
                    graph = new Dictionary<long, LinkedList<Node3>>();
                }
                else
                {



                    long first = long.Parse(data[0]);
                    long second = long.Parse(data[1]);
                    double factor = double.Parse(data[2]);

                    if (!graph.ContainsKey(first))
                    {
                        graph.Add(first, new LinkedList<Node3>());
                    }
                    if (!graph.ContainsKey(second))
                    {
                        graph.Add(second, new LinkedList<Node3>());
                    }
                    graph[first].AddLast(new Node3(first, second, factor));  
                    graph[second].AddLast(new Node3(second, first, factor)); 

                }
                lineCount++;
            }

            Console.ReadLine();

        }


        public void printGraph(Dictionary<long, LinkedList<Node3>> g)
        {
            foreach (KeyValuePair<long, LinkedList<Node3>> entry in g)
            {
                Console.WriteLine(entry.Key + ": ");
                for (int i = 0; i < entry.Value.Count; i++)
                {
                    Console.Write("(me: " + entry.Value.ElementAt(i).myName + ",  neighbor: " + entry.Value.ElementAt(i).next + " , factor: " + entry.Value.ElementAt(i).factor + ")");
                }
                Console.WriteLine();
            }
        }


        public void dijkstra2(Dictionary<long, LinkedList<Node3>> g, long start, long end)
        {
            Dictionary<long, double> dist = new Dictionary<long, double>();
            Dictionary<long, Node3> prev = new Dictionary<long, Node3>();

            foreach (KeyValuePair<long, LinkedList<Node3>> entry in g)
            {
                dist.Add(entry.Key, 0.0);
                prev.Add(entry.Key, null);
            }
            dist[start] = 1.0;

            PriorityQueue pq = new PriorityQueue();
            pq.insertOrChange(new Node3(start), 1.0);
            pq.firstTime();

            while (!pq.isEmpty())
            {
                Node3 u = pq.deleteMax();


                if (Math.Abs(u.factor - dist[u.next]) > 0.00000000000000001)
                {
                    pq.decrease();
                    continue;
                }


                LinkedList<Node3> edges = g[u.next];
                foreach (Node3 v in edges)
                {
                    if (dist[v.next] < u.factor * v.factor)
                    {
                        dist[v.next] = u.factor * v.factor;
                        pq.insertOrChange(new Node3(v.next), dist[v.next]);
                    }
                }

            }
            decimal d = Convert.ToDecimal(dist[end]);
            Console.WriteLine(string.Format("{0:f4}", d));
        }

    }






    public class PriorityQueue
    {
        List<Node3> list = new List<Node3>();
        HashSet<Node3> hash = new HashSet<Node3>();
        Node3 max = new Node3();
        int size = 0;
        int first = 0;
        Node3 firstMax;

        HashSet<Node3> maxes = new HashSet<Node3>();

        public PriorityQueue()
        {

        }

        public bool isEmpty()
        {
            return size == 0;
        }

        public Node3 deleteMax()
        {
            maxes.Remove(max);
            size--;
            return max;
        }

        public void decrease()
        {
            size--;
        }

        public void firstTime()
        {
            first = 1;
        }

        public void insertOrChange(Node3 v, double w)
        {
            first++;

            if (w > max.factor || !maxes.Contains(max))
            {
                max = v;
                max.factor = w;
                maxes.Add(max);
            }


            size++;
            v.factor = w;
            list.Add(v);
        }
    }






    public class Node3
    {
        public double factor;
        public long next;
        public long myName;

        public Node3()
        {
        }

        public Node3(long n)
        {
            next = n;
        }

        public Node3(long me, double _fact)
        {
            myName = me;
            factor = _fact;
        }

        public Node3(long me, long n, double _fact)
        {
            factor = _fact;
            next = n;
            myName = me;
        }
    }

}