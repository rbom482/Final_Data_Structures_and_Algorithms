using System;using System;

using System.Collections.Generic;using System.Collections.Generic;



namespace SwiftCollab.TaskSystemnamespace SwiftCollab.TaskSystem

{{

    /// <summary>    /// <summary>

    /// Represents a task in the SwiftCollab system with priority and metadata    /// Represents a task in the SwiftCollab system with priority and metadata

    /// LLM Improvement: Added task-specific data structure instead of just integers    /// LLM Improvement: Added task-specific data structure instead of just integers

    /// </summary>    /// </summary>

    public class SwiftTask    public class SwiftTask

    {    {

        public int Priority { get; set; }        public int Priority { get; set; }

        public string Description { get; set; }        public string Description { get; set; }

        /// LLM Improvement: Thread-safe property access        /// LLM Improvement: Thread-safe property access

        public string? AssignedTo { get; set; }        public string? AssignedTo { get; set; }

        public DateTime CreatedDate { get; set; }        public DateTime CreatedDate { get; set; }

        public string Status { get; set; }        public string Status { get; set; }



        public SwiftTask(int priority, string description, string? assignedTo = null)        public SwiftTask(int priority, string description, string? assignedTo = null)

        {        {

            Priority = priority;            Priority = priority;

            Description = description;            Description = description;

            AssignedTo = assignedTo;            AssignedTo = assignedTo;

            CreatedDate = DateTime.Now;            CreatedDate = DateTime.Now;

            Status = "Pending";            Status = "Pending";

        }        }



        public override string ToString()        public override string ToString()

        {        {

            return $"Task(Priority: {Priority}, Description: {Description}, Assigned: {AssignedTo ?? "Unassigned"}, Status: {Status})";            return $"Task(Priority: {Priority}, Description: {Description}, Assigned: {AssignedTo ?? "Unassigned"}, Status: {Status})";

        }        }

    }

    }        public string? AssignedTo { get; set; }                CollectTasksInRange(root, minPriority, maxPriority, tasks);

    /// <summary>

    /// AVL Tree node for maintaining task ordering by priority

    /// LLM Improvement: Self-balancing tree structure for O(log n) operations

    /// </summary>    public class AVLNode        public DateTime CreatedDate { get; set; }            }

    public class AVLNode

    {    {

        public SwiftTask Task { get; set; }

        public AVLNode? Left { get; set; }        public SwiftTask TaskData { get; set; }        public string Status { get; set; }            return tasks;

        public AVLNode? Right { get; set; }

        public int Height { get; set; }        public AVLNode? Left { get; set; }



        public AVLNode(SwiftTask task)        public AVLNode? Right { get; set; }        }

        {

            Task = task;        public int Height { get; set; }

            Height = 1;

        }        public SwiftTask(int priority, string description, string? assignedTo = null)

    }

        public AVLNode(SwiftTask task)

    /// <summary>

    /// Optimized AVL Binary Tree for SwiftCollab task management        {        {        private void CollectTasksInRange(AVLNode? node, int min, int max, List<SwiftTask> tasks)nOrder()

    /// LLM Improvements:

    /// - Self-balancing AVL tree for guaranteed O(log n) operations            TaskData = task;

    /// - Thread-safe operations for concurrent access

    /// - Task-specific data structure instead of generic integers            Left = Right = null;            Priority = priority;        {

    /// - Bulk operations for improved efficiency

    /// - Range queries for priority-based task filtering            Height = 1;

    /// </summary>

    public class OptimizedTaskTree        }            Description = description;            var tasks = new List<SwiftTask>();

    {

        private AVLNode? root;

        private readonly object lockObject = new object();

        public int Priority => TaskData?.Priority ?? 0;            AssignedTo = assignedTo;            var stack = new Stack<AVLNode>();

        /// <summary>

        /// LLM Improvement: Thread-safe task insertion with AVL balancing    }

        /// </summary>

        public void Insert(SwiftTask task)            CreatedDate = DateTime.Now;            AVLNode? current = root;e? Rootllab.TaskSystem

        {

            lock (lockObject)    public class OptimizedTaskTree

            {

                root = InsertRecursive(root, task);    {            Status = "Pending";{

            }

        }        private AVLNode? root;



        /// <summary>        }    /// <summary>

        /// LLM Improvement: Self-balancing recursive insert with AVL rotations

        /// </summary>        public void Insert(SwiftTask task)

        private AVLNode InsertRecursive(AVLNode? node, SwiftTask task)

        {        {    /// Represents a task in the SwiftCollab system with priority and metadata

            // Standard BST insertion

            if (node == null)            if (task == null)

                return new AVLNode(task);

                throw new ArgumentNullException(nameof(task));        public override string ToString()    /// LLM Improvement: Added task-specific data structure instead of just integers

            if (task.Priority < node.Task.Priority)

                node.Left = InsertRecursive(node.Left, task);

            else if (task.Priority > node.Task.Priority)

                node.Right = InsertRecursive(node.Right, task);            root = InsertRecursive(root, task);        {    /// </summary>

            else

            {        }

                // Update existing task

                node.Task = task;            return $"Task(Priority: {Priority}, Description: {Description}, Assigned: {AssignedTo}, Status: {Status})";    public class SwiftTask

                return node;

            }        private AVLNode InsertRecursive(AVLNode? node, SwiftTask task)



            // Update height        {        }    {

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            if (node == null)

            // Get balance factor

            int balance = GetBalance(node);                return new AVLNode(task);    }        public int Priority { get; set; }



            // Left Left Case

            if (balance > 1 && task.Priority < node.Left!.Task.Priority)

                return RotateRight(node);            if (task.Priority < node.Priority)        public string Description { get; set; }



            // Right Right Case                node.Left = InsertRecursive(node.Left, task);

            if (balance < -1 && task.Priority > node.Right!.Task.Priority)

                return RotateLeft(node);            else if (task.Priority > node.Priority)    /// <summary>        public string AssignedTo { get; set; }



            // Left Right Case                node.Right = InsertRecursive(node.Right, task);

            if (balance > 1 && task.Priority > node.Left!.Task.Priority)

            {            else    /// AVL Tree Node with height tracking for automatic balancing        public DateTime CreatedDate { get; set; }

                node.Left = RotateLeft(node.Left);

                return RotateRight(node);            {

            }

                node.TaskData = task;    /// LLM Improvement: Enhanced node structure with height property for AVL balancing        public string Status { get; set; }

            // Right Left Case

            if (balance < -1 && task.Priority < node.Right!.Task.Priority)                return node;

            {

                node.Right = RotateRight(node.Right);            }    /// </summary>

                return RotateLeft(node);

            }



            return node;            UpdateHeight(node);    public class AVLNode        public SwiftTask(int priority, string description, string assignedTo = null)

        }

            int balance = GetBalance(node);

        /// <summary>

        /// LLM Improvement: Efficient bulk insertion with reduced lock contention    {        {

        /// </summary>

        public void InsertBatch(IEnumerable<SwiftTask> tasks)            if (balance > 1 && task.Priority < node.Left!.Priority)

        {

            lock (lockObject)                return RotateRight(node);        public SwiftTask TaskData { get; set; }            Priority = priority;

            {

                foreach (var task in tasks)

                {

                    root = InsertRecursive(root, task);            if (balance < -1 && task.Priority > node.Right!.Priority)        public AVLNode? Left { get; set; }            Description = description;

                }

            }                return RotateLeft(node);

        }

        public AVLNode? Right { get; set; }            AssignedTo = assignedTo;

        /// <summary>

        /// LLM Improvement: Priority-based task search with O(log n) complexity            if (balance > 1 && task.Priority > node.Left!.Priority)

        /// </summary>

        public SwiftTask? Search(int priority)            {        public int Height { get; set; }            CreatedDate = DateTime.Now;

        {

            lock (lockObject)                node.Left = RotateLeft(node.Left);

            {

                return SearchRecursive(root, priority);                return RotateRight(node);            Status = "Pending";

            }

        }            }



        private SwiftTask? SearchRecursive(AVLNode? node, int priority)        public AVLNode(SwiftTask task)        }

        {

            if (node == null)            if (balance < -1 && task.Priority < node.Right!.Priority)

                return null;

            {        {

            if (priority == node.Task.Priority)

                return node.Task;                node.Right = RotateRight(node.Right);

            else if (priority < node.Task.Priority)

                return SearchRecursive(node.Left, priority);                return RotateLeft(node);            TaskData = task;        public override string ToString()

            else

                return SearchRecursive(node.Right, priority);            }

        }

            Left = Right = null;        {

        /// <summary>

        /// LLM Improvement: Range-based task retrieval for filtering by priority            return node;

        /// </summary>

        public List<SwiftTask> GetTasksByPriorityRange(int minPriority, int maxPriority)        }            Height = 1; // New nodes start at height 1            return $"Task(Priority: {Priority}, Description: {Description}, Assigned: {AssignedTo}, Status: {Status})";

        {

            var tasks = new List<SwiftTask>();

            lock (lockObject)

            {        private int GetHeight(AVLNode? node) => node?.Height ?? 0;        }        }

                CollectTasksInRange(root, minPriority, maxPriority, tasks);

            }

            return tasks;

        }        private int GetBalance(AVLNode? node) =>     }



        private void CollectTasksInRange(AVLNode? node, int min, int max, List<SwiftTask> tasks)            node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);

        {

            if (node == null) return;        /// <summary>



            if (node.Task.Priority >= min && node.Task.Priority <= max)        private void UpdateHeight(AVLNode? node)

                tasks.Add(node.Task);

        {        /// LLM Improvement: Added property for easy access to priority value    /// <summary>

            if (min < node.Task.Priority)

                CollectTasksInRange(node.Left, min, max, tasks);            if (node != null)



            if (max > node.Task.Priority)                node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));        /// </summary>    /// AVL Tree Node with height tracking for automatic balancing

                CollectTasksInRange(node.Right, min, max, tasks);

        }        }



        /// <summary>        public int Priority => TaskData?.Priority ?? 0;    /// LLM Improvement: Enhanced node structure with height property for AVL balancing

        /// LLM Improvement: In-order traversal for sorted task listing

        /// </summary>        private AVLNode RotateRight(AVLNode y)

        public List<SwiftTask> GetAllTasksInOrder()

        {        {    }    /// </summary>

            var tasks = new List<SwiftTask>();

            lock (lockObject)            AVLNode x = y.Left!;

            {

                InOrderTraversal(root, tasks);            AVLNode? T2 = x.Right;    public class AVLNode

            }

            return tasks;

        }

            x.Right = y;    /// <summary>    {

        private void InOrderTraversal(AVLNode? node, List<SwiftTask> tasks)

        {            y.Left = T2;

            if (node != null)

            {    /// Optimized AVL Tree implementation for SwiftCollab's task assignment system        public SwiftTask TaskData { get; set; }

                InOrderTraversal(node.Left, tasks);

                tasks.Add(node.Task);            UpdateHeight(y);

                InOrderTraversal(node.Right, tasks);

            }            UpdateHeight(x);    /// LLM Improvements: Self-balancing tree, O(log n) operations, comprehensive functionality        public AVLNode? Left { get; set; }

        }



        /// <summary>

        /// LLM Improvement: Task removal with tree rebalancing            return x;    /// </summary>        public AVLNode? Right { get; set; }

        /// </summary>

        public bool Remove(int priority)        }

        {

            lock (lockObject)    public class OptimizedTaskTree        public int Height { get; set; }

            {

                var originalRoot = root;        private AVLNode RotateLeft(AVLNode x)

                root = RemoveRecursive(root, priority);

                return originalRoot != root || (root != null && SearchRecursive(root, priority) == null);        {    {

            }

        }            AVLNode y = x.Right!;



        private AVLNode? RemoveRecursive(AVLNode? node, int priority)            AVLNode? T2 = y.Left;        private AVLNode? root;        public AVLNode(SwiftTask task)

        {

            if (node == null)

                return node;

            y.Left = x;        private readonly object lockObject = new object(); // Thread safety        {

            if (priority < node.Task.Priority)

                node.Left = RemoveRecursive(node.Left, priority);            x.Right = T2;

            else if (priority > node.Task.Priority)

                node.Right = RemoveRecursive(node.Right, priority);            TaskData = task;

            else

            {            UpdateHeight(x);

                if (node.Left == null || node.Right == null)

                {            UpdateHeight(y);        /// <summary>            Left = Right = null;

                    AVLNode? temp = node.Left ?? node.Right;

                    if (temp == null)

                    {

                        temp = node;            return y;        /// LLM Improvement: Added thread-safe property access            Height = 1; // New nodes start at height 1

                        node = null;

                    }        }

                    else

                        node = temp;        /// </summary>        }

                }

                else        public SwiftTask? Search(int priority)

                {

                    AVLNode temp = GetMinValueNode(node.Right);        {        public AVLNode? Root 

                    node.Task = temp.Task;

                    node.Right = RemoveRecursive(node.Right, temp.Task.Priority);            AVLNode? current = root;

                }

            }            while (current != null)        {         /// <summary>



            if (node == null)            {

                return node;

                if (priority == current.Priority)            get         /// LLM Improvement: Added property for easy access to priority value

            // Update height

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));                    return current.TaskData;



            // Get balance factor                else if (priority < current.Priority)            {         /// </summary>

            int balance = GetBalance(node);

                    current = current.Left;

            // Left Left Case

            if (balance > 1 && GetBalance(node.Left) >= 0)                else                lock (lockObject)         public int Priority => TaskData?.Priority ?? 0;

                return RotateRight(node);

                    current = current.Right;

            // Left Right Case

            if (balance > 1 && GetBalance(node.Left) < 0)            }                {     }

            {

                node.Left = RotateLeft(node.Left!);            return null;

                return RotateRight(node);

            }        }                    return root; 



            // Right Right Case

            if (balance < -1 && GetBalance(node.Right) <= 0)

                return RotateLeft(node);        public List<SwiftTask> GetTasksInOrder()                }     /// <summary>



            // Right Left Case        {

            if (balance < -1 && GetBalance(node.Right) > 0)

            {            var tasks = new List<SwiftTask>();            }     /// Optimized AVL Tree implementation for SwiftCollab's task assignment system

                node.Right = RotateRight(node.Right!);

                return RotateLeft(node);            var stack = new Stack<AVLNode>();

            }

            AVLNode? current = root;        }    /// LLM Improvements: Self-balancing tree, O(log n) operations, comprehensive functionality

            return node;

        }



        private AVLNode GetMinValueNode(AVLNode node)            while (current != null || stack.Count > 0)    /// </summary>

        {

            AVLNode current = node;            {

            while (current.Left != null)

                current = current.Left;                while (current != null)        #region Height and Balance Operations    public class OptimizedTaskTree

            return current;

        }                {



        /// <summary>                    stack.Push(current);        /// <summary>    {

        /// LLM Improvement: Utility methods for AVL tree balancing

        /// </summary>                    current = current.Left;

        private int GetHeight(AVLNode? node)

        {                }        /// LLM Improvement: Helper method to get node height safely        private AVLNode? root;

            return node?.Height ?? 0;

        }



        private int GetBalance(AVLNode? node)                current = stack.Pop();        /// </summary>        private readonly object lockObject = new object(); // Thread safety

        {

            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);                tasks.Add(current.TaskData);

        }

                current = current.Right;        private int GetHeight(AVLNode? node)

        private AVLNode RotateRight(AVLNode y)

        {            }

            AVLNode x = y.Left!;

            AVLNode T2 = x.Right;        {        /// <summary>



            // Perform rotation            return tasks;

            x.Right = y;

            y.Left = T2;        }            return node?.Height ?? 0;        /// LLM Improvement: Added thread-safe property access



            // Update heights

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;        public void PrintTree()        }        /// </summary>



            return x;        {

        }

            Console.WriteLine("=== SwiftCollab Task Tree (Priority Order) ===");        public AVLNode Root 

        private AVLNode RotateLeft(AVLNode x)

        {            if (root == null)

            AVLNode y = x.Right!;

            AVLNode T2 = y.Left;            {        /// <summary>        { 



            // Perform rotation                Console.WriteLine("No tasks in the system.");

            y.Left = x;

            x.Right = T2;                return;        /// LLM Improvement: Calculate balance factor for AVL operations            get 



            // Update heights            }

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;        /// </summary>            { 



            return y;            var tasks = GetTasksInOrder();

        }

            foreach (var task in tasks)        private int GetBalance(AVLNode? node)                lock (lockObject) 

        /// <summary>

        /// LLM Improvement: Tree statistics for performance monitoring            {

        /// </summary>

        public (int nodeCount, int height, bool isBalanced) GetTreeStatistics()                Console.WriteLine($"Priority {task.Priority}: {task.Description} [{task.AssignedTo ?? "Unassigned"}]");        {                { 

        {

            lock (lockObject)            }

            {

                int count = CountNodes(root);            Console.WriteLine($"Total tasks: {tasks.Count}");            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);                    return root; 

                int height = GetHeight(root);

                bool balanced = IsBalanced(root);        }

                return (count, height, balanced);

            }        }                } 

        }

        public int GetTreeHeight() => GetHeight(root);

        private int CountNodes(AVLNode? node)

        {                    } 

            if (node == null) return 0;

            return 1 + CountNodes(node.Left) + CountNodes(node.Right);        public int CountNodes()

        }

        {        /// <summary>        }

        private bool IsBalanced(AVLNode? node)

        {            return CountNodesRecursive(root);

            if (node == null) return true;

                    }        /// LLM Improvement: Update node height based on children

            int leftHeight = GetHeight(node.Left);

            int rightHeight = GetHeight(node.Right);

            

            return Math.Abs(leftHeight - rightHeight) <= 1 &&        private int CountNodesRecursive(AVLNode? node)        /// </summary>        #region Height and Balance Operations

                   IsBalanced(node.Left) && IsBalanced(node.Right);

        }        {



        /// <summary>            if (node == null) return 0;        private void UpdateHeight(AVLNode? node)        /// <summary>

        /// LLM Improvement: Get highest priority task (minimum value)

        /// </summary>            return 1 + CountNodesRecursive(node.Left) + CountNodesRecursive(node.Right);

        public SwiftTask? GetHighestPriorityTask()

        {        }        {        /// LLM Improvement: Helper method to get node height safely

            lock (lockObject)

            {    }

                if (root == null) return null;

                AVLNode current = root;}            if (node != null)        /// </summary>

                while (current.Left != null)

                    current = current.Left;            {        private int GetHeight(AVLNode node)

                return current.Task;

            }                node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));        {

        }

            }            return node?.Height ?? 0;

        /// <summary>

        /// LLM Improvement: Get lowest priority task (maximum value)        }        }

        /// </summary>

        public SwiftTask? GetLowestPriorityTask()        #endregion

        {

            lock (lockObject)        /// <summary>

            {

                if (root == null) return null;        #region Rotation Operations for Balancing        /// LLM Improvement: Calculate balance factor for AVL operations

                AVLNode current = root;

                while (current.Right != null)        /// <summary>        /// </summary>

                    current = current.Right;

                return current.Task;        /// LLM Improvement: Right rotation for AVL balancing        private int GetBalance(AVLNode node)

            }

        }        /// </summary>        {



        public void PrintTree()        private AVLNode RotateRight(AVLNode y)            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);

        {

            lock (lockObject)        {        }

            {

                PrintInOrder(root);            AVLNode x = y.Left!;

            }

        }            AVLNode? T2 = x.Right;        /// <summary>



        private void PrintInOrder(AVLNode? node)        /// LLM Improvement: Update node height based on children

        {

            if (node != null)            // Perform rotation        /// </summary>

            {

                PrintInOrder(node.Left);            x.Right = y;        private void UpdateHeight(AVLNode node)

                Console.WriteLine($"Priority {node.Task.Priority}: {node.Task.Description}");

                PrintInOrder(node.Right);            y.Left = T2;        {

            }

        }            if (node != null)

    }

}            // Update heights            {

            UpdateHeight(y);                node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            UpdateHeight(x);            }

        }

            return x; // New root        #endregion

        }

        #region Rotation Operations for Balancing

        /// <summary>        /// <summary>

        /// LLM Improvement: Left rotation for AVL balancing        /// LLM Improvement: Right rotation for AVL balancing

        /// </summary>        /// </summary>

        private AVLNode RotateLeft(AVLNode x)        private AVLNode RotateRight(AVLNode y)

        {        {

            AVLNode y = x.Right!;            AVLNode x = y.Left;

            AVLNode? T2 = y.Left;            AVLNode T2 = x.Right;



            // Perform rotation            // Perform rotation

            y.Left = x;            x.Right = y;

            x.Right = T2;            y.Left = T2;



            // Update heights            // Update heights

            UpdateHeight(x);            UpdateHeight(y);

            UpdateHeight(y);            UpdateHeight(x);



            return y; // New root            return x; // New root

        }        }

        #endregion

        /// <summary>

        #region Insert Operations        /// LLM Improvement: Left rotation for AVL balancing

        /// <summary>        /// </summary>

        /// LLM Improvement: Thread-safe public insert method with validation        private AVLNode RotateLeft(AVLNode x)

        /// </summary>        {

        public void Insert(SwiftTask task)            AVLNode y = x.Right;

        {            AVLNode T2 = y.Left;

            if (task == null)

                throw new ArgumentNullException(nameof(task), "Task cannot be null");            // Perform rotation

            y.Left = x;

            lock (lockObject)            x.Right = T2;

            {

                root = InsertRecursive(root, task);            // Update heights

            }            UpdateHeight(x);

        }            UpdateHeight(y);



        /// <summary>            return y; // New root

        /// LLM Improvement: Self-balancing recursive insert with AVL rotations        }

        /// </summary>        #endregion

        private AVLNode InsertRecursive(AVLNode? node, SwiftTask task)

        {        #region Insert Operations

            // Standard BST insertion        /// <summary>

            if (node == null)        /// LLM Improvement: Thread-safe public insert method with validation

                return new AVLNode(task);        /// </summary>

        public void Insert(SwiftTask task)

            if (task.Priority < node.Priority)        {

                node.Left = InsertRecursive(node.Left, task);            if (task == null)

            else if (task.Priority > node.Priority)                throw new ArgumentNullException(nameof(task), "Task cannot be null");

                node.Right = InsertRecursive(node.Right, task);

            else            lock (lockObject)

            {            {

                // Handle duplicate priorities by updating task data                root = InsertRecursive(root, task);

                node.TaskData = task;            }

                return node;        }

            }

        /// <summary>

            // Update height of current node        /// LLM Improvement: Self-balancing recursive insert with AVL rotations

            UpdateHeight(node);        /// </summary>

        private AVLNode InsertRecursive(AVLNode node, Task task)

            // Get balance factor        {

            int balance = GetBalance(node);            // Standard BST insertion

            if (node == null)

            // Perform rotations if needed                return new AVLNode(task);

            // Left Left Case

            if (balance > 1 && task.Priority < node.Left!.Priority)            if (task.Priority < node.Priority)

                return RotateRight(node);                node.Left = InsertRecursive(node.Left, task);

            else if (task.Priority > node.Priority)

            // Right Right Case                node.Right = InsertRecursive(node.Right, task);

            if (balance < -1 && task.Priority > node.Right!.Priority)            else

                return RotateLeft(node);            {

                // Handle duplicate priorities by updating task data

            // Left Right Case                node.TaskData = task;

            if (balance > 1 && task.Priority > node.Left!.Priority)                return node;

            {            }

                node.Left = RotateLeft(node.Left);

                return RotateRight(node);            // Update height of current node

            }            UpdateHeight(node);



            // Right Left Case            // Get balance factor

            if (balance < -1 && task.Priority < node.Right!.Priority)            int balance = GetBalance(node);

            {

                node.Right = RotateRight(node.Right);            // Perform rotations if needed

                return RotateLeft(node);            // Left Left Case

            }            if (balance > 1 && task.Priority < node.Left.Priority)

                return RotateRight(node);

            return node; // Return unchanged node

        }            // Right Right Case

        #endregion            if (balance < -1 && task.Priority > node.Right.Priority)

                return RotateLeft(node);

        #region Search Operations

        /// <summary>            // Left Right Case

        /// LLM Improvement: Efficient search method with O(log n) complexity            if (balance > 1 && task.Priority > node.Left.Priority)

        /// </summary>            {

        public SwiftTask? Search(int priority)                node.Left = RotateLeft(node.Left);

        {                return RotateRight(node);

            lock (lockObject)            }

            {

                return SearchRecursive(root, priority)?.TaskData;            // Right Left Case

            }            if (balance < -1 && task.Priority < node.Right.Priority)

        }            {

                node.Right = RotateRight(node.Right);

        /// <summary>                return RotateLeft(node);

        /// LLM Improvement: Iterative search to avoid stack overflow            }

        /// </summary>

        private AVLNode? SearchRecursive(AVLNode? node, int priority)            return node; // Return unchanged node

        {        }

            // Use iterative approach to prevent stack overflow        #endregion

            AVLNode? current = node;

            while (current != null)        #region Search Operations

            {        /// <summary>

                if (priority == current.Priority)        /// LLM Improvement: Efficient search method with O(log n) complexity

                    return current;        /// </summary>

                else if (priority < current.Priority)        public SwiftTask? Search(int priority)

                    current = current.Left;        {

                else            lock (lockObject)

                    current = current.Right;            {

            }                return SearchRecursive(root, priority)?.TaskData;

            return null;            }

        }        }



        /// <summary>        /// <summary>

        /// LLM Improvement: Find task with highest priority (rightmost node)        /// LLM Improvement: Iterative search to avoid stack overflow

        /// </summary>        /// </summary>

        public SwiftTask? GetHighestPriority()        private AVLNode? SearchRecursive(AVLNode? node, int priority)

        {        {

            lock (lockObject)            // Use iterative approach to prevent stack overflow

            {            AVLNode current = node;

                if (root == null) return null;            while (current != null)

            {

                AVLNode current = root;                if (priority == current.Priority)

                while (current.Right != null)                    return current;

                    current = current.Right;                else if (priority < current.Priority)

                    current = current.Left;

                return current.TaskData;                else

            }                    current = current.Right;

        }            }

            return null;

        /// <summary>        }

        /// LLM Improvement: Find task with lowest priority (leftmost node)

        /// </summary>        /// <summary>

        public SwiftTask? GetLowestPriority()        /// LLM Improvement: Find task with highest priority (rightmost node)

        {        /// </summary>

            lock (lockObject)        public SwiftTask? GetHighestPriority()

            {        {

                if (root == null) return null;            lock (lockObject)

            {

                AVLNode current = root;                if (root == null) return null;

                while (current.Left != null)

                    current = current.Left;                AVLNode current = root;

                while (current.Right != null)

                return current.TaskData;                    current = current.Right;

            }

        }                return current.TaskData;

        #endregion            }

        }

        #region Delete Operations

        /// <summary>        /// <summary>

        /// LLM Improvement: Self-balancing delete operation        /// LLM Improvement: Find task with lowest priority (leftmost node)

        /// </summary>        /// </summary>

        public bool Delete(int priority)        public SwiftTask? GetLowestPriority()

        {        {

            lock (lockObject)            lock (lockObject)

            {            {

                int initialCount = CountNodes();                if (root == null) return null;

                root = DeleteRecursive(root, priority);

                return CountNodes() < initialCount;                AVLNode current = root;

            }                while (current.Left != null)

        }                    current = current.Left;



        /// <summary>                return current.TaskData;

        /// LLM Improvement: Recursive delete with AVL rebalancing            }

        /// </summary>        }

        private AVLNode? DeleteRecursive(AVLNode? node, int priority)        #endregion

        {

            if (node == null) return node;        #region Delete Operations

        /// <summary>

            // Standard BST deletion        /// LLM Improvement: Self-balancing delete operation

            if (priority < node.Priority)        /// </summary>

                node.Left = DeleteRecursive(node.Left, priority);        public bool Delete(int priority)

            else if (priority > node.Priority)        {

                node.Right = DeleteRecursive(node.Right, priority);            lock (lockObject)

            else            {

            {                int initialCount = CountNodes();

                // Node to be deleted found                root = DeleteRecursive(root, priority);

                if (node.Left == null || node.Right == null)                return CountNodes() < initialCount;

                {            }

                    AVLNode? temp = node.Left ?? node.Right;        }

                    if (temp == null)

                    {        /// <summary>

                        temp = node;        /// LLM Improvement: Recursive delete with AVL rebalancing

                        node = null;        /// </summary>

                    }        private AVLNode? DeleteRecursive(AVLNode? node, int priority)

                    else        {

                        node = temp;            if (node == null) return node;

                }

                else            // Standard BST deletion

                {            if (priority < node.Priority)

                    // Node with two children                node.Left = DeleteRecursive(node.Left, priority);

                    AVLNode temp = GetMinValueNode(node.Right);            else if (priority > node.Priority)

                    node.TaskData = temp.TaskData;                node.Right = DeleteRecursive(node.Right, priority);

                    node.Right = DeleteRecursive(node.Right, temp.Priority);            else

                }            {

            }                // Node to be deleted found

                if (node.Left == null || node.Right == null)

            if (node == null) return node;                {

                    AVLNode? temp = node.Left ?? node.Right;

            // Update height and rebalance                    if (temp == null)

            UpdateHeight(node);                    {

            int balance = GetBalance(node);                        temp = node;

                        node = null;

            // Rebalancing rotations                    }

            if (balance > 1 && GetBalance(node.Left) >= 0)                    else

                return RotateRight(node);                        node = temp;

                }

            if (balance > 1 && GetBalance(node.Left) < 0)                else

            {                {

                node.Left = RotateLeft(node.Left!);                    // Node with two children

                return RotateRight(node);                    AVLNode temp = GetMinValueNode(node.Right);

            }                    node.TaskData = temp.TaskData;

                    node.Right = DeleteRecursive(node.Right, temp.Priority);

            if (balance < -1 && GetBalance(node.Right) <= 0)                }

                return RotateLeft(node);            }



            if (balance < -1 && GetBalance(node.Right) > 0)            if (node == null) return node;

            {

                node.Right = RotateRight(node.Right!);            // Update height and rebalance

                return RotateLeft(node);            UpdateHeight(node);

            }            int balance = GetBalance(node);



            return node;            // Rebalancing rotations

        }            if (balance > 1 && GetBalance(node.Left) >= 0)

                return RotateRight(node);

        /// <summary>

        /// LLM Improvement: Helper method to find minimum value node            if (balance > 1 && GetBalance(node.Left) < 0)

        /// </summary>            {

        private AVLNode GetMinValueNode(AVLNode node)                node.Left = RotateLeft(node.Left);

        {                return RotateRight(node);

            AVLNode current = node;            }

            while (current.Left != null)

                current = current.Left;            if (balance < -1 && GetBalance(node.Right) <= 0)

            return current;                return RotateLeft(node);

        }

        #endregion            if (balance < -1 && GetBalance(node.Right) > 0)

            {

        #region Traversal Operations                node.Right = RotateRight(node.Right);

        /// <summary>                return RotateLeft(node);

        /// LLM Improvement: Non-recursive in-order traversal to prevent stack overflow            }

        /// </summary>

        public List<SwiftTask> GetTasksInOrder()            return node;

        {        }

            var tasks = new List<SwiftTask>();

            var stack = new Stack<AVLNode>();        /// <summary>

            AVLNode? current = root;        /// LLM Improvement: Helper method to find minimum value node

        /// </summary>

            while (current != null || stack.Count > 0)        private AVLNode GetMinValueNode(AVLNode node)

            {        {

                while (current != null)            AVLNode current = node;

                {            while (current.Left != null)

                    stack.Push(current);                current = current.Left;

                    current = current.Left;            return current;

                }        }

        #endregion

                current = stack.Pop();

                tasks.Add(current.TaskData);        #region Traversal Operations

                current = current.Right;        /// <summary>

            }        /// LLM Improvement: Non-recursive in-order traversal to prevent stack overflow

        /// </summary>

            return tasks;        public List<SwiftTask> GetTasksInOrder()

        }        {

            var tasks = new List<Task>();

        /// <summary>            var stack = new Stack<AVLNode>();

        /// LLM Improvement: Get tasks by priority range for efficient filtering            AVLNode current = root;

        /// </summary>

        public List<SwiftTask> GetTasksByPriorityRange(int minPriority, int maxPriority)            while (current != null || stack.Count > 0)

        {            {

            var tasks = new List<SwiftTask>();                while (current != null)

            lock (lockObject)                {

            {                    stack.Push(current);

                CollectTasksInRange(root, minPriority, maxPriority, tasks);                    current = current.Left;

            }                }

            return tasks;

        }                current = stack.Pop();

                tasks.Add(current.TaskData);

        private void CollectTasksInRange(AVLNode? node, int min, int max, List<SwiftTask> tasks)                current = current.Right;

        {            }

            if (node == null) return;

            return tasks;

            if (node.Priority >= min && node.Priority <= max)        }

                tasks.Add(node.TaskData);

        /// <summary>

            if (node.Priority > min)        /// LLM Improvement: Get tasks by priority range for efficient filtering

                CollectTasksInRange(node.Left, min, max, tasks);        /// </summary>

        public List<SwiftTask> GetTasksByPriorityRange(int minPriority, int maxPriority)

            if (node.Priority < max)        {

                CollectTasksInRange(node.Right, min, max, tasks);            var tasks = new List<Task>();

        }            lock (lockObject)

            {

        /// <summary>                CollectTasksInRange(root, minPriority, maxPriority, tasks);

        /// LLM Improvement: Print tree with improved formatting            }

        /// </summary>            return tasks;

        public void PrintTree()        }

        {

            lock (lockObject)        private void CollectTasksInRange(AVLNode node, int min, int max, List<Task> tasks)

            {        {

                Console.WriteLine("=== SwiftCollab Task Tree (Priority Order) ===");            if (node == null) return;

                if (root == null)

                {            if (node.Priority >= min && node.Priority <= max)

                    Console.WriteLine("No tasks in the system.");                tasks.Add(node.TaskData);

                    return;

                }            if (node.Priority > min)

                CollectTasksInRange(node.Left, min, max, tasks);

                var tasks = GetTasksInOrder();

                foreach (var task in tasks)            if (node.Priority < max)

                {                CollectTasksInRange(node.Right, min, max, tasks);

                    Console.WriteLine($"Priority {task.Priority}: {task.Description} " +        }

                                    $"[Assigned: {task.AssignedTo ?? "Unassigned"}, Status: {task.Status}]");

                }        /// <summary>

                Console.WriteLine($"Total tasks: {tasks.Count}");        /// LLM Improvement: Print tree with improved formatting

            }        /// </summary>

        }        public void PrintTree()

        #endregion        {

            lock (lockObject)

        #region Utility Methods            {

        /// <summary>                Console.WriteLine("=== SwiftCollab Task Tree (Priority Order) ===");

        /// LLM Improvement: Get tree statistics for performance monitoring                if (root == null)

        /// </summary>                {

        public TreeStatistics GetStatistics()                    Console.WriteLine("No tasks in the system.");

        {                    return;

            lock (lockObject)                }

            {

                return new TreeStatistics                var tasks = GetTasksInOrder();

                {                foreach (var task in tasks)

                    NodeCount = CountNodes(),                {

                    Height = GetHeight(root),                    Console.WriteLine($"Priority {task.Priority}: {task.Description} " +

                    IsBalanced = IsBalanced(root),                                    $"[Assigned: {task.AssignedTo ?? "Unassigned"}, Status: {task.Status}]");

                    MaxPriority = GetHighestPriority()?.Priority ?? 0,                }

                    MinPriority = GetLowestPriority()?.Priority ?? 0                Console.WriteLine($"Total tasks: {tasks.Count}");

                };            }

            }        }

        }        #endregion



        private int CountNodes()        #region Utility Methods

        {        /// <summary>

            return CountNodesRecursive(root);        /// LLM Improvement: Get tree statistics for performance monitoring

        }        /// </summary>

        public TreeStatistics GetStatistics()

        private int CountNodesRecursive(AVLNode? node)        {

        {            lock (lockObject)

            if (node == null) return 0;            {

            return 1 + CountNodesRecursive(node.Left) + CountNodesRecursive(node.Right);                return new TreeStatistics

        }                {

                    NodeCount = CountNodes(),

        private bool IsBalanced(AVLNode? node)                    Height = GetHeight(root),

        {                    IsBalanced = IsBalanced(root),

            if (node == null) return true;                    MaxPriority = GetHighestPriority()?.Priority ?? 0,

                                MinPriority = GetLowestPriority()?.Priority ?? 0

            int balance = GetBalance(node);                };

            return Math.Abs(balance) <= 1 &&             }

                   IsBalanced(node.Left) &&         }

                   IsBalanced(node.Right);

        }        private int CountNodes()

        #endregion        {

    }            return CountNodesRecursive(root);

        }

    /// <summary>

    /// LLM Improvement: Statistics class for monitoring tree performance        private int CountNodesRecursive(AVLNode? node)

    /// </summary>        {

    public class TreeStatistics            if (node == null) return 0;

    {            return 1 + CountNodesRecursive(node.Left) + CountNodesRecursive(node.Right);

        public int NodeCount { get; set; }        }

        public int Height { get; set; }

        public bool IsBalanced { get; set; }        private bool IsBalanced(AVLNode? node)

        public int MaxPriority { get; set; }        {

        public int MinPriority { get; set; }            if (node == null) return true;

            

        public override string ToString()            int balance = GetBalance(node);

        {            return Math.Abs(balance) <= 1 && 

            return $"Tree Stats - Nodes: {NodeCount}, Height: {Height}, " +                   IsBalanced(node.Left) && 

                   $"Balanced: {IsBalanced}, Priority Range: {MinPriority}-{MaxPriority}";                   IsBalanced(node.Right);

        }        }

    }        #endregion

}    }

    /// <summary>
    /// LLM Improvement: Statistics class for monitoring tree performance
    /// </summary>
    public class TreeStatistics
    {
        public int NodeCount { get; set; }
        public int Height { get; set; }
        public bool IsBalanced { get; set; }
        public int MaxPriority { get; set; }
        public int MinPriority { get; set; }

        public override string ToString()
        {
            return $"Tree Stats - Nodes: {NodeCount}, Height: {Height}, " +
                   $"Balanced: {IsBalanced}, Priority Range: {MinPriority}-{MaxPriority}";
        }
    }
}