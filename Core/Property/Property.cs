using System;
using UnityEngine;

[Serializable]
public abstract class Property : IProperty
{
    [SerializeField] private string _referenceName = "_Ref";
    public string ReferenceName {
        get => _referenceName;
        set => _referenceName = value;
    }
    public abstract void Set(MaterialPropertyBlock prop);
}
