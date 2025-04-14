using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using log4net.Util;

[CustomEditor(typeof(CircleMovement))]
public class CoinsScriptEditor : Editor
{
    private CircleMovement _circleMovement;
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        _circleMovement = target as CircleMovement;
        _circleMovement.Radius = EditorGUILayout.FloatField("Radius", _circleMovement.Radius);
        _circleMovement.Speed = EditorGUILayout.FloatField("Speed", _circleMovement.Speed);
       
        if (_circleMovement.Radius == 0) return;
        else
        {
            _circleMovement.ArrangeChildren();
        }
        
    }
   
    private void OnSceneGUI()
    {
        Handles.DrawWireArc(_circleMovement.transform.position, Vector3.forward, Vector3.up, 360f, _circleMovement.Radius);
    }
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(_circleMovement.transform.position, Vector3.up, _circleMovement.Radius);
    }

}
