using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class InterfaceReference<TInterface, TObject> where TInterface : class where TObject : Object
{
    [SerializeField, HideInInspector] private TObject _object;

    public TInterface Value
    {
        get => _object switch
        {
            null => null,
            TInterface @interface => @interface,
            _ => throw new InvalidOperationException($"The underlying value {_object} must implement the interface {typeof(TInterface)}")
        };

        set => _object = value switch
        {
            null => null,
            TObject newValue => newValue,
            _ => throw new ArgumentException($"The assigned value {value} must be of type {typeof(TObject)}")
        };
    }

    public TObject Object => _object;

    public InterfaceReference() { }

    public InterfaceReference(TObject target) => _object = target;

    public InterfaceReference(TInterface @interface) => _object = @interface as TObject;
}

[Serializable]
public class InterfaceReference<TInterface> : InterfaceReference<TInterface, Object> where TInterface : class
{
    public InterfaceReference() { }
    public InterfaceReference(Object target) : base(target) { }
    public InterfaceReference(TInterface @interface) : base(@interface) { }
}
