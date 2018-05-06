using System;

namespace Simulation.Support
{
    interface IHeapItem<T>: IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}