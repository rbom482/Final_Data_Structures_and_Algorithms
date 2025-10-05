using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SwiftCollab.ApiScheduling
{
    /// <summary>
    /// Enhanced API Request with metadata for production use
    /// LLM Improvement: Added timestamp, request ID, and processing state for better tracking
    /// </summary>
    public class ApiRequest : IComparable<ApiRequest>
    {
        public string RequestId { get; set; }
        public string Endpoint { get; set; }
        public int Priority { get; set; }
        public DateTime Timestamp { get; set; }
        public string ClientId { get; set; }
        public RequestStatus Status { get; set; }

        public ApiRequest(string endpoint, int priority, string clientId = null)
        {
            RequestId = Guid.NewGuid().ToString();
            Endpoint = endpoint;
            Priority = priority;
            Timestamp = DateTime.UtcNow;
            ClientId = clientId ?? "anonymous";
            Status = RequestStatus.Queued;
        }

        /// <summary>
        /// LLM Improvement: Implements IComparable for heap operations
        /// Lower priority numbers = higher priority (1 = highest, 10 = lowest)
        /// </summary>
        public int CompareTo(ApiRequest other)
        {
            if (other == null) return 1;
            
            // Primary sort: Priority (ascending - lower number = higher priority)
            int priorityCompare = Priority.CompareTo(other.Priority);
            if (priorityCompare != 0) return priorityCompare;
            
            // Secondary sort: Timestamp (ascending - older requests first)
            return Timestamp.CompareTo(other.Timestamp);
        }

        public override string ToString()
        {
            return $"[{RequestId[..8]}] {Endpoint} (Priority: {Priority}, Client: {ClientId}, Status: {Status})";
        }
    }

    public enum RequestStatus
    {
        Queued,
        Processing,
        Completed,
        Failed
    }

    /// <summary>
    /// High-performance thread-safe priority queue using binary min-heap
    /// LLM Improvements: Min-heap implementation, thread safety, batch operations, performance monitoring
    /// </summary>
    public class OptimizedApiRequestQueue
    {
        private readonly List<ApiRequest> heap;
        private readonly ReaderWriterLockSlim lockSlim;
        private readonly object statsLock = new object();
        
        // Performance monitoring - LLM suggested addition
        private long totalEnqueued;
        private long totalDequeued;
        private long totalProcessingTime;

        public OptimizedApiRequestQueue(int initialCapacity = 16)
        {
            heap = new List<ApiRequest>(initialCapacity);
            lockSlim = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// LLM Improvement: Thread-safe property access with performance stats
        /// </summary>
        public int Count 
        { 
            get 
            { 
                lockSlim.EnterReadLock();
                try { return heap.Count; }
                finally { lockSlim.ExitReadLock(); }
            } 
        }

        public bool IsEmpty => Count == 0;

        #region Core Heap Operations

        /// <summary>
        /// LLM Improvement: O(log n) enqueue using binary heap instead of O(n log n) sorting
        /// Thread-safe implementation with proper locking
        /// </summary>
        public void Enqueue(ApiRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var startTime = DateTime.UtcNow;

            lockSlim.EnterWriteLock();
            try
            {
                heap.Add(request);
                HeapifyUp(heap.Count - 1);
                
                lock (statsLock)
                {
                    totalEnqueued++;
                    totalProcessingTime += (DateTime.UtcNow - startTime).Ticks;
                }
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// LLM Improvement: O(log n) dequeue using heap operations instead of O(n) array manipulation
        /// Returns highest priority request (lowest priority number)
        /// </summary>
        public ApiRequest Dequeue()
        {
            var startTime = DateTime.UtcNow;

            lockSlim.EnterWriteLock();
            try
            {
                if (heap.Count == 0)
                    return null;

                ApiRequest root = heap[0];
                ApiRequest lastElement = heap[heap.Count - 1];
                heap.RemoveAt(heap.Count - 1);

                if (heap.Count > 0)
                {
                    heap[0] = lastElement;
                    HeapifyDown(0);
                }

                root.Status = RequestStatus.Processing;
                
                lock (statsLock)
                {
                    totalDequeued++;
                    totalProcessingTime += (DateTime.UtcNow - startTime).Ticks;
                }

                return root;
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// LLM Improvement: Peek at highest priority request without removing it
        /// Useful for monitoring and decision making
        /// </summary>
        public ApiRequest Peek()
        {
            lockSlim.EnterReadLock();
            try
            {
                return heap.Count > 0 ? heap[0] : null;
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        #endregion

        #region Batch Operations - LLM Suggested Enhancement

        /// <summary>
        /// LLM Improvement: Bulk enqueue operation for high-throughput scenarios
        /// More efficient than individual enqueues for large batches
        /// </summary>
        public void EnqueueBatch(IEnumerable<ApiRequest> requests)
        {
            if (requests == null)
                throw new ArgumentNullException(nameof(requests));

            var requestList = requests as List<ApiRequest> ?? requests.ToList();
            if (!requestList.Any()) return;

            var startTime = DateTime.UtcNow;

            lockSlim.EnterWriteLock();
            try
            {
                foreach (var request in requestList)
                {
                    if (request != null)
                    {
                        heap.Add(request);
                        HeapifyUp(heap.Count - 1);
                    }
                }

                lock (statsLock)
                {
                    totalEnqueued += requestList.Count;
                    totalProcessingTime += (DateTime.UtcNow - startTime).Ticks;
                }
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// LLM Improvement: Batch dequeue for processing multiple requests efficiently
        /// Useful for worker threads that can process multiple requests in parallel
        /// </summary>
        public List<ApiRequest> DequeueBatch(int maxCount)
        {
            if (maxCount <= 0)
                throw new ArgumentException("Max count must be positive", nameof(maxCount));

            var results = new List<ApiRequest>(maxCount);
            var startTime = DateTime.UtcNow;

            lockSlim.EnterWriteLock();
            try
            {
                int itemsToTake = Math.Min(maxCount, heap.Count);
                
                for (int i = 0; i < itemsToTake; i++)
                {
                    if (heap.Count == 0) break;

                    ApiRequest root = heap[0];
                    ApiRequest lastElement = heap[heap.Count - 1];
                    heap.RemoveAt(heap.Count - 1);

                    if (heap.Count > 0)
                    {
                        heap[0] = lastElement;
                        HeapifyDown(0);
                    }

                    root.Status = RequestStatus.Processing;
                    results.Add(root);
                }

                lock (statsLock)
                {
                    totalDequeued += results.Count;
                    totalProcessingTime += (DateTime.UtcNow - startTime).Ticks;
                }

                return results;
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        #endregion

        #region Heap Maintenance Operations

        /// <summary>
        /// LLM Improvement: Efficient heap maintenance - bubble up for maintaining min-heap property
        /// Ensures parent is always smaller than children (higher priority)
        /// </summary>
        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                
                if (heap[index].CompareTo(heap[parentIndex]) >= 0)
                    break;

                // Swap with parent
                (heap[index], heap[parentIndex]) = (heap[parentIndex], heap[index]);
                index = parentIndex;
            }
        }

        /// <summary>
        /// LLM Improvement: Efficient heap maintenance - bubble down for maintaining min-heap property
        /// Ensures the min-heap property is maintained after removal
        /// </summary>
        private void HeapifyDown(int index)
        {
            while (true)
            {
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;
                int smallest = index;

                if (leftChild < heap.Count && heap[leftChild].CompareTo(heap[smallest]) < 0)
                    smallest = leftChild;

                if (rightChild < heap.Count && heap[rightChild].CompareTo(heap[smallest]) < 0)
                    smallest = rightChild;

                if (smallest == index)
                    break;

                // Swap with smallest child
                (heap[index], heap[smallest]) = (heap[smallest], heap[index]);
                index = smallest;
            }
        }

        #endregion

        #region Advanced Features - LLM Enhancements

        /// <summary>
        /// LLM Improvement: Priority-based filtering for monitoring and analytics
        /// </summary>
        public List<ApiRequest> GetRequestsByPriority(int priority)
        {
            lockSlim.EnterReadLock();
            try
            {
                return heap.Where(r => r.Priority == priority).ToList();
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// LLM Improvement: Remove specific request (useful for cancellation scenarios)
        /// </summary>
        public bool RemoveRequest(string requestId)
        {
            lockSlim.EnterWriteLock();
            try
            {
                for (int i = 0; i < heap.Count; i++)
                {
                    if (heap[i].RequestId == requestId)
                    {
                        // Move last element to this position and heapify
                        ApiRequest lastElement = heap[heap.Count - 1];
                        heap.RemoveAt(heap.Count - 1);

                        if (i < heap.Count)
                        {
                            heap[i] = lastElement;
                            
                            // Maintain heap property
                            HeapifyUp(i);
                            HeapifyDown(i);
                        }
                        
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// LLM Improvement: Performance monitoring and statistics
        /// Essential for production monitoring and capacity planning
        /// </summary>
        public QueueStatistics GetStatistics()
        {
            lockSlim.EnterReadLock();
            try
            {
                lock (statsLock)
                {
                    return new QueueStatistics
                    {
                        CurrentQueueSize = heap.Count,
                        TotalEnqueued = totalEnqueued,
                        TotalDequeued = totalDequeued,
                        AverageProcessingTimeMs = totalEnqueued > 0 
                            ? (double)totalProcessingTime / totalEnqueued / 10000 
                            : 0,
                        HighestPriorityWaiting = heap.Count > 0 ? heap[0].Priority : (int?)null,
                        OldestRequestAge = heap.Count > 0 
                            ? DateTime.UtcNow.Subtract(heap.Min(r => r.Timestamp)).TotalSeconds 
                            : (double?)null
                    };
                }
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// LLM Improvement: Clear all requests (useful for testing and emergency scenarios)
        /// </summary>
        public void Clear()
        {
            lockSlim.EnterWriteLock();
            try
            {
                heap.Clear();
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            lockSlim?.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// LLM Improvement: Statistics class for monitoring queue performance
    /// </summary>
    public class QueueStatistics
    {
        public int CurrentQueueSize { get; set; }
        public long TotalEnqueued { get; set; }
        public long TotalDequeued { get; set; }
        public double AverageProcessingTimeMs { get; set; }
        public int? HighestPriorityWaiting { get; set; }
        public double? OldestRequestAge { get; set; }

        public override string ToString()
        {
            return $"Queue Stats - Size: {CurrentQueueSize}, " +
                   $"Enqueued: {TotalEnqueued}, Dequeued: {TotalDequeued}, " +
                   $"Avg Processing: {AverageProcessingTimeMs:F2}ms, " +
                   $"Highest Priority Waiting: {HighestPriorityWaiting?.ToString() ?? "None"}, " +
                   $"Oldest Request: {OldestRequestAge?.ToString("F1") ?? "N/A"}s";
        }
    }

    /// <summary>
    /// LLM Improvement: Thread-safe worker pool for concurrent request processing
    /// Demonstrates how the optimized queue integrates with production systems
    /// </summary>
    public class ApiRequestProcessor
    {
        private readonly OptimizedApiRequestQueue queue;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly Task[] workerTasks;
        private readonly int workerCount;

        public ApiRequestProcessor(OptimizedApiRequestQueue queue, int workerCount = 4)
        {
            this.queue = queue;
            this.workerCount = workerCount;
            this.cancellationTokenSource = new CancellationTokenSource();
            this.workerTasks = new Task[workerCount];
        }

        public void StartProcessing()
        {
            for (int i = 0; i < workerCount; i++)
            {
                int workerId = i;
                workerTasks[i] = Task.Run(() => ProcessRequests(workerId, cancellationTokenSource.Token));
            }
        }

        public async Task StopProcessing()
        {
            cancellationTokenSource.Cancel();
            await Task.WhenAll(workerTasks);
        }

        private async Task ProcessRequests(int workerId, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var requests = queue.DequeueBatch(5); // Process up to 5 requests at once
                    
                    if (requests.Count == 0)
                    {
                        await Task.Delay(10, cancellationToken); // Brief wait if queue is empty
                        continue;
                    }

                    // Simulate processing requests in parallel
                    var processingTasks = requests.Select(async request =>
                    {
                        await SimulateApiProcessing(request, cancellationToken);
                        request.Status = RequestStatus.Completed;
                        Console.WriteLine($"Worker {workerId} completed: {request}");
                    });

                    await Task.WhenAll(processingTasks);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Worker {workerId} error: {ex.Message}");
                }
            }
        }

        private async Task SimulateApiProcessing(ApiRequest request, CancellationToken cancellationToken)
        {
            // Simulate different processing times based on endpoint
            int processingTime = request.Endpoint switch
            {
                "/auth" => 50,
                "/healthcheck" => 10,
                "/data" => 200,
                _ => 100
            };

            await Task.Delay(processingTime, cancellationToken);
        }
    }
}