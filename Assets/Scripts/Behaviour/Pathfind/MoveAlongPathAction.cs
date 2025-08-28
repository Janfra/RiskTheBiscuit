using AI;
using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move Along Path", story: "[MovementComponent] moves Agent along [Path]", category: "Action/Pathfind", id: "930900fee798289018f014a3a4d0d95d")]
public partial class MoveAlongPathAction : Action
{
    [SerializeReference] public BlackboardVariable<MovementComponent> MovementComponent;
    [SerializeReference] public BlackboardVariable<List<Vector2>> Path;
    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(100.0f);
    [SerializeReference] public BlackboardVariable<int> PathIndex = new BlackboardVariable<int>(0);
    private Vector2 _targetWaypoint;

    protected override Status OnStart()
    {
        List<Vector2> path = Path.Value;
        if (path == null)
        {
            LogFailure("Path is null, reference is missing.", true);
            return Status.Failure;
        }

        if (MovementComponent.Value == null)
        {
            LogFailure("Movement is null, reference is missing.", true);
            return Status.Failure;
        }

        if (path.Count <= 0)
        {
            LogFailure("Path is empty, unable to follow empty path.");
            return Status.Failure;
        }

        int pathIndex = PathIndex.Value;
        if (_targetWaypoint != path[Mathf.Clamp(pathIndex, 0, path.Count - 1)]) 
        {
            // Assume new path if data dont match
            PathIndex.Value = 0;
        }

        if (pathIndex >= path.Count)
        {
            return Status.Success;
        }

        _targetWaypoint = path[pathIndex];
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        MovementComponent movementComponent = MovementComponent.Value;
        List<Vector2> path = Path.Value;
        

        if (movementComponent.TryMoveTo(_targetWaypoint, Speed.Value))
        {
            PathIndex.Value++;
            if (PathIndex.Value >= path.Count)
            {
                movementComponent.ClearVelocity();
                return Status.Success;
            }

            _targetWaypoint = path[PathIndex.Value];
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

