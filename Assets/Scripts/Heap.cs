using System;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    public int Count => _curentItemCount;

    private T[] _items;
    private int _curentItemCount;

    public Heap(int maxHeapSize)
    {
        _items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = _curentItemCount;
        _items[_curentItemCount] = item;
        SortUp(item);
        _curentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = _items[0];
        _curentItemCount--;

        // Last item is now the first item
        T lastItem = _items[_curentItemCount];
        _items[0] = lastItem;
        lastItem.HeapIndex = 0;
        
        SortDown(lastItem);

        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public bool Contains(T item)
    {
        if (item.HeapIndex >= _curentItemCount || item.HeapIndex < 0)
        {
            return false;
        }

        return Equals(_items[item.HeapIndex], item);
    }

    private void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = GetChildLeftIndex(item);
            int childIndexRight = GetChildRightIndex(item);
            int swapIndex = 0;

            if (childIndexLeft < _curentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < _curentItemCount)
                {
                    if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                T lowestChild = _items[swapIndex];
                if (item.CompareTo(lowestChild) < 0)
                {
                    Swap(item, lowestChild);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    private void SortUp(T item)
    {
        int parentIndex = GetParentIndex(item);

        while (true)
        {
            T parentItem = _items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = GetParentIndex(item);
        }
    }

    private void Swap(T itemA, T itemB)
    {
        _items[itemA.HeapIndex] = itemB;
        _items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }

    private int GetParentIndex(T item)
    {
        return (item.HeapIndex - 1) / 2;
    }

    private int GetChildLeftIndex(T item)
    {
        return item.HeapIndex * 2 + 1;
    }

    private int GetChildRightIndex(T item)
    {
        return item.HeapIndex * 2 + 2;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    public int HeapIndex { get; set; }
}
