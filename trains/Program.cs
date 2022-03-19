using System;
using System.Collections.Generic;
using System.Linq;

namespace trains
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Passage> input = new List<Passage>();
            try
            {
                input = Parsing.ParseCsvToPassageList("./test_task_data.csv");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
            Graph graph = new Graph();
            foreach (Passage p in input)
            {
                graph.AddEdge(p);
            }

            double[,] minCostsMatrix = CreateMinCostsMatrix(graph);

            ProcessTSP tsp = new ProcessTSP(minCostsMatrix);
            List<Tuple<int, int>> route = tsp.GetNearestWay();
            List<Tuple<int, int>> sortedRoute = new List<Tuple<int, int>>();
            sortedRoute.Add(route[0]);
            for(int i = 1; i < route.Count; i++)
            {
                Tuple<int, int> currentRoute = route.Where(t => t.Item1 == sortedRoute[i-1].Item2).First();
                sortedRoute.Add(currentRoute);
            }

            Console.WriteLine($"Best route by price:");
            double totalPrice = 0;
            foreach(Tuple<int, int> item in sortedRoute)
            {
                List<Edge> edgesByStations = graph.GetEdgesByStations(item.Item1, item.Item2);
                double minCost = edgesByStations[0].passage.cost;
                List<int> trainNums = new List<int>();
                foreach(Edge edge in edgesByStations)
                {
                    if(edge.passage.cost <= minCost)
                    {
                        minCost = edge.passage.cost;
                        trainNums.Add(edge.passage.trainNum);
                    }
                }
                totalPrice += minCost;
                Console.Write($"({item.Item1.ToString()}, {item.Item2.ToString()}) by train ");
                for(int i = 0; i < trainNums.Count; i++)
                {
                    if(i != 0)
                    {
                        Console.Write("or ");
                    }
                    Console.Write(trainNums[i].ToString() + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Total price - " + totalPrice.ToString("N2"));

            ProcessGraphTraversal fastest = new ProcessGraphTraversal(graph);
            List<GraphTimeTraversal> result = fastest.GetFastestWay();
            List<int> allTime = new List<int>();
            foreach (GraphTimeTraversal item in result)
            {
                allTime.Add(item.sumTime);
            }
            int minTime = allTime.Min();
            List<GraphTimeTraversal> fastestTraversal = result.FindAll(t => t.sumTime == minTime);
            
            Console.WriteLine("\nBest route(s) by time:");
            for(int i = 0; i < fastestTraversal.Count; i++)
            {
                foreach (Tuple<int, int, int> item in fastestTraversal[i].route)
                {
                    Console.WriteLine($"({item.Item1.ToString()}, {item.Item2.ToString()}) by train {item.Item3}");
                }
                int hours = (int)fastestTraversal[i].sumTime/60;
                int minutes = fastestTraversal[i].sumTime % 60;
                Console.WriteLine($"Total time - {hours.ToString()} h {minutes.ToString()} m");
            }
        }
        static double[,] CreateMinCostsMatrix(Graph graph)
        {
            double[,] result = new double[graph.vertices.Count+1,graph.vertices.Count+1];
            int count = 1;
            foreach(Vertex v in graph.vertices)
            {
                result[0, count] = v.station;
                result[count, 0] = v.station;
                count++;
            }
            foreach (Edge e in graph.edges)
            {
                int departSt = FindStationInMatrix(result, e.vertex1);
                int arrivSt = FindStationInMatrix(result, e.vertex2);
                if(result[departSt, arrivSt] != 0 && e.passage.cost < result[departSt, arrivSt])
                {
                    result[departSt, arrivSt] = e.passage.cost;
                }
                else if(result[departSt, arrivSt] == 0)
                {
                    result[departSt, arrivSt] = e.passage.cost;
                }
            }
            for(int i = 0; i<result.GetLength(0); i++)
            {
                for(int j = 0; j<result.GetLength(1); j++)
                {
                    if(result[i, j] == 0)
                    {
                        result[i, j] = Double.PositiveInfinity;
                    }
                }
            }
            return result;
        }
        static int FindStationInMatrix(double[,] input, int statToSearch)
        {
            int result = 0;
            for(int i = 1; i<input.GetLength(0); i++)
            {
                if(input[i, 0] == statToSearch)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
    }
}
