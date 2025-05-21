using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "Defs/LocalDef", fileName = "LocalDef")]
public class LocalDef : ScriptableObject
{
    //en
    //https://docs.google.com/spreadsheets/d/e/2PACX-1vR1eTtFUMrm44N-OJdRliKWK7erdqQxH85IyLQQPpXwECsci9ANKPsdm-EsX0Z3WEdK4mEGOL1T6n97/pub?gid=0&single=true&output=tsv
    //ru
    //https://docs.google.com/spreadsheets/d/e/2PACX-1vR1eTtFUMrm44N-OJdRliKWK7erdqQxH85IyLQQPpXwECsci9ANKPsdm-EsX0Z3WEdK4mEGOL1T6n97/pub?gid=1053209026&single=true&output=tsv

    [SerializeField] private string _url;
    [SerializeField] private List<LocalItem> _localItems;

    private UnityWebRequest _request;

    public Dictionary<string, string> GetData()
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var item in _localItems)
        {
            dictionary.Add(item.Key, item.Value);
        }
        return dictionary;
    }
    [ContextMenu("Update local")]
    public void LoadLocal()
    {
        if (_request != null) return;

        _request = UnityWebRequest.Get(_url);
        _request.SendWebRequest().completed += OnDataLoaded;
    }

    private void OnDataLoaded(AsyncOperation operation)
    {
        if (operation.isDone)
        {
            var rows = _request.downloadHandler.text.Split('\n');
            _localItems.Clear();
            foreach (var row in rows)
            {
                AddLocalItem(row);
            }
          

        }
    }

    private void AddLocalItem(string row)
    {
        try
        {
            var parts = row.Split('\t');
            _localItems.Add(new LocalItem { Key = parts[0], Value = parts[1] });

        }
        catch (Exception ex)
        {
            Debug.LogError($"Can't parse row: {row} \n {ex}");
        }
    }

    [Serializable]
    private class LocalItem
    {
        public string Key;
        public string Value;
    }

}
