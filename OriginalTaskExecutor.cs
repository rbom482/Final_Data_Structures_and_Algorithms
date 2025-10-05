using System;
using System.Collections.Generic;

namespace SwiftCollab.TaskExecution
{
    /// <summary>
    /// ORIGINAL IMPLEMENTATION - Contains bugs and inefficiencies
    /// This is the buggy code provided in Assignment 4 that needs LLM-assisted debugging
    /// 
    /// IDENTIFIED PROBLEMS (through LLM analysis):
    /// 1. No exception handling - crashes terminate entire program
    /// 2. Null tasks cause unhandled exceptions
    /// 3. Failed tasks stop all subsequent processing
    /// 4. No logging or error tracking capabilities
    /// 5. No retry mechanism for recoverable failures
    /// 6. Poor separation of concerns (processing and execution mixed)
    /// 7. No input validation before adding tasks
    /// 8. Exception messages are not descriptive
    /// </summary>
    public class OriginalTaskExecutor
    {
        private Queue<string> taskQueue = new Queue<string>();

        public void AddTask(string task)
        {
            // BUG: No null validation - allows null tasks in queue
            taskQueue.Enqueue(task);
        }

        public void ProcessTasks()
        {
            // BUG: No exception handling - any task failure crashes entire system
            while (taskQueue.Count > 0)
            {
                string task = taskQueue.Dequeue();
                Console.WriteLine($"Processing task: {task}");
                ExecuteTask(task); // This can throw unhandled exceptions
            }
        }

        private void ExecuteTask(string task)
        {
            // BUG: Throws exceptions instead of handling them gracefully
            if (task == null)
            {
                throw new Exception("Task is null");
            }
            if (task.Contains("Fail"))
            {
                throw new Exception("Task execution failed");
            }
            Console.WriteLine($"Task {task} completed successfully.");
        }

        /// <summary>
        /// Demonstrates the original buggy behavior
        /// This will crash on the null task and never process "Task 2"
        /// </summary>
        public static void DemonstrateBuggyBehavior()
        {
            Console.WriteLine("=== ORIGINAL BUGGY IMPLEMENTATION ===");
            Console.WriteLine("This will crash and not process all tasks:");
            
            try
            {
                OriginalTaskExecutor executor = new OriginalTaskExecutor();
                executor.AddTask("Task 1");
                executor.AddTask(null); // This will cause a crash
                executor.AddTask("Fail Task"); // This will also cause a crash
                executor.AddTask("Task 2");
                executor.ProcessTasks();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SYSTEM CRASHED: {ex.Message}");
                Console.WriteLine("Notice how 'Task 2' was never processed due to the crash.");
            }
            
            Console.WriteLine();
        }
    }
}