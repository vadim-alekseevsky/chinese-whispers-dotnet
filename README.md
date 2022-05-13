# Chinese Whispers (clustering method) C# Implementation

## Getting Started

Chinese Whispers algorithm is described at https://en.wikipedia.org/wiki/Chinese_Whispers_(clustering_method)

Fisher–Yates shuffle algorithm is used for random permutation https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle


       List<int> TestData = new List<int>() { 0, 1, 2, 20, 21, 27, 60, 61, 72 };
       
       ICollection<IEnumerable<int>> Test1Clusters = TestData.ChineseWhispersCluster((v1, v2) => 1 / (double)(Math.Abs(v1 - v2)));
       
       /* Result
        *  0,  1,  2
        * 20, 21, 27
        * 60, 61, 72
        */
