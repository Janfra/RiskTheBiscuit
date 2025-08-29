using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BehaviorUtils;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Raycast Check to Target", story: "[Agent] raycasts to [Target] with [Layers]", category: "Action", id: "7cd2d789fd5a9481f874ba4aa2b41258")]
public partial class RaycastCheckToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<List<string>> Layers;
    [Tooltip("If distance is equal or lower to 0, the check has unlimited distance")]
    [SerializeReference] public BlackboardVariable<float> Distance;
    [Tooltip("If provided and collision is expected, it will verify that the collision is in this layer")]
    [SerializeReference] public BlackboardVariable<string> TargetLayer;
    [SerializeReference] public BlackboardVariable<float> Radius;
    [SerializeReference] public BlackboardVariable<bool> ExpectsCollision = new BlackboardVariable<bool>(false);

    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            this.LogReferenceFail(Agent.Value);
            return Status.Failure;
        }

        if (Target.Value == null)
        {
            this.LogReferenceFail(Target.Value);
            return Status.Failure;
        }

        Vector2 agentPosition = Agent.Value.transform.position;
        LayerMask mask = LayerMask.GetMask(Layers.Value.ToArray());
        Vector2 direction = (Vector2)Target.Value.position - agentPosition;
        RaycastHit2D hitResult = Physics2D.CircleCast(agentPosition, Radius.Value, direction.normalized, Distance.Value, mask.value);

        bool isCollision = hitResult.collider;
        if (ExpectsCollision.Value)
        {
            if (TargetLayer.Value.Length > 0)
            {
                return hitResult.collider.gameObject.layer == LayerMask.NameToLayer(TargetLayer.Value) ? Status.Success : Status.Failure;
            }
            return Status.Success;
        }
        else
        {
            return !isCollision ? Status.Success : Status.Failure;
        }
    }
}

