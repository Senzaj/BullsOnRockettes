using System;
using GameAssets.Common.Scripts;
using UnityEngine;

namespace GameAssets.Player.Scripts
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private ObjectPool _playerRocketsPool;
        [SerializeField] private Rigidbody2D _shootPointRigidbody2D;
        [SerializeField] private Vector2 _startPos;
        [SerializeField] private PeacefulInstance[] _peacefulInstances;

        public event Action PeacefulCollided;
        public event Action EnemyCollided;

        private const int _startCount = 5;
        
        private RocketMissile _currentMissile;
        private int _count;

        private void OnEnable()
        {
            _playerRocketsPool.InstantiateStartCount();
            _count = _startCount; //show

            TryPrepareNextMissile();

            foreach (PeacefulInstance peaceful in _peacefulInstances)
                peaceful.EnemyCollided += OnEnemyCollidedPeaceful;
        }

        private void OnDisable()
        {
            foreach (PeacefulInstance peaceful in _peacefulInstances)
                peaceful.EnemyCollided -= OnEnemyCollidedPeaceful;
        }

        private void OnEnemyCollidedPeaceful()
        {
            PeacefulCollided?.Invoke();
        }

        private void OnEnemyCollidedRocket(RocketInstance rocketInstance)
        {
            SkipRocket(rocketInstance);
            EnemyCollided?.Invoke();
        }

        private void OnAmmoCollidedRocket()
        {
            _count +=2; //show
        }

        private void SkipRocket(RocketInstance instance)
        {
            instance.EnemyCollided -= OnEnemyCollidedRocket;
            instance.AmmoCollided -= OnAmmoCollidedRocket;
            instance.Skipped -= SkipRocket;
        }
        
        private void TryPrepareNextMissile()
        {
            if (_currentMissile != null)
                _currentMissile.Launched -= TryPrepareNextMissile;
            
            if (_count > 0)
            {
                RocketMissile newMissile = _playerRocketsPool.Get().GetComponent<RocketMissile>();
                _currentMissile = newMissile;
                newMissile.gameObject.SetActive(true);
                newMissile.enabled = true;
                newMissile.SetShootPoint(_shootPointRigidbody2D, _startPos);
                newMissile.Launched += TryPrepareNextMissile;
                newMissile.Instance.EnemyCollided += OnEnemyCollidedRocket;
                newMissile.Instance.AmmoCollided += OnAmmoCollidedRocket;
                newMissile.Instance.Skipped += SkipRocket;
                _count--;
            }
        }
    }
}
