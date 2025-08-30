using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Direction to Rotation", story: "Convert [Direction] to [Rotation]", category: "Action", id: "d14a9dc29049c070136b86b0c54c6a11")]
public partial class DirectionToRotationAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector2> Direction;
    [SerializeReference] public BlackboardVariable<Vector3> Rotation;

    protected override Status OnStart()
    {
        Vector2 direction = Direction.Value.normalized;
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Rotation.Value = Quaternion.Euler(0f, 0f, rot_z - 90).eulerAngles;
        return Status.Success;
    }
}

