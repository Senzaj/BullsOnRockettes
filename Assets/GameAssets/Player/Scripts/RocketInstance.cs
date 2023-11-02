using System;
using GameAssets.Common.Scripts;
using GameAssets.Enemy.Scripts;
using UnityEngine;

namespace GameAssets.Player.Scripts
{
    public class RocketInstance : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private Body _bull;
        [SerializeField] private Transform _bullPosition;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ParticleSystem _work;
        [SerializeField] private ParticleSystem _explosion;
        
        public event Action<RocketInstance> EnemyCollided;
        public event Action AmmoCollided;
        public event Action<RocketInstance> Skipped;

        private bool _isCollidedEnemy;

        private void OnEnable()
        {
            _work.Play();
            _bull.MakeKinematic();
            _bull.transform.position = _bullPosition.position;
            _isCollidedEnemy = false;
            _spriteRenderer.enabled = true;
        }

        private void OnDisable()
        {
            Skipped?.Invoke(this);
        }

        public void DisableCollider()
        {
            _collider2D.enabled = false;
        }

        public void EnableCollider()
        {
            _collider2D.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out RocketAmmo ammo))
            {
                ammo.OnCollided();
                ammo.gameObject.SetActive(false);
                AmmoCollided?.Invoke();
            }

            if (other.TryGetComponent(out RocketEnemy enemy))
            {
                if (!_isCollidedEnemy)
                {
                    _work.Stop();
                    _isCollidedEnemy = true;
                    _bull.makeDynamic();
                    Instantiate(_explosion, transform.position, Quaternion.identity);
                    enemy.OnCollided();
                    _spriteRenderer.enabled = false;
                    EnemyCollided?.Invoke(this);
                }
            }
        }
    }
}
