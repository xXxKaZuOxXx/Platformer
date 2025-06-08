using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelCrew.Components
{
    public class DestroyObject : MonoBehaviour
    {
        [SerializeField] private GameObject _objToDestroy;
        [SerializeField] private RestoreStateComponent _state;
        public void DestroyObj()
        {
            Destroy(_objToDestroy);
            if (_state != null)
                FindObjectOfType<GameSession>().StoreState(_state.Id);
        }
    }

}

