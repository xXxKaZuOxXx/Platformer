using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;
        private PlayerData _save;
        [SerializeField] private string _defaultCheckPoint;

        public QuickInventoryModel QuickInventory { get; private set; }
        public PerksModel PerksModel { get; private set; }

        public StatsModel StatsModel { get; private set; }

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        
        private List<string> _checkpoints = new List<string>();
        public void Save()
        {
            _save = _data.Clone();
        }

        internal void LoadLastSave()
        {
            _data = _save.Clone();
            _trash.Dispose();
           
           
            InitModels();
        }

        private void Awake()
        {

            
            var existsSession = GetExistsSession();
            if(existsSession != null)
            {
                existsSession.StartSession(_defaultCheckPoint);
                DestroyImmediate(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
                StartSession(_defaultCheckPoint);
            }
        }

        private void StartSession(string defaultCheckPoint)
        {
            SetChecked(defaultCheckPoint);
            LoadHud();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkpoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckPoint = _checkpoints.Last();
            foreach (var checkpoint in checkpoints)
            {
                if(checkpoint.Id == lastCheckPoint)
                {
                    checkpoint.SpawnHero();
                    break;
                }
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(Data);
            _trash.Retain(QuickInventory);

            PerksModel = new PerksModel(Data);
            _trash.Retain(PerksModel);
            
            StatsModel = new StatsModel(Data);
            _trash.Retain(StatsModel);

            _data.Hp.Value = (int)StatsModel.GetValue(StatId.Hp);
        }

        private void LoadHud()
        {
           SceneManager.LoadScene("HUD",LoadSceneMode.Additive);
        }

        private GameSession GetExistsSession()
        {
           var sessions = FindObjectsOfType<GameSession>();
            foreach (var session in sessions)
            {
                if (session != this)
                {
                    return session; 
                }
            }
            return null;
        }
        private void OnDestroy()
        {
            _trash.Dispose();
        }

        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id);
        }

        internal void SetChecked(string id)
        {
            if(!_checkpoints.Contains(id))
            {
                Save();
                _checkpoints.Add(id);
            }
               
        }
        public List<string> _removedItems = new List<string>();
        public bool RestoreState(string itemId)
        {
            return _removedItems.Contains(itemId);
            
        }

        public void StoreState(string itemId)
        {
            if(!_removedItems.Contains(itemId))
                _removedItems.Add(itemId);
           
        }
    }
    
}

