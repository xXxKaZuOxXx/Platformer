using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;
        private PlayerData _save;

        public void Save()
        {
            _save = _data.Clone();
        }

        internal void LoadLastSave()
        {
           _data = _save.Clone();
        }

        private void Awake()
        {
            LoadHud();
            if(IsSessionExist())
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Save();
                DontDestroyOnLoad(this);
            }
        }

        private void LoadHud()
        {
           SceneManager.LoadScene("HUD",LoadSceneMode.Additive);
        }

        private bool IsSessionExist()
        {
           var sessions = FindObjectsOfType<GameSession>();
            foreach (var session in sessions)
            {
                if (session != this)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

