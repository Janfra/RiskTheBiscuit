using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move To Target", story: "[Movement] moves at [Speed] towards [Target]", category: "Action/Movement", id: "ca1b91e62f875cf0d17fa98cdd9f1d70")]
public partial class MoveToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<MovementComponent> Movement;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> Speed;

    protected override Status OnStart()
    {
        if (Movement.Value == null)
        {
            return Status.Failure;
        }

        if (Target.Value == null)
        {
            return Status.Failure;
        }

        Movement.Value.TryMoveTo(Target.Value.position, Speed.Value);
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

