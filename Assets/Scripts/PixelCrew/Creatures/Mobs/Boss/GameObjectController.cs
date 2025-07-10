using PixelCrew;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectController : MonoBehaviour
{
    [SerializeField] private GameObject[] _leftAndRightAttacks;
    public void Activate()
    {
        foreach (var go in _leftAndRightAttacks)
        {
            go.SetActive(true);
            go.GetComponent<CheckCircleOverlap>().Check();
        }
    }
    public void Deactivate()
    {
        foreach (var go in _leftAndRightAttacks)
        {
            go.SetActive(false);
        }
    }
}
