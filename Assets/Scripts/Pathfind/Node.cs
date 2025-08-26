using System;
using UnityEngine;

namespace AStarPathfind
{
    [Serializable]
    public class Node : IHeapItem<Node>
    {
        public readonly Vector2Int Indexes;
        public readonly Vector2 Position;
        public bool IsWalkable;
        public NodeCosts Costs;
        public Node Parent;
        public int HeapIndex { get => _heapIndex; set => _heapIndex = value; }
        private int _heapIndex;

        public Node(bool isWalkable, Vector2 position, Vector2Int indexes)
        {
            IsWalkable = isWalkable;
            Position = position;
            Indexes = indexes;
        }

        public Node(bool isWalkable, Vector2 position, int indexX, int indexY)
        {
            IsWalkable = isWalkable;
            Position = position;
            Indexes = new Vector2Int(indexX, indexY);
        }

        public int CompareTo(Node other)
        {
            int compare = Costs.f.CompareTo(other.Costs.f);
            if (compare == 0)
            {
                compare = Costs.h.CompareTo(other.Costs.h);
            }

            return ~compare;
        }
    }

    public struct NodeCosts
    {
        public int g;
        public int h;
        public int f => g + h;
    }
}
