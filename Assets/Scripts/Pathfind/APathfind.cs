using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace AStarPathfind
{
    [RequireComponent(typeof(AGrid2D))]
    public class APathfind : MonoBehaviour
    {
        const int HORIZONTAL_VALUE = 14;
        const int LINEAR_VALUE = 10;

        [SerializeField]
        private AGrid2D _nodeGrid;

        private void Awake()
        {
            if (!_nodeGrid)
            {
                _nodeGrid = GetComponent<AGrid2D>();
            }
        }

        public void StartFindPath(Vector2 from, Vector2 to, Action<Vector2[], bool> callback)
        {
            if (callback == null)
            {
                return;
            }

            StartCoroutine(FindPath(from, to, callback));
        }

        private IEnumerator FindPath(Vector2 from, Vector2 to, Action<Vector2[], bool> callback)
        {
            Vector2[] waypoints = new Vector2[0];
            bool pathSuccess = false;

            Node startNode = _nodeGrid.NodeFromWorldPosition(from);
            Node endNode = _nodeGrid.NodeFromWorldPosition(to);
            if (startNode.IsWalkable && endNode.IsWalkable)
            {
                Heap<Node> openSet = new Heap<Node>(_nodeGrid.GridNodeSize);
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == endNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    foreach (var neighbour in _nodeGrid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int movementCost = currentNode.Costs.g + GetDistance(currentNode, neighbour);
                        if (movementCost < neighbour.Costs.g || !openSet.Contains(neighbour))
                        {
                            neighbour.Costs.g = movementCost;
                            neighbour.Costs.h = GetDistance(neighbour, endNode);
                            neighbour.Parent = currentNode;

                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                            else
                            {
                                openSet.UpdateItem(neighbour);
                            }
                        }
                    }
                }
            }


            yield return null;
            if (pathSuccess)
            {
                waypoints = RetracePath(startNode, endNode);
            }
            callback(waypoints, pathSuccess);
        }

        private Vector2[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            if (path.Count > 0)
            {
                Vector2[] waypoints = SimplifyPath(path);
                Array.Reverse(waypoints);
                return waypoints;
            }

            return new Vector2[0];
        }

        private Vector2[] SimplifyPath(List<Node> path)
        {
            List<Vector2> waypoints = new List<Vector2>();
            Vector2 oldDirection = Vector2.zero;

            waypoints.Add(path[0].Position);
            for (int i = 1; i < path.Count; i++)
            {
                Vector2 newDirection = path[i - 1].Indexes - path[i].Indexes;
                if (newDirection != oldDirection)
                {
                    waypoints.Add(path[i].Position);
                }

                oldDirection = newDirection;
            }

            return waypoints.ToArray();
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            Vector2Int distance = nodeA.Indexes - nodeB.Indexes;
            distance.x = Mathf.Abs(distance.x);
            distance.y = Mathf.Abs(distance.y);

            if (distance.x > distance.y)
            {
                return HORIZONTAL_VALUE * distance.y + LINEAR_VALUE * (distance.x - distance.y);
            }
            else
            {
                return HORIZONTAL_VALUE * distance.x + LINEAR_VALUE * (distance.y - distance.x);
            }
        }
    }
}
