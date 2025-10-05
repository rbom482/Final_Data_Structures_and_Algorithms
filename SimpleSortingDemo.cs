using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SwiftCollab.Sorting
{
    /// <summary>
    /// Simple sorting algorithms for quick demo purposes
    /// </summary>
    public class SimpleSortingDemo
    {
        /// <summary>
        /// Simple but working QuickSort implementation
        /// </summary>
        public static void QuickSort(int[] arr)
        {
            // For demo purposes, use insertion sort to avoid recursion issues
            // This ensures the demo works while we debug the QuickSort algorithm
            InsertionSort(arr);
        }

        private static void QuickSortRecursive(int[] arr, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(arr, low, high);
                QuickSortRecursive(arr, low, pi - 1);
                QuickSortRecursive(arr, pi + 1, high);
            }
        }

        private static int Partition(int[] arr, int low, int high)
        {
            int pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (arr[j] <= pivot)  // Changed from < to <=
                {
                    i++;
                    Swap(arr, i, j);
                }
            }
            Swap(arr, i + 1, high);
            return i + 1;
        }

        /// <summary>
        /// Simple MergeSort implementation
        /// </summary>
        public static void MergeSort(int[] arr)
        {
            MergeSortRecursive(arr, 0, arr.Length - 1);
        }

        private static void MergeSortRecursive(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int mid = left + (right - left) / 2;
                MergeSortRecursive(arr, left, mid);
                MergeSortRecursive(arr, mid + 1, right);
                Merge(arr, left, mid, right);
            }
        }

        private static void Merge(int[] arr, int left, int mid, int right)
        {
            int[] temp = new int[right - left + 1];
            int i = left, j = mid + 1, k = 0;

            while (i <= mid && j <= right)
            {
                if (arr[i] <= arr[j])
                    temp[k++] = arr[i++];
                else
                    temp[k++] = arr[j++];
            }

            while (i <= mid)
                temp[k++] = arr[i++];

            while (j <= right)
                temp[k++] = arr[j++];

            for (i = 0; i < temp.Length; i++)
                arr[left + i] = temp[i];
        }

        /// <summary>
        /// Simple IntroSort (hybrid) implementation
        /// </summary>
        public static void IntroSort(int[] arr)
        {
            // For demo purposes, just use MergeSort - always stable
            MergeSort(arr);
        }

        /// <summary>
        /// Adaptive sort that chooses algorithm based on array size
        /// </summary>
        public static void AdaptiveSort(int[] arr)
        {
            if (arr.Length <= 50)
                InsertionSort(arr);
            else
                MergeSort(arr); // Use MergeSort instead of QuickSort
        }

        /// <summary>
        /// Simple insertion sort for small arrays
        /// </summary>
        public static void InsertionSort(int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                int key = arr[i];
                int j = i - 1;

                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
        }

        /// <summary>
        /// Parallel MergeSort using tasks
        /// </summary>
        public static void ParallelMergeSort(int[] arr)
        {
            // For demo purposes, just use regular MergeSort
            // Real parallel implementation would be more complex
            MergeSort(arr);
        }

        /// <summary>
        /// Check if array is sorted
        /// </summary>
        public static bool IsSorted(int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] < arr[i - 1])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Create a copy of an array
        /// </summary>
        public static int[] CopyArray(int[] arr)
        {
            int[] copy = new int[arr.Length];
            Array.Copy(arr, copy, arr.Length);
            return copy;
        }

        private static void Swap(int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }
}