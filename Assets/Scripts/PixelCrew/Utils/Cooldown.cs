﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Utils
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float _value;
        
        public float Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private float _timesUp;
        public void Reset()
        {
            _timesUp = Time.time + _value;
        }

        public float RemainingTime => Mathf.Max(_timesUp - Time.time, 0);
        public bool IsReady => _timesUp <= Time.time;
    }
}
