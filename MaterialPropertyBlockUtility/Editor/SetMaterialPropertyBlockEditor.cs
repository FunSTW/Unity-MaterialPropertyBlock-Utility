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

    private SerializedProperty overrideSettings;
    private SerializedProperty m_properties;

    private void OnEnable() {
        overrideSettings = serializedObject.FindProperty("m_overrideSettings");
        m_properties = serializedObject.FindProperty("m_properties");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        SetMaterialPropertyBlock target = base.target as SetMaterialPropertyBlock;
        //specify type
        if(target == null) {
            return;
        }
        bool isOverrideSetting = overrideSettings.objectReferenceValue != null;

        if(_implementations == null) {
            //this is probably the most imporant part:
            //find all implementations of INode using System.Reflection.Module
            _implementations = GetImplementations<IProperty>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }

        GUIStyle Header = new GUIStyle(EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Setting", Header);

        EditorGUI.indentLevel += 1;

        GUILayout.BeginHorizontal();
        
        //select implementation from editor popup
        _implementationTypeIndex = EditorGUILayout.Popup(new GUIContent("Add List type"),
            _implementationTypeIndex, _implementations.Select(impl => impl.FullName).ToArray());

        if(GUILayout.Button("Create",GUILayout.Width(60))) {
            //set new value
            var prop = (IProperty)Activator.CreateInstance(_implementations[_implementationTypeIndex]);
            prop.InspectorName = $"{prop.ReferenceName} ({prop.GetType()})";
            target.GetCurrentSetting.Add(prop);
        }

        GUILayout.EndHorizontal();

        EditorGUI.indentLevel -= 1;

        EditorGUILayout.LabelField("Property", Header);

        /*-------------------------*/

        EditorGUI.indentLevel += 1;

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(overrideSettings, new GUIContent("Override Setting Scriptable Object"));
        if(GUILayout.Button("Close",GUILayout.Width(50))) {
            target.OverrideSettings = null;
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
            CreateCachedEditor(this.overrideSettings.objectReferenceValue, null, ref _propertyScriptableObjectEditor);
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

        serializedObject.ApplyModifiedProperties();
    }

    private static Type[] GetImplementations<T>() {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());

        var interfaceType = typeof(T);
        return types.Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();
    }
}