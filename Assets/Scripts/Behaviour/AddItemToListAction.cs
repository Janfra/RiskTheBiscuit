using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Add Item to String List", story: "Add [Item] to [List]", category: "Action/List", id: "6be4ede7683b8be6f7647d66b53a49b6")]
public partial class AddItemToStringListAction : Action
{
    [SerializeReference] public BlackboardVariable<string> Item;
    [SerializeReference] public BlackboardVariable<List<string>> List;

    protected override Status OnStart()
    {
        List.Value.Add(Item.Value);
        return Status.Success;
    }
}

