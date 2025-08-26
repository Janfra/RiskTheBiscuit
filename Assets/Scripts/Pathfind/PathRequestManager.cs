using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStarPathfind
{
    [RequireComponent(typeof(APathfind))]
    public class PathRequestManager : MonoBehaviour, IPathfinder
    {
        [SerializeField]
        private APathfind _pathfinder;

        private Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();
        private PathRequest _currentPathRequest;
        bool _isProcessingPath;

        private void Awake()
        {
            if (_pathfinder == null)
            {
                _pathfinder = GetComponent<APathfind>();
            }

            PathfinderLocator.Provide(this);
        }

        public void RequestPath(PathRequest request)
        {
            _pathRequestQueue.Enqueue(request);
            TryProcessNext();
        }

        private void TryProcessNext()
        {
            if (!_isProcessingPath && _pathRequestQueue.Count > 0)
            {
                _currentPathRequest = _pathRequestQueue.Dequeue();
                _isProcessingPath = true;
                _pathfinder.StartFindPath(_currentPathRequest.From, _currentPathRequest.To, FinishedProcessingPath);
            }
        }

        public void FinishedProcessingPath(Vector2[] path, bool success)
        {
            _currentPathRequest.Callback(path, success);
            _isProcessingPath = false;
            TryProcessNext();
        }
    }

    public class PathfinderLocator
    {
        public class NullPathfinder : IPathfinder
        {
            public void RequestPath(PathRequest request)
            {
                Debug.LogError($"Pathfind service has not been provided yet.");
            }
        }

        private static IPathfinder _service;

        public static IPathfinder GetPathfinder()
        {
            if (_service == null)
            {
                _service = new NullPathfinder();
            }

            return _service;
        }

        public static void Provide(IPathfinder service)
        {
            _service = service;
        }
    }

    public interface IPathfinder
    {
        public void RequestPath(PathRequest request);
    }

    public struct PathRequest
    {
        public Vector2 From;
        public Vector2 To;
        public Action<Vector2[], bool> Callback;

        public PathRequest(Vector2 from, Vector2 to, Action<Vector2[], bool> callback)
        {
            From = from;
            To = to;
            Callback = callback;
        }
    }
}
