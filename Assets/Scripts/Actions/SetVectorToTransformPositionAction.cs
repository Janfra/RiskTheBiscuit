using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Vector to Transform Position", story: "[Vector] is set to [Transform] position", category: "Action", id: "1c61e85a14e07791935fad92d159b731")]
public partial class SetVectorToTransformPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector2> Vector;
    [SerializeReference] public BlackboardVariable<Transform> Transform;
    protected override Status OnStart()
    {
        Vector.Value = Transform.Value.position;
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

