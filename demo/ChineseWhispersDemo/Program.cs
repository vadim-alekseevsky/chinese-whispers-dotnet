using VA.Graph.Clustering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChineseWhispersDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test 1\n");
            List<int> TestData = new List<int>() { 0, 1, 2, 20, 21, 27, 60, 61, 72 };
            //Weighted
            ICollection<IEnumerable<int>> Test1Clusters = TestData.ChineseWhispersCluster((v1, v2) => 1 / (double)(Math.Abs(v1 - v2)));

            /* Result
             *  0,  1,  2
             * 20, 21, 27
             * 60, 61, 72
             */
            foreach (IEnumerable<int> Cluster in Test1Clusters)
            {
                Console.WriteLine(String.Join(", ", Cluster.OrderBy(c => c).ToList()));
            }

            Console.WriteLine("\nTest 2\n");
            //Connect only odd and even numbers
            ICollection<IEnumerable<int>> Test2Clusters = TestData.ChineseWhispersCluster(
                (v1, v2) => 1/(double)(Math.Abs(v1 - v2)),
                (v1,v2,w)=> v1%2==v2%2);

            /* Result
             * 0, 2, 20
             * 1, 21, 27, 61
             * 60, 72
             */
            
            foreach (IEnumerable<int> Cluster in Test2Clusters)
            {
                Console.WriteLine(String.Join(", ", Cluster.OrderBy(c => c).ToList()));
            }

            Console.WriteLine("\nTest 3\n");
            //Connect numbers with weight at least 0.25
            ICollection<IEnumerable<int>> Test3Clusters = TestData.ChineseWhispersCluster(
                (v1, v2) => 1 / (double)(Math.Abs(v1 - v2)),
                0.25 //or (v1, v2, w) => w>=0.25
                );

            /* Result
             * 0, 1, 2
             * 20, 21
             * 27
             * 60, 61
             * 72
             */
            foreach (IEnumerable<int> Cluster in Test3Clusters)
            {
                Console.WriteLine(String.Join(", ", Cluster.OrderBy(c=>c).ToList()));
                
            }


            Console.ReadLine();
        }
    }
}
