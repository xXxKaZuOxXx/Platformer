using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    
    private AudioSource _source;
    [SerializeField] private AudioData[] _sounds;

    public void Play(string id)
    {
        foreach (var item in _sounds)
        {
            if(item.Id != id) continue;

            if (_source == null)
                _source = AudioUtils.FindSfxSource();

            _source.PlayOneShot(item.Clip);
            
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
