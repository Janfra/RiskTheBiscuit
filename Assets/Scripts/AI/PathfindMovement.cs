using AStarPathfind;
using System.Collections;
using UnityEngine;

namespace AI
{
    public class PathfindMovement : MovementComponent
    {
        [SerializeField]
        private float _speed;

        private Vector2[] _path;
        private int _targetIndex;

        private void Start()
        {
            StartMovingTowards(Vector2.zero);
        }

        public void StartMovingTowards(Vector2 position)
        {
            PathfinderLocator.GetPathfinder().RequestPath(new PathRequest(transform.position, position, OnPathFound));
        }

        private void OnPathFound(Vector2[] path, bool wasSuccessful)
        {
            if (!wasSuccessful)
            {
                return;
            }

            _path = path;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }

        private IEnumerator FollowPath()
        {
            Vector2 currentWaypoint = _path[0];

            while (true)
            {
                if (TryMoveTo(currentWaypoint, _speed))
                {
                    _targetIndex++;
                    if (_targetIndex >= _path.Length)
                    {
                        ClearVelocity();
                        _targetIndex = 0;
                        _path = null;
                        yield break;
                    }

                    currentWaypoint = _path[_targetIndex];
                }

                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            if (_path == null)
            {
                return;
            }

            for (int i = _targetIndex; i < _path.Length; i++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(_path[i], Vector2.one);

                if (i == _targetIndex)
                {
                    Gizmos.DrawLine(transform.position, _path[i]);
                }
                else
                {
                    Gizmos.DrawLine(_path[i - 1], _path[i]);
                }
            }
        }
    }
}
