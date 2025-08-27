using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using AStarPathfind;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find Path to Target", story: "[Agent] generates [Path] to [Target]", category: "Action/Pathfind", id: "ddd2b5c4facb5e6098ecd95212fe898a")]
public partial class FindPathToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<List<Vector2>> Path;
    private IPathfinder _pathfinder;
    private Status _currentStatus;

    protected override Status OnStart()
    {
        if (Target.Value == null)
        {
            LogFailure("No target to pathfind to.");
            return Status.Failure;
        }

        _pathfinder = PathfinderLocator.GetPathfinder();
        if (_pathfinder == null)
        {
            LogFailure("Unable to retrieve a valid pathfinder.", true);
            return Status.Failure;
        }

        _currentStatus = Status.Running;
        Path.Value.Clear();
        _pathfinder.RequestPath(new PathRequest(Agent.Value.transform.position, Target.Value.position, OnPathResult));
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return _currentStatus;
    }

    protected void OnPathResult(Vector2[] waypoints, bool wasSucessful)
    {
        if (wasSucessful)
        {
            Path.Value.AddRange(waypoints);
            _currentStatus = Status.Success;
        }
        else
        {
            _currentStatus = Status.Failure;
        }
    }
}

