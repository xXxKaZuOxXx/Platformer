using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(CustomButton), true)]
[CanEditMultipleObjects]
public class CustomButtonEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_normal"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_select"));
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}
