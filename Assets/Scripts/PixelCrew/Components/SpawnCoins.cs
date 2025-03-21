using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnCoins : MonoBehaviour
    {
        [SerializeField] private int _quantity;
        [SerializeField] private int _percentageOfGold;
        [SerializeField] private GameObject _silverCoinWitchRigidBody;
        [SerializeField] private GameObject _goldCoinWitchRigidBody;
        [SerializeField] private float _velocity;
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _silverCoinPrefab;
        [SerializeField] private GameObject _goldCoinPrefab;
        [SerializeField]private SwitchPrefab _switchPrefab;

        [ContextMenu("Spawn")]
        public void Start()
        {
            StartCoroutine(Logic());

        }
        public IEnumerator Logic()
        {
            List<GameObject> silverCoins = new List<GameObject>();
            List<GameObject> goldCoins = new List<GameObject>();
            //var trajectory = UnityEngine.Random.insideUnitCircle * _velocity;
            //_target.transform.position += new Vector3(trajectory.x * Time.deltaTime, trajectory.y * Time.deltaTime, _target.transform.position.z);
            for (int i = 0; i < _quantity; i++)
            {
                if (Random.Range(0, 100) > _percentageOfGold)
                {
                    var instantiate = Instantiate(_silverCoinWitchRigidBody, _target.position, Quaternion.identity);
                    silverCoins.Add(instantiate);
                }
                else
                {
                    var instantiate = Instantiate(_goldCoinWitchRigidBody, _target.position, Quaternion.identity);
                    goldCoins.Add(instantiate);
                }

            }
            foreach (var item in goldCoins)
            {
                var trajectoryY = UnityEngine.Random.Range(0.4f, 1f);
                var trajectoryX = UnityEngine.Random.Range(-0.5f, 0.5f);
                item.GetComponent<Rigidbody2D>().velocity += new Vector2(trajectoryX, trajectoryY) * _velocity;
            }
            foreach (var item in silverCoins)
            {
                var trajectoryY = UnityEngine.Random.Range(0.4f, 1f);
                var trajectoryX = UnityEngine.Random.Range(-0.5f, 0.5f);
                item.GetComponent<Rigidbody2D>().velocity += new Vector2(trajectoryX, trajectoryY) * _velocity;
            }
            var goldCoinsArray = goldCoins.ToArray();
            var silverCoinsArray = silverCoins.ToArray();
            _switchPrefab = GetComponent<SwitchPrefab>();

            yield return new WaitForSeconds(1);
              int a = 5;
            if (goldCoinsArray.Length > 0)
                _switchPrefab.SwapAllByArray(_goldCoinPrefab, goldCoinsArray);
            if( silverCoinsArray.Length > 0)
                _switchPrefab.SwapAllByArray(_silverCoinPrefab, silverCoinsArray);

            yield break;

        }
    }

}
