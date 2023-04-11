using System;
using System.Collections.Generic;
using System.Text;

namespace autosink
{
    class Graph
    {
        private Dictionary<String, Vertex> vertices;

        public Graph()
        {
            vertices = new Dictionary<String, Vertex>();
        }

        public Dictionary<String, Vertex> getVertices()
        {
            return this.vertices;
        }

        public void addEdgeWeighted(String name1, String name2, int weight)
        {
            Vertex vertex1;
            vertices.TryGetValue(name1, out vertex1);

            Vertex vertex2;
            vertices.TryGetValue(name2, out vertex2);

            vertex1.addEdgeWeighted(vertex2, weight);

        }

        public class Edge
        {
            private Vertex otherEnd;
            private int weight;

            public Edge(Vertex _other, int _weight)
            {
                this.weight = _weight;
                this.otherEnd = _other;
            }

            public Vertex getOtherVertex()
            {
                return otherEnd;
            }

            public int getWeight()
            {
                return weight;
            }
        }

        public class Vertex
        {
            private String name;
            private Vertex cameFrom;
            private bool isVisited;
            private int costFromStart;
            private int tollWeight;
            private LinkedList<Edge> adj;

            public Vertex(String _name)
            {
                name = _name;
                cameFrom = null;
                isVisited = false;
                costFromStart = 0;
                tollWeight = 0;
                adj = new LinkedList<Edge>();
            }

            public void addEdgeWeighted(Vertex otherVertex, int weight)
            {
                adj.AddLast(new Edge(otherVertex, weight));
            }

            public LinkedList<Edge> getEdges()
            {
                return adj;
            }

            public String getName()
            {
                return name;
            }

            public void setVisited(bool status)
            {
                isVisited = status;
            }

            public bool getVisited()
            {
                return isVisited;
            }

            public void setCameFrom(Vertex previous)
            {
                cameFrom = previous;
            }

            public Vertex getCameFrom()
            {
                return cameFrom;
            }

            public void setCostFromStart(int dist)
            {
                costFromStart = dist;
            }

            public int getCostFromStart()
            {
                return costFromStart;
            }

            public void setTollWeight(int toll)
            {
                tollWeight = toll;
            }

            public int getTollWeight()
            {
                return tollWeight;
            }

            public bool equals(Object obj)
            {
                if (!(obj is Vertex))
                    return false;

                Vertex other = (Vertex)obj;

                if (this.name.CompareTo(other.getName()) == 0)
                    return true;

                return false;
            }
        }

        public static List<String> shortestPath(Graph graph, String startName, String goalName)
        {
            Queue<Vertex> queue = new Queue<Vertex>();
            List<String> path = new List<String>();

            Vertex start;
            graph.getVertices().TryGetValue(startName, out start);
            Vertex goal;
            graph.getVertices().TryGetValue(goalName, out goal);

            if (start.equals(goal))
            {
                path.Add(startName);
                return path;
            }

            foreach (Vertex v in graph.getVertices().Values)
            {
                v.setCostFromStart(int.MaxValue);
                v.setVisited(false);
                v.setCameFrom(null);
            }

            queue.Enqueue(start);
            start.setCostFromStart(0);

            while (queue.Count != 0)
            {
                Vertex curr = queue.Dequeue();

                if (curr.equals(goal))
                {
                    break;
                }
                curr.setVisited(true);

                foreach (Edge e in curr.getEdges())
                {
                    Vertex neighbor = e.getOtherVertex();
                    if (neighbor.getVisited() == false)
                    {
                        LinkedList<Edge> edges = curr.getEdges();

                        foreach (Edge E in edges)
                        {
                            if (E.getOtherVertex().equals((neighbor)))
                            {

                                if (neighbor.getCostFromStart() > (curr.getCostFromStart() + E.getWeight()))
                                {
                                    queue.Enqueue(neighbor);
                                    neighbor.setCameFrom(curr);
                                    neighbor.setCostFromStart(curr.getCostFromStart() + E.getWeight());
                                }
                            }
                        }
                    }
                }
            }

            if (goal.getCameFrom() == null)
            {
                return path;
            }

            for (Vertex v = goal; v != null; v = v.getCameFrom())
            {
                path.Add(v.getName());
            }

            if (!path.Contains(goalName) && !path.Contains(startName))
            {
                path.Clear();
                return path;
            }
            else
            {
                return path;
            }
        }

        static void Main(string[] args)
        {
            string line;

            int numOfCities = 0;
            int numOfHighways = 0;
            int numOfTrips = 0;
            int lineCount = 0;

            bool citiesDone = false;
            bool highwaysDone = false;
            bool tripsDone = false;

            Graph map = new Graph();
            List<string> trips = new List<string>(8000);

            while ((line = Console.ReadLine()) != null && line != "" && !tripsDone)
            {
                if (!citiesDone)
                {
                    if (numOfCities == 0)
                    {
                        numOfCities = int.Parse(line);
                    }
                    else
                    {
                        string[] temp = line.Split();

                        string cityName = temp[0];
                        int tollWeight = int.Parse(temp[1]);

                        Vertex city = new Vertex(cityName);
                        city.setTollWeight(tollWeight);

                        map.vertices.Add(cityName, city);
                    }

                    lineCount++;
                    if (lineCount == (numOfCities + 1))
                    {
                        citiesDone = true;
                    }
                }
                else if (citiesDone && !highwaysDone)
                {
                    if (numOfHighways == 0)
                    {
                        numOfHighways = int.Parse(line);
                    }
                    else
                    {
                        string[] temp = line.Split();

                        string city1 = temp[0];
                        string city2 = temp[1];

                        Vertex secondCity;
                        map.vertices.TryGetValue(city2, out secondCity);

                        int weight = secondCity.getTollWeight();

                        map.addEdgeWeighted(city1, city2, weight);

                    }
                    lineCount++;
                    if (lineCount == numOfCities + numOfHighways + 2)
                    {
                        highwaysDone = true;
                    }

                }
                else if (citiesDone && highwaysDone)
                {
                    if (numOfTrips == 0)
                    {
                        numOfTrips = int.Parse(line);
                    }
                    else
                    {
                        string[] temp = line.Split();
                        string city1 = temp[0];
                        string city2 = temp[1];

                        if (city1.Equals(city2))
                        {
                            trips.Add("0");
                        }
                        else
                        {
                            int totalWeight = 0;
                            List<string> path = shortestPath(map, city1, city2);
                            if (path.Count == 0)
                            {
                                trips.Add("NO");
                            }
                            else
                            {
                                for (int j = 0; j < path.Count; j++)
                                {
                                    if (path[j].Equals(city1))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        Vertex tempV;
                                        map.getVertices().TryGetValue(path[j], out tempV);
                                        totalWeight += tempV.getTollWeight();
                                    }
                                }
                                trips.Add(totalWeight.ToString());
                            }
                        }
                    }
                    lineCount++;
                    if (lineCount == numOfCities + numOfHighways + numOfTrips + 3)
                    {
                        tripsDone = true;
                    }
                }
            }

            foreach (String s in trips)
            {
                Console.Out.WriteLine(s);
            }
            Console.ReadLine();
        }
    }
}