using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WindowUtils
{
    public static void CreateWindow(string resourcePath)
    {
        var window = Resources.Load<GameObject>(resourcePath);
        var canvas = Object.FindObjectOfType<Canvas>();
        var can = GameObject.FindGameObjectWithTag("OverlayCanvas");
        Object.Instantiate(window, can.transform);
    }
}
