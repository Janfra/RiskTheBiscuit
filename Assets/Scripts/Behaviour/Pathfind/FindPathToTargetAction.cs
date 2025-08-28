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

    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent set to find path from.", true);
            return Status.Failure;
        }

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

        Path.Value.Clear();
        _pathfinder.RequestPath(new PathRequest(Agent.Value.transform.position, Target.Value.position, OnPathResult));
        return Status.Waiting;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected void OnPathResult(Vector2[] waypoints, bool wasSucessful)
    {
        if (wasSucessful)
        {
            Path.Value.AddRange(waypoints);
        }
        AwakeNode(this);
    }
}

