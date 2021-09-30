using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropertySetting", menuName = "Material Property Block/Property Setting")]
public class PropertyScriptableObject : ScriptableObject
{
    [SerializeReference]
    public List<IProperty> m_properties = new List<IProperty>();
}
