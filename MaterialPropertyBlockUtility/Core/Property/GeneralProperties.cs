using System;
using UnityEngine;

[Serializable] public class IntProperty : Property
{
    [SerializeField] private int value = default;

    public override void Set(MaterialPropertyBlock prop) {
        prop.SetInt(ReferenceName, value);
    }
}
[Serializable] public class FloatProperty : Property
{
    [SerializeField] private float value = default;

    public override void Set(MaterialPropertyBlock prop) {
        prop.SetFloat(ReferenceName, value);
    }
}
// Matrix4x4 https://www.reddit.com/r/Unity3D/comments/dtxntl/shaders_how_do_i_use_the_matrix_from/
[Serializable] public class ColorProperty : Property
{
    [SerializeField] private Color value = default;

    public override void Set(MaterialPropertyBlock prop) {
        prop.SetColor(ReferenceName, value);
    }
}
[Serializable] public class VectorProperty : Property
{
    [SerializeField] private Vector4 value = default;

    public override void Set(MaterialPropertyBlock prop) {
        prop.SetVector(ReferenceName, value);
    }
}