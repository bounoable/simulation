namespace Simulation.Support
{
    class Heap<T> where T: IHeapItem<T>
    {
        public int Count { get; private set; } = 0;
        
        T[] items;

        public Heap(int maxSize)
        {
            items = new T[maxSize];
        }

        public void Add(T item)
        {
            item.HeapIndex = Count;
            items[Count] = item;
            SortUp(item);
            Count++;
        }

        public bool Contains(T item)
        {
            return item.HeapIndex < Count && items[item.HeapIndex].Equals(item);
        }

        public void UpdateItem(T item)
        {
            SortDown(item);
            SortUp(item);
        }

        /// <summary>
        /// Remove the first item and return it.
        /// </summary>
        /// <returns>The removed item.</returns>
        public T Unshift()
        {
            if (items.Length == 0)
                return default(T);
            
            T firstItem = items[0];
            Count--;
            items[0] = items[Count];
            items[0].HeapIndex = 0;
            SortDown(items[0]);

            return firstItem;
        }

        public T RemoveFirst() => Unshift();

        void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true) {
                T parent = items[parentIndex];

                if (item.CompareTo(parent) > 0) {
                    Swap(item, parent);
                } else {
                    break;
                }

                parentIndex = (parentIndex - 1) / 2;
            }
        }

        void SortDown(T item)
        {   
            while (true) {
                int childAIndex = item.HeapIndex * 2 + 1;
                int childBIndex = item.HeapIndex * 2 + 2;
                int swapIndex = 0;

                if (childAIndex >= Count)
                    return;

                swapIndex = childAIndex;

                if (childBIndex < Count && items[childAIndex].CompareTo(items[childBIndex]) < 0) {
                    swapIndex = childBIndex;
                }

                if (item.CompareTo(items[swapIndex]) > -1)
                    return;

                Swap(item, items[swapIndex]);
            }
        }

        void Swap(T itemA, T itemB)
        {
            int itemAIndex = itemA.HeapIndex;
            items[itemA.HeapIndex] = itemB;
            items[itemB.HeapIndex] = itemA;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }
    }
}