using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteAlways]
public class SetMaterialPropertyBlock : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] bool ExecuteInEditor = false;
#endif

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

    public List<IProperty> CurrentSettings => m_overrideSettings ? m_overrideSettings.m_properties : m_properties;

#if UNITY_EDITOR
    private void OnValidate() {
        if(!m_meshRenderer) m_meshRenderer = GetComponent<MeshRenderer>();
        if(ExecuteInEditor) { 
            ApplyCurrentSetting();
        }
    }
#endif

    void Start() {
        m_meshRenderer = GetComponent<MeshRenderer>();
        ApplyCurrentSetting();
    }

    public void ApplyCurrentSetting() {
        Apply(CurrentSettings,m_meshRenderer);
    }

    static void Apply(List<IProperty> properties,MeshRenderer meshRenderer) {
        if(!Check(properties, meshRenderer)) { return; }
        var prop = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(prop);
        foreach(var propValue in properties) {
            propValue.Set(prop);
        }
        meshRenderer.SetPropertyBlock(prop);
    }

    static bool Check(List<IProperty> m_properties, MeshRenderer m_meshRenderer) {
        bool check = m_meshRenderer != null && m_properties.Count != 0;
        return check;
    }
}
