using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/ValuesInitialised")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "ValuesInitialised", message: "Values have been initialised", category: "Events/Initialisation", id: "e3799b30b5e80f083025cdfd9fdaa614")]
public sealed partial class ValuesInitialised : EventChannel { }

