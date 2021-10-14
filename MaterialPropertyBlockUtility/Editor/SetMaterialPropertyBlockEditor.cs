using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(SetMaterialPropertyBlock))]
class SetMaterialPropertyBlockEditor : Editor
{
    private Type[] _implementations;
    private int _implementationTypeIndex;

    private SerializedProperty m_renderer;
    private SerializedProperty m_overrideSettings;
    private SerializedProperty m_properties;
    private SerializedProperty m_ExecuteInEditor;

    private void OnEnable() {
        m_renderer = serializedObject.FindProperty("m_renderer");
        m_overrideSettings = serializedObject.FindProperty("m_overrideSettings");
        m_properties = serializedObject.FindProperty("m_properties");
        m_ExecuteInEditor = serializedObject.FindProperty("ExecuteInEditor");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        SetMaterialPropertyBlock target = base.target as SetMaterialPropertyBlock;
        //specify type
        if(target == null) {
            return;
        }
        
        bool isOverrideSetting = m_overrideSettings.objectReferenceValue != null;

        GUILayout.BeginHorizontal("Box");

        EditorGUILayout.PropertyField(m_ExecuteInEditor);

        if(!m_ExecuteInEditor.boolValue) GUI.enabled = false;
        if(GUILayout.Button("Force Update", GUILayout.Width(90))) {
            for(int i = 0; i < targets.Length; i++) {
                var tempTarget = targets[i] as SetMaterialPropertyBlock;
                tempTarget.OnValidate();
            }
        }
        if(!m_ExecuteInEditor.boolValue) GUI.enabled = true;

        if(GUILayout.Button("Clear Data", GUILayout.Width(80))) {
            for(int i = 0; i < targets.Length; i++) {
                var tempTarget = targets[i] as SetMaterialPropertyBlock;
                tempTarget.ClearBlockData();
            }
        }

        GUILayout.EndHorizontal();

        bool haveRenderer = m_renderer.objectReferenceValue == null;

        if(haveRenderer) {
            EditorGUILayout.HelpBox("Require Renderer Component", MessageType.Error);
        }

        GUI.enabled = haveRenderer;

        EditorGUILayout.ObjectField(m_renderer);

        GUI.enabled = true;

        if(_implementations == null) {
            //this is probably the most imporant part:
            //find all implementations of INode using System.Reflection.Module
            _implementations = GetImplementations<IProperty>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).OrderBy(c => c.Name).ToArray();
        }

        GUIStyle Header = new GUIStyle(EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Properties Setting", Header);

        EditorGUI.indentLevel += 1;

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(m_overrideSettings, new GUIContent("Override Setting Scriptable Object"));
        if(GUILayout.Button("Close", GUILayout.Width(60))) {
            for(int i = 0; i < targets.Length; i++) {
                var tempTarget = targets[i] as SetMaterialPropertyBlock;
                tempTarget.OverrideSettings = null;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        
        //select implementation from editor popup
        _implementationTypeIndex = EditorGUILayout.Popup(new GUIContent("Add List type"),
            _implementationTypeIndex, _implementations.Select(impl => impl.FullName).ToArray());

        if(GUILayout.Button("Create",GUILayout.Width(60))) {
            //set new value
            var prop = (IProperty)Activator.CreateInstance(_implementations[_implementationTypeIndex]);
            if(!isOverrideSetting) {
                for(int i = 0; i < targets.Length; i++) {
                    var tempTarget = targets[i] as SetMaterialPropertyBlock;
                    tempTarget.CurrentSettings.Add(prop);
                }
            } else {
                target.CurrentSettings.Add(prop);
            }
        }

        GUILayout.EndHorizontal();

        GUIStyle SOArea = new GUIStyle(EditorStyles.helpBox);
        int padding = 10;
        SOArea.padding = new RectOffset(padding, padding, padding, padding);
        var origin = GUI.color;

        if(isOverrideSetting) GUI.color = origin + new Color(0.4f, 0.1f, 0.1f);
        
        GUILayout.BeginVertical(SOArea);

        if(isOverrideSetting) {
            Editor _propertyScriptableObjectEditor = null;
            CreateCachedEditor(this.m_overrideSettings.objectReferenceValue, null, ref _propertyScriptableObjectEditor);
            if(_propertyScriptableObjectEditor) {
                _propertyScriptableObjectEditor.OnInspectorGUI();
            }
        } else {
            EditorGUILayout.PropertyField(m_properties, new GUIContent(m_properties.displayName));
        }

        EditorGUI.indentLevel -= 1;

        GUILayout.EndVertical();

        if(isOverrideSetting) GUI.color = origin;

        //if(GUILayout.Button("Refresh List")) {
        //    //this is probably the most imporant part:
        //    //find all implementations of INode using System.Reflection.Module
        //    _implementations = GetImplementations<IProperty>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        //}

        RefreshListName(target.CurrentSettings);

        serializedObject.ApplyModifiedProperties();
    }

    void RefreshListName(List<IProperty> currentSettings) {
        if(currentSettings == null || currentSettings.Count == 0) return;
        foreach(var item in currentSettings) {
            if(item == null) continue;
            item.InspectorName = $"{item.ReferenceName} ({item.GetType()})";
        }
    }

    private static Type[] GetImplementations<T>() {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());

        var interfaceType = typeof(T);
        return types.Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();
    }
}