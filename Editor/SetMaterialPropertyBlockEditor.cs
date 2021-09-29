using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetMaterialPropertyBlock))]
class TestBehaviourInspector : Editor
{
    private Type[] _implementations;
    private int _implementationTypeIndex;
    private SerializedProperty m_properties;

    private void OnEnable() {
        m_properties = serializedObject.FindProperty("m_properties");
    }

    public override void OnInspectorGUI() {
        SetMaterialPropertyBlock target = base.target as SetMaterialPropertyBlock;
        //specify type
        if(target == null) {
            return;
        }

        if(_implementations == null) {
            //this is probably the most imporant part:
            //find all implementations of INode using System.Reflection.Module
            _implementations = GetImplementations<IProperty>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }

        GUILayout.BeginVertical("HelpBox");
        GUILayout.BeginHorizontal();

        //select implementation from editor popup
        _implementationTypeIndex = EditorGUILayout.Popup(new GUIContent("Property Type"),
            _implementationTypeIndex, _implementations.Select(impl => impl.FullName).ToArray());

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if(GUILayout.Button("Create")) {
            //set new value
            target.m_properties.Add((IProperty)Activator.CreateInstance(_implementations[_implementationTypeIndex]));
        }

        if(GUILayout.Button("Refresh")) {
            //this is probably the most imporant part:
            //find all implementations of INode using System.Reflection.Module
            _implementations = GetImplementations<IProperty>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        EditorGUILayout.PropertyField(m_properties, new GUIContent(m_properties.displayName));
    }

    private static Type[] GetImplementations<T>() {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());

        var interfaceType = typeof(T);
        return types.Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();
    }
}