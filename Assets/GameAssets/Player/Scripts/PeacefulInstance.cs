using System;
using GameAssets.Enemy.Scripts;
using UnityEngine;

namespace GameAssets.Player.Scripts
{
    public class PeacefulInstance : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _audioSource;
        public event Action EnemyCollided;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out RocketEnemy enemy))
            {
                if (enemy.IsDowned == false)
                {
                    _audioSource.Play();
                    _animator.Play("Lose0");
                    enemy.OnCollided();
                    EnemyCollided?.Invoke();
                }
            }
        }
    }
}
