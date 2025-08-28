using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsNull", story: "Is [Variable] Null", category: "Conditions", id: "e8d8a9c0bf7bd89d3af14d13b2919801")]
public partial class IsNullCondition : Condition
{
    [SerializeReference] public BlackboardVariable Variable;

    public override bool IsTrue()
    {
        if (Variable.Type.IsValueType)
        {
            return false;
        }

        // Due to bug with `ObjectValue` never returning true when null checking.
        return Variable.ObjectValue is null || Variable.ObjectValue.Equals(null);
    }
}
