﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class Switch : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _state; 
        [SerializeField] private string _animationKey;
        [SerializeField] private bool _updateOnStart;

        private void Start()
        {
            if(_updateOnStart)
                _animator.SetBool(_animationKey, _state);
        }
        public void SwitchState()
        {
            _state = !_state;
            _animator.SetBool(_animationKey, _state);
        }
        [ContextMenu("SwitchState")]
        public void SwitchIt()
        {
            SwitchState();
        }
    }
}

