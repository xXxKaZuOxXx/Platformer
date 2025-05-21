using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class DialogData
{
    [SerializeField] public string[] _sentences;
    [SerializeField] public string[] _localisationKeys;

    public string[] Sentences => _sentences;
    
    //сделать скрипт с локализацие текста из кода получать компонент для bounds/sentences по ключу
    //смотреть если 0 ключ существет то заменяем 0 sentence. Для external делаем также
    


}
