using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace BehaviorUtils
{
    public static class NodeUtils
    {
        public static void LogReferenceFail<TNode, TReference>(this TNode node, TReference reference) where TNode : Node
        {
            node.LogFailure($"{nameof(reference)} reference is null. Unable to perform {(nameof(node))} in {node.GameObject}.", true);
        }
    }
}
