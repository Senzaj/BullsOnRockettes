using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameAssets.Common.Scripts
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _startCount;
        
        private List<GameObject> _spawned;

        public void InstantiateStartCount()
        {
            _spawned = new List<GameObject>();

            for (var i = 0; i < _startCount; i++)
                Instantiate();
        }

        public GameObject Get()
        {
            GameObject spawned = _spawned.FirstOrDefault(obj => obj.gameObject.activeSelf == false);
            return spawned == null ? Instantiate() : spawned;
        }

        private GameObject Instantiate()
        {
            GameObject instantiated = Instantiate(_prefab, transform);
            instantiated.gameObject.SetActive(false);
            _spawned.Add(instantiated);
            return instantiated;
        }
    }
}
