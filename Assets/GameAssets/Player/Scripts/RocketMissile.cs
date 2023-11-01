using System;
using System.Collections;
using UnityEngine;

namespace GameAssets.Player.Scripts
{
    public class RocketMissile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _missileRigidbody2D;
        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private SpringJoint2D _springJoint2D;
        [SerializeField] private RocketInstance _instance;

        public RocketInstance Instance => _instance;
        
        public event Action Launched;

        private const float AppearingSpeed = 8;
        private const float MaxDistance = 2;
        
        private Rigidbody2D _shootPointRigidbody2D;
        private bool _isPressed;
        private bool _isUnderShootPoint;
        private bool _isLaunched;
        private bool _isReady;

        public void SetShootPoint(Rigidbody2D shootPointRigidbody2D, Vector2 startPos)
        {
            transform.position = startPos;
            _shootPointRigidbody2D = shootPointRigidbody2D;
            StartCoroutine(MoveToShootPoint());
        }
        
        private void OnEnable()
        {
            _collider2D.enabled = false;
            _instance.DisableCollider();
            _missileRigidbody2D.isKinematic = true;
            _isReady = false;
            _isLaunched = false;
        }

        private void Update()
        {
            if (_isPressed)
            {
                Vector2 touch = Camera.main.ScreenToWorldPoint(Input.touches[0].position); 

                if (Vector2.Distance(touch, _shootPointRigidbody2D.position) > MaxDistance)
                    _missileRigidbody2D.position = _shootPointRigidbody2D.position + (touch - _shootPointRigidbody2D.position).normalized * MaxDistance;
                else
                    _missileRigidbody2D.position = touch;
            }
        
            if (!_isUnderShootPoint && !_isLaunched)
                _instance.transform.up = (_shootPointRigidbody2D.position - (Vector2)transform.position);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ShootPoint _))
                _isUnderShootPoint = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out ShootPoint _))
                _isUnderShootPoint = false;
        }

        private void OnMouseDown()
        {
            if (_isReady && !_isLaunched)
            {
                _missileRigidbody2D.velocity = Vector2.zero;
                _isPressed = true;
                _missileRigidbody2D.isKinematic = true;
                _missileRigidbody2D.useFullKinematicContacts = true;
            }
        }

        private void OnMouseUp()
        {
            if (_isReady && !_isLaunched)
            {
                _isPressed = false;
                _missileRigidbody2D.useFullKinematicContacts = false;
                _missileRigidbody2D.isKinematic = false;
                StartCoroutine(Shoot());
            }
        }

        private IEnumerator MoveToShootPoint()
        {
            while (Vector2.Distance(_shootPointRigidbody2D.position, transform.position) >= 0.065)
            {
                _missileRigidbody2D.velocity = (_shootPointRigidbody2D.position - (Vector2)transform.position).normalized * AppearingSpeed;
                yield return null;
            }

            _missileRigidbody2D.velocity = Vector2.zero;
            transform.position = _shootPointRigidbody2D.position;
            _missileRigidbody2D.isKinematic = false;
            _springJoint2D.connectedBody = _shootPointRigidbody2D;
            _isReady = true;
            _collider2D.enabled = true;
        }
        
        private IEnumerator Shoot()
        {
            _isLaunched = true;
            _instance.EnableCollider();
            yield return new WaitForSeconds(0.1f);

            gameObject.GetComponent<SpringJoint2D>().enabled = false;
            Launched?.Invoke();
            enabled = false;

            yield return new WaitForSeconds(2);
        }
    }
}
