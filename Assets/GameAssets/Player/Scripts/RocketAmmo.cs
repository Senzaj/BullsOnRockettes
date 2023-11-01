using UnityEngine;

namespace GameAssets.Player.Scripts
{
    public class RocketAmmo : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;

        private const float MaxSpeed = 2f;
        private const float MinSpeed = 1.7f;

        public void StartFlyTo(Vector2 dir)
        {
            _rigidbody2D.velocity = dir * Random.Range(MinSpeed, MaxSpeed);
            transform.up = _rigidbody2D.velocity;
        }

        public void OnCollided()
        {
            //
        }
    }
}
