using UnityEngine;

public interface IProperty
{
#if UNITY_EDITOR
    string InspectorName { get; set; }
#endif
    string ReferenceName { get; set; }
    void Set(MaterialPropertyBlock prop);
}