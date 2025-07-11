﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuWindow : AnimatedWindow
{
    private Action _closeAction;
    public void OnShowSettings()
    {
        WindowUtils.CreateWindow("UI/SettingsWindow");
    }
    public void OnLanguages()
    {
        WindowUtils.CreateWindow("UI/LocalisationWindow");
    }
    public void OnStartGame()
    {
        _closeAction = () => { 
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel("Level1");
            
        };
        Close();
    }

    public void OnExit()
    {
        _closeAction = () =>
        {
            

            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        };
       Close();
        
    }

    public override void OnCloseAnimationComplete()
    {
        base.OnCloseAnimationComplete();
        _closeAction?.Invoke();
    }
}
