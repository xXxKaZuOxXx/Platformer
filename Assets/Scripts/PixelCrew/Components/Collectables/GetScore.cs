using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GetScore : MonoBehaviour
{
    
    [SerializeField] private string _tag;
    [SerializeField] private int _value;
    private Hero _hero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _hero = collision.gameObject.GetComponent<Hero>();
        }
    }

    public void Score()
    {
        _hero.Score += _value;
        Debug.Log(_hero.Score);
    }
    public void GetSword()
    {
        _hero.NumberOfSwords += _value;
    }
    
   
}
