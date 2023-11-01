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

        public event Action PeacefulCollided;
        public event Action EnemyCollided;

        private const int _startCount = 5;
        
        private RocketMissile _currentMissile;
        private int _count;

        private void OnEnable()
        {
            _playerRocketsPool.InstantiateStartCount();
            _count = _startCount;
            
            TryPrepareNextMissile();
            //unsub
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
                _count--;
            }
        }
    }
}
