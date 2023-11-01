using System;
using UnityEngine;

namespace GameAssets.Player.Scripts
{
    public class PeacefulInstance : MonoBehaviour
    {
        public event Action<RocketInstance> EnemyCollided;

        private void OnTriggerEnter2D(Collider2D other)
        {
            
        }
    }
}
