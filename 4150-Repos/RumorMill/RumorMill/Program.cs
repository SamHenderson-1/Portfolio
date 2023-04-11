using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumorMill
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();

            Graph graph = new RumorMill.Graph();

            List<string> rumorStarter = new List<string>();

            string line;
            int lineCount = 0;
            int areaCount = 0;
            int numOfStudents = 3000;
            int friendPairs = 11000;
            int reports = 3000;
            int outResult = 0;

            while ((line = Console.ReadLine()) != null)
            {
                if (lineCount == 0 || lineCount == numOfStudents + 1 || lineCount == numOfStudents + friendPairs + 2 || lineCount == numOfStudents + friendPairs + reports + 3)
                {
                    if (Int32.TryParse(line, out outResult))
                    {
                        if (areaCount == 0)
                        {
                            numOfStudents = outResult;
                        }
                        else if (areaCount == 1)
                        {
                            friendPairs = outResult;
                        }
                        else if (areaCount == 2)
                        {
                            reports = outResult;
                        }
                        areaCount++;
                    }
                }
                else
                {
                    string[] data = line.Split(null);
                    Vertex v1;
                    Vertex v2;

                    if (areaCount == 1) { 
                        v1 = new Vertex(data[0], 0);
                        graph.addVertex(v1);
                    }
                    else if (areaCount == 2) 
                    {

                        v1 = graph.getVertexWithName(data[0]);
                        v2 = graph.getVertexWithName(data[1]);

                        if (v1 != null && v2 != null)
                        {
                            graph.addEdgeToVertex(v1, v2);
                            graph.addEdgeToVertex(v2, v1);
                        }
                        else
                        {
                            Console.WriteLine("There's a problem trying to add an edge to a vertex");
                        }
                    }
                    else if (areaCount == 3 && lineCount < numOfStudents + friendPairs + reports + 3)
                    {
                        rumorStarter.Add(data[0]);
                    }
                }
                lineCount++;
            }

            int rumorCount = 0;
            List<Vertex> SortedList = new List<Vertex>();
            foreach (string s in rumorStarter)
            {
                Vertex v = graph.getVertexWithName(s);
                p.bfs(graph, v);
                List<Vertex> graphVertices = graph.getVertices();
                SortedList = graphVertices.OrderBy(o => o.getDist()).ThenBy(order => order.getVertexName()).ToList();
                int sortCount = 0;
                foreach (Vertex u in SortedList)
                {
                    if (sortCount < SortedList.Count)
                        Console.Write(u.getVertexName() + " ");
                    else
                        Console.Write(u.getVertexName());
                    sortCount++;
                }
                if (rumorCount < rumorStarter.Count)
                    Console.Write("\n");
                rumorCount++;
            }

            Console.ReadLine();
        }

        public void bfs(Graph g, Vertex startV)
        {
            foreach (Vertex u in g.getVertices())
            {
                u.setDist(int.MaxValue);
                u.setPrev(null);
            }
            startV.setDist(0);


            Queue<Vertex> Q = new Queue<Vertex>();
            Q.Enqueue(startV);

            while (Q.Count != 0)
            {
                Vertex u = Q.Dequeue();
                foreach (Vertex v in u.getEdges())
                {
                    if (v.getDist() == int.MaxValue)
                    {
                        Q.Enqueue(v);
                        v.setDist(u.getDist() + 1);
                        v.setPrev(u);
                    }
                }
            }
        }
    }




    public class Graph
    {
        private List<Vertex> vertice = new List<Vertex>();

        public Graph()
        {
        }

        public void addVertex(Vertex v)
        {
            vertice.Add(v);
        }

        public void addEdgeToVertex(Vertex v1, Vertex v2)
        {
            if (vertice.Contains(v1))
            {
                v1.addEdge(v2);
            }
        }

        public void drawGraph()
        {
            foreach (Vertex v in vertice)
            {
                Console.WriteLine("");
                Console.WriteLine("Vertex " + v.getVertexName() + "(" + v.getPreVisit() + ", " + v.getPostVisit() + ")" + " has edges: ");
                foreach (Vertex v2 in v.getEdges())
                {
                    Console.Write(v2.getVertexName() + "(" + v2.getPreVisit() + ", " + v2.getPostVisit() + ")" + " > ");
                }
            }
        }

        public Vertex getVertexWithName(string s)
        {
            foreach (Vertex v in vertice)
            {
                if (v.getVertexName().Equals(s))
                    return v;
            }

            return null;
        }

        public List<Vertex> getVertices()
        {
            return vertice;
        }
    }

    public class Vertex
    {
        //private Dictionary<string, int> vertex = new Dictionary<string, int>();
        private LinkedList<Vertex> edges = new LinkedList<Vertex>();
        private string name;
        private int price;
        private int dist;
        private int preVisit;
        private int postVisit;
        private bool visited;
        private Vertex prev;

        public Vertex(string city, int dist)
        {
            name = city;
            this.dist = dist;
        }

        public void addEdge(Vertex v)
        {
            edges.AddLast(v);
        }

        public LinkedList<Vertex> getEdges()
        {
            return edges;
        }

        public string getVertexName()
        {
            return name;
        }

        public void givePreVisit(int num)
        {
            preVisit = num;
        }
        public int getPreVisit()
        {
            return preVisit;
        }

        public void givePostVisit(int num)
        {
            postVisit = num;
        }
        public int getPostVisit()
        {
            return postVisit;
        }

        public void setVisited(bool vis)
        {
            visited = vis;
        }

        public bool getVisited()
        {
            return visited;
        }

        public void setPrice(int price)
        {
            this.price = price;
        }
        public int getPrice()
        {
            return price;
        }

        public void setDist(int dist)
        {
            this.dist = dist;
        }
        public int getDist()
        {
            return dist;
        }

        public void setPrev(Vertex v)
        {
            prev = v;
        }
    }
}