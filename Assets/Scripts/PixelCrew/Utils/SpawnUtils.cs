using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUtils : MonoBehaviour
{
    private const string ContainerName = "###SPAWNED###";

    public static GameObject Spawn(GameObject prefab, Vector3 position)
    {
        var container = GameObject.Find(ContainerName);
        if (container == null)
            container = new GameObject(ContainerName);
        return GameObject.Instantiate(prefab, position, Quaternion.identity, container.transform);
    }
}
