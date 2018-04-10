using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VA.Graph.Clustering
{
    public class ChineseWhispers<T>
    {
        //Internal CW Weighted Graph Structure
        #region Graph Structure Classes
        protected class CWGraph
        {
            public List<CWVertex> Vertices;
        }

        protected class CWVertex
        {
            public int Cluster;
            public List<CWEdge> Edges=new List<CWEdge>();
            public T Source;
        }

        protected class CWEdge
        {
            public double Weight =0;
            public CWVertex Vertex1;
            public CWVertex Vertex2;

            public CWVertex GetVertex(CWVertex Vertex)
            {
                return (Vertex == Vertex1) ? Vertex2 : Vertex1;
            }
        }
        #endregion

        protected ICollection<IEnumerable<T>> _Clusters;

        
        public ChineseWhispers(IEnumerable<T> Collection, Func<T, T, double> WeightFunction, double Threshold, int? MaxIterations = null)
        {
            Compute(Collection, WeightFunction, (v1, v2, w) => w>Threshold, MaxIterations);
        }
        public ChineseWhispers(IEnumerable<T> Collection, Func<T, T, double> WeightFunction, Func<T, T,double, bool> ConnectionFunction=null, int? MaxIterations = null)
        {
            Compute(Collection, WeightFunction, ConnectionFunction, MaxIterations);
            
        }
        protected void Compute(IEnumerable<T> Collection, Func<T, T, double> WeightFunction, Func<T, T,double, bool> ConnectionFunction, int? MaxIterations = null)
        {
            CWGraph Graph = CreateGraph(Collection, WeightFunction, ConnectionFunction);
            int IterationIndex = 0;
            bool isChanged = false;
            Dictionary<int, double> WeightMap = new Dictionary<int, double>();
            do
            {
                isChanged = false;
                IterationIndex++;
                Randomize(Graph.Vertices);
                foreach (CWVertex Vertex in Graph.Vertices)
                {

                    foreach (CWEdge Edge in Vertex.Edges)
                    {

                        if (Edge.GetVertex(Vertex).Edges.Count == 0) continue;
                        if (!WeightMap.ContainsKey(Edge.GetVertex(Vertex).Cluster))
                        {
                            WeightMap.Add(Edge.GetVertex(Vertex).Cluster, 0);

                        }
                        WeightMap[Edge.GetVertex(Vertex).Cluster] += Edge.Weight;//  Edge.GetVertex(Vertex).Edges.Sum(edge => edge.Weight);
                    }
                    if (WeightMap.Count == 0) continue;
                    KeyValuePair<int, double> Max = GetMaxPair(WeightMap);
                    if (Max.Key != Vertex.Cluster)
                    {
                        isChanged = true;
                        Vertex.Cluster = Max.Key;
                    }
                    WeightMap.Clear();
                }
            }
            while (isChanged || (MaxIterations.HasValue && IterationIndex >= MaxIterations.Value));

            _Clusters = Graph.Vertices.GroupBy(c => c.Cluster).Select(group => group.AsEnumerable().Select(v => v.Source)).ToList();
        }

        protected KeyValuePair<int,double> GetMaxPair(Dictionary<int,double> dictionary)
        {
            double max = double.MinValue;
            KeyValuePair<int, double> Result;
            foreach(KeyValuePair<int,double> Pair in dictionary)
            {
                if(Pair.Value>max)
                {
                    max = Pair.Value;
                    Result = Pair;
                }
            }
            return Result;
        }

        protected CWGraph CreateGraph(IEnumerable<T> Collection, Func<T, T, double> WeightFunction, Func<T, T,double, bool> ConnectionFunction)
        {
            CWGraph Graph = new CWGraph();
            if(Collection is ICollection<T>)
            {
                Graph.Vertices = new List<CWVertex>(((ICollection<T>)Collection).Count);
            }
            else
            {
                Graph.Vertices = new List<CWVertex>();
            }
            foreach(T Source in Collection)
            {
                CWVertex Vertex = new CWVertex() { Source = Source };
                Vertex.Cluster = Vertex.GetHashCode();
                Graph.Vertices.Add(Vertex);
            }
            for(int i=0;i<Graph.Vertices.Count;i++)
            {
                for(int j=i+1;j<Graph.Vertices.Count;j++)
                {
                    double Weight = WeightFunction(Graph.Vertices[i].Source, Graph.Vertices[j].Source);
                    if(ConnectionFunction==null || ConnectionFunction(Graph.Vertices[i].Source, Graph.Vertices[j].Source,Weight))
                    {
                        CWEdge Edge = new CWEdge() { Vertex1 = Graph.Vertices[i], Vertex2 = Graph.Vertices[j], Weight = Weight };
                        Graph.Vertices[i].Edges.Add(Edge);
                        Graph.Vertices[j].Edges.Add(Edge);
                    }
                }
            }
            return Graph;
        }

        //Fisher–Yates shuffle https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        protected void Randomize(List<CWVertex> Vertices)
        {
           Random Rand = new Random();
           int n = Vertices.Count;
           while (n > 1)
           {
               n--;
               int k = Rand.Next(n + 1);
               CWVertex value = Vertices[k];
               Vertices[k] = Vertices[n];
               Vertices[n] = value;
           }
        }

        public ICollection<IEnumerable<T>> Clusters
        {
            get
            {
                return _Clusters;
            }
        }

    }
}
