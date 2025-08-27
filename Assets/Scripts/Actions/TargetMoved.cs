using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/Target Moved")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "Target Moved", message: "Target left their location", category: "Events", id: "923a20e392aa363a30ca4e7b3116e88f")]
public sealed partial class TargetMoved : EventChannel { }

