using System;
using GameAssets.Common.Scripts;
using TMPro;
using UnityEngine;

namespace GameAssets.Player.Scripts
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private ObjectPool _playerRocketsPool;
        [SerializeField] private Rigidbody2D _shootPointRigidbody2D;
        [SerializeField] private Vector2 _startPos;
        [SerializeField] private PeacefulInstance[] _peacefulInstances;

        [SerializeField] private TMP_Text _rocketsTMP;
        
        public event Action PeacefulCollided;
        public event Action EnemyCollided;

        private const int _startCount = 5;
        
        private RocketMissile _currentMissile;
        private int _count;

        private void OnEnable()
        {
            _playerRocketsPool.InstantiateStartCount();
            _count = _startCount;
            _rocketsTMP.text = _count.ToString();

            TryPrepareNextMissile();

            foreach (PeacefulInstance peaceful in _peacefulInstances)
                peaceful.EnemyCollided += OnEnemyCollidedPeaceful;
        }

        private void OnDisable()
        {
            foreach (PeacefulInstance peaceful in _peacefulInstances)
                peaceful.EnemyCollided -= OnEnemyCollidedPeaceful;
        }

        public void CurrentMissileIsntReady()
        {
            if (_currentMissile != null)
                _currentMissile.IsntReady();
        }

        public void CurrentMissileReady()
        {
            if (_currentMissile != null)
                _currentMissile.Ready();
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
            _count +=2;
            _rocketsTMP.text = _count.ToString();
            
            if (_currentMissile == null)
                TryPrepareNextMissile();
        }

        private void SkipRocket(RocketInstance instance)
        {
            instance.EnemyCollided -= OnEnemyCollidedRocket;
            instance.AmmoCollided -= OnAmmoCollidedRocket;
            instance.Skipped -= SkipRocket;
        }

        private void OnLaunched()
        {
            if (_currentMissile != null)
            {
                _currentMissile.Launched -= OnLaunched;
                _currentMissile = null;
            }
            
            TryPrepareNextMissile();
        }
        
        private void TryPrepareNextMissile()
        {
            if (_count > 0)
            {
                RocketMissile newMissile = _playerRocketsPool.Get().GetComponent<RocketMissile>();
                _currentMissile = newMissile;
                newMissile.gameObject.SetActive(true);
                newMissile.enabled = true;
                newMissile.SetShootPoint(_shootPointRigidbody2D, _startPos);
                newMissile.Launched += OnLaunched;
                newMissile.Instance.EnemyCollided += OnEnemyCollidedRocket;
                newMissile.Instance.AmmoCollided += OnAmmoCollidedRocket;
                newMissile.Instance.Skipped += SkipRocket;
                _count--;
                _rocketsTMP.text = _count.ToString();
            }
        }
    }
}
