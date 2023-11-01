using System;
using System.Collections;
using GameAssets.Common.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameAssets.Enemy.Scripts
{
    public class RocketEnemy : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Body _bear;
        [SerializeField] private Transform _bearPosition;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private const float MaxSpeed = 1.4f;
        private const float MinSpeed = 1.3f;
        private const float MaxWaitingTime = 4.5f;
        private const float MinWaitingTime = 0;

        private float _constSpeed;
        private Coroutine _flyingToTarget;

        private void OnEnable()
        {
            _rigidbody2D.gravityScale = 0;
            _spriteRenderer.enabled = true;
            _bear.MakeKinematic();
            _bear.transform.position = _bearPosition.position;
        }

        private void OnDisable()
        {
            if (_flyingToTarget != null)
                StopCoroutine(_flyingToTarget);
        }

        public void OnCollided()
        {
            //
            
            if (_flyingToTarget != null)
                StopCoroutine(_flyingToTarget);
            
            _rigidbody2D.gravityScale = 1;
            _spriteRenderer.enabled = false;
            _bear.makeDynamic();
        }
        
        public void StartFlyTo(Vector2 startDir, Vector2 endPoint)
        {
            _constSpeed = Random.Range(MinSpeed, MaxSpeed);
            _rigidbody2D.velocity = startDir.normalized * _constSpeed;

            if (startDir == Vector2.right)
                transform.localScale = new Vector3(-0.9f, transform.localScale.y, transform.localScale.z);
            else
                transform.localScale =
                    new Vector3(0.9f, transform.localScale.y, transform.localScale.z);

            transform.up = startDir;

            if (_flyingToTarget != null)
                StopCoroutine(_flyingToTarget);

            _flyingToTarget = StartCoroutine(Flying(endPoint));
        }

        private IEnumerator Flying(Vector2 target)
        {
            yield return new WaitForSeconds(Random.Range(MinWaitingTime, MaxWaitingTime));

            while (gameObject.activeSelf)
            {
                _rigidbody2D.velocity = Vector2.MoveTowards(_rigidbody2D.velocity,
                    (target - (Vector2)transform.position).normalized * _constSpeed, Time.deltaTime);
                transform.up = _rigidbody2D.velocity;
                yield return null;
            }
        }
    }
}
