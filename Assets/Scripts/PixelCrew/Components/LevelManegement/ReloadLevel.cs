﻿using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace PixelCrew.Components
{
    public class ReloadLevel : MonoBehaviour
    {
       
      
        public void Reload()
        {
            var session = FindObjectOfType<GameSession>();
            
            session.LoadLastSave();
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            //session = FindObjectOfType<GameSession>();
            //session = _session;

        }
    }

}

