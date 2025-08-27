using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Target Has Moved", story: "[Target] has moved more than [Threshold] from [Position]", category: "Conditions", id: "eb74c65c25f2c4df30089517717969b7")]
public partial class TargetHasMovedCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> Threshold;
    [SerializeReference] public BlackboardVariable<Vector2> Position;

    public override bool IsTrue()
    {
        Vector2 currentPosition = Target.Value.position;
        Vector2 oldPosition = Position.Value;

        Vector2 distance = oldPosition - currentPosition;
        float threshold = Threshold.Value;
        threshold *= threshold;
        return distance.sqrMagnitude > threshold;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
