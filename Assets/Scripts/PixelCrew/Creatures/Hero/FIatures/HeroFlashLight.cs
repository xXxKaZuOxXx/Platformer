using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFlashLight : MonoBehaviour
{
    [SerializeField] private float _consumePerSecond;
    private GameSession _session;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
    }
    private void Update()
    {
        var consumed = Time.deltaTime* _consumePerSecond;
        var currentValue =_session.Data.Fuel.Value;
        var nextValue = currentValue - consumed;
        nextValue = Mathf.Max(nextValue, 0);
        _session.Data.Fuel.Value = nextValue;
    }
}
