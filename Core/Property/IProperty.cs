using UnityEngine;

public interface IProperty
{
    string ReferenceName { get; set; }
    void Set(MaterialPropertyBlock prop);
}