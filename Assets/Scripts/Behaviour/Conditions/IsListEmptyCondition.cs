using System;
using System.Collections;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Is List Empty", story: "Is [List] Empty", category: "Conditions", id: "984e0d34dbaf59c95889fe4106fbe6bd")]
public partial class IsListEmptyCondition : Condition
{
    [SerializeReference] public BlackboardVariable List;

    public override bool IsTrue()
    {
        if (List.ObjectValue is IList list)
        {
            return list.Count <= 0;
        }

        Debug.LogError($"{nameof(IsListEmptyCondition)} should only take in list types. Current value ({List.Name}) will always return true.");
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
