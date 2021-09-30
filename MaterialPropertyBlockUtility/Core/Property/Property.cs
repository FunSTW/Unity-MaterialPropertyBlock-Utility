using System;
using UnityEngine;

[Serializable]
public abstract class Property : IProperty
{
#if UNITY_EDITOR
    [SerializeField,HideInInspector] private string _inspectorName = "undefined";
    public string InspectorName {
        get => _inspectorName;
        set => _inspectorName = value;
    }
#endif
    [SerializeField] private string _referenceName = "_ShaderPropRef";
    public string ReferenceName {
        get => _referenceName;
        set => _referenceName = value;
    }
    public abstract void Set(MaterialPropertyBlock prop);
}
