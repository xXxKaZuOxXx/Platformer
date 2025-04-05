using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CircleMovement : MonoBehaviour
{
    public float Radius = 1f;
    public float Speed = 1f;
    private float _globalAngle = 0f;
    private Transform[] allChildren;
    private List<Transform> childObjects = new List<Transform>();
    private int prevChildCount;

    private void Awake()
    {
        allChildren = GetComponentsInChildren<Transform>();
        for (int i = 1; i < allChildren.Length; i++)
        {
            childObjects.Add(allChildren[i]);
        }
        
    }

    private void Update()
    {
        if (!Application.isPlaying && transform.childCount != prevChildCount)
        {
            ArrangeChildren();
            prevChildCount = transform.childCount;
        }
        _globalAngle = Time.time * Speed;
        float angleStep = 360f / childObjects.Count;

        for (int i = 0; i < childObjects.Count; i++)
        {
            if(childObjects[i] != null)
            {
                Transform child = childObjects[i];
                float angle = (angleStep * i + _globalAngle) * Mathf.Deg2Rad;
                var x = Mathf.Cos(angle) * Radius;
                var y = Mathf.Sin(angle) * Radius;
                child.localPosition = new Vector3(x, y, 0);
            }
            
            
        }
    }

    public void ArrangeChildren()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        float angleStep = 360f / childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            float angle = angleStep * i * Mathf.Deg2Rad;

            float x = Mathf.Cos(angle) * Radius;
            float y = Mathf.Sin(angle) * Radius;
            child.localPosition = new Vector3(x, y, 0);
        }
    }

}
