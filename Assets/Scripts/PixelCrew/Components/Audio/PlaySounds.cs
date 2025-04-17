using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    [SerializeField] private AudioSource _sourse;
    [SerializeField] private AudioData[] _sounds;

    public void Play(string id)
    {
        foreach (var item in _sounds)
        {
            if(item.Id != id) continue;
            
            _sourse.PlayOneShot(item.Clip);
            Debug.Log(id);
            break;
            
        }
    }

    [Serializable]
    public class AudioData
    {
        [SerializeField] private string _id;
        [SerializeField] private AudioClip _clip;

        public string Id => _id;
        public AudioClip Clip => _clip;
    }
}
