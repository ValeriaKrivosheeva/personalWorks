using System.Collections.Generic;
using System.Linq;
namespace trains
{
    public class Graph
    {
        public List<Vertex> vertices;
        public List<Edge> edges;
        public Graph()
        {
            vertices = new List<Vertex>();
            edges = new List<Edge>();
        }
        public void AddEdge(Passage passageToAdd)
        {
            Edge edgeToAdd = new Edge(passageToAdd);
            edges.Add(edgeToAdd);
            Vertex v1 = new Vertex(passageToAdd.departSt);
            ProcessVertex(v1, edgeToAdd);
        }
        public List<Edge> GetEdgesByStations(int depart, int arrive)
        {
            List<Edge> allDepartVertices = vertices.Find(v => v.station == depart).edges;
            List<Edge> result = new List<Edge>();
            foreach (Edge item in allDepartVertices)
            {
                if(item.vertex2 == arrive)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        private void ProcessVertex(Vertex v1, Edge edgeToAdd)
        {
            if(vertices.Where(v => v.station == v1.station).Count() != 0)
            {
                vertices.FindAll(v => v.station == v1.station).ForEach(v => v.edges.Add(edgeToAdd));
            }
            else
            {
                v1.edges.Add(edgeToAdd);
                vertices.Add(v1);
            }
        }
        public Vertex GetVertexByNumber(int number)
        {
            Vertex result = (Vertex)vertices.Where(v => v.station == number).First();
            return result;
        }
    }
    public class Vertex
    {
        public int station;
        public List<Edge> edges;
        public Vertex(int station)
        {
            this.station = station;
            edges = new List<Edge>();
        }
    }
    public class Edge
    {
        public Passage passage;
        public int vertex1;
        public int vertex2;
        public Edge(Passage passage)
        {
            this.passage = passage;
            this.vertex1 = passage.departSt;
            this.vertex2 = passage.arrivalSt;
        }

    }
}