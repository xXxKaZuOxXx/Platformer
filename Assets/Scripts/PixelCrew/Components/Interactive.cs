﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class Interactive : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;
        public void Interact()
        {
            _action?.Invoke();

        }
    }

}

