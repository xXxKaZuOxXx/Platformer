using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class SetFollowComponent : MonoBehaviour
{
    private void Start()
    {
        var vCamera = GetComponent<CinemachineVirtualCamera>();
        vCamera.Follow = FindObjectOfType<Hero>().transform;
    }

}
