using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable] public class IntRandomProperty : Property
{
    [SerializeField] private int valueMin = 0;
    [SerializeField] private int valueMax = 1;

    public override void Set(MaterialPropertyBlock prop) {
        prop.SetInt(ReferenceName, Random.Range(valueMin, valueMax));
    }
}
[Serializable] public class FloatRandomProperty : Property
{
    [SerializeField] private float valueMin = 0;
    [SerializeField] private float valueMax = 1;

    public override void Set(MaterialPropertyBlock prop) {
        prop.SetFloat(ReferenceName, Random.Range(valueMin, valueMax));
    }
}
[Serializable] public class VectorRandomProperty : Property
{
    [SerializeField] private Vector4 valueMin = Vector4.zero;
    [SerializeField] private Vector4 valueMax = Vector4.one;
    public override void Set(MaterialPropertyBlock prop) {
        Vector4 vector = new Vector4(Random.Range(valueMin.x, valueMax.x), Random.Range(valueMin.y, valueMax.y), Random.Range(valueMin.z, valueMax.z), Random.Range(valueMin.w, valueMax.w));
        prop.SetColor(ReferenceName, vector);
    }
}
[Serializable] public class ColorHSVRandomProperty : Property
{
    [SerializeField] private Vector4 HSVAMin = Vector4.zero;
    [SerializeField] private Vector4 HSVAMax = Vector4.one;
    public override void Set(MaterialPropertyBlock prop) {
        prop.SetColor(ReferenceName, Random.ColorHSV(HSVAMin.x, HSVAMax.x, HSVAMin.y, HSVAMax.y, HSVAMin.z, HSVAMax.z, HSVAMin.w, HSVAMax.w));
    }
}
[Serializable] public class ColorRandomProperty : Property
{
    [SerializeField] private bool alpha = false;
    public override void Set(MaterialPropertyBlock prop) {
        Color color = new Color(Random.value, Random.value, Random.value, alpha ? Random.value : 1);
        prop.SetColor(ReferenceName, color);
    }
}