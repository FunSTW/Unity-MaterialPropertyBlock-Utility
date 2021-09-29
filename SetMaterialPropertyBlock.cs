/* Reference https://medium.com/@trepala.aleksander/serializereference-in-unity-b4ee10274f48 */

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SetMaterialPropertyBlock : MonoBehaviour
{
    MeshRenderer m_meshRenderer = null;
    [SerializeReference]
    public List<IProperty> m_properties = new List<IProperty>();

    private void Start() {
        m_meshRenderer = GetComponent<MeshRenderer>(); 
        Apply();
    }

    void Apply() {
        if(Check()) { return; }
        var prop = new MaterialPropertyBlock();
        m_meshRenderer.GetPropertyBlock(prop);
        foreach(var propValue in m_properties) {
            propValue.Set(prop);
        }
        m_meshRenderer.SetPropertyBlock(prop);
    }

    bool Check() {
        bool check = !m_meshRenderer && m_properties.Count != 0;
        return check;
    }
}
