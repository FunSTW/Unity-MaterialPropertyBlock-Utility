using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SetMaterialPropertyBlock : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] bool ExecuteInEditor = false;
#endif

    [SerializeField] Renderer m_renderer = null;
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
    public void OnValidate() {
        GetRequired();
        if(ExecuteInEditor) {
            ApplyCurrentSetting();
        }
    }
#endif

    void Start() {
        GetRequired();
        ApplyCurrentSetting();
    }

    void GetRequired() {
        if(!m_renderer) m_renderer = GetComponent<Renderer>();
    }

    public void ApplyCurrentSetting() {
        List<IProperty> properties = CurrentSettings;

        if(m_renderer == null || properties.Count == 0) return;

        var prop = new MaterialPropertyBlock();
        m_renderer.GetPropertyBlock(prop);
        foreach(var propValue in properties) {
            propValue.Set(prop);
        }
        m_renderer.SetPropertyBlock(prop);
    }

    public void ClearBlockData() {
        Renderer renderer = m_renderer;

        if(renderer == null) { return; }
        var prop = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(prop);
        prop.Clear();
        renderer.SetPropertyBlock(prop);
    }
}
