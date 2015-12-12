using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_q2
{
    class HeapData<T> {
        public int priority = -1;
        public T value;

        public HeapData()
        {
        }

        public HeapData(T _value, int _priority)
        {
            value = _value;
            priority = _priority;
        }
    }

    // implemented as a min heap type (lowest priority being at the root)
    // binary heap provides O(log n) average for insertion and removal
    // .. and it's easy to visualize, so i like it
    /* notes:
        - assuming base 1 indexing..
        - left child index = 2 * i
        - right child index = 2 * i + 1
        - parent index = i / 2
    */
    class BinaryHeap<T> {
        // member data
        public List<HeapData<T>> heap = new List<HeapData<T>>();
        
        // ctor
        public BinaryHeap()
        {
            // use the 0 index as a dummy to simplify calculation
            heap.Add( new HeapData<T>() );
        }

        // primary public interface
        public void insert(T value, int priority)
        {
            heap.Add( new HeapData<T>( value, priority ) );

            printHeap();

            heapUp( heap.Count - 1 );
        }
        
        public void removeMin()
        {
            if( !isValidIndex( heap.Count - 1 ) )
            {
                // nothing to remove, bail
                return;
            }

            // remove the lowest priority element: the root
            // replace the root with the last element for resorting
            heap[1] = heap[heap.Count - 1];

            // remove last element
            heap.RemoveAt( heap.Count - 1 );

            printHeap();

            // resort
            heapDown( 1 );
        }
        
        public void removeMax()
        {
            if( !isValidIndex( heap.Count - 1 ) )
            {
                // nothing to look for
                return;
            }

            // find the element with the biggest priority
            int targetIdx = 1;

            for( int i = 1; i < heap.Count; ++i )
            {
                if( heap[i].priority > getPriority( targetIdx ) )
                {
                    targetIdx = i;
                }
            }
            
            // replace target element with the last element for resorting
            // this also acts as the removal of the target element
            heap[targetIdx] = heap[heap.Count - 1];

            // remove last element
            heap.RemoveAt( heap.Count - 1 );

            printHeap();

            // resort
            heapDown( targetIdx );
        }

        // for testing and debugging
        public void initWith(List<HeapData<T>> data)
        {
            heap = data;
        }

        public void printHeap()
        {
            string output = "";
            
            for( int i = 1; i < heap.Count; ++i )
            {
                output = output + " " + heap[i].priority;
            }

            Console.WriteLine( output );
        }

        // private interface
        private void heapUp(int currentIdx)
        {
            // check fail condition
            if( !isValidIndex( currentIdx ) )
            {
                return;
            }
            
            int parentIdx = getParentIndex( currentIdx );

            // check if the current priority is lower than the parent priority..
            if( isValidIndex( parentIdx ) &&
                getPriority( currentIdx ) < getPriority( parentIdx ) )
            {
                // it is, swap elements
                swapData( currentIdx, parentIdx );

                printHeap();

                // moving on..
                heapUp( parentIdx );
            }
        }

        private void heapDown(int currentIdx)
        {
            int leftIdx = getLeftChildIndex( currentIdx );
            int rightIdx = getRightChildIndex( currentIdx );

            // move left by default
            int smallerPriorityIdx = leftIdx;

            if( isValidIndex( smallerPriorityIdx ) &&                           // valid index
                isValidIndex( rightIdx ) &&                                     // valid index
                getPriority( smallerPriorityIdx ) > getPriority( rightIdx ) )   // right child is actually smaller
            {
                // use the right child instead
                smallerPriorityIdx = rightIdx;
            }

            // still going
            if( isValidIndex( smallerPriorityIdx ) )
            {
                // swap
                swapData( currentIdx, smallerPriorityIdx );

                printHeap();

                // moving on..
                heapDown( smallerPriorityIdx );
            }
        }

        // helpers
        private bool isValidIndex(int idx)
        {
            return 0 < idx && idx < heap.Count;
        }

        private int getParentIndex(int childIdx)
        {
            int ret = childIdx / 2;
            
            // check if valid
            if( !isValidIndex( childIdx ) )
            {
                return -1;
            }

            return ret; 
        }

        private int getLeftChildIndex(int parentIdx)
        {
            int ret = 2 * parentIdx;

            // check if valid
            if( !isValidIndex( parentIdx ) )
            {
                return -1;
            }

            return ret;
        }

        private int getRightChildIndex(int parentIdx)
        {
            int ret = 2 * parentIdx + 1;

            // check if valid
            if( !isValidIndex( parentIdx ) )
            {
                return -1;
            }

            return ret;
        }

        private int getPriority(int idx)
        {
            // assumes a valid index, so do error checking before calling this
            return heap.ElementAt( idx ).priority;
        }

        private void swapData(int indexA, int indexB)
        {
            HeapData<T> temp = heap[indexA];
            heap[indexA] = heap[indexB];
            heap[indexB] = temp;
        }
    }

    
    class PriorityQueue<T> { 
        // private member data
        private BinaryHeap<T> heap = new BinaryHeap<T>();

        // ctor
        public PriorityQueue()
        {
        }

        // primary interface
        public void Enqueue(T value, int priority)
        {
            // ~ O(log n)
            heap.insert( value, priority );
        }

        public void DequeueMin()
        {
            // ~ O(log n)
            heap.removeMin();
        }

        public void DequeueMax()
        {
            // ~ O(n) + O(log n)
            heap.removeMax();
        }
    }

    
    class Program
    {
        static void Main(string [] args)
        {
            PriorityQueue<int> foo = new PriorityQueue<int>();

            Console.WriteLine( "Inserting:" );
            foo.Enqueue( 0, 4 );
            foo.Enqueue( 0, 9 );
            foo.Enqueue( 0, 12 );
            foo.Enqueue( 0, 8 );
            foo.Enqueue( 0, 56 );
            foo.Enqueue( 0, 1 );
            foo.Enqueue( 0, 19 );
            foo.Enqueue( 0, 24 );
            foo.Enqueue( 0, 7 );

            Console.WriteLine( "\nDeleting Min" );
            foo.DequeueMin();

            Console.WriteLine( "\nDeleting Max" );
            foo.DequeueMax();

            // done
            Console.ReadLine();
        }
    }
}
