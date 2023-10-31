using System;
using System.Collections;
using GameAssets.Player.Scripts;
using UnityEngine;

public class RocketMissile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _missileRigidbody2D; 
    [SerializeField] private Rigidbody2D _shootPointRigidbody2D;
    [SerializeField] private RocketInstance _instance;

    private const float maxDistance = 2f;

    private bool _isPressed;
    private bool _isUnderShootPoint;
    private bool _isLaunched;

    private void OnEnable()
    {
        _isLaunched = false;
    }

    private void Update()
    {
        if (_isPressed)
        {
            Vector2 touch = Camera.main.ScreenToWorldPoint(Input.touches[0].position); 

            if (Vector2.Distance(touch, _shootPointRigidbody2D.position) > maxDistance)
                _missileRigidbody2D.position = _shootPointRigidbody2D.position + (touch - _shootPointRigidbody2D.position).normalized * maxDistance;
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
        _missileRigidbody2D.velocity = Vector2.zero;
        _isPressed = true;
        _missileRigidbody2D.isKinematic = true;
        _missileRigidbody2D.useFullKinematicContacts = true;
    }

    private void OnMouseUp()
    {
        _isPressed = false;
        _missileRigidbody2D.useFullKinematicContacts = false;
        _missileRigidbody2D.isKinematic = false;
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.GetComponent<SpringJoint2D>().enabled = false;
        enabled = false; 
        Destroy(gameObject, 5); 

        yield return new WaitForSeconds(2);
    }
}
