using System;
using System.Collections.Generic;
using System.Text;

namespace VA.Graph.Clustering
{
    public static class ChineseWhispersExtension
    {
        public static ICollection<IEnumerable<T>> ChineseWhispersCluster<T>(this IEnumerable<T> Collection, Func<T, T, double> WeightFunction, double Threshold , int? MaxIterations = null)
        {
            return new ChineseWhispers<T>(Collection,WeightFunction,Threshold,MaxIterations).Clusters;
        }
        public static ICollection<IEnumerable<T>> ChineseWhispersCluster<T>(this IEnumerable<T> Collection, Func<T, T, double> WeightFunction, Func<T,T,double, bool> ConnectionFunction=null, int? MaxIterations = null)
        {
            return new ChineseWhispers<T>(Collection, WeightFunction, ConnectionFunction, MaxIterations).Clusters;
        }

        
    }
}
