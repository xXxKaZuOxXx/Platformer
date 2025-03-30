using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class DoInteraction : MonoBehaviour
    {
        public void DoInteract(GameObject go)
        {
            var interactable = go.GetComponent<Interactive>();
            if (interactable != null)
            {
                interactable.Interact();
            }

        }
    }
}

