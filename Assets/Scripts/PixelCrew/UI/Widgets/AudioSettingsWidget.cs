﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsWidget : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _value;

    private FloatPersistentProperty _model;

    private void Start()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        _model.Value = value;
    }

    public void SetModel(FloatPersistentProperty model)
    {
        _model = model;
        model.OnChanged += OnValueChanged;
        OnValueChanged(model.Value, model.Value);
    }

    private void OnValueChanged(float newValue, float oldValue)
    {
        var textvalue = Mathf.Round(newValue * 100);
        _value.text  = textvalue.ToString();
        _slider.normalizedValue = newValue;

    }
    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        _model.OnChanged -= OnValueChanged;
    }
}
