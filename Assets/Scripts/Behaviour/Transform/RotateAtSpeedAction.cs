using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Rotate 2D at Speed", story: "Rotate 2D [Target] at [Speed] towards [Rotation]", category: "Action/Transform", id: "2fafec8d082e5565e5172a019f062880")]
public partial class RotateAtSpeedAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> Speed;
    [SerializeReference] public BlackboardVariable<Vector3> Rotation;

    protected override Status OnStart()
    {
        if (Target.Value == null)
        {
            return Status.Failure;
        }

        Transform transform = Target.Value;
        Vector3 startRotation = transform.rotation.eulerAngles;
        Vector3 endRotation = Rotation.Value;
        if (startRotation.z == endRotation.z)
        {
            return Status.Success;
        }

        transform.rotation = Quaternion.Euler(endRotation);
        return Status.Success;
    }
}

