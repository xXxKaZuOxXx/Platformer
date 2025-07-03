using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.Experimental.Rendering.Universal;

public class HeroFlashLight : MonoBehaviour
{
    [SerializeField] private float _consumePerSecond;
    [SerializeField] private UnityEngine.Experimental.Rendering.Universal.Light2D _light;

    private GameSession _session;
    private float _defaultIntensivity;
    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        _defaultIntensivity = _light.intensity;
    }
    private void Update()
    {
        var consumed = Time.deltaTime* _consumePerSecond;
        var currentValue =_session.Data.Fuel.Value;
        var nextValue = currentValue - consumed;
        nextValue = Mathf.Max(nextValue, 0);
        _session.Data.Fuel.Value = nextValue;

        var progress = Mathf.Min(nextValue/20, 1);
        _light.intensity = _defaultIntensivity * progress;

    }
}
