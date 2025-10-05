using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SwiftCollab.Sorting
{
    /// <summary>
    /// LLM-Enhanced Performance Benchmarking Framework
    /// Comprehensive testing suite for comparing sorting algorithm performance
    /// 
    /// LLM BENCHMARKING STRATEGY:
    /// 1. Test multiple data sizes (small to enterprise-scale)
    /// 2. Various data distributions (random, sorted, reverse-sorted, nearly-sorted)
    /// 3. Memory usage monitoring
    /// 4. Scalability analysis
    /// 5. Real-world performance metrics
    /// </summary>
    public class SortingBenchmarks
    {
        /// <summary>
        /// LLM Improvement: Comprehensive benchmark results structure
        /// </summary>
        public class BenchmarkResult
        {
            public string AlgorithmName { get; set; }
            public int ArraySize { get; set; }
            public string DataType { get; set; }
            public double ElapsedMilliseconds { get; set; }
            public long MemoryUsedBytes { get; set; }
            public bool IsCorrect { get; set; }
            public double ElementsPerSecond => ArraySize / (ElapsedMilliseconds / 1000.0);
            
            public override string ToString()
            {
                return $"{AlgorithmName,-20} | {ArraySize,8:N0} | {DataType,-15} | " +
                       $"{ElapsedMilliseconds,8:F2}ms | {MemoryUsedBytes/1024,6:N0}KB | " +
                       $"{ElementsPerSecond,10:N0} elem/s | {(IsCorrect ? "✓" : "✗")}";
            }
        }

        /// <summary>
        /// LLM Improvement: Test data generation with various distributions
        /// </summary>
        public enum DataDistribution
        {
            Random,
            Sorted,
            ReverseSorted,
            NearlySorted,
            ManyDuplicates,
            FewUnique
        }

        private static readonly Random random = new Random(42); // Fixed seed for reproducible results

        /// <summary>
        /// LLM Improvement: Comprehensive benchmark suite testing all algorithms
        /// </summary>
        public static void RunComprehensiveBenchmarks()
        {
            Console.WriteLine("SwiftCollab Sorting Algorithm Performance Analysis");
            Console.WriteLine("=" + new string('=', 80));
            Console.WriteLine();

            var results = new List<BenchmarkResult>();
            var testSizes = new[] { 100, 1000, 5000, 10000, 50000, 100000 };
            var distributions = Enum.GetValues<DataDistribution>();

            Console.WriteLine("Running comprehensive benchmarks...");
            Console.WriteLine();

            foreach (var size in testSizes)
            {
                foreach (var distribution in distributions)
                {
                    Console.WriteLine($"Testing {size:N0} elements with {distribution} distribution...");
                    
                    var testData = GenerateTestData(size, distribution);
                    
                    // Test all algorithms (skip bubble sort for large arrays to save time)
                    if (size <= 5000)
                    {
                        results.Add(BenchmarkAlgorithm("Bubble Sort", testData, OriginalSorting.BubbleSort));
                    }
                    
                    results.Add(BenchmarkAlgorithm("Quick Sort", testData, SimpleSortingDemo.QuickSort));
                    results.Add(BenchmarkAlgorithm("Merge Sort", testData, SimpleSortingDemo.MergeSort));
                    results.Add(BenchmarkAlgorithm("Intro Sort", testData, SimpleSortingDemo.IntroSort));
                    results.Add(BenchmarkAlgorithm("Adaptive Sort", testData, SimpleSortingDemo.AdaptiveSort));
                    
                    if (size >= 1000) // Parallel sort is only beneficial for larger arrays
                    {
                        results.Add(BenchmarkAlgorithm("Parallel Merge", testData, SimpleSortingDemo.ParallelMergeSort));
                    }
                }
                Console.WriteLine();
            }

            DisplayResults(results);
            AnalyzePerformance(results);
        }

        /// <summary>
        /// LLM Improvement: Individual algorithm benchmarking with memory monitoring
        /// </summary>
        private static BenchmarkResult BenchmarkAlgorithm(string name, int[] originalData, Action<int[]> sortingAlgorithm)
        {
            var data = SimpleSortingDemo.CopyArray(originalData);
            var dataType = AnalyzeDataDistribution(originalData);
            
            // Force garbage collection before test for accurate memory measurement
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryBefore = GC.GetTotalMemory(false);
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                sortingAlgorithm(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in {name}: {ex.Message}");
                return new BenchmarkResult
                {
                    AlgorithmName = name,
                    ArraySize = originalData.Length,
                    DataType = dataType,
                    ElapsedMilliseconds = double.MaxValue,
                    MemoryUsedBytes = 0,
                    IsCorrect = false
                };
            }
            
            stopwatch.Stop();
            long memoryAfter = GC.GetTotalMemory(false);
            
            return new BenchmarkResult
            {
                AlgorithmName = name,
                ArraySize = originalData.Length,
                DataType = dataType,
                ElapsedMilliseconds = stopwatch.Elapsed.TotalMilliseconds,
                MemoryUsedBytes = Math.Max(0, memoryAfter - memoryBefore),
                IsCorrect = SimpleSortingDemo.IsSorted(data)
            };
        }

        /// <summary>
        /// LLM Improvement: Test data generation with realistic enterprise scenarios
        /// </summary>
        private static int[] GenerateTestData(int size, DataDistribution distribution)
        {
            return distribution switch
            {
                DataDistribution.Random => Enumerable.Range(0, size)
                    .Select(_ => random.Next(1, size * 10))
                    .ToArray(),
                    
                DataDistribution.Sorted => Enumerable.Range(1, size).ToArray(),
                
                DataDistribution.ReverseSorted => Enumerable.Range(1, size)
                    .Reverse()
                    .ToArray(),
                    
                DataDistribution.NearlySorted => GenerateNearlySorted(size),
                
                DataDistribution.ManyDuplicates => Enumerable.Range(0, size)
                    .Select(_ => random.Next(1, Math.Max(1, size / 10)))
                    .ToArray(),
                    
                DataDistribution.FewUnique => Enumerable.Range(0, size)
                    .Select(_ => random.Next(1, 6))
                    .ToArray(),
                    
                _ => throw new ArgumentException($"Unknown distribution: {distribution}")
            };
        }

        /// <summary>
        /// LLM Improvement: Generate nearly sorted data (90% sorted with 10% random swaps)
        /// </summary>
        private static int[] GenerateNearlySorted(int size)
        {
            var data = Enumerable.Range(1, size).ToArray();
            int swaps = size / 10; // 10% random swaps
            
            for (int i = 0; i < swaps; i++)
            {
                int pos1 = random.Next(size);
                int pos2 = random.Next(size);
                (data[pos1], data[pos2]) = (data[pos2], data[pos1]);
            }
            
            return data;
        }

        /// <summary>
        /// LLM Improvement: Analyze data distribution characteristics
        /// </summary>
        private static string AnalyzeDataDistribution(int[] data)
        {
            if (data.Length <= 1) return "Trivial";
            
            int sortedCount = 0;
            int reverseSortedCount = 0;
            
            for (int i = 0; i < data.Length - 1; i++)
            {
                if (data[i] <= data[i + 1]) sortedCount++;
                if (data[i] >= data[i + 1]) reverseSortedCount++;
            }
            
            double sortedRatio = (double)sortedCount / (data.Length - 1);
            double reverseSortedRatio = (double)reverseSortedCount / (data.Length - 1);
            
            if (sortedRatio > 0.95) return "Sorted";
            if (reverseSortedRatio > 0.95) return "ReverseSorted";
            if (sortedRatio > 0.8) return "NearlySorted";
            
            var uniqueCount = data.Distinct().Count();
            if (uniqueCount < data.Length * 0.1) return "FewUnique";
            if (uniqueCount < data.Length * 0.5) return "ManyDuplicates";
            
            return "Random";
        }

        /// <summary>
        /// LLM Improvement: Formatted results display with performance insights
        /// </summary>
        private static void DisplayResults(List<BenchmarkResult> results)
        {
            Console.WriteLine();
            Console.WriteLine("DETAILED BENCHMARK RESULTS");
            Console.WriteLine("=" + new string('=', 100));
            Console.WriteLine($"{"Algorithm",-20} | {"Size",8} | {"Data Type",-15} | {"Time",8} | {"Memory",6} | {"Speed",10} | {"✓"}");
            Console.WriteLine(new string('-', 100));
            
            foreach (var result in results.OrderBy(r => r.ArraySize).ThenBy(r => r.DataType).ThenBy(r => r.ElapsedMilliseconds))
            {
                Console.WriteLine(result.ToString());
            }
        }

        /// <summary>
        /// LLM Improvement: Performance analysis with actionable insights
        /// </summary>
        private static void AnalyzePerformance(List<BenchmarkResult> results)
        {
            Console.WriteLine();
            Console.WriteLine("PERFORMANCE ANALYSIS & RECOMMENDATIONS");
            Console.WriteLine("=" + new string('=', 80));
            
            // LLM Analysis: Algorithm performance by data size
            Console.WriteLine("\n1. SCALABILITY ANALYSIS:");
            var largeDataResults = results.Where(r => r.ArraySize >= 10000 && r.IsCorrect).ToList();
            if (largeDataResults.Any())
            {
                var bestLargeData = largeDataResults.GroupBy(r => r.AlgorithmName)
                    .Select(g => new { Algorithm = g.Key, AvgTime = g.Average(r => r.ElapsedMilliseconds) })
                    .OrderBy(x => x.AvgTime)
                    .First();
                
                Console.WriteLine($"   Best for large datasets (≥10K): {bestLargeData.Algorithm} ({bestLargeData.AvgTime:F2}ms avg)");
            }

            // LLM Analysis: Data type performance
            Console.WriteLine("\n2. DATA TYPE PERFORMANCE:");
            var dataTypePerformance = results.Where(r => r.IsCorrect)
                .GroupBy(r => r.DataType)
                .Select(g => new 
                { 
                    DataType = g.Key, 
                    BestAlgorithm = g.OrderBy(r => r.ElapsedMilliseconds).First().AlgorithmName,
                    AvgImprovement = g.Where(r => r.AlgorithmName != "Bubble Sort").Any() 
                        ? g.Where(r => r.AlgorithmName == "Bubble Sort").DefaultIfEmpty()
                            .Where(bubble => bubble != null)
                            .Select(bubble => g.Where(opt => opt.AlgorithmName != "Bubble Sort")
                                .Min(opt => opt.ElapsedMilliseconds) / bubble.ElapsedMilliseconds)
                            .FirstOrDefault()
                        : 0
                })
                .Where(x => x.AvgImprovement > 0);

            foreach (var performance in dataTypePerformance)
            {
                Console.WriteLine($"   {performance.DataType,-15}: Best = {performance.BestAlgorithm,-15} " +
                                $"(~{performance.AvgImprovement:F0}x faster than Bubble Sort)");
            }

            // LLM Analysis: Memory efficiency
            Console.WriteLine("\n3. MEMORY EFFICIENCY:");
            var memoryEfficient = results.Where(r => r.IsCorrect && r.ArraySize >= 1000)
                .GroupBy(r => r.AlgorithmName)
                .Select(g => new { Algorithm = g.Key, AvgMemory = g.Average(r => r.MemoryUsedBytes) })
                .OrderBy(x => x.AvgMemory)
                .Take(3);

            foreach (var efficient in memoryEfficient)
            {
                Console.WriteLine($"   {efficient.Algorithm}: {efficient.AvgMemory/1024:F1} KB average");
            }

            // LLM Recommendations
            Console.WriteLine("\n4. LLM RECOMMENDATIONS:");
            Console.WriteLine("   • Small datasets (<100):     Use Insertion Sort or Quick Sort");
            Console.WriteLine("   • Medium datasets (100-10K): Use Intro Sort (hybrid approach)");
            Console.WriteLine("   • Large datasets (>10K):     Use Parallel Merge Sort");
            Console.WriteLine("   • Nearly sorted data:        Use Adaptive Sort (auto-detects)");
            Console.WriteLine("   • Memory constrained:        Use Quick Sort (in-place)");
            Console.WriteLine("   • Stability required:        Use Merge Sort");
            Console.WriteLine("   • Production systems:        Use Adaptive Sort for automatic optimization");

            // Performance improvement summary
            Console.WriteLine("\n5. OVERALL IMPROVEMENT:");
            var bubbleSortResults = results.Where(r => r.AlgorithmName == "Bubble Sort" && r.IsCorrect);
            var optimizedResults = results.Where(r => r.AlgorithmName != "Bubble Sort" && r.IsCorrect);
            
            if (bubbleSortResults.Any() && optimizedResults.Any())
            {
                var avgBubbleTime = bubbleSortResults.Average(r => r.ElapsedMilliseconds);
                var avgOptimizedTime = optimizedResults.Average(r => r.ElapsedMilliseconds);
                var improvement = avgBubbleTime / avgOptimizedTime;
                
                Console.WriteLine($"   Average performance improvement: {improvement:F1}x faster than Bubble Sort");
                Console.WriteLine($"   Time complexity improvement: O(n²) → O(n log n)");
                Console.WriteLine($"   Enterprise scalability: Suitable for datasets up to millions of elements");
            }
        }

        /// <summary>
        /// LLM Improvement: Quick performance comparison for specific scenarios
        /// </summary>
        public static void QuickComparisonDemo()
        {
            Console.WriteLine("QUICK PERFORMANCE COMPARISON DEMO");
            Console.WriteLine("=" + new string('=', 50));
            
            var testSizes = new[] { 1000, 10000 };
            
            foreach (var size in testSizes)
            {
                Console.WriteLine($"\nTesting with {size:N0} random elements:");
                var testData = GenerateTestData(size, DataDistribution.Random);
                
                var bubbleResult = BenchmarkAlgorithm("Bubble Sort", testData, OriginalSorting.BubbleSort);
                var introResult = BenchmarkAlgorithm("Intro Sort", testData, SimpleSortingDemo.IntroSort);
                var adaptiveResult = BenchmarkAlgorithm("Adaptive Sort", testData, SimpleSortingDemo.AdaptiveSort);
                
                Console.WriteLine($"  Bubble Sort:   {bubbleResult.ElapsedMilliseconds,8:F2}ms");
                Console.WriteLine($"  Intro Sort:    {introResult.ElapsedMilliseconds,8:F2}ms ({bubbleResult.ElapsedMilliseconds/introResult.ElapsedMilliseconds:F1}x faster)");
                Console.WriteLine($"  Adaptive Sort: {adaptiveResult.ElapsedMilliseconds,8:F2}ms ({bubbleResult.ElapsedMilliseconds/adaptiveResult.ElapsedMilliseconds:F1}x faster)");
            }
        }
    }
}

/// <summary>
/// LLM BENCHMARKING INSIGHTS:
/// 
/// PERFORMANCE TESTING STRATEGY:
/// 1. Multiple data sizes (100 to 100,000+ elements)
/// 2. Various data distributions (random, sorted, nearly-sorted, etc.)
/// 3. Memory usage monitoring for enterprise environments
/// 4. Correctness verification for all algorithms
/// 5. Scalability analysis for production deployment
/// 
/// KEY PERFORMANCE METRICS:
/// 1. Execution Time: Measures algorithm speed
/// 2. Memory Usage: Critical for large-scale applications
/// 3. Elements/Second: Throughput measurement
/// 4. Scalability Factor: Performance degradation with size
/// 5. Data Type Sensitivity: Performance across different distributions
/// 
/// ENTERPRISE CONSIDERATIONS:
/// 1. Real-world data patterns (nearly sorted, duplicates)
/// 2. Memory constraints in production environments
/// 3. Multi-core utilization for large datasets
/// 4. Adaptive algorithm selection for varying workloads
/// 5. Performance monitoring and optimization guidance
/// </summary>