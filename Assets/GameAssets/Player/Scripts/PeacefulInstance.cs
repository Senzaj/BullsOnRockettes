using System;
using GameAssets.Enemy.Scripts;
using UnityEngine;

namespace GameAssets.Player.Scripts
{
    public class PeacefulInstance : MonoBehaviour
    {
        public event Action EnemyCollided;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out RocketEnemy enemy))
            {
                //
                enemy.OnCollided();
                EnemyCollided?.Invoke();
            }
        }
    }
}
