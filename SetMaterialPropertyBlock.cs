/* Reference https://medium.com/@trepala.aleksander/serializereference-in-unity-b4ee10274f48 */

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SetMaterialPropertyBlock : MonoBehaviour
{
    MeshRenderer m_meshRenderer = null;
    [SerializeField] PropertyScriptableObject m_overrideSettings = null;
    [SerializeReference] List<IProperty> m_properties = new List<IProperty>();

    public PropertyScriptableObject OverrideSettings {
        get {
            return m_overrideSettings;
        }
        set {
            m_overrideSettings = value;
        }
    }

    public List<IProperty> GetCurrentSetting => m_overrideSettings ? m_overrideSettings.m_properties : m_properties;

    void Start() {
        m_meshRenderer = GetComponent<MeshRenderer>();
        if(m_overrideSettings != null) {
            Apply(m_overrideSettings.m_properties);
        } else {
            Apply(m_properties);
        }
    }

    public void Apply(List<IProperty> m_properties) {
        if(Check(m_properties)) { return; }
        var prop = new MaterialPropertyBlock();
        m_meshRenderer.GetPropertyBlock(prop);
        foreach(var propValue in m_overrideSettings.m_properties) {
            propValue.Set(prop);
        }
        m_meshRenderer.SetPropertyBlock(prop);
    }

    bool Check(List<IProperty> m_properties) {
        bool check = !m_meshRenderer && m_properties.Count != 0;
        return check;
    }
}
