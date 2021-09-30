using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable] public class IntCurvesProperty : Property
{
    [SerializeField] private AnimationCurve value = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public override void Set(MaterialPropertyBlock prop) {
        prop.SetInt(ReferenceName, (int)value.Evaluate(Random.value));
    }
}
[Serializable] public class FloatCurvesProperty : Property
{
    [SerializeField] private AnimationCurve value = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public override void Set(MaterialPropertyBlock prop) {
        prop.SetFloat(ReferenceName, value.Evaluate(Random.value));
    }
}