using UnityEngine;

namespace GameAssets.Player.Scripts
{
    public class RocketInstance : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider2D;

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
            
        }
    }
}
