using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GetScore : MonoBehaviour
{
    [SerializeField] private Hero _hero;
    [SerializeField] private int _value;
    private void Score()
    {
        _hero.Score += _value;
    }
    public void LogScore()
    {
        Score();
        Debug.Log(_hero.Score);
    }
   
}
