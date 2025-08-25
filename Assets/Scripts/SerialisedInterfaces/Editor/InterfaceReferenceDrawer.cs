using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(InterfaceReference<>))]
[CustomPropertyDrawer(typeof(InterfaceReference<,>))]
public class InterfaceReferenceDrawer : PropertyDrawer
{
    const string UnderlyingObjectFieldName = "_object";
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var underlyingObjectProperty = property.FindPropertyRelative(UnderlyingObjectFieldName);
        if (underlyingObjectProperty == null)
        {
            EditorGUI.LabelField(position, label.text, "Error: Could not find underlying object property.");
            return;
        }

        var arguments = GetInterfaceArguments(fieldInfo);
        EditorGUI.BeginProperty(position, label, property);
        var assignedObject = EditorGUI.ObjectField(position, label, underlyingObjectProperty.objectReferenceValue, arguments.ObjectType, true);

        if (assignedObject != null)
        {
            Object component = null;

            if (assignedObject is GameObject gameObject)
            {
                component = gameObject.GetComponent(arguments.InterfaceType);
            }
            else if (arguments.InterfaceType.IsAssignableFrom(assignedObject.GetType()))
            {
                component = assignedObject;
            }

            if (component != null)
            {
                ValidateAndAssignObject(underlyingObjectProperty, component, component.name, arguments.InterfaceType.Name);
            }
            else
            {
                Debug.LogWarning($"Assigned object does not implement required interface '{arguments.InterfaceType.Name}'.");
                underlyingObjectProperty.objectReferenceValue = null;
            }
        }
        else
        {
            underlyingObjectProperty.objectReferenceValue = null;
        }
        EditorGUI.EndProperty();
        InterfaceReferenceUtil.OnGUI(position, property, label, arguments);
    }

    static InterfaceArguments GetInterfaceArguments(FieldInfo fieldInfo)
    {
        Type objectType = null, interfaceType = null;
        Type fieldType = fieldInfo.FieldType;

        bool TryGetTypesFromInterfaceReference(Type type, out Type objectType, out Type interfaceType)
        {
            objectType = interfaceType = null;
            if (type?.IsGenericType == false)
            {
                return false;
            }

            var genericType = type.GetGenericTypeDefinition();
            if (genericType == typeof(InterfaceReference<>))
            {
                type = type.BaseType;
            }

            if (type?.GetGenericTypeDefinition() == typeof(InterfaceReference<,>))
            {
                var genericArguments = type.GetGenericArguments();
                interfaceType = genericArguments[0];
                objectType = genericArguments[1];
                return true;
            }

            return false;
        }

        void GetTypesFromList(Type type, out Type objectType, out Type interfaceType)
        {
            objectType = interfaceType = null;
            var listInterface = type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>));
            if (listInterface != null)
            {
                var elementType = listInterface.GetGenericArguments()[0];
                if (!TryGetTypesFromInterfaceReference(elementType, out objectType, out interfaceType))
                {
                    throw new InvalidOperationException($"The list element type {elementType} must be of type InterfaceReference<,> or InterfaceReference<>.");
                }
            }
        }

        if (!TryGetTypesFromInterfaceReference(fieldType, out objectType, out interfaceType))
        {
            GetTypesFromList(fieldType, out objectType, out interfaceType);
        }

        return new InterfaceArguments(objectType, interfaceType);
    }

    static void ValidateAndAssignObject(SerializedProperty property, Object targetObject, string componentNameOrType, string interfaceName = null)
    {
        if (targetObject != null)
        {
            property.objectReferenceValue = targetObject;
        }
        else
        {
            Debug.LogWarning($"The {(interfaceName != null ? $"GameObject '{componentNameOrType}'" : $"assigned object")} does not have a component that implements '{componentNameOrType}'");
            property.objectReferenceValue = null;
        }
    }
}

public struct InterfaceArguments
{
    public readonly Type ObjectType;
    public readonly Type InterfaceType;

    public InterfaceArguments(Type objectType, Type interfaceType)
    {
        Debug.Assert(typeof(Object).IsAssignableFrom(objectType), $"{nameof(objectType)} must be of type {typeof(Object)}.");
        Debug.Assert(interfaceType.IsInterface, $"{nameof(interfaceType)} must be an interface type.");

        ObjectType = objectType;
        InterfaceType = interfaceType;
    }
}