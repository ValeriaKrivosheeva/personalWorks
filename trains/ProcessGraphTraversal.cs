using System.Collections.Generic;
using System;
using System.Linq;
namespace trains
{
    public class GraphTimeTraversal
    {
        public int sumTime;
        public List<Tuple<int, int, int>> route;
        public GraphTimeTraversal(int sumTime, List<Tuple<int, int, int>> route)
        {
            this.sumTime = sumTime;
            this.route = route;
        }
    }
    public class ProcessGraphTraversal
    {
        List<GraphTimeTraversal> routes;
        Graph graph;
        public ProcessGraphTraversal(Graph graph)
        {
            this.graph = graph;
            routes = new List<GraphTimeTraversal>();
        }
        public List<GraphTimeTraversal> GetFastestWay()
        {
            foreach(Vertex v in graph.vertices)
            {
                Recursion(new List<Tuple<int, int, int>>(), v, 0, new DateTime());
            }
            return routes;
        }
        private void Recursion(List<Tuple<int, int, int>> currentV, Vertex v, int sumTime, DateTime lastArrive)
        {
            List<Tuple<int, int, int>> copyList = new List<Tuple<int, int, int>>();
            foreach (Tuple<int, int, int> item in currentV)
            {
                copyList.Add(item);
            }
            foreach(Edge e in v.edges)
            {
                List<Tuple<int, int, int>> curTraversal = new List<Tuple<int, int, int>>();
                foreach (Tuple<int, int, int> item in copyList)
                {
                    curTraversal.Add(item);
                }
                if(curTraversal.Count != 0 && curTraversal.Where(t => t.Item2 == e.vertex2).Count() != 0 || curTraversal.Where(t => t.Item1 == e.vertex2).Count() != 0) 
                {
                    continue;
                }
                if(curTraversal.Count != 0)
                {
                    if(e.passage.departTime < lastArrive)
                    {
                        sumTime += 24*60 - (lastArrive.Hour * 60 + lastArrive.Minute - e.passage.departTime.Hour*60 - e.passage.departTime.Minute); 
                    }
                    else
                    {
                        sumTime += e.passage.departTime.Hour * 60 + e.passage.departTime.Minute - lastArrive.Hour * 60 - lastArrive.Minute;
                    }
                }
                if(e.passage.departTime < e.passage.arrivalTime)
                {
                    sumTime += e.passage.arrivalTime.Hour * 60 + e.passage.arrivalTime.Minute - e.passage.departTime.Hour * 60 - e.passage.departTime.Minute;
                }
                else
                {
                    sumTime += 24*60 - (e.passage.departTime.Hour * 60 + e.passage.departTime.Minute - e.passage.arrivalTime.Hour*60 - e.passage.arrivalTime.Minute); 
                }
                curTraversal.Add(new Tuple<int, int, int>(e.vertex1, e.vertex2, e.passage.trainNum));
                if(curTraversal.Count == 5)
                {
                    GraphTimeTraversal currentTraversal = new GraphTimeTraversal(sumTime, curTraversal);
                    routes.Add(currentTraversal);
                }
                else
                {
                    List<Tuple<int, int, int>> currentList = curTraversal;
                    Recursion(currentList, graph.GetVertexByNumber(e.vertex2), sumTime, e.passage.arrivalTime);
                }
            }
        }
    }
}