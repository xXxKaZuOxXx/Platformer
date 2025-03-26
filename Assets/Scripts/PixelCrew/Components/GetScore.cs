using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GetScore : MonoBehaviour
{
    private Hero _hero;
    [SerializeField] private string _tag;
    [SerializeField] private int _value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Hero>().Score += _value;
            
            Debug.Log(collision.gameObject.GetComponent<Hero>().Score);
            LogScore();
        }
    }
    
    private void Score()
    {

       // _hero.Score += _value;
    }
    public void LogScore()
    {
        
        
    }
   
}
