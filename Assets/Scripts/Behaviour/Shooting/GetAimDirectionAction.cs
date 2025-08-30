using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get Aim Direction", story: "Set [Vector] to [Shooting] aim direction", category: "Action/Shooting", id: "49fca9e55d545c08a38e44bb218a7d1a")]
public partial class GetAimDirectionAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector2> Vector;
    [SerializeReference] public BlackboardVariable<BaseShootingComponent> Shooting;

    protected override Status OnStart()
    {
        if (Shooting.Value == null)
        {
            return Status.Failure;
        }

        Vector.Value = Shooting.Value.GetIntendedAimDirection();
        return Status.Success;
    }
}

