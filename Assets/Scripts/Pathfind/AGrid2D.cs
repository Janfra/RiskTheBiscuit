using System.Collections.Generic;
using UnityEngine;

namespace AStarPathfind
{
    public class AGrid2D : MonoBehaviour
    {
        public LayerMask UnwalkableMask;
        public Vector2 GridSize;
        public float NodeRadius;

        public int GridNodeSize => _nodeSizeX * _nodeSizeY;

        [SerializeField]
        private Node[,] _gridNodes;

        private float _nodeDiameter;
        private int _nodeSizeX, _nodeSizeY;

        private void Awake()
        {
            if (_gridNodes == null)
            {
                CreateGrid();
            }

            if (_nodeDiameter == 0)
            {
                SetNodeGridSizes();
            }
        }

        public Node NodeFromWorldPosition(Vector2 position)
        {
            float percentX = Mathf.Clamp01((position.x + GridSize.x * 0.5f) / GridSize.x);
            float percentY = Mathf.Clamp01((position.y + GridSize.y * 0.5f) / GridSize.y);

            int x = Mathf.RoundToInt((_nodeSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((_nodeSizeY - 1) * percentY);
            return _gridNodes[x, y];
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int checkX = node.Indexes.x + x;
                    int checkY = node.Indexes.y + y;

                    if (TryGetNode(checkX, checkY, out Node neighbour))
                    {
                        neighbours.Add(neighbour);
                    }
                }
            }

            return neighbours;
        }

        [ContextMenu("Bake Grid")]
        private void CreateGrid()
        {
            SetNodeGridSizes();

            _gridNodes = new Node[_nodeSizeX, _nodeSizeY];
            Vector2 bottomLeftCorner = GetBottomLeftCorner();

            for (int x = 0; x < _nodeSizeX; x++)
            {
                for (int y = 0; y < _nodeSizeY; y++)
                {
                    float xOffset = (x * _nodeDiameter + NodeRadius);
                    float yOffset = (y * _nodeDiameter + NodeRadius);
                    Vector2 point = bottomLeftCorner + Vector2.right * xOffset + Vector2.up * yOffset;
                    bool isWalkable = !Physics2D.OverlapCircle(point, NodeRadius, UnwalkableMask.value);
                    _gridNodes[x, y] = new Node(isWalkable, point, x, y);
                }
            }
        }

        private Vector2 GetBottomLeftCorner()
        {
            Vector2 gridPosition = transform.position;
            return gridPosition - Vector2.right * GridSize.x / 2 - Vector2.up * GridSize.y / 2;
        }

        private void SetNodeGridSizes()
        {
            _nodeDiameter = NodeRadius * 2;
            _nodeSizeX = Mathf.FloorToInt(GridSize.x / _nodeDiameter);
            _nodeSizeY = Mathf.FloorToInt(GridSize.y / _nodeDiameter);
        }

        private bool TryGetNode(int x, int y, out Node node)
        {
            node = null;
            if (!IsIndexInGrid(x, y))
            {
                return false;
            }

            node = _gridNodes[x, y];
            return true;
        }

        private bool IsIndexInGrid(int x, int y)
        {
            return x >= 0 && x < _nodeSizeX && y >= 0 && y < _nodeSizeY;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, GridSize);
            if (_gridNodes != null)
            {
                foreach (Node node in _gridNodes)
                {
                    Gizmos.color = node.IsWalkable ? Color.green : Color.red;
                    Gizmos.DrawCube(node.Position, Vector2.one * (_nodeDiameter - 0.1f));
                }
            }
        }
    }
}
