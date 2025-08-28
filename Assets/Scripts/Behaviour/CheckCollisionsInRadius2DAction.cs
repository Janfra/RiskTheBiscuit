using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Check Collisions in Radius 2D", story: "Check [Agent] collisions in [Radius] 2D in [Layer]", category: "Action/Physics2D", id: "bb030dd183fcab3fac2bae5e4c978e29")]
public partial class CheckCollisionsInRadius2DAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> Radius;
    [SerializeReference] public BlackboardVariable<string> Layer;
    [SerializeReference] public BlackboardVariable<string> Tag;
    [Tooltip("[Out Value] This field is assigned with the collided object, if a collision was found.")]
    [SerializeReference] public BlackboardVariable<GameObject> CollidedObject;

    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent set.");
            return Status.Failure;
        }

        LayerMask layer = LayerMask.GetMask(Layer.Value);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(Agent.Value.transform.position, Radius.Value, layer.value);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            Collider2D hitCollider = hitColliders[i];
            if (hitCollider.gameObject == Agent.Value
                || (Tag != null && Tag.Value != string.Empty && !hitCollider.CompareTag(Tag.Value)))
            {
                continue;
            }

            CollidedObject.Value = hitCollider.gameObject;
            return Status.Success;
        }

        return Status.Failure;
    }
}

