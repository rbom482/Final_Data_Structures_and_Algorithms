using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SwiftCollab.TaskExecution
{
    /// <summary>
    /// LLM-OPTIMIZED IMPLEMENTATION - Robust task execution with comprehensive error handling
    /// 
    /// LLM-ASSISTED IMPROVEMENTS IMPLEMENTED:
    /// 1. Comprehensive exception handling with try-catch blocks
    /// 2. Null validation and input sanitization
    /// 3. Structured logging system for error tracking
    /// 4. Simple retry logic for failed tasks (non-concurrent)
    /// 5. Task execution statistics and monitoring
    /// 6. Graceful degradation - failed tasks don't stop processing
    /// 7. Configurable retry policies and error recovery
    /// 8. Detailed error reporting and diagnostics
    /// </summary>
    public class OptimizedTaskExecutor
    {
        private readonly Queue<TaskItem> taskQueue = new Queue<TaskItem>();
        private readonly List<string> executionLog = new List<string>();
        private readonly TaskExecutionStats stats = new TaskExecutionStats();

        // LLM SUGGESTION: Configuration for retry behavior
        public int MaxRetryAttempts { get; set; } = 3;
        public TimeSpan RetryDelay { get; set; } = TimeSpan.FromMilliseconds(100);
        public bool EnableLogging { get; set; } = true;

        /// <summary>
        /// LLM IMPROVEMENT: Enhanced task addition with comprehensive validation
        /// Prevents null/invalid tasks from entering the system
        /// </summary>
        public bool AddTask(string task)
        {
            // LLM SUGGESTION: Input validation before adding to queue
            if (string.IsNullOrWhiteSpace(task))
            {
                LogError("Attempted to add null or empty task - rejected");
                stats.InvalidTasksRejected++;
                return false;
            }

            // LLM SUGGESTION: Sanitize task input
            task = task.Trim();

            var taskItem = new TaskItem
            {
                Id = Guid.NewGuid().ToString()[..8],
                Content = task,
                CreatedAt = DateTime.Now,
                RetryCount = 0
            };

            taskQueue.Enqueue(taskItem);
            LogInfo($"Task added: {taskItem.Id} - '{task}'");
            stats.TasksAdded++;
            return true;
        }

        /// <summary>
        /// LLM IMPROVEMENT: Robust task processing with comprehensive error handling
        /// Ensures all tasks are processed even if some fail
        /// </summary>
        public TaskExecutionResult ProcessTasks()
        {
            LogInfo("Starting task processing...");
            var result = new TaskExecutionResult { StartTime = DateTime.Now };

            // LLM SUGGESTION: Process all tasks with individual error handling
            while (taskQueue.Count > 0)
            {
                var taskItem = taskQueue.Dequeue();
                
                try
                {
                    // LLM IMPROVEMENT: Individual task processing with error recovery
                    bool success = ProcessSingleTask(taskItem);
                    
                    if (success)
                    {
                        result.SuccessfulTasks++;
                        stats.TasksCompleted++;
                    }
                    else
                    {
                        result.FailedTasks++;
                        stats.TasksFailed++;
                    }
                }
                catch (Exception ex)
                {
                    // LLM SUGGESTION: Log errors but continue processing
                    LogError($"Unexpected error processing task {taskItem.Id}: {ex.Message}");
                    result.FailedTasks++;
                    stats.TasksFailed++;
                }
            }

            result.EndTime = DateTime.Now;
            result.TotalProcessingTime = result.EndTime - result.StartTime;
            
            LogInfo($"Task processing completed. Success: {result.SuccessfulTasks}, Failed: {result.FailedTasks}");
            return result;
        }

        /// <summary>
        /// LLM IMPROVEMENT: Individual task processing with retry logic
        /// Implements simple retry mechanism without complex concurrency
        /// </summary>
        private bool ProcessSingleTask(TaskItem taskItem)
        {
            for (int attempt = 1; attempt <= MaxRetryAttempts; attempt++)
            {
                try
                {
                    LogInfo($"Processing task {taskItem.Id} (attempt {attempt}): '{taskItem.Content}'");
                    
                    // LLM SUGGESTION: Validate task before execution
                    if (string.IsNullOrEmpty(taskItem.Content))
                    {
                        LogError($"Task {taskItem.Id} has no content - skipping");
                        return false;
                    }

                    // Execute the actual task
                    ExecuteTask(taskItem);
                    
                    LogInfo($"Task {taskItem.Id} completed successfully");
                    return true;
                }
                catch (TaskExecutionException ex)
                {
                    // LLM IMPROVEMENT: Handle specific task failures with retry logic
                    LogWarning($"Task {taskItem.Id} failed (attempt {attempt}/{MaxRetryAttempts}): {ex.Message}");
                    
                    if (attempt < MaxRetryAttempts && ex.IsRetryable)
                    {
                        LogInfo($"Retrying task {taskItem.Id} in {RetryDelay.TotalMilliseconds}ms...");
                        Thread.Sleep(RetryDelay); // Simple delay for retry
                        continue;
                    }
                    else
                    {
                        LogError($"Task {taskItem.Id} failed permanently after {MaxRetryAttempts} attempts");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // LLM SUGGESTION: Handle unexpected errors gracefully
                    LogError($"Unexpected error in task {taskItem.Id}: {ex.GetType().Name} - {ex.Message}");
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// LLM IMPROVEMENT: Enhanced task execution with specific exception types
        /// Provides better error categorization and recovery options
        /// </summary>
        private void ExecuteTask(TaskItem taskItem)
        {
            var task = taskItem.Content;

            // LLM SUGGESTION: More sophisticated task validation
            if (task.Contains("Fail", StringComparison.OrdinalIgnoreCase))
            {
                throw new TaskExecutionException("Task marked as failure scenario", isRetryable: true);
            }

            if (task.Contains("Critical", StringComparison.OrdinalIgnoreCase))
            {
                throw new TaskExecutionException("Critical task failure - non-retryable", isRetryable: false);
            }

            // LLM IMPROVEMENT: Simulate different types of work
            if (task.Contains("Slow", StringComparison.OrdinalIgnoreCase))
            {
                Thread.Sleep(500); // Simulate slow operation
            }

            // Successful execution
            stats.TotalProcessingTime += TimeSpan.FromMilliseconds(10);
        }

        /// <summary>
        /// LLM IMPROVEMENT: Comprehensive logging system
        /// Provides structured logging for debugging and monitoring
        /// </summary>
        private void LogInfo(string message)
        {
            if (EnableLogging)
            {
                var logEntry = $"[INFO] {DateTime.Now:HH:mm:ss.fff} - {message}";
                executionLog.Add(logEntry);
                Console.WriteLine(logEntry);
            }
        }

        private void LogWarning(string message)
        {
            if (EnableLogging)
            {
                var logEntry = $"[WARN] {DateTime.Now:HH:mm:ss.fff} - {message}";
                executionLog.Add(logEntry);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(logEntry);
                Console.ResetColor();
            }
        }

        private void LogError(string message)
        {
            if (EnableLogging)
            {
                var logEntry = $"[ERROR] {DateTime.Now:HH:mm:ss.fff} - {message}";
                executionLog.Add(logEntry);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(logEntry);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// LLM SUGGESTION: Provide execution statistics for monitoring
        /// </summary>
        public TaskExecutionStats GetStatistics()
        {
            return stats;
        }

        /// <summary>
        /// LLM IMPROVEMENT: Export execution logs for analysis
        /// </summary>
        public List<string> GetExecutionLog()
        {
            return new List<string>(executionLog);
        }

        /// <summary>
        /// LLM SUGGESTION: Clear logs and reset statistics
        /// </summary>
        public void Reset()
        {
            executionLog.Clear();
            stats.Reset();
        }
    }

    /// <summary>
    /// LLM IMPROVEMENT: Structured task representation
    /// Provides better task tracking and metadata
    /// </summary>
    internal class TaskItem
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RetryCount { get; set; }
    }

    /// <summary>
    /// LLM IMPROVEMENT: Custom exception for better error handling
    /// Provides specific task execution error information
    /// </summary>
    public class TaskExecutionException : Exception
    {
        public bool IsRetryable { get; }

        public TaskExecutionException(string message, bool isRetryable = true) : base(message)
        {
            IsRetryable = isRetryable;
        }

        public TaskExecutionException(string message, Exception innerException, bool isRetryable = true) 
            : base(message, innerException)
        {
            IsRetryable = isRetryable;
        }
    }

    /// <summary>
    /// LLM IMPROVEMENT: Execution result tracking
    /// Provides comprehensive execution metrics
    /// </summary>
    public class TaskExecutionResult
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan TotalProcessingTime { get; set; }
        public int SuccessfulTasks { get; set; }
        public int FailedTasks { get; set; }
        public int TotalTasks => SuccessfulTasks + FailedTasks;
        public double SuccessRate => TotalTasks > 0 ? (double)SuccessfulTasks / TotalTasks * 100 : 0;
    }

    /// <summary>
    /// LLM IMPROVEMENT: Comprehensive execution statistics
    /// Provides detailed metrics for system monitoring
    /// </summary>
    public class TaskExecutionStats
    {
        public int TasksAdded { get; set; }
        public int TasksCompleted { get; set; }
        public int TasksFailed { get; set; }
        public int InvalidTasksRejected { get; set; }
        public TimeSpan TotalProcessingTime { get; set; }

        public double SuccessRate => TasksAdded > 0 ? (double)TasksCompleted / TasksAdded * 100 : 0;

        public void Reset()
        {
            TasksAdded = 0;
            TasksCompleted = 0;
            TasksFailed = 0;
            InvalidTasksRejected = 0;
            TotalProcessingTime = TimeSpan.Zero;
        }

        public override string ToString()
        {
            return $"Stats: Added={TasksAdded}, Completed={TasksCompleted}, Failed={TasksFailed}, " +
                   $"Rejected={InvalidTasksRejected}, Success Rate={SuccessRate:F1}%";
        }
    }
}