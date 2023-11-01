using System;
using GameAssets.Enemy.Scripts;
using GameAssets.Player.Scripts;
using UnityEngine;

namespace GameAssets.Main.Scripts
{
    public class Disabler : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out RocketInstance rocketInstance))
            {
                rocketInstance.transform.parent.gameObject.SetActive(false);
            }
            
            if (other.TryGetComponent(out RocketAmmo rocketAmmo))
            {
                rocketAmmo.gameObject.SetActive(false);
            }
            
            if (other.TryGetComponent(out RocketEnemy rocketEnemy))
            {
                rocketEnemy.gameObject.SetActive(false);
            }
        }
    }
}
